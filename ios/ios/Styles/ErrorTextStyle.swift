//
//  ErrorTextStyle.swift
//  ios
//
//  Created by stefan on 2.10.25..
//

import SwiftUI

struct ErrorTextStyle: ViewModifier {
    func body(content: Content) -> some View {
        content
            .foregroundColor(.cRed)
            .font(.subheadline).bold()
            .multilineTextAlignment(.center)
            .padding(.vertical, 4)
            .padding(.horizontal, 16)
            .background(
                RoundedRectangle(cornerRadius: 8)
                    .fill(.cRedLight)
            )
            .frame(maxWidth: .infinity)
            .padding(.horizontal)
            .padding(.vertical, 0)
    }
}
