import { useState, useEffect } from "react";
import { Gallery, getGallery } from "../services/gallery";

export const useGallery = (galleryId: number) => {
  const [gallery, setGallery] = useState<Gallery | null>(null);
  const [loadingGallery, setLoadingGallery] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchGallery = async () => {
      if (galleryId <= 0) {
        setGallery(null);
        setLoadingGallery(false);
        return;
      }
      try {
        setLoadingGallery(true);
        const response = await getGallery(galleryId);
        if (!isCancelled) {
          setGallery(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch gallery.");
          setGallery(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingGallery(false);
        }
      }
    };

    fetchGallery();

    return () => {
      isCancelled = true;
    };
  }, [galleryId]);

  return { gallery, loadingGallery };
};
