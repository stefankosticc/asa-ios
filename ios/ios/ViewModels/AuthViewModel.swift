//
//  AuthViewModel.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import Foundation

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
    
    private let api: APIServiceProtocol
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    private func isValidEmail(_ email: String) -> Bool {
        let emailPattern = #"^\S+@\S+\.\S+$"#
        return email.range(of: emailPattern, options: .regularExpression) != nil
    }
    
    func login() async {
        let request = LoginRequest(email: loginForm.email, password: loginForm.password)
        do {
            let response: LoginResponse = try await api.post(endpoint: "auth/login", body: request)
            await api.saveTokens(accessToken: response.accessToken, refreshToken: response.refreshToken)
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
            self.loginForm.password = ""
        } catch {
            self.errorMessage = error.localizedDescription
        }
    }
    
    func signUp() async {
        guard !signUpForm.email.isEmpty, !signUpForm.password.isEmpty, !signUpForm.confirmPassword.isEmpty,
              !signUpForm.name.isEmpty, !signUpForm.userName.isEmpty else {
            self.errorMessage = "Please fill in all fields."
            return
        }
        
        guard isValidEmail(signUpForm.email) else {
            self.errorMessage = "Please enter a valid email address."
            return
        }
        
        guard signUpForm.password == signUpForm.confirmPassword else {
            self.errorMessage = "Passwords do not match."
            return
        }
        
        let request = SignUpRequest(name: signUpForm.name, email: signUpForm.email, userName: signUpForm.userName, password: signUpForm.password)
        do {
            try await api.post(endpoint: "auth/register", body: request)
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
            self.signUpForm.password = ""
            self.signUpForm.confirmPassword = ""
        } catch {
            self.errorMessage = error.localizedDescription
        }
    }
}
