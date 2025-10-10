//
//  SearchView.swift
//  ios
//
//  Created by stefan on 9.10.25..
//

import SwiftUI

struct SearchView: View {
    @StateObject private var searchVM = SearchViewModel()
    
    var body: some View {
        NavigationStack {
            ZStack {
                Color.black.ignoresSafeArea()
                
                // MARK: - Search bar
                VStack {
                    LabeledContent {
                        TextField(text: $searchVM.searchString, label: {
                            Text("Search")
                                .foregroundStyle(.cGrayLight)
                        })
                        .textInputAutocapitalization(.never)
                        .autocorrectionDisabled()
                    } label: {
                        Image(systemName: "magnifyingglass")
                            .foregroundStyle(searchVM.searchString.isEmpty ? Color.cGrayLight : .white)
                    }
                    .padding(.vertical, 10)
                    .padding(.horizontal, 24)
                    .foregroundColor(.white)
                    .background(
                        Capsule()
                            .foregroundStyle(.cBlackHighlight)
                    )
                    
                    // MARK: - Filters
                    ScrollView(.horizontal, showsIndicators: false) {
                        HStack {
                            ForEach(searchVM.filters) { filter in
                                HStack {
                                    filter.icon
                                    Text(filter.title)
                                }
                                .bold(searchVM.selectedFilter == filter.title)
                                .font(.subheadline)
                                .frame(height: 20)
                                .foregroundStyle(searchVM.selectedFilter == filter.title ? filter.color : .cGrayLight)
                                .padding(.horizontal)
                                .padding(.vertical, 8)
                                .background(
                                    Capsule()
                                        .foregroundStyle(searchVM.selectedFilter == filter.title ? filter.color.opacity(0.2) : .clear)
                                )
                                .overlay {
                                    Capsule()
                                        .strokeBorder(searchVM.selectedFilter == filter.title ? filter.color : .cGrayLight)
                                }
                                .onTapGesture {
                                    searchVM.selectedFilter = filter.title
                                }
                            }
                        }
                        .padding(.vertical, 4)
                        .padding(.leading, 8)
                    }
                   
                    // MARK: - Results
                    if searchVM.isLoading {
                        VStack {
                            Spacer()
                            ProgressView()
                                .tint(.cGrayLight)
                                .scaleEffect(1.5)
                            Spacer()
                        }
                    }
                    else if searchVM.isSearchFinished && searchVM.searchResults.isEmpty {
                        Spacer()
                        Text("No results found")
                            .foregroundStyle(.cGrayLight)
                            .font(.subheadline)
                        Spacer()
                    } else {
                        ScrollView(showsIndicators: false) {
                            LazyVStack {
                                switch searchVM.selectedFilter {
                                case "Artworks":
                                    if let artworks = searchVM.searchResults as? [ArtworkSearchResponse] {
                                        ForEach(artworks) { artwork in
                                            ArtworkSearchCardView(artwork: artwork)
                                        }
                                    }
                                case "Artists":
                                    if let artists = searchVM.searchResults as? [UserSearchResponse] {
                                        ForEach(artists) { artist in
                                            ArtistSearchCardView(artist: artist)
                                        }
                                    }
                                case "Cities":
                                    if let cities = searchVM.searchResults as? [City] {
                                        ForEach(cities) { city in
                                            SearchCardWithIconView(data: city, icon: "building.2.fill", iconColor: Color.cBlue)
                                        }
                                    }
                                case "Galleries":
                                    if let galleries = searchVM.searchResults as? [Gallery] {
                                        ForEach(galleries) { gallery in
                                            SearchCardWithIconView(data: gallery, icon: "building.columns.fill", iconColor: Color.cOrange)
                                        }
                                    }
                                default:
                                    EmptyView()
                                }
                            }
                        }
                    }
                }
            }
            .foregroundStyle(.white)
            .navigationTitle("Search")
            .navigationBarTitleDisplayMode(.inline)
            .preferredColorScheme(/*@START_MENU_TOKEN@*/.dark/*@END_MENU_TOKEN@*/)
            .padding()
            .tint(.cPurple)
        }
    }
}

#Preview {
    SearchView()
}
