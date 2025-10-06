//
//  ArtistDiscoverView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI
import Kingfisher
import SVGKit

struct ArtistDiscoverView: View {
    @State var artist: TopArtistResponse
    @State private var loadFailed = false
    
    var body: some View {
        VStack(spacing: 10){
            if loadFailed {
                KFImage(URL(string: Constants.ARTIST_FALLBACK_IMAGE))
                    .resizable()
                    .scaledToFill()
                    .frame(width: 90, height: 90)
                    .clipShape(Circle())
            } else {
                KFImage(URL(string: "\(Constants.BACKEND_URL)\(artist.profilePhoto)"))
                    .setProcessor(SVGProcessor())
                    .placeholder {
                        ProgressView()
                    }
                    .onFailure { _ in
                        loadFailed = true
                    }
                    .resizable()
                    .scaledToFill()
                    .frame(width: 90, height: 90)
                    .clipShape(Circle())
                    .clipped()
                //  .overlay(Circle().stroke(Color.gray.opacity(0.5), lineWidth: 2))
            }
            
            
            Text(artist.name)
                .font(.subheadline)
                .lineLimit(2)
                .multilineTextAlignment(.center)
        }
        .frame(width: 110)
    }
}

#Preview {
    DiscoverView()
}
