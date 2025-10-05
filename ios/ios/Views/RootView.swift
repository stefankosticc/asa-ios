//
//  RootView.swift
//  ios
//
//  Created by stefan on 5.10.25..
//

import SwiftUI

struct RootView: View {
    @StateObject private var authViewModel = AuthViewModel()
    @AppStorage("isAuthenticated") private var isAuthenticated: Bool = false
    
    var body: some View {
        Group {
            if isAuthenticated {
                MainTabView()
            } else {
                GetStartedView()
            }
        }
        .task {
            await authViewModel.checkAuthentication()
        }
    }
}

#Preview {
    RootView()
}
