import { useState, useEffect } from "react";
import { getCityGalleries } from "../services/city";
import { Gallery } from "../services/gallery";

export const useCityGalleries = (cityId: number) => {
  const [cityGalleries, setCityGalleries] = useState<Gallery[] | null>(null);
  const [loadingCityGalleries, setLoadingCityGalleries] =
    useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchCityGalleries = async () => {
      try {
        setLoadingCityGalleries(true);
        const response = await getCityGalleries(cityId);
        if (!isCancelled) {
          setCityGalleries(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch city galleries.");
          setCityGalleries([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingCityGalleries(false);
        }
      }
    };

    fetchCityGalleries();

    return () => {
      isCancelled = true;
    };
  }, [cityId]);

  return { cityGalleries, loadingCityGalleries };
};
