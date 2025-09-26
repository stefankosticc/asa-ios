import { DiscoverArtworkResponse } from "./artwork";
import authAxios from "./authAxios";
import { Currency } from "./enums";

export interface TopArtistResponse {
  id: number;
  name: string;
  userName: string;
  profilePhoto: string;
}

export interface HighStakesAuctionResponse {
  auctionId: number;
  artworkId: number;
  artworkTitle: string;
  currentPrice: number;
  offerCount: number;
  currency: Currency;
}

export interface DiscoverData {
  topArtistsByLikes: TopArtistResponse[];
  highStakeAuctions: HighStakesAuctionResponse[];
  trendingArtworks: DiscoverArtworkResponse[];
}

export async function getDiscoverData(): Promise<DiscoverData> {
  const response = await authAxios.get(`discover`);
  return response.data;
}
