import { createContext, useContext, useState, ReactNode } from "react";

type AuctionContextType = {
  refetchAuction: number;
  triggerRefetchAuction: () => void;
};

const AuctionContext = createContext<AuctionContextType | undefined>(undefined);

export const AuctionProvider = ({ children }: { children: ReactNode }) => {
  const [refetchAuction, setRefetchAuction] = useState(0);

  const triggerRefetchAuction = () => setRefetchAuction((prev) => prev + 1);

  return (
    <AuctionContext.Provider value={{ refetchAuction, triggerRefetchAuction }}>
      {children}
    </AuctionContext.Provider>
  );
};

export const useAuctionContext = () => {
  const context = useContext(AuctionContext);
  if (!context) {
    throw new Error("useAuctionContext must be used within AuctionProvider");
  }
  return context;
};
