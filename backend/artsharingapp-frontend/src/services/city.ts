import { ArtworkCardData } from "./artwork";
import authAxios from "./authAxios";
import { Gallery } from "./gallery";

export interface City {
  id: number;
  name: string;
  country: string | null;
}

export async function searchCities(name: string): Promise<City[]> {
  const response = await authAxios.get(`/cities/search`, {
    params: { name },
  });
  return response.data;
}

export async function getCity(cityId: number): Promise<City> {
  const response = await authAxios.get(`/city/${cityId}`);
  return response.data;
}

export async function getCityArtworks(
  cityId: number
): Promise<ArtworkCardData[]> {
  const response = await authAxios.get(`/city/${cityId}/artworks`);
  return response.data;
}

export async function getCityGalleries(cityId: number): Promise<Gallery[]> {
  const response = await authAxios.get(`/city/${cityId}/galleries`);
  return response.data;
}
