//
//  AuthView.swift
//  ios
//
//  Created by stefan on 28.9.25..
//

import SwiftUI

struct LogInView: View {
    @State var email : String = ""
    @State var password : String = ""
    
    @FocusState private var focusedField: Field?
    enum Field { case email, password }
    
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
                    Text("Log in")
                        .font(/*@START_MENU_TOKEN@*/.title/*@END_MENU_TOKEN@*/)
                        .fontWeight(.bold)
                        .padding(.bottom, 10)
                    
                    TextField(text: $email, label: {
                        Text("Email")
                            .foregroundStyle(Color.gray)
                    })
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .email))
                    .focused($focusedField, equals: .email)
                    .textInputAutocapitalization(.never)
                    .keyboardType(.emailAddress)
                    .textContentType(.emailAddress)
                        
                    SecureField(text: $password) {
                        Text("Password")
                            .foregroundStyle(Color.gray)
                    }
                    .textFieldStyle(AuthTextFieldStyle(isFocused: focusedField == .password))
                    .focused($focusedField, equals: .password)
                    .textInputAutocapitalization(.never)
                    
                    Button(action: /*@START_MENU_TOKEN@*/{}/*@END_MENU_TOKEN@*/, label: {
                        Text("Forgot password?")
                            .frame(maxWidth: .infinity, alignment: .trailing)
                            .foregroundColor(.gray)
                            .padding(.top, -10)
                            .font(.custom("", size: 14))
                    })
                    
                    
                    Button(action: /*@START_MENU_TOKEN@*/{}/*@END_MENU_TOKEN@*/, label: {
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
                .frame(height: 700)
                .background(Color.black)
                .cornerRadius(18)
                .padding()
            }.onTapGesture {
                focusedField = nil
            }
    }
}

#Preview {
    LogInView()
}
