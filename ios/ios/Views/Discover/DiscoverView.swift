//
//  DiscoverView.swift
//  ios
//
//  Created by stefan on 3.10.25..
//

import SwiftUI

struct DiscoverView: View {
    @State private var selectedSegment = 0
    let segments = ["Discover", "Following"]
    
    var body: some View {
        ZStack{
            Color(.black)
                .ignoresSafeArea()
            
            ScrollView(showsIndicators: false) {
                // Discover and Following tab navigation
                VStack {
                    HStack(spacing: 28) {
                        ForEach(0..<segments.count, id: \.self) { index in
                            Button(action: {
                                withAnimation {
                                    selectedSegment = index
                                }
                            }) {
                                VStack {
                                    Text(segments[index])
                                        .font(.headline)
                                        .foregroundColor(selectedSegment == index ? .cPurple : .cGray)
                                    
                                    // underline
                                    Rectangle()
                                        .fill(selectedSegment == index ? Color.cPurple : Color.clear)
                                        .frame(width: 20 ,height: 1)
                                        .padding(.vertical, -6)
                                }
                            }
                            
                        }
                    }
                    .padding(.horizontal)
                    
                    if segments[selectedSegment] == "Discover" {
                        VStack(alignment: .leading, spacing: 20) {
                            DiscoverSectionView(title: "Top Artists 🧑‍🎨") {
                                ArtistDiscoverView()
                                ArtistDiscoverView(name: "Peter Parker")
                                ArtistDiscoverView(name: "Peter Parker")
                            }
                            
                            DiscoverSectionView(title: "High Stakes Auctions 🔥") {
                                HighStakesAuctionView(artworkTitle: "Self Portrait with Thorn Necklace and Hummingbird, 1940, By Frida")
                                HighStakesAuctionView(artworkTitle: "Girl with a Pearl Earring")
                                HighStakesAuctionView()
                            }
                            
                            DiscoverSectionView(title: "On The Rise ✨") {
                                ArtworkDiscoverCardView(width: 290, height: 200, disableHorizontalPadding: true)
                                ArtworkDiscoverCardView(artworkImage: URL(string: "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/Girl_with_a_Pearl_Earring_by_Johannes_Vermeer_1665_800x.jpg"), width: 290, height: 200, disableHorizontalPadding: true)
                                ArtworkDiscoverCardView(artworkImage: URL(string: "https://www.minimastersart.com/cdn/shop/articles/Starry_Night_-_Vincent_Van_Gogh_1402x.png?v=1734545704"), width: 290, height: 200, disableHorizontalPadding: true)
                            }
                            
                            VStack(alignment: .leading){
                                Text("Fresh Finds")
                                    .font(.headline)
                                    .padding(.leading, 24)
                                    .padding(.vertical)
                                
                                LazyVStack(spacing: 20){
                                    ArtworkDiscoverCardView()
                                    ArtworkDiscoverCardView(artworkImage: URL(string: "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/Girl_with_a_Pearl_Earring_by_Johannes_Vermeer_1665_800x.jpg"))
                                    ArtworkDiscoverCardView(artworkImage: URL(string: "https://www.minimastersart.com/cdn/shop/articles/Starry_Night_-_Vincent_Van_Gogh_1402x.png?v=1734545704"))
                                }
                            }
                            
                        }
                        .padding(.leading)
                    }
                    else if segments[selectedSegment] == "Following" {
                        FollowingFeedView()
                    }
                    
                    Spacer()
                }
            }
        }
        .foregroundStyle(.white)
    }
}

#Preview {
    MainTabView()
}
