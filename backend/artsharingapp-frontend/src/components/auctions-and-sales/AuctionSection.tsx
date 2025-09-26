import { useActiveAuction } from "../../hooks/useActiveAuction";
import { Currency } from "../../services/enums";
import "./styles/AuctionSection.css";
import { AiOutlineSend } from "react-icons/ai";
import Countdown from "../Countdown";
import { useState } from "react";
import { makeAnOffer, OfferRequest } from "../../services/auction";
import { useAuctionContext } from "../../context/AuctionContext";
import { toast } from "react-toastify";

type AuctionSectionProps = {
  artworkId: number;
};

const AuctionSection = ({ artworkId }: AuctionSectionProps) => {
  const [offerRequest, setOfferRequest] = useState<OfferRequest>({ amount: 0 });
  const { triggerRefetchAuction } = useAuctionContext();

  const { auction } = useActiveAuction(artworkId);

  if (!auction) return null;

  const handleSendAnOffer = async () => {
    if (
      window.confirm(
        `Confirm your offer for the amount of ${offerRequest.amount.toLocaleString(
          "en-US"
        )} ${Currency[auction.currency]}`
      )
    ) {
      const success = await makeAnOffer(auction.id, offerRequest);
      if (success) {
        triggerRefetchAuction();
        toast.success(
          `Your offer of ${offerRequest.amount.toLocaleString("en-US")} ${
            Currency[auction.currency]
          } has been sent!`
        );
      }
    }
  };

  return (
    <div className="auction-section-container">
      <div className="auction-section-header">
        <h4>Active Auction </h4>
        <p
          className="auction-section-time"
          title={`${new Date(auction.startTime).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })} → ${new Date(auction.endTime).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })}`}
        >
          Time left: <Countdown endTime={auction.endTime} />
        </p>
      </div>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>Current Price</p>
          <p className="auction-section-column-info">
            {auction.currentPrice.toLocaleString("en-US")}{" "}
            {Currency[auction.currency]}
          </p>
        </div>
        <div className="auction-section-column">
          <p>No. of Offers</p>
          <p className="auction-section-column-info">{auction.offerCount}</p>
        </div>
        <div className="auction-section-column">
          <p>Make an Offer</p>
          <div className="auction-section-offer">
            <input
              type="number"
              name="amount"
              id="aso-amount"
              min={auction.currentPrice}
              placeholder={`${auction.currentPrice}`}
              step={10}
              onChange={(e) =>
                setOfferRequest({
                  ...offerRequest,
                  amount: Number(e.target.value),
                })
              }
            />
            <button title="Send offer" onClick={handleSendAnOffer}>
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuctionSection;
