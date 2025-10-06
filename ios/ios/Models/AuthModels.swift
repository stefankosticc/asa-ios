//
//  AuthModels.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import Foundation

struct LoginRequest: Codable {
    let email: String
    let password: String
}

struct LoginResponse: Codable {
    let accessToken: String
    let refreshToken: String
}

struct SignUpRequest: Codable {
    let name: String
    let email: String
    let userName: String
    let password: String
}
