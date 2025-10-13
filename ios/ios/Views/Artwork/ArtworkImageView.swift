//
//  ArtworkImageView.swift
//  ios
//
//  Created by stefan on 10.10.25..
//

import SwiftUI
import Kingfisher

struct ArtworkImageView: View {
    var url: String = ""
    @State private var loadingImageFailed = false
    
    var body: some View {
        if loadingImageFailed {
            KFImage(URL(string: Constants.ARTWORK_FALLBACK_IMAGE))
                .resizable()
                .scaledToFill()
                .frame(width: 300, height: 300)
                .clipShape(RoundedRectangle(cornerRadius: 14))
                .padding(.horizontal, 24)
                .padding(.top, 80)
                .padding(.bottom, 50)
                .clipped()
                .shadow(color: Color.cGray.opacity(0.15), radius: 10, x: 0, y: 0)
                .shadow(color: Color.cGray.opacity(0.3), radius: 20, x: 0, y: 0)
                .shadow(color: Color.cGray.opacity(0.15), radius: 35, x: 0, y: 0)
        } else {
            KFImage(URL(string: "\(Constants.BACKEND_URL)\(url)"))
                .placeholder {
                    ProgressView()
                }
                .onFailure { _ in
                    loadingImageFailed = true
                }
                .resizable()
                .scaledToFill()
                
        }
    }
}

#Preview {
    ArtworkImageView()
}
