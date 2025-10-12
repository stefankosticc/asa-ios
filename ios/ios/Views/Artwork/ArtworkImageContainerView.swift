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
    @State var selectedImage: PhotosPickerItem?
    let geo: GeometryProxy
    
    var body: some View {
        ZStack {
            RadialGradient(
                gradient: Gradient(colors: [
                    .black,
                    Color(hex: artwork?.color ?? "5c5c5c")
                ]),
                center: .center,
                startRadius: 50,
                endRadius: geo.size.height * 0.6
            )
            .clipShape(RoundedRectangle(cornerRadius: 30))
            .ignoresSafeArea()
            
            if let art = artwork {
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
                    
                    PhotosPicker(selection: $selectedImage) {
                        Image(systemName: "square.and.arrow.up")
                            .font(.largeTitle)
                            .foregroundStyle(.white)
                    }
                }
            }
        }
    }
}

#Preview {
    ArtworkView()
}
