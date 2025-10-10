//
//  BiographyView.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI
import Foundation

struct BiographyView: View {
    var text: String
    
    var body: some View {
        Text(AttributedString.fromHTML(text))
    }
}

#Preview {
    ProfileView()
}
