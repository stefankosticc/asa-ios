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
    
    @StateObject private var discoverVM = DiscoverViewModel()
    @State private var discoverData: DiscoverData? = nil
//    @State private var discoverArtworks: [DiscoverArtworkResponse]? = nil
    
    
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
                                ForEach(discoverData?.topArtistsByLikes ?? []) {artist in
                                    ArtistDiscoverView(artist: artist)
                                }
                            }
                            
                            DiscoverSectionView(title: "High Stakes Auctions 🔥") {
                                ForEach(discoverData?.highStakeAuctions ?? []) {auction in
                                    HighStakesAuctionView(auction: auction)
                                }
                            }
                            
                            DiscoverSectionView(title: "On The Rise ✨") {
                                ForEach(discoverData?.trendingArtworks ?? []) {artwork in
                                    ArtworkDiscoverCardView(artwork: artwork, width: 290, height: 200, disableHorizontalPadding: true)
                                }
                            }
                            
                            VStack(alignment: .leading){
                                Text("Fresh Finds")
                                    .font(.headline)
                                    .padding(.leading, 24)
                                    .padding(.vertical)
                                
                                LazyVStack(spacing: 20){
                                    ForEach(discoverVM.discoverArtworksItems) { artwork in
                                        ArtworkDiscoverCardView(artwork: artwork)
                                            .onAppear {
                                                if artwork.id == discoverVM.discoverArtworks.items.last?.id {
                                                    Task { await discoverVM.discoverArtworks.loadMore() }
                                                }
                                            }
                                    }
                                    
                                    if discoverVM.discoverArtworks.isLoading {
                                        ProgressView()
                                            .padding()
                                    }
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
            .onAppear {
                Task {
                    if let data = await discoverVM.getDiscoverData() {
                        self.discoverData = data
                    }
                    await discoverVM.discoverArtworks.loadMore()
                }
            }
        }
        .foregroundStyle(.white)
    }
}

#Preview {
    DiscoverView()
}
