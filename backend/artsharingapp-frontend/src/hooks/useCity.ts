import { useState, useEffect } from "react";
import { City, getCity } from "../services/city";

export const useCity = (cityId: number) => {
  const [city, setCity] = useState<City | null>(null);
  const [loadingGallery, setLoadingGallery] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchCity = async () => {
      if (cityId <= 0) {
        setCity(null);
        setLoadingGallery(false);
        return;
      }
      try {
        setLoadingGallery(true);
        const response = await getCity(cityId);
        if (!isCancelled) {
          setCity(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch city.");
          setCity(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingGallery(false);
        }
      }
    };

    fetchCity();

    return () => {
      isCancelled = true;
    };
  }, [cityId]);

  return { city, loadingGallery };
};
