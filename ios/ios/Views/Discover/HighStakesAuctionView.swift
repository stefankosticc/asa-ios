//
//  HighStakesAuctionView.swift
//  ios
//
//  Created by stefan on 4.10.25..
//

import SwiftUI

struct HighStakesAuctionView: View {
    var artworkTitle: String = "-"
    
    var body: some View {
        VStack(spacing: 20) {
            Text(artworkTitle)
                .bold()
                .multilineTextAlignment(.center)
                .lineLimit(2)
            
            HStack(spacing: 28) {
                VStack(spacing: 8) {
                    Text("Current Price")
                        .font(.subheadline)
                    Text("\(10000.formatted(.number.grouping(.automatic))) \(Currency.USD)")
                        .foregroundStyle(.cGrayLight)
                }
                
                VStack(spacing: 8) {
                    Text("No. of Offers")
                        .font(.subheadline)
                    Text("\(7.formatted(.number.grouping(.automatic)))")
                        .foregroundStyle(.cGrayLight)
                }
            }
            
        }
        .padding(.vertical, 40)
        .padding(.horizontal, 20)
        .frame(width: 290, height: 200)
        .background(Color.black)
        .clipShape(RoundedRectangle(cornerRadius: 10))
        .overlay(RoundedRectangle(cornerRadius: 10).stroke(Color.cPurple, lineWidth: 1))
    }
}

#Preview {
    DiscoverView()
}
