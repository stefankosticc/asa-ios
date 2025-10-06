//
//  GetStartedView.swift
//  ios
//
//  Created by stefan on 28.9.25..
//

import SwiftUI

struct GetStartedView: View {
    var body: some View {
        NavigationStack {
            ZStack{
                Image("background")
                    .resizable()
                    .scaledToFill()
                    .ignoresSafeArea()
                
                VStack {
                    Text("Turn creativity into meaningful moments")
                        .font(.largeTitle)
                        .fontWeight(.bold)
                        .multilineTextAlignment(.center)
                        .padding()
                    
                    NavigationLink(destination: LogInView()) {
                        HStack {
                            Text("Get Started")
                                .font(.headline)
                            Image(systemName: "arrow.right")
                                .font(.headline)
                        }
                        .foregroundColor(.black)
                        .frame(maxWidth: .infinity)
                        .padding(.vertical, 14)
                        .padding(.horizontal, 34)
                        .background(Color("cPurple"))
                        .clipShape(Capsule())
                    }
                    .padding()
                }.padding()
            }
        }
        .tint(.cPurple)
    
    }
}

#Preview {
    GetStartedView()
}
