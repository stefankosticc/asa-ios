//
//  DiscoverModels.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import Foundation

struct TopArtistResponse: Codable, Identifiable {
    let id: Int
    let name: String
    let userName: String
    let profilePhoto: String
}

struct HighStakesAuctionResponse: Codable, Identifiable {
    let auctionId: Int
    let artworkId: Int
    let artworkTitle: String
    let currentPrice: Double
    let offerCount: Int
    let currency: Currency
    
    var id: Int { auctionId }
}

struct DiscoverArtworkResponse: Codable, Identifiable {
    let id: Int
    let title: String
    let image: String
    let postedByUserName: String
}

struct DiscoverData: Codable {
    let topArtistsByLikes: [TopArtistResponse]
    let highStakeAuctions: [HighStakesAuctionResponse]
    let trendingArtworks: [DiscoverArtworkResponse]
}
