//
//  ArtworkDiscoverCardView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkDiscoverCardView: View {
    @State var artwork: any DiscoverArtworkProtocol
    var width: CGFloat? = nil
    var height: CGFloat = 290
    var disableHorizontalPadding: Bool = false
    
    var body: some View {
        NavigationLink(destination: ArtworkView(artwork: artwork)) {
            VStack(spacing: 0) {
                KFImage(URL(string: "\(Constants.BACKEND_URL)\(artwork.image)"))
                    .placeholder {
                        ProgressView()
                    }
                    .resizable()
                    .scaledToFill()
                    .frame(width: width)
                    .frame(minHeight: height, maxHeight: height)
                    .clipped()
                
                
                HStack(spacing: 6) {
                    Text(artwork.title)
                        .foregroundStyle(.white)
                        .lineLimit(2)
                        .multilineTextAlignment(.leading)
                    
                    Spacer()
                    
                    Text("@\(artwork.postedByUserName)")
                        .foregroundStyle(.cGrayLight)
                        .lineLimit(2)
                }
                .padding()
            }
            .frame(width: width)
            .background(Color.black)
            .clipShape(RoundedRectangle(cornerRadius: 14))
            .overlay(RoundedRectangle(cornerRadius: 14).stroke(Color.cGrayLight, lineWidth: 0.5))
            .padding(.horizontal, disableHorizontalPadding ? 0 : nil)
        }
    }
}

#Preview {
    MainTabView()
}
