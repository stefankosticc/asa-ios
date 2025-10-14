//
//  ArtworkHeaderView.swift
//  ios
//
//  Created by stefan on 11.10.25..
//

import SwiftUI
import AlertToast

struct ArtworkHeaderView: View {
    @ObservedObject var artworkVM: ArtworkViewModel
    var isNew: Bool
    var loggedInUserId: Int?
    @Binding var artworkRequest: ArtworkRequest
    @Environment(\.dismiss) private var dismiss
    @State private var showDeleteAlert = false
    
    @EnvironmentObject var artworkAlertVM: AlertViewModel
    
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
                    Task {
                        if let artworkId = artworkVM.artwork?.id {
                            if artworkVM.isLiked {
                                await artworkVM.dislikeArtwork(artworkId: artworkId)
                            } else {
                                await artworkVM.likeArtwork(artworkId: artworkId)
                            }
                        }
                    }
                }, label: {
                    Image(systemName: artworkVM.isLiked ? "heart.fill" : "heart")
                        .font(.title3)
                        .foregroundStyle(artworkVM.isLiked ? .cRed : .cGrayLight)
                })
            }
            
            if artworkVM.isOwnArtwork || isNew {
                Button(action: {
                    Task {
                        if !isNew && !artworkVM.isEditing {
                            if let artworkId = artworkVM.artwork?.id {
                                await artworkVM.changeArtworkVisibility(artworkId: artworkId, makePrivate: !artworkVM.isPrivate)
                                artworkAlertVM.alertToast = AlertToast(displayMode: .hud, type: .systemImage(artworkVM.isPrivate ? "lock" : "lock.open", .white), title: artworkVM.isPrivate ? "Made private" : "Made public")
                            }
                        } else {
                            artworkVM.isPrivate.toggle()
                            artworkRequest.isPrivate.toggle()
                        }
                    }
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
                        if !artworkVM.isEditing {
                            artworkVM.selectedImage = nil
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
                        Button("Delete", role: .destructive) { showDeleteAlert = true }
                    } label: {
                        Image(systemName: "ellipsis")
                            .rotationEffect(.degrees(90))
                            .font(.title3)
                            .foregroundStyle(.cGrayLight)
                    }
                    .alert("Are you sure you want to delete this artwork?", isPresented: $showDeleteAlert) {
                        Button("Delete", role: .destructive) {
                            Task {
                                if let artworkId = artworkVM.artwork?.id {
                                    let success = await artworkVM.deleteArtwork(artworkId: artworkId)
                                    if success {
                                        dismiss()
                                    }
                                }
                            }
                        }
                        Button("Cancel", role: .cancel) { }
                    } message: {
                        Text("This cannot be undone!")
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
