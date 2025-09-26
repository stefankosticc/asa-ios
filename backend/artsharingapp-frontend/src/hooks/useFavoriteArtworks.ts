import { useState, useEffect } from "react";
import { FavoriteArtwork, getFavoriteArtworks } from "../services/artwork";
import { User } from "../services/auth";

type UseFavoriteArtworksOptions = {
  likedByUser: User | null | undefined;
  activeTab?: string;
};

export const useFavoriteArtworks = ({
  likedByUser,
  activeTab = "favorites",
}: UseFavoriteArtworksOptions) => {
  const [favorites, setFavorites] = useState<FavoriteArtwork[] | null>(null);
  const [loadingFavorites, setLoadingFavorites] = useState<boolean>(false);
  const [hasFetchedFavorites, setHasFetchedFavorites] =
    useState<boolean>(false);

  useEffect(() => {
    if (activeTab !== "favorites" || !likedByUser || hasFetchedFavorites)
      return;

    let isCancelled = false;

    const fetchFavorites = async () => {
      try {
        setLoadingFavorites(true);
        const response = await getFavoriteArtworks(likedByUser.id);
        if (!isCancelled) {
          setFavorites(response);
          setHasFetchedFavorites(true);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch favorites");
          setFavorites([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingFavorites(false);
        }
      }
    };

    fetchFavorites();

    return () => {
      isCancelled = true;
    };
  }, [activeTab, likedByUser]);

  return { favorites, loadingFavorites };
};
