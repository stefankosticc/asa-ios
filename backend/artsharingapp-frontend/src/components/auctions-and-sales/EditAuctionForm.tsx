import { AuctionResponse, AuctionUpdateRequest } from "../../services/auction";
import { Currency } from "../../services/enums";
import "./styles/EditAuctionForm.css";

type NewAuctionFormProps = {
  auction: AuctionResponse;
  auctionData: AuctionUpdateRequest;
  setAuctionData: React.Dispatch<React.SetStateAction<AuctionUpdateRequest>>;
  handleEndAuction: () => void;
};

const EditAuctionForm = ({
  auction,
  auctionData,
  setAuctionData,
  handleEndAuction,
}: NewAuctionFormProps) => {
  return (
    <div className="psm-auction-form">
      <h4 className="psm-auction-title">Edit Current Auction</h4>

      <div className="psm-form-field">
        <p className="psm-edit-auction-label">Starting Price:</p>
        <div className="psm-edit-auction-data">
          {auction.currentPrice.toLocaleString("en-US")}
        </div>
      </div>

      <div className="psm-form-field">
        <p className="psm-edit-auction-label">Currency:</p>
        <div className="psm-currency-select psm-edit-auction-currency">
          {Currency[auction.currency]}
        </div>
      </div>

      <div className="psm-auction-time">
        <div className="psm-form-field">
          <p className="psm-edit-auction-label">Start Time:</p>
          <div className="psm-edit-auction-data">
            {new Date(auction.startTime).toLocaleString("en-GB", {
              year: "numeric",
              month: "numeric",
              day: "numeric",
              hour: "2-digit",
              minute: "2-digit",
            })}
          </div>
        </div>
        <span>→</span>
        <div className="psm-form-field">
          <label htmlFor="auction-end">End Time:</label>
          <input
            id="auction-end"
            type="datetime-local"
            min={new Date().toISOString().slice(0, 16)}
            value={
              auctionData.endTime
                ? new Date(auctionData.endTime)
                    .toLocaleString("sv-SE")
                    .replace(" ", "T")
                    .slice(0, 16)
                : ""
            }
            onChange={(e) =>
              setAuctionData({
                endTime: e.target.value
                  ? new Date(e.target.value)
                  : new Date(auction.endTime),
              })
            }
          />
        </div>
      </div>
      <button id="psm-end-auction-btn" onClick={handleEndAuction}>
        End Auction
      </button>
    </div>
  );
};

export default EditAuctionForm;
