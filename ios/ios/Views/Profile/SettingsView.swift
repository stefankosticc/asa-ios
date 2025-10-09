//
//  SettingsView.swift
//  ios
//
//  Created by stefan on 8.10.25..
//

import SwiftUI

struct SettingsView: View {
    @StateObject private var authVM = AuthViewModel()
    @State private var showLogOutAlert = false
    
    var body: some View {
        NavigationStack {
            ZStack {
                Color.black.ignoresSafeArea()
                
                List {
                    Section {
                        NavigationLink(destination: AccountSettingsView()) {
                            Text("Account")
                        }
                        
                        NavigationLink(destination: {}) {
                            Text("Privacy")
                        }
                        
                        NavigationLink(destination: {}) {
                            Text("Payments")
                        }
                    }
                    .listRowBackground(Color.cBlackHighlight)
                    
                    Section {
                        Button(action: {
                            showLogOutAlert = true
                        }, label: {
                            Text("Log out")
                                .foregroundStyle(Color.cRed)
                                .fontWeight(/*@START_MENU_TOKEN@*/.bold/*@END_MENU_TOKEN@*/)
                        })
                        .alert("Are you sure you want to log out?", isPresented: $showLogOutAlert) {
                            Button("Log out", role: .destructive) {
                                Task {
                                    await authVM.logout()
                                }
                            }
                            Button("Cancel", role: .cancel) { }
                        }

                    }
                    .listRowBackground(Color.cRedLight)
                }
                .scrollContentBackground(.hidden)
            }
            .navigationTitle("Settings")
            .navigationBarTitleDisplayMode(.large)
            .preferredColorScheme(/*@START_MENU_TOKEN@*/.dark/*@END_MENU_TOKEN@*/)
        }
        .tint(.cPurple)
    }
}

#Preview {
    SettingsView()
}
