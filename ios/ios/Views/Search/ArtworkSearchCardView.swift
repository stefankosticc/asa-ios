//
//  ArtworkSearchCardView.swift
//  ios
//
//  Created by stefan on 9.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkSearchCardView: View {
    var artwork: ArtworkSearchResponse
    @State private var loadingImageFailed = false
    
    var body: some View {
        HStack {
            if loadingImageFailed {
                KFImage(URL(string: Constants.ARTWORK_FALLBACK_IMAGE))
                    .resizable()
                    .scaledToFill()
                    .frame(width: 80, height: 80)
                    .clipShape(RoundedRectangle(cornerRadius: 14))
                    .padding(.horizontal)
                    .padding(.vertical, 10)
            } else {
                KFImage(URL(string: "\(Constants.BACKEND_URL)\(artwork.image)"))
                    .placeholder {
                        ProgressView()
                    }
                    .onFailure { _ in
                        loadingImageFailed = true
                    }
                    .resizable()
                    .scaledToFill()
                    .frame(width: 80, height: 80)
                    .clipShape(RoundedRectangle(cornerRadius: 14))
                    .padding(.horizontal)
                    .padding(.vertical, 10)
            }
            
            VStack(alignment: .leading, spacing: 10) {
                Text(artwork.title)
                    .lineLimit(2)
                Text("@\(artwork.postedByUserName)")
                    .lineLimit(1)
                    .foregroundStyle(.cGrayLight)
            }
            
            Spacer()
            
        }
        .background(
            RoundedRectangle(cornerRadius: 10)
                .foregroundStyle(.cBlackHighlight)
        )
        .font(.subheadline)
    }
}

#Preview {
    SearchView()
}
