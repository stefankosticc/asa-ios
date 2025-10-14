//
//  AlertViewModel.swift
//  ios
//
//  Created by stefan on 14.10.25..
//

import Foundation
import AlertToast

class AlertViewModel: ObservableObject{
    
    @Published var show = false
    @Published var alertToast = AlertToast(type: .regular, title: "") {
        didSet {
            show.toggle()
        }
    }
    
}
