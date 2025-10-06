//
//  SVGImageProcessor.swift
//  ios
//
//  Created by stefan on 6.10.25..
//

import SwiftUI
import Kingfisher
import SVGKit

struct SVGProcessor: ImageProcessor {
    let identifier = "com.asa.SVGProcessor"
    
    
    func process(item: ImageProcessItem, options: KingfisherParsedOptionsInfo) -> KFCrossPlatformImage? {
        switch item {
        case .data(let data):
            // Detect SVG content
            if let svgString = String(data: data, encoding: .utf8),
               svgString.contains("<svg"),
               let svgImage = SVGKImage(data: data) {

                return svgImage.uiImage
            } else {
                // Non-SVG: use default processor
                return DefaultImageProcessor.default.process(item: item, options: options)
            }
            
        default:
            return nil
        }
    }
}
