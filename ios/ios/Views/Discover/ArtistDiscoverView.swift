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
    
    var body: some View {
        VStack(spacing: 10){
            ProfilePhoto(url: artist.profilePhoto, size: 90)
            
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
