import { useState, useEffect } from "react";
import { ArtworkCardData } from "../services/artwork";
import { getCityArtworks } from "../services/city";

export const useCityArtworks = (cityId: number) => {
  const [cityArtworks, setCityArtworks] = useState<ArtworkCardData[] | null>(
    null
  );
  const [loadingCityArtworks, setLoadingCityArtworks] =
    useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchCityArtworks = async () => {
      try {
        setLoadingCityArtworks(true);
        const response = await getCityArtworks(cityId);
        if (!isCancelled) {
          setCityArtworks(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch city artworks.");
          setCityArtworks([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingCityArtworks(false);
        }
      }
    };

    fetchCityArtworks();

    return () => {
      isCancelled = true;
    };
  }, [cityId]);

  return { cityArtworks, loadingCityArtworks };
};
