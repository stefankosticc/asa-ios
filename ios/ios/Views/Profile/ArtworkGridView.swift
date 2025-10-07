//
//  ArtworkGridView.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI

struct ArtworkGridView: View {
    let items = Array(1...20)
    // Define how many columns and their sizing
    let columns = [
        GridItem(.flexible(), spacing: 2),
        GridItem(.flexible(), spacing: 2),
        GridItem(.flexible(), spacing: 2)
    ]
    
    var body: some View {
        ScrollView {
            LazyVGrid(columns: columns, spacing: 2) {
                ForEach(items, id: \.self) { item in
                    RoundedRectangle(cornerRadius: 12)
                        .fill(Color.cPurple.opacity(0.8))
                        .frame(height: 200)
                        .overlay(
                            Text("Item \(item)")
                                .foregroundColor(.white)
                        )
                }
            }
        }
    }
}

#Preview {
    ArtworkGridView()
}
