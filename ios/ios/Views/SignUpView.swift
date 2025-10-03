//
//  SignUpView.swift
//  ios
//
//  Created by stefan on 29.9.25..
//

import SwiftUI

struct SignUpView: View {
    @StateObject private var authViewModel = AuthViewModel()
    
    @FocusState private var focusedField: Field?
    enum Field { case email, password, name, userName, confirmPassword }
    
    @Environment(\.dismiss) private var dismiss
    
    var body: some View {
            ZStack {
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
                
                VStack(spacing: 20) {
                    Text("Sign up")
                        .font(/*@START_MENU_TOKEN@*/.title/*@END_MENU_TOKEN@*/)
                        .fontWeight(.bold)
                        .padding(.bottom, 10)
                    
                    TextField(text: $authViewModel.signUpForm.name, label: {
                        Text("Name")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .name))
                    .focused($focusedField, equals: .name)
                    
                    TextField(text: $authViewModel.signUpForm.userName, label: {
                        Text("Username")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .userName))
                    .focused($focusedField, equals: .userName)
                    .textInputAutocapitalization(.never)
                    
                    TextField(text: $authViewModel.signUpForm.email, label: {
                        Text("Email")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .email))
                    .focused($focusedField, equals: .email)
                    .textInputAutocapitalization(.never)
                    .keyboardType(.emailAddress)
                    .textContentType(.emailAddress)
                    
                    SecureField(text: $authViewModel.signUpForm.password, label: {
                        Text("Password")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .password))
                    .focused($focusedField, equals: .password)
                    .textInputAutocapitalization(.never)
                    
                    SecureField(text: $authViewModel.signUpForm.confirmPassword, label: {
                        Text("Confirm Password")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .confirmPassword))
                    .focused($focusedField, equals: .confirmPassword)
                    .textInputAutocapitalization(.never)
                    
                    if let error = authViewModel.errorMessage {
                        Text(error)
                            .modifier(ErrorTextStyle())
                    }
                    
                    Button(action: {
                        Task{
                            await authViewModel.signUp()
                        }
                    }, label: {
                        Text("Sign up")
                    })
                    .foregroundColor(.black)
                    .bold()
                    .frame(maxWidth: .infinity)
                    .padding(.vertical, 6)
                    .background(.cPurple)
                    .clipShape(Capsule())
                    .padding([.top, .leading, .trailing], 10)
                    
                    HStack(spacing: 4, content: {
                        Text("Already have an account? ")
                        Button(action: {dismiss()}, label: {
                            Text("Log in")
                                .underline()
                                .bold()
                                .foregroundStyle(Color("cPurple"))
                        })
                    })
                      
                }
                .padding(.horizontal, 28)
                .foregroundColor(.white)
                .frame(height: 700)
                .background(Color.black)
                .cornerRadius(18)
                .padding()
            }
            .onTapGesture {
                focusedField = nil
            }
    }
}

#Preview {
    SignUpView()
}
