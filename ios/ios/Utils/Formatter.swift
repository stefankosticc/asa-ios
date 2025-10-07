//
//  Formatter.swift
//  ios
//
//  Created by stefan on 7.10.25..
//

import Foundation

class Formatter{
    struct Threshold {
        let value: Double
        let suffix: String
        let decimals: Int
        let divisor: Double?
    }
    
    static func formatFollowCount(_ count: Int?) -> String {
        guard let count = count else { return "0" }
        
        let thresholds: [Threshold] = [
            Threshold(value: 1_000_000_000, suffix: "B", decimals: 2, divisor: nil),
            Threshold(value: 1_000_000, suffix: "M", decimals: 1, divisor: nil),
            Threshold(value: 10_000, suffix: "K", decimals: 1, divisor: 1_000)
        ]
        
        for threshold in thresholds {
            if Double(count) >= threshold.value {
                let divisor = threshold.divisor ?? threshold.value
                let factor = pow(10.0, Double(threshold.decimals))
                let num = floor((Double(count) / divisor) * factor) / factor
                let formatted = String(format: "%.\(threshold.decimals)f", num)
                    .replacingOccurrences(of: "\\.0+$", with: "", options: .regularExpression)
                return formatted + threshold.suffix
            }
        }
        
        return "\(count)"
    }
}
