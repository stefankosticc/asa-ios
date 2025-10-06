//
//  DiscoverViewModel.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import Foundation

@MainActor
class DiscoverViewModel : ObservableObject {
    private let api: APIServiceProtocol
    
    let followedArtworks: InfiniteScroll<FollowedUserArtworkResponse>
    @Published var followedArtworksItems: [FollowedUserArtworkResponse] = []
    
    let discoverArtworks: InfiniteScroll<DiscoverArtworkResponse>
    @Published var discoverArtworksItems: [DiscoverArtworkResponse] = []
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
        
        self.followedArtworks = InfiniteScroll<FollowedUserArtworkResponse>(api: api, take: 5) { skip, take in
            "followed-users/artworks?skip=\(skip)&take=\(take)"
        }
        
        self.discoverArtworks = InfiniteScroll<DiscoverArtworkResponse>(api: api, take: 4) { skip, take in
            "artworks/discover?skip=\(skip)&take=\(take)"
        }
        
        Task { [weak self] in
            guard let self = self else { return }
            for await _ in self.followedArtworks.$items.values {
                self.followedArtworksItems = self.followedArtworks.items
            }
        }
        
        Task { [weak self] in
            guard let self = self else { return }
            for await _ in self.discoverArtworks.$items.values {
                self.discoverArtworksItems = self.discoverArtworks.items
            }
        }
    }
    
    @Published var errorMessage: String?
    
    func getDiscoverData() async -> DiscoverData? {
        do {
            let response: DiscoverData = try await api.get(endpoint: "discover")
            return response
        } catch let APIServiceError.httpError(_, message) {
            self.errorMessage = message ?? "Unknown error"
        } catch {
            self.errorMessage = error.localizedDescription
        }
        return nil
    }
    
//    func getDiscoverArtworks(skip: Int = 0, take: Int = 10) async -> [DiscoverArtworkResponse]? {
//        do {
//            let response: [DiscoverArtworkResponse] = try await api.get(endpoint: "artworks/discover?skip=\(skip)&take=\(take)")
//            return response
//        } catch let APIServiceError.httpError(_, message) {
//            self.errorMessage = message ?? "Unknown error"
//        } catch {
//            self.errorMessage = error.localizedDescription
//        }
//        return nil
//    }
//    
//    func getFollowedUsersArtworks(skip: Int = 0, take: Int = 10) async -> [FollowedUserArtworkResponse]? {
//        do {
//            let response: [FollowedUserArtworkResponse] = try await api.get(endpoint: "followed-users/artworks?skip=\(skip)&take=\(take)")
//            return response
//        } catch let APIServiceError.httpError(_, message) {
//            self.errorMessage = message ?? "Unknown error"
//        } catch {
//            self.errorMessage = error.localizedDescription
//        }
//        return nil
//    }
    
//    lazy var followedArtworks = InfiniteScroll<FollowedUserArtworkResponse>(api: api, take: 4) { skip, take in
//        "followed-users/artworks?skip=\(skip)&take=\(take)"
//    }
    
//    lazy var discoverArtworks = InfiniteScroll<DiscoverArtworkResponse>(api: api) { skip, take in
//        "artworks/discover?skip=\(skip)&take=\(take)"
//    }
}
