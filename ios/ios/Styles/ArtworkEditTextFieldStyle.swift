//
//  ArtworkEditTextFieldStyle.swift
//  ios
//
//  Created by stefan on 13.10.25..
//

import SwiftUI

struct ArtworkEditTextFieldStyle: TextFieldStyle {
    var minLineLimit = 1
    var maxLineLimit = 6
    
    func _body(configuration: TextField<_Label>) -> some View {
        configuration
            .lineLimit(minLineLimit...maxLineLimit)
            .font(.subheadline)
            .bold()
            .autocorrectionDisabled()
            .tint(.cPurple)
            .padding(.horizontal, 6)
            .padding(.vertical, 4)
            .background(
                RoundedRectangle(cornerRadius: 6)
                    .fill(.clear)
                    .stroke(.cGray)
            )
    }
}
