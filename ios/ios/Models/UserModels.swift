//
//  UserModels.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import Foundation

struct User: Codable, Identifiable {
    let id: Int
    let name: String
    let email: String
    let userName: String
    let biography: String?
    let roleId: Int
    let roleName: String?
    let followersCount: Int
    let followingCount: Int
    let profilePhoto: String?
    let isFollowedByLoggedInUser: Bool?
}

struct UpdateUserBiographyRequest: Codable {
    let biography: String
}

struct UserSearchResponse: Codable, Identifiable {
    let id: Int
    let name: String
    let userName: String
    let profilePhoto: String
}

struct UpdateUserProfileRequest: Codable {
    let name: String
    let removePhoto: Bool
}
