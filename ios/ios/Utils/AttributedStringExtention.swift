//
//  AttributedStringExtention.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation
import SwiftUI

extension AttributedString {
    static func fromHTML(_ html: String) -> AttributedString {
        var result = AttributedString()
        var attributes = AttributeContainer()
        var tagStack: [String] = []
        var inListItem = false
        
        // Pre-clean HTML
        let content = html
            .replacingOccurrences(of: "\n", with: "")
            .replacingOccurrences(of: "\r", with: "")
        
        let scanner = Scanner(string: content)
        scanner.charactersToBeSkipped = nil
        
        while !scanner.isAtEnd {
            // Text before tag
            if let text = scanner.scanUpToString("<"), !text.isEmpty {
                var chunk = AttributedString(text)
                chunk.mergeAttributes(attributes)
                result.append(chunk)
            }
            
            // Read tag
            guard scanner.scanString("<") != nil,
                  let tag = scanner.scanUpToString(">") else { break }
            _ = scanner.scanString(">")
            
            let tagLower = tag.lowercased()
            
            // Closing tag
            if tagLower.hasPrefix("/") {
                let name = String(tagLower.dropFirst())
                _ = tagStack.popLast()
                
                if name == "li" {
                    inListItem = false
                }
                
                attributes = AttributeContainer()
                continue
            }
            
            // Opening tag
            tagStack.append(tagLower)
            if tagLower.contains("style"){
                let parts = tagLower.split(separator: " ")
                if parts.count > 1 {
                    let stylePart = parts.dropFirst().joined(separator: " ")
                    
                    if let color = extractColor(from: stylePart) {
                        attributes.foregroundColor = color
                    }
                }
            }
            
            // Style rules
            switch true {
            case tagLower.starts(with: "h1"):
                attributes.font = .title2.bold()
                if !result.characters.isEmpty { // only add spacing when heading is not the first element in the string
                    result.append(AttributedString("\n\n"))
                }
            case tagLower.starts(with: "h2"):
                attributes.font = .title3.bold()
                if !result.characters.isEmpty { // only add spacing when heading is not the first element in the string
                    result.append(AttributedString("\n\n"))
                }
                
            case tagLower.starts(with: "ul"):
                result.append(AttributedString("\n"))
                
            case tagLower.starts(with: "li"):
                inListItem = true
                var bullet = AttributedString("\n • ")
                bullet.mergeAttributes(attributes)
                attributes.font = .subheadline
                result.append(bullet)
                
            case tagLower.starts(with: "p"):
                attributes.font = .subheadline
                if !inListItem {
                    result.append(AttributedString("\n\n"))
                }
                
            case tagLower.starts(with: "strong"), tagLower.starts(with: "b"):
                attributes.font = .subheadline.bold()
                
            case tagLower.starts(with: "em"), tagLower.starts(with: "i"):
                attributes.font = .subheadline.italic()
                
            case tagLower.starts(with: "br"):
                result.append(AttributedString("\n"))
                
            default:
                break
            }
        }
        
        return result
    }
}

private func extractColor(from tag: String) -> Color? {
    // Example: span style="color: rgb(149, 141, 241)"
    guard let styleRange = tag.range(of: "style=") else { return nil }
    let styleContent = tag[styleRange.upperBound...]
    guard let rgbRange = styleContent.range(of: "rgb(") else { return nil }
    
    let rgbPart = styleContent[rgbRange.upperBound...].split(separator: ")").first ?? ""
    let comps = rgbPart.split(separator: ",").compactMap { Double($0.trimmingCharacters(in: .whitespaces)) }
    
    guard comps.count >= 3 else { return nil }
    return Color(red: comps[0]/255, green: comps[1]/255, blue: comps[2]/255)
}
