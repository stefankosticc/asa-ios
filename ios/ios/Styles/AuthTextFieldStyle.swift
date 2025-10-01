//
//  AuthTextFieldStyle.swift
//  ios
//
//  Created by stefan on 1.10.25..
//

import SwiftUI

struct AuthTextFieldStyle: TextFieldStyle {
    var isFocused: Bool = false
    
    func _body(configuration: TextField<_Label>) -> some View {
        configuration
            .padding(.vertical, 10)
            .padding(.horizontal, 24)
            .foregroundColor(.white)
            .background(
                RoundedRectangle(cornerRadius: 10)
                    .stroke(isFocused ? .cPurple : .cGray)
            )
            .autocorrectionDisabled()
    }
}
