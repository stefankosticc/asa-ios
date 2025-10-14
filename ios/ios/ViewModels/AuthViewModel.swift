//
//  AuthViewModel.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import Foundation
import SwiftUI

@MainActor
class AuthViewModel : ObservableObject {
    struct LoginForm {
        var email: String = ""
        var password: String = ""
    }
    
    struct SignUpForm {
        var email: String = ""
        var password: String = ""
        var confirmPassword : String = ""
        var name : String = ""
        var userName : String = ""
    }
    
    @Published var loginForm = LoginForm()
    @Published var signUpForm = SignUpForm()
    @Published var errorMessage: String?
    @Published var isLoading: Bool = false
    
    @AppStorage("isAuthenticated") private var isAuthenticated: Bool = false
    
    private let api: APIServiceProtocol
    private let tokenStore = TokenStore()
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    private func isValidEmail(_ email: String) -> Bool {
        let emailPattern = #"^\S+@\S+\.\S+$"#
        return email.range(of: emailPattern, options: .regularExpression) != nil
    }
    
    func login() async -> Bool {
        let request = LoginRequest(
            email: loginForm.email,
            password: loginForm.password
        )
        
        do {
            isLoading = true
            let response: LoginResponse = try await api.post(endpoint: "auth/login", body: request)
            await api.saveTokens(accessToken: response.accessToken, refreshToken: response.refreshToken)
            
            isLoading = false
            isAuthenticated = true
            return true
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
            self.loginForm.password = ""
            isAuthenticated = false
        } catch {
            self.errorMessage = error.localizedDescription
            isAuthenticated = false
        }
        
        isLoading = false
        return false
    }
    
    func signUp() async -> Bool {
        guard !signUpForm.email.isEmpty, !signUpForm.password.isEmpty, !signUpForm.confirmPassword.isEmpty,
              !signUpForm.name.isEmpty, !signUpForm.userName.isEmpty else {
            self.errorMessage = "Please fill in all fields."
            return false
        }
        
        guard isValidEmail(signUpForm.email) else {
            self.errorMessage = "Please enter a valid email address."
            return false
        }
        
        guard signUpForm.password == signUpForm.confirmPassword else {
            self.errorMessage = "Passwords do not match."
            return false
        }
        
        let request = SignUpRequest(
            name: signUpForm.name,
            email: signUpForm.email,
            userName: signUpForm.userName,
            password: signUpForm.password
        )
        
        do {
            try await api.post(endpoint: "auth/register", body: request)
            return true
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
            self.signUpForm.password = ""
            self.signUpForm.confirmPassword = ""
        } catch {
            self.errorMessage = error.localizedDescription
        }
        
        return false
    }
    
    func checkAuthentication() async {
        isAuthenticated = await tokenStore.getRefreshToken() != nil
    }
    
    func logout() async {
        do {
            try await api.post(endpoint: "auth/logout")
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        
        await tokenStore.clearTokens()
        isAuthenticated = false
    }
}
