//
//  ArtworkModels.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation

struct ArtworkCardData : Codable, Identifiable, Searchable {
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

struct Artwork : Codable {
    let id: Int
    let title: String
    let story: String
    let image: String
    let date: String
    let tipsAndTricks: String
    let isPrivate: Bool
    let isOnSale: Bool
    let price: Int?
    let currency: Currency
    let createdByArtistId: Int
    let createdByArtistUserName: String
    let postedByUserId: Int
    let postedByUserName: String
    let cityId: Int?
    let cityName: String?
    let galleryId: Int?
    let galleryName: String?
    let isLikedByLoggedInUser: Bool?
    let color: String?
}

struct ArtworkRequest : Codable {
    var title: String
    var story: String
    var date: String
    var tipsAndTricks: String
    var isPrivate: Bool
    var createdByArtistId: Int
    var postedByUserId: Int
    var cityId: Int?
    var galleryId: Int?
    var color: String?
}

extension ArtworkRequest {
    init(from artwork: Artwork) {
        self.title = artwork.title
        self.story = artwork.story
        self.date = artwork.date
        self.tipsAndTricks = artwork.tipsAndTricks
        self.isPrivate = artwork.isPrivate
        self.createdByArtistId = artwork.createdByArtistId
        self.postedByUserId = artwork.postedByUserId
        self.cityId = artwork.cityId
        self.galleryId = artwork.galleryId
        self.color = artwork.color
    }
}

struct ChangeArtworkVisibilityRequest : Codable {
    let isPrivate: Bool
}
