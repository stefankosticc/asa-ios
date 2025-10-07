//
//  ProfileViewModel.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation

@MainActor
class ProfileViewModel : ObservableObject {
    private let api: APIServiceProtocol
    @Published var errorMessage: String?
    @Published var isLoading: Bool = false
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    func getLoggedInUser() async -> User? {
        do {
            isLoading = true
            let response: User = try await api.get(endpoint: "auth/loggedin-user")
            isLoading = false
            return response
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        isLoading = false
        return nil
    }
}
