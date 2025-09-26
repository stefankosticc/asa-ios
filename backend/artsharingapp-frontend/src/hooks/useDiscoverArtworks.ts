import { useState, useEffect } from "react";
import {
  DiscoverArtworkResponse,
  FollowedUserArtworkResponse,
  getDiscoverArtworks,
  getFollowedUsersArtworks,
} from "../services/artwork";

type UseDiscoverArtworksReturn<T> = {
  artworks: T[];
  loadingArtworks: boolean;
  loadMoreArtworks: () => void;
};

export function useDiscoverArtworks(
  type: "discover"
): UseDiscoverArtworksReturn<DiscoverArtworkResponse>;

export function useDiscoverArtworks(
  type: "following"
): UseDiscoverArtworksReturn<FollowedUserArtworkResponse>;

export function useDiscoverArtworks(
  type: "discover" | "following",
  refetch?: boolean
): UseDiscoverArtworksReturn<any> {
  const [artworks, setArtworks] = useState<
    FollowedUserArtworkResponse[] | DiscoverArtworkResponse[]
  >([]);
  const [loadingArtworks, setLoadingArtworks] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 30;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchArtworks = async () => {
      try {
        setLoadingArtworks(true);
        const initialData =
          type === "following"
            ? await getFollowedUsersArtworks(0, take)
            : await getDiscoverArtworks(0, take);
        if (!isCancelled) {
          setArtworks(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch artworks.");
          setArtworks([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingArtworks(false);
        }
      }
    };

    fetchArtworks();

    return () => {
      isCancelled = true;
    };
  }, [refetch, type]);

  const loadMoreArtworks = async () => {
    if (loadingArtworks || !hasMore) return;

    setLoadingArtworks(true);
    try {
      const newArtworks =
        type === "following"
          ? await getFollowedUsersArtworks(skip, take)
          : await getDiscoverArtworks(skip, take);
      setArtworks((prev) => [...prev, ...newArtworks]);
      setSkip((prev) => prev + newArtworks.length);
      if (newArtworks.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more artworks.");
    } finally {
      setLoadingArtworks(false);
    }
  };

  return {
    artworks,
    loadingArtworks,
    loadMoreArtworks,
  };
}
