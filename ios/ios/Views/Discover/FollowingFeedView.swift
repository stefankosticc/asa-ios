//
//  FollowingFeedView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI

struct FollowingFeedView: View {
    var body: some View {
        VStack(alignment: .leading){
            Text("Following")
                .font(.title)
                .bold()
                .padding(.leading, 24)
                .padding(.vertical)
            
            ScrollView
            {
                VStack(spacing: 20){
                    ArtworkDiscoverCardView()
                    ArtworkDiscoverCardView(artworkImage: URL(string: "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/Girl_with_a_Pearl_Earring_by_Johannes_Vermeer_1665_800x.jpg"))
                    ArtworkDiscoverCardView(artworkImage: URL(string: "https://www.minimastersart.com/cdn/shop/articles/Starry_Night_-_Vincent_Van_Gogh_1402x.png?v=1734545704"))
                }
            }
        }
    }
}

#Preview {
    MainTabView()
}
