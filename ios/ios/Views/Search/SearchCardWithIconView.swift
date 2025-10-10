//
//  SearchCardWithIconView.swift
//  ios
//
//  Created by stefan on 9.10.25..
//

import SwiftUI

struct SearchCardWithIconView: View {
    let data: any Searchable
    let icon: String
    let iconColor: Color
    
    var body: some View {
        HStack {
            Image(systemName: icon)
                .foregroundStyle(iconColor)
                .frame(width: 50, height: 50)
                .font(.title)
                .padding(.horizontal)
                .padding(.vertical, 10)
            
            if data is City {
                if let city = data as? City {
                    Text("\(city.name)\((city.country != nil) ? ", \(city.country!)" : "")")
                        .lineLimit(2)
                }
            } else if data is Gallery {
                if let gallery = data as? Gallery {
                    HStack(spacing: 10) {
                        Text("\(gallery.name)")
                            .lineLimit(2)
                        if let cityName = gallery.cityName {
                            Spacer()
                            HStack {
                                Image(systemName: "building.2.fill")
                                Text(cityName)
                            }
                            .bold()
                            .font(.caption)
                            .foregroundStyle(.cGrayLight)
                            .padding(.horizontal, 8)
                            .padding(.vertical, 4)
                            .background(
                                Capsule()
                                    .foregroundStyle(.clear)
                            )
                            .overlay {
                                Capsule()
                                    .strokeBorder(.cGrayLight)
                            }
                        }
                    }
                }
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
