//
//  ArtworkHeaderView.swift
//  ios
//
//  Created by stefan on 11.10.25..
//

import SwiftUI

struct ArtworkHeaderView: View {
    @State private var isLiked: Bool = false
    @State private var isPrivate: Bool = false
    
    @ObservedObject var artworkVM: ArtworkViewModel
    var isNew: Bool
    var loggedInUserId: Int?
    
    var body: some View {
        HStack(spacing: 10) {
            Text(artworkVM.artwork?.title ?? "-")
                .lineLimit(4)
                .font(.title)
                .bold()
            
            Spacer()
            
            if !isNew {
                Button(action: {
                    isLiked.toggle()
                }, label: {
                    Image(systemName: isLiked ? "heart.fill" : "heart")
                        .font(.title3)
                        .foregroundStyle(isLiked ? .cRed : .cGrayLight)
                })
            }
            
            if artworkVM.isOwnArtwork || isNew {
                Button(action: {
                    isPrivate.toggle()
                }, label: {
                    Image(systemName: isPrivate ? "lock" : "lock.open")
                        .font(.title3)
                        .foregroundStyle(.cGrayLight)
                })
                
                if !isNew {
                    Image(systemName: "pencil")
                        .font(.title3)
                        .foregroundStyle(.cGrayLight)
                    
                    Menu {
                        Button("Put On Sale") {}
                        Button("Remove From Sale") {}
                        Button("Auction Analytics") {}
                        Button("Transfer") {}
                        Button("Delete", role: .destructive) {}
                    } label: {
                        Image(systemName: "ellipsis")
                            .rotationEffect(.degrees(90))
                            .font(.title3)
                            .foregroundStyle(.cGrayLight)
                    }
                }
            }
        }
        .onAppear {
            Task {
                if let artwork = artworkVM.artwork, (loggedInUserId != nil) {
                    artworkVM.checkOwnership(for: loggedInUserId!)
                    self.isLiked = artwork.isLikedByLoggedInUser ?? false
                    self.isPrivate = artwork.isPrivate
                }
            }
        }
    }
}

#Preview {
    ArtworkView()
}
