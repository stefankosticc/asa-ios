//
//  ProfilePhoto.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import SwiftUI
import Kingfisher

struct ProfilePhoto: View {
    @State private var loadFailed = false
    var url: String = ""
    var size: CGFloat = 90
    
    
    var body: some View {
        if loadFailed {
            KFImage(URL(string: Constants.ARTIST_FALLBACK_IMAGE))
                .resizable()
                .scaledToFill()
                .frame(width: size, height: size)
                .clipShape(Circle())
        } else {
            KFImage(URL(string: "\(Constants.BACKEND_URL)\(url)"))
                .setProcessor(SVGProcessor())
                .placeholder {
                    ProgressView()
                }
                .onFailure { _ in
                    loadFailed = true
                }
                .resizable()
                .scaledToFill()
                .frame(width: size, height: size)
                .clipShape(Circle())
            //  .overlay(Circle().stroke(Color.gray.opacity(0.5), lineWidth: 2))
        }
    }
}

#Preview {
    ProfilePhoto()
}
