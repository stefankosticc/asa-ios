//
//  ArtworkHeaderView.swift
//  ios
//
//  Created by stefan on 11.10.25..
//

import SwiftUI

struct ArtworkHeaderView: View {
    @ObservedObject var artworkVM: ArtworkViewModel
    var isNew: Bool
    var loggedInUserId: Int?
    @Binding var artworkRequest: ArtworkRequest
    
    var body: some View {
        HStack(spacing: 10) {
            if artworkVM.isEditing || isNew {
                TextField("Artwork Title", text: $artworkRequest.title, axis: .vertical)
                    .lineLimit(1...6)
                    .font(.title)
                    .bold()
                    .autocorrectionDisabled()
                    .tint(.cPurple)
                    .padding(.horizontal, 6)
                    .padding(.vertical, 4)
                    .background(
                        RoundedRectangle(cornerRadius: 6)
                            .fill(.clear)
                            .stroke(.cGray)
                    )
            } else {
                Text(artworkVM.artwork?.title ?? "-")
                    .lineLimit(4)
                    .font(.title)
                    .bold()
            }
            
            Spacer()
            
            if !isNew {
                Button(action: {
                    artworkVM.isLiked.toggle()
                }, label: {
                    Image(systemName: artworkVM.isLiked ? "heart.fill" : "heart")
                        .font(.title3)
                        .foregroundStyle(artworkVM.isLiked ? .cRed : .cGrayLight)
                })
            }
            
            if artworkVM.isOwnArtwork || isNew {
                Button(action: {
                    artworkVM.isPrivate.toggle()
                    artworkRequest.isPrivate.toggle()
                }, label: {
                    Image(systemName: artworkVM.isPrivate ? "lock" : "lock.open")
                        .font(.title3)
                        .foregroundStyle(.cGrayLight)
                })
                
                if !isNew {
                    Button {
                        artworkVM.isEditing.toggle()
                        if let artworkDataToEdit = artworkVM.artwork {
                            artworkRequest = ArtworkRequest(from: artworkDataToEdit)
                        }
                    } label: {
                        Image(systemName: "pencil")
                            .font(.title3)
                            .foregroundStyle(.cGrayLight)
                    }
                    
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
                if loggedInUserId != nil {
                    artworkVM.checkOwnership(for: loggedInUserId!)
                }
            }
        }
    }
}

#Preview {
    ArtworkView(isNew: true)
}
