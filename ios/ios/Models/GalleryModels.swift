//
//  GalleryModels.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import Foundation

struct Gallery : Codable, Identifiable, Searchable {
    let id: Int
    let name: String
    let address: String?
    let cityId: Int
    let cityName: String?
}
