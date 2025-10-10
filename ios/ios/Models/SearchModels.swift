//
//  SearchModels.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import Foundation

protocol Searchable: Identifiable {
    var id: Int { get }
}

struct ArtworkSearchResponse : Codable, Identifiable, Searchable {
    let id: Int
    let title: String
    let image: String
    let isOnSale: Bool
    let postedByUserId: Int
    let postedByUserName: String
    let cityId: Int?
    let cityName: String?
    let country: String?
    let galleryId: Int?
    let galleryName: String?
}

struct UserSearchResponse: Codable, Identifiable, Searchable {
    let id: Int
    let name: String
    let userName: String
    let profilePhoto: String
}
