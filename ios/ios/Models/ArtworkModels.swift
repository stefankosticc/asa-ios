//
//  ArtworkModels.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation

struct ArtworkCardData : Codable, Identifiable {
    let id: Int
    let title: String
    let image: String
    let isPrivate: Bool
    let postedByUserId: Int
    let postedByUserName: String
    let date: String
}

struct UserArtworksResponse : Codable {
    let privateArtworks: [ArtworkCardData]
    let publicArtworks: [ArtworkCardData]
}

struct FavoriteArtwork : Codable, Identifiable {
    let userId: Int
    let artworkId: Int
    let artworkTitle: String?
    let artworkImage: String?
    
    var id: Int { artworkId }
}

extension ArtworkCardData {
    init(fav: FavoriteArtwork) {
        self.id = fav.artworkId
        self.title = fav.artworkTitle ?? "Untitled"
        self.image = fav.artworkImage ?? "\(Constants.ARTWORK_FALLBACK_IMAGE)"
        self.isPrivate = false
        self.postedByUserId = fav.userId
        self.postedByUserName = ""
        self.date = ""
    }
    
    static func fromFavorites(_ favs: [FavoriteArtwork]) -> [ArtworkCardData] {
        favs.map { ArtworkCardData(fav: $0) }
    }
}

