//
//  InfiniteScroll.swift
//  ios
//
//  Created by stefan on 6.10.25..
//

import Foundation

@MainActor
class InfiniteScroll<Item: Identifiable & Decodable>: ObservableObject {
    private let api: APIServiceProtocol
    private let endpoint: (Int, Int) -> String
    private let take: Int
    
    @Published var items: [Item] = []
    @Published var isLoading = false
    @Published var errorMessage: String?
    
    private var skip = 0
    private var allLoaded = false
    
    init(api: APIServiceProtocol, take: Int = 10, endpoint: @escaping (Int, Int) -> String) {
        self.api = api
        self.take = take
        self.endpoint = endpoint
    }
    
    func loadMore() async {
        guard !isLoading, !allLoaded else { return }
        isLoading = true
        
        do {
            let response: [Item] = try await api.get(endpoint: endpoint(skip, take))
            items.append(contentsOf: response)
            skip += take
            if response.count < take { allLoaded = true }
        } catch let APIServiceError.httpError(_, message) {
            errorMessage = message ?? "Unknown error"
        } catch {
            errorMessage = error.localizedDescription
        }
        
        isLoading = false
    }
    
    func refresh() async {
        skip = 0
        allLoaded = false
        items.removeAll()
        await loadMore()
    }
}
