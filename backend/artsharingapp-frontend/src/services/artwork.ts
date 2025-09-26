import authAxios from "./authAxios";
import { Currency } from "./enums";

export interface ArtworkCardData {
  id: number;
  title: string;
  image: string;
  isPrivate: boolean;
  postedByUserId: number;
  postedByUserName: string;
  date: Date;
}

export interface FavoriteArtwork {
  userId: number;
  artworkId: number;
  artworkTitle: string | null;
  artworkImage: string | null;
}

export interface Artwork {
  id: number;
  title: string;
  story: string;
  image: string;
  date: Date;
  tipsAndTricks: string;
  isPrivate: boolean;
  isOnSale: boolean;
  price: number | null;
  currency: Currency;
  createdByArtistId: number;
  createdByArtistUserName: string;
  postedByUserId: number;
  postedByUserName: string;
  cityId: number | null;
  cityName: string | null;
  galleryId: number | null;
  galleryName: string | null;
  isLikedByLoggedInUser: boolean | null;
  color: string | null;
}

export interface ArtworkRequest {
  title: string;
  story: string;
  date: Date | string;
  tipsAndTricks: string;
  isPrivate: boolean;
  createdByArtistId: number;
  postedByUserId: number;
  cityId: number | null;
  galleryId: number | null;
  color: string | null;
}

export interface ArtworkSearchResponse {
  id: number;
  title: string;
  image: string;
  isOnSale: boolean;
  postedByUserId: number;
  postedByUserName: string;
  cityId: number | null;
  cityName: string | null;
  country: string | null;
  galleryId: number | null;
  galleryName: string | null;
}

export interface PutArtworkOnSaleRequest {
  isOnSale: boolean;
  price: number;
  currency: Currency;
}

export interface FollowedUserArtworkResponse {
  id: number;
  title: string;
  image: string;
  postedByUserName: string;
  color: string;
}

export interface DiscoverArtworkResponse {
  id: number;
  title: string;
  image: string;
  postedByUserName: string;
}

export interface UserArtworksResponse {
  privateArtworks: ArtworkCardData[];
  publicArtworks: ArtworkCardData[];
}

export interface ChangeArtworkVisibilityRequest {
  isPrivate: boolean;
}

export async function getUserArtworks(
  userId: number
): Promise<UserArtworksResponse> {
  const response = await authAxios.get(`user/${userId}/artworks`);
  return response.data;
}

export async function getFavoriteArtworks(
  userId: number
): Promise<FavoriteArtwork[]> {
  const response = await authAxios.get(`/user/${userId}/liked-artworks`);
  return response.data;
}

export async function getArtwork(artworkId: number): Promise<Artwork> {
  const response = await authAxios.get(`/artwork/${artworkId}`);
  return response.data;
}

export async function likeArtwork(artworkId: number): Promise<boolean> {
  try {
    await authAxios.post(`/artwork/${artworkId}/like`);
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

export async function dislikeArtwork(artworkId: number): Promise<void> {
  try {
    await authAxios.delete(`/artwork/${artworkId}/dislike`);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}

export async function updateArtwork(
  artworkId: number,
  request: ArtworkRequest,
  artworkImage: File | null
): Promise<void> {
  try {
    const formData = new FormData();
    for (const key in request) {
      const value = (request as any)[key];
      formData.append(key, value !== null ? value.toString() : "");
    }

    if (artworkImage) {
      formData.append("artworkImage", artworkImage);
    }

    await authAxios.put(`/artwork/${artworkId}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred while updating artwork.";
    console.error("Error:", message);
  }
}

export async function addNewArtwork(
  artwork: ArtworkRequest,
  artworkImage: File
): Promise<void> {
  try {
    const formData = new FormData();
    for (const key in artwork) {
      const value = (artwork as any)[key];
      formData.append(key, value !== null ? value.toString() : "");
    }
    formData.append("artworkImage", artworkImage);

    await authAxios.post(`/artwork`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}

export async function searchArtworks(
  title: string
): Promise<ArtworkSearchResponse[]> {
  const response = await authAxios.get(`/artworks/search`, {
    params: { title },
  });
  return response.data;
}

export async function getArtworkImage(imageUrl: string): Promise<string> {
  try {
    const response = await authAxios.get(imageUrl, { responseType: "blob" });
    const url = URL.createObjectURL(response.data);
    return url;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return "";
  }
}

export async function extractArtworkColor(
  imageFile: File
): Promise<string | null> {
  const formData = new FormData();
  formData.append("image", imageFile);

  try {
    const response = await authAxios.post("/artwork/extract-color", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data;
  } catch (err) {
    console.error("Failed to extract color", err);
    return null;
  }
}

export async function deleteArtwork(artworkId: number): Promise<boolean> {
  try {
    await authAxios.delete(`/artwork/${artworkId}`);
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

export async function putArtworkOnSale(
  artworkId: number,
  request: PutArtworkOnSaleRequest
): Promise<boolean> {
  try {
    await authAxios.put(`artwork/${artworkId}/put-on-sale`, request);
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

export async function removeArtworkFromSale(
  artworkId: number
): Promise<boolean> {
  try {
    await authAxios.put(`artwork/${artworkId}/remove-from-sale`);
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

export async function transferArtwork(
  artworkId: number,
  userId: number
): Promise<boolean> {
  try {
    await authAxios.put(`artwork/${artworkId}/transfer/to-user/${userId}`);
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

export async function getFollowedUsersArtworks(
  skip = 0,
  take = 30
): Promise<FollowedUserArtworkResponse[]> {
  const response = await authAxios.get(`followed-users/artworks`, {
    params: { skip, take },
  });
  return response.data;
}

export async function getDiscoverArtworks(
  skip = 0,
  take = 30
): Promise<DiscoverArtworkResponse[]> {
  const response = await authAxios.get(`artworks/discover`, {
    params: { skip, take },
  });
  return response.data;
}

export async function changeArtworkVisibility(
  artworkId: number,
  request: ChangeArtworkVisibilityRequest
): Promise<boolean> {
  try {
    await authAxios.put(`artwork/${artworkId}/change-visibility`, request);
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
