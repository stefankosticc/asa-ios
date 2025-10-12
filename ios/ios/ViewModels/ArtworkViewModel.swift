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
}
