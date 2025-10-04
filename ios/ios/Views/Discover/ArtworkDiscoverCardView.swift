//
//  ArtworkDiscoverCardView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkDiscoverCardView: View {
//    let artworkFallbackImage: URL? = URL(string: "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500")
    var artworkImage: URL? = URL(string: "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/The_Scream_by_Edvard_Munch_1893_800x.png")
    
    var width: CGFloat = .infinity
    var height: CGFloat = 290
    var disableHorizontalPadding: Bool = false
    
    var body: some View {
        VStack(spacing: 0) {
            KFImage(artworkImage)
                .placeholder {
                    ProgressView()
                }
                .resizable()
                .scaledToFill()
                .frame(width: width)
                .frame( minHeight: height, maxHeight: height)
                .clipped()
            
            
            HStack(spacing: 6) {
                Text("Artwork Title")
                    .foregroundStyle(.white)
                    .lineLimit(2)
                
                Spacer()
                
                Text("@artist")
                    .foregroundStyle(.cGrayLight)
                    .lineLimit(2)
            }
            .padding()
        }
        .background(Color.black)
        .clipShape(RoundedRectangle(cornerRadius: 14))
        .overlay(RoundedRectangle(cornerRadius: 14).stroke(Color.cGrayLight, lineWidth: 0.5))
//        .shadow(color: Color.cGray.opacity(0.15), radius: 10, x: 0, y: 0)
//        .shadow(color: Color.cGray.opacity(0.3), radius: 20, x: 0, y: 0)
//        .shadow(color: Color.cGray.opacity(0.15), radius: 35, x: 0, y: 0)
        .padding(.horizontal, disableHorizontalPadding ? 0 : nil)
    }
}

#Preview {
    MainTabView()
}
