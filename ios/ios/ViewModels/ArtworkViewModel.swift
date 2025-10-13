//
//  ArtworkViewModel.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import Foundation

@MainActor
class ArtworkViewModel : ObservableObject {
    private let api : APIServiceProtocol
    @Published var errorMessage: String?
    @Published var isLoading: Bool = false
    
    @Published var artwork: Artwork? = nil
    @Published var isOwnArtwork: Bool = false
    @Published var isLiked: Bool = false
    @Published var isPrivate: Bool = false
    @Published var isEditing: Bool = false
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    // Checks if the logged in user is the one who posted the artwork
    func checkOwnership(for userId: Int) {
        if let artwork = artwork {
            isOwnArtwork = artwork.postedByUserId == userId
        }
    }
    
    func getArtwork(artworkId: Int) async -> Artwork? {
        do {
            isLoading = true
            let response: Artwork = try await api.get(endpoint: "artwork/\(artworkId)")
            
            self.isLiked = response.isLikedByLoggedInUser ?? false
            self.isPrivate = response.isPrivate
            
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
    
    func addNewArtwork(data: ArtworkRequest, artworkImage: Data) async -> Bool{
        if data.title.isEmpty {
            self.errorMessage = "Title is required."
            return false
        }
        
        if data.title.count > 100 {
            self.errorMessage = "Title must be under 100 characters."
            return false
        }
        
        do {
            try await api.postWithImage(endpoint: "artwork", body: data, image: artworkImage, imageFieldName: "artworkImage")
            return true
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return false
    }
    
    func updateArtwork(artworkId: Int, data: ArtworkRequest, artworkImage: Data? = nil) async -> Bool{
        do {
            try await api.putWithImage(endpoint: "artwork/\(artworkId)", body: data, image: artworkImage, imageFieldName: "artworkImage")
            return true
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return false
    }
    
    func extractArtworkColor(imageData: Data) async -> String? {
        do {
            let color: String? = try await api.postWithImage(endpoint: "artwork/extract-color", image: imageData, imageFieldName: "image", decodeAsText: true)
            return color
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return nil
    }
}
