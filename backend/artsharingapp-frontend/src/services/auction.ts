import { toast } from "react-toastify";
import authAxios from "./authAxios";
import { Currency, OfferStatus } from "./enums";

export interface AuctionStartRequest {
  startTime: Date;
  endTime: Date;
  startingPrice: number;
  currency: Currency;
}

export interface AuctionUpdateRequest {
  endTime: Date;
}

export interface AuctionResponse {
  id: number;
  startTime: Date;
  endTime: Date;
  currentPrice: number;
  offerCount: number;
  currency: Currency;
}

export interface OfferRequest {
  amount: number;
}

export interface OfferResponse {
  id: number;
  amount: number;
  userId: number;
  userName: string;
  status: OfferStatus;
}

export async function startAnAuction(
  artworkId: number,
  auctionData: AuctionStartRequest
): Promise<boolean> {
  try {
    await authAxios.post(`artwork/${artworkId}/auction/start`, auctionData);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    toast.error(message);
    return false;
  }
}

export async function getActiveAuction(
  artworkId: number
): Promise<AuctionResponse> {
  const response = await authAxios.get(`artwork/${artworkId}/auction/active`);
  return response.data;
}

export async function makeAnOffer(
  auctionId: number,
  request: OfferRequest
): Promise<boolean> {
  try {
    await authAxios.post(`auction/${auctionId}/make-an-offer`, request);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    toast.error(message);
    console.error("Error:", message);
    return false;
  }
}

export async function updateAuction(
  auctionId: number,
  request: AuctionUpdateRequest
): Promise<boolean> {
  try {
    await authAxios.put(`auction/${auctionId}`, request);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    toast.error(message);
    return false;
  }
}

export async function getOffers(auctionId: number): Promise<OfferResponse[]> {
  const response = await authAxios.get(`auction/${auctionId}/offers`);
  return response.data;
}

export async function acceptOffer(offerId: number): Promise<boolean> {
  try {
    await authAxios.put(`offer/${offerId}/accept`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function rejectOffer(offerId: number): Promise<boolean> {
  try {
    await authAxios.put(`offer/${offerId}/reject`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}
