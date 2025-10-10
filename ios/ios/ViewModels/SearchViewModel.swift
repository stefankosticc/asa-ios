//
//  SearchViewModel.swift
//  ios
//
//  Created by stefan on 9.10.25..
//

import Foundation
import SwiftUI
import Combine

struct Filter : Identifiable {
    let title: String
    let icon: Image
    let color: Color
    
    var id: String { title }
}

@MainActor
class SearchViewModel : ObservableObject {
    private let api: APIServiceProtocol
    @Published var errorMessage: String?
    @Published var isLoading: Bool = false
    
    
    @Published var searchString: String = ""
    @Published var selectedFilter: String = "Artworks"
    @Published var searchResults: [any Searchable] = []
    @Published var isSearchFinished: Bool = false
    
    private var cancellables = Set<AnyCancellable>()
    
    @Published var filters: [Filter] = [
        Filter(title: "Artworks", icon: Image(systemName: "rectangle.portrait.on.rectangle.portrait.angled.fill"), color: .cBlueLight),
        Filter(title: "Artists", icon: Image(systemName: "person.fill"), color: .cPurpleDark),
        Filter(title: "Cities", icon: Image(systemName: "building.2.fill"), color: .cBlue),
        Filter(title: "Galleries", icon: Image(systemName: "building.columns.fill"), color: .cOrange)
    ]
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
        
        // it will react every time search string or filter changes
        Publishers.CombineLatest($searchString, $selectedFilter)
            .debounce(for: .milliseconds(500), scheduler: RunLoop.main) // adds delay
            .removeDuplicates { previous, current in
                previous.0 == current.0 && previous.1 == current.1
            }
            .sink { [weak self] (text, filter) in
                Task {
                    await self?.performSearch(query: text, filter: filter)
                }
            }
            .store(in: &cancellables)
    }
    
    func performSearch(query: String, filter: String) async {
        guard !query.isEmpty else {
            await MainActor.run {
                searchResults = []
                isSearchFinished = false
            }
            return
        }
        var results: [any Searchable]? = nil
        
        switch filter {
        case "Artworks":
            results = await searchArtworks(title: query)
        case "Artists":
            results = await searchArtists(name: query)
        case "Cities":
            results = await searchCities(name: query)
        case "Galleries":
            results = await searchGalleries(name: query)
        default:
            results = []
        }
        
        await MainActor.run {
            searchResults = results ?? []
            isSearchFinished = true
        }
    }
    
    func searchArtworks(title: String) async -> [ArtworkSearchResponse]? {
        do {
            isLoading = true
            let response: [ArtworkSearchResponse] = try await api.get(endpoint: "artworks/search?title=\(title)")
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
    
    func searchArtists(name: String) async -> [UserSearchResponse]? {
        do {
            isLoading = true
            let response: [UserSearchResponse] = try await api.get(endpoint: "users/search?searchString=\(name)")
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
    
    func searchCities(name: String) async -> [City]? {
        do {
            isLoading = true
            let response: [City] = try await api.get(endpoint: "cities/search?name=\(name)")
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
    
    func searchGalleries(name: String) async -> [Gallery]? {
        do {
            isLoading = true
            let response: [Gallery] = try await api.get(endpoint: "galleries/search?name=\(name)")
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
