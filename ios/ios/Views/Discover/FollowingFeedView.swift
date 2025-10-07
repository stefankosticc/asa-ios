//
//  FollowingFeedView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI

struct FollowingFeedView: View {
    @StateObject private var authViewModel = AuthViewModel()
    @StateObject private var discoverVM = DiscoverViewModel()
    
    var body: some View {
        VStack(alignment: .leading) {
            Text("Following")
                .font(.title)
                .bold()
                .padding(.leading, 24)
                .padding(.vertical)
            
            LazyVStack(spacing: 20) {
                ForEach(discoverVM.followedArtworksItems) { artwork in
                    ArtworkDiscoverCardView(artwork: artwork)
                        .onAppear {
                            if artwork.id == discoverVM.followedArtworks.items.last?.id {
                                Task { await discoverVM.followedArtworks.loadMore() }
                            }
                        }
                }
                
                if discoverVM.followedArtworks.isLoading || discoverVM.followedArtworksItems.isEmpty {
                    ProgressView()
                        .padding()
                }
            }
            .onAppear {
                Task { await discoverVM.followedArtworks.loadMore() }
            }
        }
    }
}

#Preview {
    MainTabView()
}
