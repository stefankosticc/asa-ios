//
//  ArtworkView.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import SwiftUI
import Kingfisher
import AlertToast

struct ArtworkView: View {
    @StateObject private var artworkVM = ArtworkViewModel()
    @StateObject private var profileVM = ProfileViewModel()
    @StateObject private var artworkAlertVM = AlertViewModel()
    @Environment(\.dismiss) private var dismiss
    
    var artwork: (any Searchable)?
    var isNew: Bool = false
    
    @State private var artworkRequest = ArtworkRequest(
        title: "",
        story: "",
        date: Date().formatted(date: .complete, time: .omitted),
        tipsAndTricks: "",
        isPrivate: false,
        createdByArtistId: 0,
        postedByUserId: 0,
        cityId: nil,
        galleryId: nil,
        color: nil
    )
    
    private func saveArtwork() async {
        if artworkRequest.story.isEmpty {
            artworkRequest.story = "<p></p>"
        }
        
        if artworkRequest.tipsAndTricks.isEmpty {
            artworkRequest.tipsAndTricks = "<p></p>"
        }
        
        if isNew {
            await addNewArtwork()
        } else if let existingArtwork = artworkVM.artwork {
            await updateExistingArtwork(existingArtwork)
        }
    }
    
    private func addNewArtwork() async {
        if artworkVM.selectedImage == nil {
            artworkAlertVM.alertToast = AlertToast(displayMode: .hud, type: .error(.red), title: "Image is not selected!")
            return
        }
        artworkRequest.postedByUserId = profileVM.loggedInUser?.id ?? -1
        artworkRequest.createdByArtistId = profileVM.loggedInUser?.id ?? -1
        
        let imageData = artworkVM.selectedImage?.jpegData(compressionQuality: 1)
        
        if let imageData, await artworkVM.addNewArtwork(data: artworkRequest, artworkImage: imageData) {
            artworkRequest.reset()
            artworkVM.selectedImage = nil
            artworkAlertVM.alertToast = AlertToast(type: .complete(.green), title: "Artwork added")
        } else if let error = artworkVM.errorMessage {
            artworkAlertVM.alertToast = AlertToast(displayMode: .hud, type: .error(.red), title: error)
        }
    }
    
    private func updateExistingArtwork(_ existingArtwork: Artwork) async {
        artworkRequest.postedByUserId = existingArtwork.postedByUserId
        artworkRequest.createdByArtistId = existingArtwork.createdByArtistId
        artworkRequest.date = existingArtwork.date
        
        let imageData = artworkVM.selectedImage?.jpegData(compressionQuality: 1)
        
        if await artworkVM.updateArtwork(artworkId: existingArtwork.id, data: artworkRequest, artworkImage: imageData) {
            // image and cache handling
            if imageData != nil {
                let key = "\(Constants.BACKEND_URL)\(existingArtwork.image)"
                do {
                    try await KingfisherManager.shared.cache.removeImage(forKey: key)
                } catch {
                    print("Error removing cached artwork image after updating")
                }
            }
            
            // refetch artwork
            if let data = await artworkVM.getArtwork(artworkId: existingArtwork.id) {
                artworkVM.artwork = data
            }
            
            artworkVM.isEditing = false
        } else if let error = artworkVM.errorMessage {
            artworkAlertVM.alertToast = AlertToast(displayMode: .hud, type: .error(.red), title: error)
        }
    }
    
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
                        ArtworkImageContainerView(
                            artwork: artworkVM.artwork,
                            geo: geo,
                            isNew: isNew,
                            artworkRequest: $artworkRequest,
                            artworkVM: artworkVM,
                            selectedImage: $artworkVM.selectedImage
                        )
                        
                        VStack(alignment: .leading, spacing: 20) {
                            // MARK: - Title and Actions
                            ArtworkHeaderView(artworkVM: artworkVM, isNew: isNew, loggedInUserId: profileVM.loggedInUser?.id, artworkRequest: $artworkRequest)
                            
                            // MARK: - Artwork info
                            HStack(alignment: .top, spacing: 10) {
                                VStack(alignment: .leading) {
                                    Text("POSTED BY")
                                        .foregroundStyle(.cGrayLight)
                                    if isNew, let user = profileVM.loggedInUser {
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
                                    if isNew, let user = profileVM.loggedInUser {
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
                            if isNew || artworkVM.isEditing {
                                Text("Story")
                                    .font(.title3)
                                    .bold()
                                
                                TextField("", text: Binding(
                                    get: {
                                        artworkRequest.story == "<p></p>" ? "" : artworkRequest.story
                                    },
                                    set: { newValue in
                                        artworkRequest.story = newValue
                                    }
                                ), axis: .vertical)
                                    .textFieldStyle(ArtworkEditTextFieldStyle(minLineLimit: 5, maxLineLimit: 14))
                            } else if let story = artworkVM.artwork?.story, story != "<p></p>" {
                                Text("Story")
                                    .font(.title3)
                                    .bold()
                                Text(AttributedString.fromHTML(story))
                            }
                            
                            if isNew || artworkVM.isEditing {
                                Text("Tips & Tricks")
                                    .font(.title3)
                                    .bold()
                                
                                TextField("", text: Binding(
                                    get: {
                                        artworkRequest.tipsAndTricks == "<p></p>" ? "" : artworkRequest.tipsAndTricks
                                    },
                                    set: { newValue in
                                        artworkRequest.tipsAndTricks = newValue
                                    }
                                ), axis: .vertical)
                                    .textFieldStyle(ArtworkEditTextFieldStyle(minLineLimit: 5, maxLineLimit: 14))
                            } else if let tips = artworkVM.artwork?.tipsAndTricks, tips != "<p></p>" {
                                Text("Tips & Tricks")
                                    .font(.title3)
                                    .bold()
                                Text(AttributedString.fromHTML(tips))
                            }
                            
                            if isNew || artworkVM.isEditing {
                                HStack {
                                    Spacer()
                                    
                                    Button(action: {
                                        if isNew {
                                            artworkRequest.reset()
                                        } else if artworkVM.isEditing {
                                            artworkVM.isEditing = false
                                            artworkRequest.color = nil
                                        }
                                        artworkVM.selectedImage = nil
                                    }, label: {
                                        Text("Cancel")
                                            .foregroundStyle(.black)
                                            .padding(.vertical, 8)
                                            .padding(.horizontal, 16)
                                            .font(.subheadline)
                                            .bold()
                                            .frame(width: 82)
                                            .background(RoundedRectangle(cornerRadius: 6).foregroundStyle(.cGrayLight))
                                    })
                                    
                                    Button(action: {
                                        Task {
                                            await saveArtwork()
                                        }
                                    }, label: {
                                        Text("Save")
                                            .foregroundStyle(.black)
                                            .padding(.vertical, 8)
                                            .padding(.horizontal, 16)
                                            .font(.subheadline)
                                            .bold()
                                            .frame(minWidth: 82)
                                            .background(RoundedRectangle(cornerRadius: 6).foregroundStyle(.cPurple))
                                    })
                                }
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
        .environmentObject(artworkAlertVM)
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
                    profileVM.loggedInUser = user
                }
            }
        }
        .toast(isPresenting: $artworkAlertVM.show){
            artworkAlertVM.alertToast
        }
    }
}

#Preview {
    ArtworkView()
}
