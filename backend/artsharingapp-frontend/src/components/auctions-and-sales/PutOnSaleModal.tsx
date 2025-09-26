import { useEffect, useState } from "react";
import "./styles/PutOnSaleModal.css";
import { Currency } from "../../services/enums";
import {
  putArtworkOnSale,
  PutArtworkOnSaleRequest,
} from "../../services/artwork";
import {
  AuctionStartRequest,
  AuctionUpdateRequest,
  startAnAuction,
  updateAuction,
} from "../../services/auction";
import { useActiveAuction } from "../../hooks/useActiveAuction";
import NewAuctionForm from "./NewAuctionForm";
import { useAuctionContext } from "../../context/AuctionContext";
import EditAuctionForm from "./EditAuctionForm";
import { toast } from "react-toastify";

type PutOnSaleModalProps = {
  onClose: () => void;
  artworkId: number;
  refetchArtwork: () => void;
};

const PutOnSaleModal = ({
  onClose,
  artworkId,
  refetchArtwork,
}: PutOnSaleModalProps) => {
  const [activeTab, setActiveTab] = useState<string>("fixed");
  const [fixedPrice, setFixedPrice] = useState<number>(0);
  const [fixedCurrency, setFixedCurrency] = useState<Currency>(Currency.USD);
  const { triggerRefetchAuction } = useAuctionContext();

  const [auctionData, setAuctionData] = useState<AuctionStartRequest>({
    startTime: new Date(),
    endTime: new Date(),
    startingPrice: 0,
    currency: Currency.USD,
  });

  const [editActiveAuctionData, setEditActiveAuctionData] =
    useState<AuctionUpdateRequest>({
      endTime: new Date(),
    });

  const { auction } = useActiveAuction(artworkId);

  useEffect(() => {
    if (auction) setEditActiveAuctionData({ endTime: auction.endTime });
  }, [auction]);

  const handleSave = async () => {
    if (!artworkId) return;

    let success = false;
    if (activeTab === "fixed") {
      const request: PutArtworkOnSaleRequest = {
        isOnSale: true,
        price: fixedPrice,
        currency: fixedCurrency,
      };
      success = await putArtworkOnSale(artworkId, request);
      if (success) {
        onClose();
        refetchArtwork();
      }
    } else {
      if (activeTab === "auction" && auction) {
        success = await updateAuction(auction.id, editActiveAuctionData);
        if (success) toast.success("Auction updated successfully!");
      } else {
        success = await startAnAuction(artworkId, auctionData);
        if (success)
          toast.success(
            `Auction ${
              auctionData.startTime <= new Date() ? "started" : "scheduled"
            } successfully!`
          );
      }
      if (success) {
        onClose();
        triggerRefetchAuction();
      }
    }
  };

  const handleEndAuction = async () => {
    if (!auction) return;

    const success = await updateAuction(auction.id, { endTime: new Date() });
    if (success) {
      onClose();
      triggerRefetchAuction();
      toast.success("Auction ended!");
    } else {
      toast.error("Failed to end auction.");
    }
  };

  return (
    <div className="psm-page">
      <div className="psm-container">
        <div className="psm-tabs">
          <button
            className={activeTab === "fixed" ? "active" : ""}
            onClick={() => setActiveTab("fixed")}
          >
            Fixed Price
          </button>
          <button
            className={activeTab === "auction" ? "active" : ""}
            onClick={() => setActiveTab("auction")}
          >
            Auction
          </button>
        </div>

        {activeTab === "fixed" && (
          <div className="psm-fixed-form">
            <div className="psm-form-field">
              <label htmlFor="psm-fixed-price">Price:</label>
              <input
                type="number"
                id="psm-fixed-price"
                placeholder={"0"}
                onChange={(e) => setFixedPrice(Number(e.target.value))}
              />
            </div>
            <div className="psm-form-field">
              <label htmlFor="currency">Currency:</label>
              <select
                id="currency"
                value={fixedCurrency}
                className="psm-currency-select"
                onChange={(e) => setFixedCurrency(Number(e.target.value))}
              >
                {Object.keys(Currency)
                  .filter((key) => isNaN(Number(key)))
                  .map((cur) => (
                    <option
                      key={cur}
                      value={Currency[cur as keyof typeof Currency]}
                    >
                      {cur}
                    </option>
                  ))}
              </select>
            </div>
          </div>
        )}

        {activeTab === "auction" &&
          (auction ? (
            <EditAuctionForm
              auction={auction}
              auctionData={editActiveAuctionData}
              setAuctionData={setEditActiveAuctionData}
              handleEndAuction={handleEndAuction}
            />
          ) : (
            <NewAuctionForm
              auctionData={auctionData}
              setAuctionData={setAuctionData}
            />
          ))}

        <div className="psm-buttons">
          <button id="psm-cancel" onClick={onClose}>
            Cancel
          </button>
          <button id="psm-save" onClick={handleSave}>
            Save
          </button>
        </div>
      </div>
    </div>
  );
};

export default PutOnSaleModal;
