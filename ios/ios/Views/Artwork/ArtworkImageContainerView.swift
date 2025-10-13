//
//  ArtworkImageContainerView.swift
//  ios
//
//  Created by stefan on 11.10.25..
//

import SwiftUI
import PhotosUI

struct ArtworkImageContainerView: View {
    var artwork: Artwork?
    let geo: GeometryProxy
    let isNew: Bool
    @Binding var artworkRequest: ArtworkRequest
    @ObservedObject var artworkVM: ArtworkViewModel
    
    @Binding var selectedImage: UIImage?
    @State private var selectedItem: PhotosPickerItem? = nil
    @State private var selectedColor: Color = .cBlue
    
    private func showArtworkBgColor() -> Color {
        if (artworkRequest.color != nil) {
            return Color(hex: (artworkRequest.color!))
        } else if let color = artwork?.color, selectedColor != .clear {
            return Color(hex: color)
        } else {
            return selectedColor
        }
    }
    
    var body: some View {
        ZStack {
            RadialGradient(
                gradient: Gradient(colors: [
                    .black,
                    showArtworkBgColor()
                ]),
                center: .center,
                startRadius: 50,
                endRadius: geo.size.height * 0.6
            )
            .clipShape(RoundedRectangle(cornerRadius: 30))
            .ignoresSafeArea()
            
            VStack {
                if let selectedImage {
                    Image(uiImage: selectedImage)
                        .resizable()
                        .scaledToFill()
                        .frame(
                            minWidth: geo.size.width * 0.6,
                            maxWidth: geo.size.width * 0.8,
                            maxHeight: geo.size.height * 0.7
                        )
                        .clipShape(RoundedRectangle(cornerRadius: 14))
                        .padding(.horizontal, 24)
                        .padding(.top, 80)
                        .padding(.bottom, 50)
                        .clipped()
                        .shadow(color: Color.cGray.opacity(0.15), radius: 10, x: 0, y: 0)
                        .shadow(color: Color.cGray.opacity(0.3), radius: 20, x: 0, y: 0)
                        .shadow(color: Color.cGray.opacity(0.15), radius: 35, x: 0, y: 0)
                } else if let art = artwork {
                    ArtworkImageView(url: art.image)
                        .frame(
                            minWidth: geo.size.width * 0.6,
                            maxWidth: geo.size.width * 0.8,
                            maxHeight: geo.size.height * 0.7
                        )
                        .clipShape(RoundedRectangle(cornerRadius: 14))
                        .padding(.horizontal, 24)
                        .padding(.top, 80)
                        .padding(.bottom, 50)
                        .clipped()
                        .shadow(color: Color.cGray.opacity(0.15), radius: 10, x: 0, y: 0)
                        .shadow(color: Color.cGray.opacity(0.3), radius: 20, x: 0, y: 0)
                        .shadow(color: Color.cGray.opacity(0.15), radius: 35, x: 0, y: 0)
                } else {
                    ZStack {
                        RoundedRectangle(cornerRadius: 14)
                            .foregroundStyle(.cGray)
                            .frame(width: 300, height: 400)
                            .padding(.horizontal, 24)
                            .padding(.top, 80)
                            .padding(.bottom, 50)
                            .shadow(color: Color.cGray.opacity(0.15), radius: 10, x: 0, y: 0)
                            .shadow(color: Color.cGray.opacity(0.3), radius: 20, x: 0, y: 0)
                            .shadow(color: Color.cGray.opacity(0.15), radius: 35, x: 0, y: 0)
                        
                        PhotosPicker(selection: $selectedItem, matching: .images) {
                            Image(systemName: "square.and.arrow.up")
                                .font(.largeTitle)
                                .foregroundStyle(.white)
                        }
                    }
                }
                
                if artworkVM.isEditing || isNew {
                    HStack {
                        RoundedRectangle(cornerRadius: 6)
                            .frame(width: 30, height: 30)
                            .foregroundStyle(.cGrayLight)
                            .overlay {
                                RoundedRectangle(cornerRadius: 6)
                                    .foregroundStyle((artworkRequest.color == "5C5C5C") ? .cBlue : Color(hex: artworkRequest.color ?? "1D90FF"))
                                    .padding(2)
                                ColorPicker("Color", selection: $selectedColor, supportsOpacity: false)
                                    .labelsHidden()
                                    .opacity(0.014)
                            }
                        
                        Button {
                            selectedColor = .clear
                        } label: {
                            Text("Clear Color")
                                .font(.footnote)
                                .padding(.horizontal, 6)
                                .padding(.vertical, 4)
                                .background(
                                    Capsule().foregroundStyle(.cGrayLight)
                                )
                        }
                        
                        Button {
                            selectedColor = Color(hex: artwork?.color ?? "5C5C5C")
                        } label: {
                            Text("Revert Color")
                                .font(.footnote)
                                .padding(.horizontal, 6)
                                .padding(.vertical, 4)
                                .background(
                                    Capsule().foregroundStyle(.cGrayLight)
                                )
                        }
                        
                        PhotosPicker(selection: $selectedItem, matching: .images) {
                            Image(systemName: "square.and.arrow.up.circle.fill")
                                .font(.title)
                                .symbolRenderingMode(.palette)
                                .foregroundStyle(Color.white, Color.cGrayLight)
                        }
                        .onChange(of: selectedItem, { oldValue, newItem in
                            Task {
                                if let data = try? await newItem?.loadTransferable(type: Data.self),
                                   let uiImage = UIImage(data: data) {
                                    selectedImage = uiImage
                                    if let extractedColor = await artworkVM.extractArtworkColor(imageData: data) {
                                        artworkRequest.color = extractedColor
                                    }
                                    
                                }
                            }
                        })
                        
                    }
                    .padding(.top, -40)
                    .onChange(of: selectedColor) {
                        if selectedColor == .clear {
                            artworkRequest.color = nil
                        } else {
                            artworkRequest.color = selectedColor.toHexString()
                        }
                    }
                    .onAppear {
                        artworkRequest.color = artwork?.color ?? "5C5C5C"
                    }
                }
            }
        }
    }
}

#Preview {
    ArtworkView()
}
