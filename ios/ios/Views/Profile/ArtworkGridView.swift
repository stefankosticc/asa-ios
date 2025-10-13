//
//  ArtworkGridView.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkCardView: View {
    var artwork: ArtworkCardData
    
    var body: some View {
        NavigationLink(destination: ArtworkView(artwork: artwork)) {
            ZStack(alignment: .bottomLeading) {
                GeometryReader { geo in
                    KFImage(URL(string: "\(Constants.BACKEND_URL)\(artwork.image)"))
                        .resizable()
                        .scaledToFill()
                        .frame(maxWidth: geo.size.width)
                        .clipShape(RoundedRectangle(cornerRadius: 12))
                        .clipped()
                }
                
                LinearGradient(
                    gradient: Gradient(colors: [Color.black.opacity(0.8), Color.clear]),
                    startPoint: .bottom,
                    endPoint: .top
                )
                .clipShape(RoundedRectangle(cornerRadius: 12))
                
                Text(artwork.title)
                    .foregroundStyle(.white)
                    .font(.subheadline)
                    .fontWeight(/*@START_MENU_TOKEN@*/.bold/*@END_MENU_TOKEN@*/)
                    .lineLimit(2)
                    .padding(.horizontal, 8)
                    .padding(.vertical, 10)
            }
            .frame(height: 200)
        }
    }
}

struct ArtworkGridView: View {
    var artworks: [ArtworkCardData]
    @State var showPrivateArtworksCard: Bool = false
    @Binding var showPrivateArtworks: Bool
    
    let columns = [
        GridItem(.flexible(), spacing: 2),
        GridItem(.flexible(), spacing: 2),
        GridItem(.flexible(), spacing: 2)
    ]
    
    var body: some View {
        ScrollView {
            LazyVGrid(columns: columns, spacing: 2) {
                if showPrivateArtworksCard {
                    RadialGradient(
                        gradient: Gradient(colors: [
                            Color(red: 0.031, green: 0.024, blue: 0.055),
                            Color(red: 0.235, green: 0.129, blue: 0.729),
                            Color(red: 0.674, green: 0.639, blue: 1.0)
                        ]),
                        center: .center,
                        startRadius: 0,
                        endRadius: 500
                    )
                    .frame(height: 200)
                    .clipShape(RoundedRectangle(cornerRadius: 12))
                    .overlay(
                        Image(systemName: showPrivateArtworks ? "lock.open" : "lock")
                            .font(.title2)
                            .foregroundStyle(.cPurple)
                    )
                    .onTapGesture {
                        showPrivateArtworks.toggle()
                    }
                }
                ForEach(artworks) { artwork in
                    ArtworkCardView(artwork: artwork)
                }
            }
        }
    }
}

#Preview {
    ProfileView()
}
