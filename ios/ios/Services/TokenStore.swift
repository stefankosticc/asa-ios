//
//  TokenStore.swift
//  ios
//
//  Created by stefan on 3.10.25..
//

import Foundation
import Security

actor TokenStore {
    private let accessTokenKey = "accessToken"
    private let refreshTokenKey = "refreshToken"
    private let service = "com.asa.tokens"

    func setTokens(accessToken: String?, refreshToken: String?) async {
        await setToken(accessToken, forKey: accessTokenKey)
        await setToken(refreshToken, forKey: refreshTokenKey)
    }

    func getAccessToken() async -> String? {
        await getToken(forKey: accessTokenKey)
    }

    func getRefreshToken() async -> String? {
        await getToken(forKey: refreshTokenKey)
    }

    func clearTokens() async {
        await setToken(nil, forKey: accessTokenKey)
        await setToken(nil, forKey: refreshTokenKey)
    }

    private func setToken(_ token: String?, forKey key: String) async {
        let query: [String: Any] = [
            kSecClass as String: kSecClassGenericPassword,
            kSecAttrService as String: service,
            kSecAttrAccount as String: key
        ]
        SecItemDelete(query as CFDictionary)
        guard let token = token else { return }
        let attributes: [String: Any] = [
            kSecClass as String: kSecClassGenericPassword,
            kSecAttrService as String: service,
            kSecAttrAccount as String: key,
            kSecValueData as String: token.data(using: .utf8)!
        ]
        SecItemAdd(attributes as CFDictionary, nil)
    }

    private func getToken(forKey key: String) async -> String? {
        let query: [String: Any] = [
            kSecClass as String: kSecClassGenericPassword,
            kSecAttrService as String: service,
            kSecAttrAccount as String: key,
            kSecReturnData as String: true,
            kSecMatchLimit as String: kSecMatchLimitOne
        ]
        var dataTypeRef: AnyObject?
        let status = SecItemCopyMatching(query as CFDictionary, &dataTypeRef)
        guard status == errSecSuccess,
              let data = dataTypeRef as? Data,
              let token = String(data: data, encoding: .utf8) else {
            return nil
        }
        return token
    }
}
