//
//  AccountSettingsView.swift
//  ios
//
//  Created by stefan on 8.10.25..
//

import SwiftUI
import PhotosUI
import Kingfisher
import AlertToast

struct AccountSettingsView: View {
    @EnvironmentObject var profileVM: ProfileViewModel
    
    @State private var name: String = ""
    @State private var profilePhotoURL: String = ""
    
    @State private var removePhotoFlag: Bool = false
    @State private var selectedItem: PhotosPickerItem? = nil
    @State private var selectedItemData: Data? = nil
    @State private var selectedImage: UIImage? = nil
    
    @Environment(\.dismiss) private var dismiss
    @EnvironmentObject var profileAlertVM: AlertViewModel
    
    var body: some View {
        ZStack {
            Color.black.ignoresSafeArea()
            
            VStack(spacing: 20) {
                if let selectedImage {
                    Image(uiImage: selectedImage)
                        .resizable()
                        .scaledToFill()
                        .frame(width: 100, height: 100)
                        .clipShape(Circle())
                } else if profilePhotoURL.isEmpty {
                    ProfilePhoto(size: 100)
                } else {
                    ProfilePhoto(url: profilePhotoURL, size: 100)
                }
                
                HStack(spacing: 30) {
                    Button(action: {
                        selectedImage = nil
                        profilePhotoURL = ""
                        removePhotoFlag = true
                    }) {
                        Text("Remove")
                            .font(.subheadline)
                            .foregroundColor(selectedImage != nil || !profilePhotoURL.isEmpty ? .cRed : .cGrayLight)
                            .clipShape(RoundedRectangle(cornerRadius: 8))
                            .underline()
                    }
                    .disabled(selectedImage == nil && profilePhotoURL.isEmpty)
                    
                    PhotosPicker(selection: $selectedItem, matching: .images) {
                        Text("Choose Photo")
                            .foregroundColor(.cPurple)
                            .font(.subheadline)
                            .clipShape(RoundedRectangle(cornerRadius: 8))
                            .underline()
                    }
                }
         
            
            Form {
                Section("Account info") {
                    LabeledContent {
                        TextField(text: $name, label: {
                            Text("Name")
                                .foregroundStyle(.gray)
                        })
                        .textInputAutocapitalization(.words)
                    } label: {
                        Text("Name:")
                    }
                    
                    HStack {
                        Text("Username:")
                        Text("@\(profileVM.loggedInUser?.userName ?? "username")")
                            .foregroundStyle(.gray)
                    }
                    
                    HStack {
                        Text("Email:")
                        Text(profileVM.loggedInUser?.email ?? "Email")
                            .foregroundStyle(.gray)
                    }
                    
                }
                .listRowBackground(Color.cBlackHighlight)
                .foregroundStyle(.white)
                
                HStack {
                    Spacer()
                    Button(action: {
                        Task {
                            let updateProfileData = UpdateUserProfileRequest(name: name, removePhoto: removePhotoFlag)
                            let imageData = selectedImage?.jpegData(compressionQuality: 1)
                            
                            if await profileVM.updateUserProfile(data: updateProfileData, profilePhoto: imageData) {
                                
                                let key = "\(Constants.BACKEND_URL)\("/api/user/\(profileVM.loggedInUser?.id ?? -1)/profile-photo")"
                                do {
                                    // Remove old image from cache
                                    try await KingfisherManager.shared.cache.removeImage(forKey: key, processorIdentifier: SVGProcessor().identifier)
                                    
                                    // Refresh ProfileView Data
                                    if (profileVM.loggedInUser == profileVM.profileUser){
                                        if let data = await profileVM.getLoggedInUser() {
                                            profileVM.loggedInUser = data
                                            profileVM.profileUser = profileVM.loggedInUser
                                            
                                            // navigate to profile
                                            profileVM.showSettings = false
                                            profileAlertVM.alertToast = AlertToast(type: .complete(.green), title: "Updated", subTitle: nil)
                                        }
                                    }
                                } catch {
                                    print("Error removing cached image after updating profile")
                                }
                            }
                        }
                    }) {
                        Text("Save")
                            .font(.headline)
                            .padding(.horizontal, 20)
                            .padding(.vertical, 8)
                            .background(name.isEmpty ? Color.cPurpleLight : Color.cPurple)
                            .foregroundColor(.black)
                            .clipShape(RoundedRectangle(cornerRadius: 8))
                    }
                    .disabled(name.isEmpty)
                }
                .listRowBackground(Color.clear)


            }
            .scrollContentBackground(.hidden)
            }
            .onChange(of: selectedItem, { oldValue, newItem in
                Task {
                    if let data = try? await newItem?.loadTransferable(type: Data.self),
                       let uiImage = UIImage(data: data) {
                        selectedImage = uiImage
                        selectedItemData = data
                        removePhotoFlag = false
                    }
                }
            })
        }
        .navigationTitle("Account Settings")
        .navigationBarTitleDisplayMode(.inline)
        .onAppear {
            name  = profileVM.loggedInUser?.name  ?? ""
            profilePhotoURL = "/api/user/\(profileVM.loggedInUser?.id ?? -1)/profile-photo"
        }
    }
}

#Preview {
    AccountSettingsView()
        .environmentObject(ProfileViewModel())
}
