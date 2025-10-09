//
//  ProfileViewModel.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation

@MainActor
class ProfileViewModel : ObservableObject {
    private let api: APIServiceProtocol
    @Published var errorMessage: String?
    @Published var isLoading: Bool = false
    
    @Published var profileUser: User? = nil // User whose profile we're viewing
    @Published var loggedInUser: User? = nil
    
    @Published var artworks: UserArtworksResponse? = nil
    @Published var favoriteArtworks: [FavoriteArtwork]? = nil
    
    public var isOwnProfile: Bool {
        guard let me = loggedInUser, let viewing = profileUser else { return false }
        return me.id == viewing.id
    }
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    // Returns currently logged in user
    func getLoggedInUser() async -> User? {
        do {
            isLoading = true
            let response: User = try await api.get(endpoint: "auth/loggedin-user")
            isLoading = false
            return response
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        isLoading = false
        return nil
    }
    
    // Returns artworks posted by user with userId
    func getUserArtworks(for userId: Int) async -> UserArtworksResponse? {
        do {
            let response: UserArtworksResponse = try await api.get(endpoint: "user/\(userId)/artworks")
            return response
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return nil
    }
    
    // Returns liked artworks by user with userId
    func getFavoriteArtworks(for userId: Int) async -> [FavoriteArtwork]? {
        do {
            let response: [FavoriteArtwork] = try await api.get(endpoint: "user/\(userId)/liked-artworks")
            return response
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return nil
    }
    
    func updateUserProfile(data: UpdateUserProfileRequest, profilePhoto: Data? = nil) async -> Bool{
        do {
            try await api.putWithImage(endpoint: "user", body: data, image: profilePhoto, imageFieldName: "profilePhoto")
            return true
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return false
    }
}
