//
//  APIService.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import Foundation
import UniformTypeIdentifiers

enum APIServiceError: Error, LocalizedError {
    case invalidURL
    case invalidResponse
    case httpError(statusCode: Int, message: String?)
    case noData
    case decodingError(Error)
    
    var errorDescription: String? {
        switch self {
        case .invalidURL:
            return "Invalid URL."
        case .invalidResponse:
            return "Invalid server response."
        case .httpError(let code, let message):
            return message ?? "HTTP Error \(code)"
        case .noData:
            return "No data received."
        case .decodingError(let error):
            return "Failed to decode response: \(error.localizedDescription)"
        }
    }
}

struct APIErrorResponse: Decodable {
    let error: String
}

struct EmptyResponse: Decodable {}

private extension Data {
    mutating func append(_ string: String) {
        if let data = string.data(using: .utf8) {
            append(data)
        }
    }
}

protocol APIServiceProtocol {
    func get<T: Decodable>(endpoint: String) async throws -> T
    func post<T: Decodable, Body: Encodable>(endpoint: String, body: Body) async throws -> T
    func put<T: Decodable, Body: Encodable>(endpoint: String, body: Body) async throws -> T
    func delete<T: Decodable>(endpoint: String) async throws -> T
    
    func post<Body: Encodable>(endpoint: String, body: Body) async throws
    func post(endpoint: String) async throws
    func put<Body: Encodable>(endpoint: String, body: Body) async throws
    func delete(endpoint: String) async throws
    
    func saveTokens(accessToken: String, refreshToken: String) async
    
    func putWithImage<Body: Encodable>(endpoint: String, body: Body, image: Data?, imageFieldName: String) async throws
}


class APIService: APIServiceProtocol {
    private let baseURL: String
    private let tokenStore: TokenStore
    private let refreshEndpoint = "auth/refresh-token"
    
    init(baseURL: String = "http://localhost:5125/api", tokenStore: TokenStore = TokenStore()) {
        self.baseURL = baseURL
        self.tokenStore = tokenStore
    }
    
    // Token Refresh Logic
    private func refreshAccessToken() async throws {
        guard let refreshToken = await tokenStore.getRefreshToken() else {
            throw APIServiceError.httpError(statusCode: 401, message: "No refresh token available.")
        }
        struct RefreshRequest: Codable { let refreshToken: String }
        struct RefreshResponse: Codable { let accessToken: String; let refreshToken: String }
        let body = RefreshRequest(refreshToken: refreshToken)
        let bodyData = try JSONEncoder().encode(body)
        guard let url = URL(string: "\(baseURL)/\(refreshEndpoint)") else {
            throw APIServiceError.invalidURL
        }
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = bodyData
        
        let (data, response) = try await URLSession.shared.data(for: request)
        guard let httpResponse = response as? HTTPURLResponse else {
            throw APIServiceError.invalidResponse
        }
        guard (200...299).contains(httpResponse.statusCode) else {
            await tokenStore.clearTokens()
            var message: String? = nil
            if let apiError = try? JSONDecoder().decode(APIErrorResponse.self, from: data) {
                message = apiError.error
            }
            throw APIServiceError.httpError(statusCode: httpResponse.statusCode, message: message)
        }
        let decoded = try JSONDecoder().decode(RefreshResponse.self, from: data)
        await tokenStore.setTokens(accessToken: decoded.accessToken, refreshToken: decoded.refreshToken)
    }
    
    // Generic Request with Refresh Support
    private func request<T: Decodable>(
        endpoint: String,
        method: String,
        body: Data? = nil,
        retryOnAuthFail: Bool = true
    ) async throws -> T {
        guard let url = URL(string: "\(baseURL)/\(endpoint)") else {
            throw APIServiceError.invalidURL
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = method
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        if let token = await tokenStore.getAccessToken() {
            request.setValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
        }
        request.httpBody = body
        
        let (data, response) = try await URLSession.shared.data(for: request)
        
        guard let httpResponse = response as? HTTPURLResponse else {
            throw APIServiceError.invalidResponse
        }
        
        if httpResponse.statusCode == 401, retryOnAuthFail, (await tokenStore.getRefreshToken()) != nil {
            // Try to refresh token and retry once
            do {
                try await refreshAccessToken()
                return try await self.request(endpoint: endpoint, method: method, body: body, retryOnAuthFail: false)
            } catch {
                await tokenStore.clearTokens()
                throw APIServiceError.httpError(statusCode: 401, message: "Session expired. Please log in again.")
            }
        }
        
        guard (200...299).contains(httpResponse.statusCode) else {
            var message: String? = nil
            if let apiError = try? JSONDecoder().decode(APIErrorResponse.self, from: data) {
                message = apiError.error
            }
            throw APIServiceError.httpError(statusCode: httpResponse.statusCode, message: message)
        }
        
        // When request doesn't return anything
        if data.isEmpty, T.self == EmptyResponse.self {
            return EmptyResponse() as! T
        }
        
        do {
            let decoder = JSONDecoder()
            decoder.keyDecodingStrategy = .convertFromSnakeCase
            return try decoder.decode(T.self, from: data)
        } catch {
            throw APIServiceError.decodingError(error)
        }
    }
    
    func get<T: Decodable>(endpoint: String) async throws -> T {
        try await request(endpoint: endpoint, method: "GET")
    }
    
    func post<T: Decodable, Body: Encodable>(endpoint: String, body: Body) async throws -> T {
        let bodyData = try JSONEncoder().encode(body)
        return try await request(endpoint: endpoint, method: "POST", body: bodyData)
    }
    
    func put<T: Decodable, Body: Encodable>(endpoint: String, body: Body) async throws -> T {
        let bodyData = try JSONEncoder().encode(body)
        return try await request(endpoint: endpoint, method: "PUT", body: bodyData)
    }
    
    func delete<T: Decodable>(endpoint: String) async throws -> T {
        try await request(endpoint: endpoint, method: "DELETE")
    }
    
    // POST returning nothing
    func post<Body: Encodable>(endpoint: String, body: Body) async throws {
        let _: EmptyResponse = try await post(endpoint: endpoint, body: body)
    }
    
    // POST without a body
    func post(endpoint: String) async throws {
        let _: EmptyResponse = try await request(endpoint: endpoint, method: "POST")
    }
    
    // PUT returning nothing
    func put<Body: Encodable>(endpoint: String, body: Body) async throws {
        let _: EmptyResponse = try await put(endpoint: endpoint, body: body)
    }
    
    // DELETE returning nothing
    func delete(endpoint: String) async throws {
        let _: EmptyResponse = try await delete(endpoint: endpoint)
    }
    
    func saveTokens(accessToken: String, refreshToken: String) async {
        await tokenStore.setTokens(accessToken: accessToken, refreshToken: refreshToken)
    }
    
    private func requestWithImage<T: Decodable, Body: Encodable>(
        endpoint: String,
        imageData: Data?,
        body: Body,
        imageFieldName: String = "file",
        fileName: String = "image",
        method: String // PUT or POST
    ) async throws -> T {
        guard let url = URL(string: "\(baseURL)/\(endpoint)") else {
            throw APIServiceError.invalidURL
        }
        
        let boundary = "Boundary-\(UUID().uuidString)"
        var request = URLRequest(url: url)
        request.httpMethod = method
        request.setValue("multipart/form-data; boundary=\(boundary)", forHTTPHeaderField: "Content-Type")
        
        if let token = await tokenStore.getAccessToken() {
            request.setValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
        }
        
        var bodyData = Data()
        
        // Add image part
        if let imageData {
            let mime = mimeType(for: imageData)
            bodyData.append("--\(boundary)\r\n")
            bodyData.append("Content-Disposition: form-data; name=\"\(imageFieldName)\"; filename=\"\(fileName)\"\r\n")
            bodyData.append("Content-Type: \(mime)\r\n\r\n")
            bodyData.append(imageData)
            bodyData.append("\r\n")
        }
        
        // Add JSON part
        let json = try JSONSerialization.jsonObject(with: JSONEncoder().encode(body))
        if let dict = json as? [String: Any] {
            for (key, value) in dict {
                bodyData.append("--\(boundary)\r\n")
                bodyData.append("Content-Disposition: form-data; name=\"\(key)\"\r\n\r\n")
                
                // Booleans must be sent as "true" or "false", not as "0" or "1" for .NET backend
                if let boolValue = value as? Bool {
                    bodyData.append(boolValue ? "true\r\n" : "false\r\n")
                } else {
                    bodyData.append("\(value)\r\n")
                }
            }
        }
        
        bodyData.append("--\(boundary)--\r\n")
        
        request.httpBody = bodyData
        
        let (data, response) = try await URLSession.shared.data(for: request)
        
        guard let httpResponse = response as? HTTPURLResponse else {
            throw APIServiceError.invalidResponse
        }
        
        if httpResponse.statusCode == 401, (await tokenStore.getRefreshToken()) != nil {
            do {
                try await refreshAccessToken()
                return try await requestWithImage(
                    endpoint: endpoint,
                    imageData: imageData,
                    body: body,
                    imageFieldName: imageFieldName,
                    fileName: fileName,
                    method: method
                )
            } catch {
                await tokenStore.clearTokens()
                throw APIServiceError.httpError(statusCode: 401, message: "Session expired. Please log in again.")
            }
        }
        
        guard (200...299).contains(httpResponse.statusCode) else {
            var message: String? = nil
            if let apiError = try? JSONDecoder().decode(APIErrorResponse.self, from: data) {
                message = apiError.error
            }
            throw APIServiceError.httpError(statusCode: httpResponse.statusCode, message: message)
        }
        
        if data.isEmpty, T.self == EmptyResponse.self {
            return EmptyResponse() as! T
        }
        
        do {
            let decoder = JSONDecoder()
            decoder.keyDecodingStrategy = .convertFromSnakeCase
            return try decoder.decode(T.self, from: data)
        } catch {
            throw APIServiceError.decodingError(error)
        }
    }
    
    
    private func mimeType(for data: Data) -> String {
        let bytes = [UInt8](data.prefix(8))
        if bytes.starts(with: [0xFF, 0xD8, 0xFF]) {
            return "image/jpeg"
        } else if bytes.starts(with: [0x89, 0x50, 0x4E, 0x47]) {
            return "image/png"
        } else if bytes.starts(with: [0x47, 0x49, 0x46, 0x38]) {
            return "image/gif"
        }
        return "application/octet-stream"
    }
    
    func putWithImage<Body: Encodable>(endpoint: String, body: Body, image: Data?, imageFieldName: String) async throws {
        let _: EmptyResponse = try await requestWithImage(endpoint: endpoint, imageData: image, body: body, imageFieldName: imageFieldName, method: "PUT")
    }
}
