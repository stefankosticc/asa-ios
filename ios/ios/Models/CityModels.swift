//
//  CityModels.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import Foundation

struct City : Codable, Identifiable, Searchable {
    let id: Int
    let name: String
    let country: String?
}
