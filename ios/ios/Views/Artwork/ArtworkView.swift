//
//  ArtworkView.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkView: View {
    @StateObject private var artworkVM = ArtworkViewModel()
    @StateObject private var profileVM = ProfileViewModel()
    @Environment(\.dismiss) private var dismiss
    
    var artwork: (any Searchable)?
    var isNew: Bool = false
    
    @State private var isEditing: Bool = false
    @State private var loggedInUser: User? = nil

    
    var body: some View {
        ZStack {
            Color.black.ignoresSafeArea()
            
            if artworkVM.isLoading || profileVM.isLoading {
                ProgressView()
                    .tint(.cGrayLight)
                    .scaleEffect(1.5)
            } else {
                GeometryReader { geo in
                    ScrollView() {
                        // MARK: - Artwork image
                        ArtworkImageContainerView(artwork: artworkVM.artwork, geo: geo)
                        
                        VStack(alignment: .leading, spacing: 20) {
                            // MARK: - Title and Actions
                            ArtworkHeaderView(artworkVM: artworkVM, isNew: isNew, loggedInUserId: loggedInUser?.id)
                            
                            // MARK: - Artwork info
                            HStack(alignment: .top, spacing: 10) {
                                VStack(alignment: .leading) {
                                    Text("POSTED BY")
                                        .foregroundStyle(.cGrayLight)
                                    if isNew, let user = loggedInUser {
                                        HStack {
                                            ProfilePhoto(url: "/api/user/\(user.id)/profile-photo", size: 20)
                                            Text("@\(user.userName)")
                                                .foregroundStyle(.cGrayLight)
                                                .lineLimit(2)
                                        }
                                    } else {
                                        HStack {
                                            ProfilePhoto(url: "/api/user/\(artworkVM.artwork?.postedByUserId ?? -1)/profile-photo", size: 20)
                                            Text("@\(artworkVM.artwork?.postedByUserName ?? "-")")
                                                .foregroundStyle(.cGrayLight)
                                                .lineLimit(2)
                                        }
                                    }
                                }
                                
                                Spacer()
                                
                                VStack(alignment: .leading) {
                                    Text("CREATED BY")
                                        .foregroundStyle(.cGrayLight)
                                    if isNew, let user = loggedInUser {
                                        HStack {
                                            ProfilePhoto(url: "/api/user/\(user.id)/profile-photo", size: 20)
                                            Text("@\(user.userName)")
                                                .foregroundStyle(.cGrayLight)
                                                .lineLimit(2)
                                        }
                                    } else {
                                        HStack {
                                            ProfilePhoto(url: "/api/user/\(artworkVM.artwork?.createdByArtistId ?? -1)/profile-photo", size: 20)
                                            Text("@\(artworkVM.artwork?.createdByArtistUserName ?? "-")")
                                                .foregroundStyle(.cGrayLight)
                                                .lineLimit(2)
                                        }
                                    }
                                }
                            }
                            .padding(.bottom, 4)
                            .font(.subheadline)
                            
                            VStack(alignment: .leading) {
                                Text("DATE")
                                    .foregroundStyle(.cGrayLight)
                                Text(isNew
                                     ? Formatter.formatDate(Date.now)
                                     : artworkVM.artwork?.date ?? "-")
                                    .foregroundStyle(.cGrayLight)
                            }
                            .font(.subheadline)
                            
                            // MARK: - Artwork text
                            if let story = artworkVM.artwork?.story, story != "<p></p>" {
                                Text("Story")
                                    .font(.title3)
                                    .bold()
                                Text(AttributedString.fromHTML(story))
                            }
                            
                            if let tips = artworkVM.artwork?.tipsAndTricks, tips != "<p></p>" {
                                Text("Tips & Tricks")
                                    .font(.title3)
                                    .bold()
                                Text(AttributedString.fromHTML(tips))
                            }
                            
                        }
                        .frame(maxWidth: .infinity, alignment: .leading)
                        .padding()
                        
                    }
                    .scrollIndicators(.never)
                }
                .ignoresSafeArea(edges: .top)
            }
        }
        .foregroundStyle(.white)
        .toolbarBackground(.hidden, for: .navigationBar)
        .navigationBarBackButtonHidden()
        .toolbar(content: {
            ToolbarItem(placement: .topBarLeading) {
                Button(action: { dismiss() }, label: {
                    Image(systemName: "chevron.backward")
                        .foregroundStyle(.white)
                        .font(.title3)
                })
            }
        })
        .onAppear {
            Task {
                if let artworkId = artwork?.id, !isNew {
                    if let data = await artworkVM.getArtwork(artworkId: artworkId) {
                        artworkVM.artwork = data
                    }
                }
                if let user = await profileVM.getLoggedInUser(){
                    self.loggedInUser = user
                }
            }
        }
    }
}

#Preview {
    ArtworkView()
}
