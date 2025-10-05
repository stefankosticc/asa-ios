//
//  DiscoverViewModel.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import Foundation

class DiscoverViewModel : ObservableObject {
    private let api: APIServiceProtocol
    
    init(api: APIServiceProtocol = APIService()) {
        self.api = api
    }
    
    
}
