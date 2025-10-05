//
//  DiscoverSectionView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI

struct DiscoverSectionView<Content: View>: View {
    var title: String
    @ViewBuilder var content: () -> Content
    
    var body: some View {
        VStack(alignment: .leading){
            Text(title)
                .font(.headline)
                .padding(.leading, 24)
                .padding(.vertical)
            ScrollView(.horizontal, showsIndicators: false){
                HStack(alignment: .top, spacing: 20){
                    content()
                }
                .padding()
                .padding(.vertical, 4)
                .padding(.horizontal, 12)
                .background(Color.cBlackHighlight)
                .clipShape(RoundedRectangle(cornerRadius: 14))
                .padding(.horizontal)
            }
        }
    }
}

#Preview {
    DiscoverView()
}
