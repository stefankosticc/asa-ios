//
//  ProfileView.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI

struct ProfileView: View {
    @State private var selectedTab = 2
    @StateObject private var profileVM = ProfileViewModel()
    @State private var user: User? = nil
    
    init() {
        UISegmentedControl.appearance().backgroundColor = UIColor.cBlackHighlight
        UISegmentedControl.appearance().setTitleTextAttributes([.foregroundColor: UIColor.black], for: .selected)
        UISegmentedControl.appearance().setTitleTextAttributes([.foregroundColor: UIColor.gray], for: .normal)
    }
    
    var body: some View {
        ZStack{
            Color(.black)
                .ignoresSafeArea()
            
            if profileVM.isLoading {
                VStack(spacing: 16) {
                    ProgressView()
                        .tint(.cGrayLight)
                        .scaleEffect(1.5)
                }
            } else if let u = user {
                ScrollView(showsIndicators: false) {
                    HStack(spacing: 40) {
                        VStack(spacing: 20) {
                            ProfilePhoto(url: "/api/user/\(u.id)/profile-photo", size: 100)
                            VStack{
                                Text(u.name)
                                    .foregroundStyle(.white)
                                    .bold()
                                Text("@\(user?.userName ?? "")")
                                    .foregroundStyle(.cGrayLight)
                                    .font(.subheadline)
                            }
                        }
                        
                        Spacer()
                        
                        VStack(spacing: 20) {
                            VStack{
                                Text("Followers")
                                    .foregroundStyle(.white)
                                Text(Formatter.formatFollowCount(user?.followersCount))
                                    .foregroundStyle(.cGrayLight)
                                    .font(.subheadline)
                            }
                            
                            VStack{
                                Text("Following")
                                    .foregroundStyle(.white)
                                Text(Formatter.formatFollowCount(user?.followingCount))
                                    .foregroundStyle(.cGrayLight)
                                    .font(.subheadline)
                            }
                        }
                    }
                    .padding()
                    
                    
                    Picker("Profile Tabs", selection: $selectedTab) {
                        Text("Artworks").tag(0)
                        Text("Favorites").tag(1)
                        Text("Biography").tag(2)
                    }
                    .pickerStyle(.segmented)
                    
                    VStack {
                        switch selectedTab {
                        case 0:
                        ArtworkGridView()
                            Text("artworks")
                                .foregroundStyle(.white)
                        case 1:
//                        FavoritesView()
                            Text("favorites")
                                .foregroundStyle(.white)
                        case 2:
                            BiographyView(text: user?.biography ?? "")
                                .foregroundColor(.white)
                        default:
                            EmptyView()
                        }
                    }
                    .padding(.top, 16)
                    
                }
                .padding(.horizontal, 24)
                
            }
        }
        .onAppear {
            Task {
                if let data = await profileVM.getLoggedInUser() {
                    self.user = data
                }
            }
        }
    }
}

#Preview {
    ProfileView()
}
