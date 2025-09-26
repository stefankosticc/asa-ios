import { useState, useEffect } from "react";
import { getOffers, OfferResponse } from "../services/auction";

export const useOffers = (auctionId: number, refetch: boolean = false) => {
  const [offers, setOffers] = useState<OfferResponse[] | null>(null);
  const [loadingOffers, setLoadingOffers] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchOffers = async () => {
      if (auctionId <= 0) {
        setOffers(null);
        setLoadingOffers(false);
        return;
      }
      try {
        setLoadingOffers(true);
        const response = await getOffers(auctionId);
        if (!isCancelled) {
          setOffers(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch offers.");
          setOffers([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingOffers(false);
        }
      }
    };

    fetchOffers();

    return () => {
      isCancelled = true;
    };
  }, [auctionId, refetch]);

  return { offers, loadingOffers };
};
