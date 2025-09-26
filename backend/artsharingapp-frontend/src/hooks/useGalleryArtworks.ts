import { useState, useEffect } from "react";
import { ArtworkCardData } from "../services/artwork";
import { getGalleryArtworks } from "../services/gallery";

export const useGalleryArtworks = (galleryId: number) => {
  const [galleryArtworks, setGalleryArtworks] = useState<
    ArtworkCardData[] | null
  >(null);
  const [loadingGalleryArtworks, setLoadingGalleryArtworks] =
    useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchGalleryArtworks = async () => {
      try {
        setLoadingGalleryArtworks(true);
        const response = await getGalleryArtworks(galleryId);
        if (!isCancelled) {
          setGalleryArtworks(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch gallery artworks.");
          setGalleryArtworks([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingGalleryArtworks(false);
        }
      }
    };

    fetchGalleryArtworks();

    return () => {
      isCancelled = true;
    };
  }, [galleryId]);

  return { galleryArtworks, loadingGalleryArtworks };
};
