//
//  ArtistDiscoverView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI
import Kingfisher

struct ArtistDiscoverView: View {
    @State var name: String = "Artist Name"
    @State var profilePhoto: URL? = URL(string: "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png")
    
    var body: some View {
        VStack(spacing: 10){
            KFImage(profilePhoto)
                .placeholder {
                    ProgressView()
                }
                .resizable()
                .scaledToFill()
                .frame(width: 90, height: 90)
                .clipShape(Circle())
//                .overlay(Circle().stroke(Color.gray.opacity(0.5), lineWidth: 2))
            
            Text(name)
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
