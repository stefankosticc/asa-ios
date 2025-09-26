import { useActiveAuction } from "../../hooks/useActiveAuction";
import { useOffers } from "../../hooks/useOffers";
import "./styles/AuctionAnalyticsModal.css";
import { IoCloseCircleOutline } from "react-icons/io5";
import OfferCard from "./OfferCard";
import { useState } from "react";

type AuctionAnalyticsModalProps = {
  artworkId: number;
  onClose: () => void;
};

const AuctionAnalyticsModal = ({
  artworkId,
  onClose,
}: AuctionAnalyticsModalProps) => {
  const [refetchOffers, setRefetchOffers] = useState<boolean>(false);

  const { auction } = useActiveAuction(artworkId);

  const { offers } = useOffers(auction?.id ? auction.id : -1, refetchOffers);

  return (
    <div className="blured-page">
      <div className="modal-container aam-container">
        <IoCloseCircleOutline
          className="aam-close"
          onClick={onClose}
          title="Close"
        />

        <h4>Auction Analytics</h4>

        <div className="aam-content">
          <div className="aam-sidebar">
            {auction ? (
              <>
                <p className="aam-sidebar-section">Active</p>
                <div className="aam-auction">
                  <p className="aam-auction-time">
                    {new Date(auction.startTime).toLocaleString("en-GB", {
                      year: "numeric",
                      month: "numeric",
                      day: "numeric",
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </p>
                  <span className="aam-arrow">→</span>
                  <p className="aam-auction-time">
                    {new Date(auction.endTime).toLocaleString("en-GB", {
                      year: "numeric",
                      month: "numeric",
                      day: "numeric",
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </p>
                </div>
              </>
            ) : (
              <p className="aam-auction-not-found">
                There is no active auctions
              </p>
            )}
          </div>

          {offers && offers.length !== 0 && (
            <div className="aam-offers">
              {offers.map((offer) => (
                <OfferCard
                  key={offer.id}
                  offer={offer}
                  currency={auction?.currency}
                  onClose={onClose}
                  refetchOffers={() => setRefetchOffers((prev) => !prev)}
                />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AuctionAnalyticsModal;
