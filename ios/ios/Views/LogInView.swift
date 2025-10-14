//
//  AuthView.swift
//  ios
//
//  Created by stefan on 28.9.25..
//

import SwiftUI

struct LogInView: View {
    @StateObject private var authViewModel = AuthViewModel()
    
    @FocusState private var focusedField: Field?
    enum Field { case email, password }
    
    var body: some View {
            ZStack {
                if authViewModel.isLoading {
                    Color.black.ignoresSafeArea()
                    
                    VStack(spacing: 16) {
                        ProgressView()
                            .tint(.cGrayLight)
                            .scaleEffect(1.5)
                            .navigationBarBackButtonHidden()
                    }
                } else {
                    RadialGradient(
                        gradient: Gradient(colors: [
                            Color(red: 0.031, green: 0.024, blue: 0.055),
                            Color(red: 0.235, green: 0.129, blue: 0.729),
                            Color(red: 0.674, green: 0.639, blue: 1.0)
                        ]),
                        center: .center,
                        startRadius: 0,
                        endRadius: 500
                    )
                    .ignoresSafeArea()
                    
                    GeometryReader{ geometry in
                        VStack(spacing: 20) {
                            Text("Log in")
                                .font(.title)
                                .fontWeight(.bold)
                                .padding(.bottom, 10)
                            
                            TextField(text: $authViewModel.loginForm.email, label: {
                                Text("Email")
                                    .foregroundStyle(Color.gray)
                            })
                            .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .email))
                            .focused($focusedField, equals: .email)
                            .textInputAutocapitalization(.never)
                            .keyboardType(.emailAddress)
                            .textContentType(.emailAddress)
                            
                            SecureField(text: $authViewModel.loginForm.password) {
                                Text("Password")
                                    .foregroundStyle(Color.gray)
                            }
                            .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .password))
                            .focused($focusedField, equals: .password)
                            .textInputAutocapitalization(.never)
                            
                            Button(action: {}, label: {
                                Text("Forgot password?")
                                    .frame(maxWidth: .infinity, alignment: .trailing)
                                    .foregroundColor(.gray)
                                    .padding(.top, -10)
                                    .font(.custom("", size: 14))
                            })
                            
                            if let error = authViewModel.errorMessage {
                                Text(error)
                                    .modifier(ErrorTextStyle())
                            }
                            
                            Button(action: {
                                Task{
                                    await authViewModel.login()
                                }
                            }, label: {
                                Text("Log in")
                            })
                            .foregroundColor(.black)
                            .bold()
                            .frame(maxWidth: .infinity)
                            .padding(.vertical, 6)
                            .background(.cPurple)
                            .clipShape(Capsule())
                            .padding([.top, .leading, .trailing], 10)
                            
                            HStack(spacing: 4, content: {
                                Text("Don't have an account? ")
                                NavigationLink(destination: SignUpView(), label: {
                                    Text("Sign up")
                                        .underline()
                                        .bold()
                                        .foregroundStyle(.cPurple)
                                })
                            })
                            
                            
                        }
                        .padding(.horizontal, 28)
                        .foregroundColor(.white)
                        .frame(height: geometry.size.height * 0.98)
                        .background(Color.black)
                        .cornerRadius(18)
                        .padding()
                    }
                }
            }.onTapGesture {
                focusedField = nil
            }
    }
}

#Preview {
    LogInView()
}
