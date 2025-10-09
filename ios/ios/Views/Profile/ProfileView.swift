//
//  ProfileView.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI

struct ProfileView: View {
    @State private var selectedTab = 0
    @StateObject private var profileVM = ProfileViewModel()
    @State private var showPrivateArtworks: Bool = false
    @State private var showSettings = false
    
    init() {
        UISegmentedControl.appearance().backgroundColor = UIColor.cBlackHighlight
        UISegmentedControl.appearance().setTitleTextAttributes([.foregroundColor: UIColor.black], for: .selected)
        UISegmentedControl.appearance().setTitleTextAttributes([.foregroundColor: UIColor.gray], for: .normal)
    }
    
    var body: some View {
        NavigationStack {
            ZStack{
                Color(.black)
                    .ignoresSafeArea()
                
                if profileVM.isLoading {
                    VStack(spacing: 16) {
                        ProgressView()
                            .tint(.cGrayLight)
                            .scaleEffect(1.5)
                    }
                } else if let u = profileVM.profileUser {
                    ScrollView(showsIndicators: false) {
                        HStack() {
                            Spacer()
                            Button (action: {
                                showSettings = true
                            }, label: {
                                Image(systemName: "gearshape")
                                    .font(.body)
                                    .foregroundStyle(.cGrayLight)
                                    .padding(.trailing, 10)
                            })
                        }
                        
                        HStack(spacing: 40) {
                            VStack(spacing: 20) {
                                ProfilePhoto(url: "/api/user/\(u.id)/profile-photo", size: 100)
                                VStack{
                                    Text(u.name)
                                        .foregroundStyle(.white)
                                        .bold()
                                    Text("@\(profileVM.profileUser?.userName ?? "")")
                                        .foregroundStyle(.cGrayLight)
                                        .font(.subheadline)
                                }
                            }
                            
                            Spacer()
                            
                            VStack(spacing: 20) {
                                VStack{
                                    Text("Followers")
                                        .foregroundStyle(.white)
                                    Text(Formatter.formatFollowCount(profileVM.profileUser?.followersCount))
                                        .foregroundStyle(.cGrayLight)
                                        .font(.subheadline)
                                }
                                
                                VStack{
                                    Text("Following")
                                        .foregroundStyle(.white)
                                    Text(Formatter.formatFollowCount(profileVM.profileUser?.followingCount))
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
                                ArtworkGridView(artworks: (showPrivateArtworks ? profileVM.artworks?.privateArtworks : profileVM.artworks?.publicArtworks) ?? [], showPrivateArtworksCard: profileVM.isOwnProfile, showPrivateArtworks: $showPrivateArtworks)
                                    .onAppear {
                                        Task {
                                            profileVM.artworks = await profileVM.getUserArtworks(for: u.id) ?? nil
                                        }
                                    }
                                
                            case 1:
                                // Favorites
                                ArtworkGridView(artworks: ArtworkCardData.fromFavorites(profileVM.favoriteArtworks ?? []), showPrivateArtworks: $showPrivateArtworks)
                                    .onAppear {
                                        Task {
                                            profileVM.favoriteArtworks = await profileVM.getFavoriteArtworks(for: u.id) ?? nil
                                        }
                                    }
                            case 2:
                                BiographyView(text: profileVM.profileUser?.biography ?? "")
                                    .foregroundColor(.white)
                            default:
                                EmptyView()
                            }
                        }
                        .padding(.top, 16)
                        
                    }
                    .padding(.horizontal, 14)
                    
                }
            }
            .onAppear {
                Task {
                    // if user is not passed to the ProfileView get the logged in user
                    if let data = await profileVM.getLoggedInUser() {
                        profileVM.loggedInUser = data
                    }
                    if profileVM.profileUser == nil {
                        profileVM.profileUser = profileVM.loggedInUser
                    }
                }
            }
            .sheet(isPresented: $showSettings) { SettingsView() }
            .environmentObject(profileVM)
        }
    }
}

#Preview {
    ProfileView()
}
