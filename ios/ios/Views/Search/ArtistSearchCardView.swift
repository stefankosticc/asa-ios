//
//  ArtistSearchCardView.swift
//  ios
//
//  Created by stefan on 9.10.25..
//

import SwiftUI

struct ArtistSearchCardView: View {
    let artist: UserSearchResponse
    
    var body: some View {
        NavigationLink(destination: ProfileView(of: artist)) {
            HStack {
                ProfilePhoto(url: "/api/user/\(artist.id)/profile-photo", size: 70)
                    .padding(.horizontal)
                    .padding(.vertical, 10)
                
                VStack(alignment: .leading, spacing: 10) {
                    Text(artist.name)
                        .lineLimit(2)
                    Text("@\(artist.userName)")
                        .lineLimit(1)
                        .foregroundStyle(.cGrayLight)
                }
                
                Spacer()
                
            }
            .background(
                RoundedRectangle(cornerRadius: 10)
                    .foregroundStyle(.cBlackHighlight)
            )
            .font(.subheadline)
        }
    }
}

#Preview {
    SearchView()
}
