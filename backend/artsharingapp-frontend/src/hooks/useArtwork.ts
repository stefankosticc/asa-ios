import { useState, useEffect } from "react";
import { Artwork, getArtwork } from "../services/artwork";

export const useArtwork = (artworkId: number, refetch: boolean = false) => {
  const [artwork, setArtwork] = useState<Artwork | null>(null);
  const [loadingArtwork, setLoadingArtwork] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchArtwork = async () => {
      if (artworkId <= 0) {
        setArtwork(null);
        setLoadingArtwork(false);
        return;
      }
      try {
        setLoadingArtwork(true);
        const response = await getArtwork(artworkId);
        if (!isCancelled) {
          setArtwork(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch artwork.");
          setArtwork(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingArtwork(false);
        }
      }
    };

    fetchArtwork();

    return () => {
      isCancelled = true;
    };
  }, [artworkId, refetch]);

  return { artwork, loadingArtwork };
};
