import { useState, useEffect } from "react";
import { AuctionResponse, getActiveAuction } from "../services/auction";
import { useAuctionContext } from "../context/AuctionContext";

export const useActiveAuction = (artworkId: number) => {
  const [auction, setAuction] = useState<AuctionResponse | null>(null);
  const [loadingAuction, setLoadingAuction] = useState<boolean>(false);
  const { refetchAuction } = useAuctionContext();

  useEffect(() => {
    let isCancelled = false;

    const fetchAuction = async () => {
      if (artworkId <= 0) {
        setAuction(null);
        setLoadingAuction(false);
        return;
      }
      try {
        setLoadingAuction(true);
        const response = await getActiveAuction(artworkId);
        if (!isCancelled) {
          setAuction(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch auction.");
          setAuction(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingAuction(false);
        }
      }
    };

    fetchAuction();

    return () => {
      isCancelled = true;
    };
  }, [artworkId, refetchAuction]);

  return { auction, loadingAuction };
};
