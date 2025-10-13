//
//  MainTabView.swift
//  ios
//
//  Created by stefan on 3.10.25..
//

import SwiftUI

struct MainTabView: View {
    @State private var selectedTab = 2
    
    init() {
        let appearance = UITabBarAppearance()
        
        let blurEffect = UIBlurEffect(style: .systemMaterialDark)
        let blurEffectView = UIVisualEffectView(effect: blurEffect)
        blurEffectView.alpha = 0.85

        UIGraphicsBeginImageContextWithOptions(CGSize(width: 1, height: 1), false, 0)
        blurEffectView.layer.render(in: UIGraphicsGetCurrentContext()!)
        _ = UIGraphicsGetImageFromCurrentImageContext()
        UIGraphicsEndImageContext()

        appearance.backgroundEffect = blurEffect
        appearance.backgroundColor = UIColor(red: 8/255, green: 6/255, blue: 14/255, alpha: 0.85)
        
        appearance.shadowColor = .clear

        UITabBar.appearance().standardAppearance = appearance
        UITabBar.appearance().scrollEdgeAppearance = appearance
    }
    
    var body: some View {
        TabView(selection: $selectedTab) {
            SearchView()
                .tabItem {
                    Image(systemName: "magnifyingglass")
                    Text("Search")
                }
                .tag(0)
            
            
            ProfileView()
                .tabItem {
                    Image(systemName: "person.fill")
                    Text("Profile")
                }
                .tag(1)
            
            NavigationStack {
                DiscoverView()
            }
            .tabItem {
                Image(systemName: "rectangle.portrait.on.rectangle.portrait.angled.fill")
                Text("Discover")
            }
            .tag(2)
            
            ArtworkView(isNew: true)
                .tabItem {
                    Image(systemName: "plus.square.fill")
                    Text("Add")
                }
                .tag(3)
            
            DiscoverView()
                .tabItem {
                    Image(systemName: "bubble.fill")
                    Text("Chat")
                }
                .tag(4)
        }
        .accentColor(.cPurple)   
    }
}

#Preview {
    MainTabView()
}
