import { useNavigate } from "react-router-dom";
import {
  acceptOffer,
  OfferResponse,
  rejectOffer,
} from "../../services/auction";
import { Currency, OfferStatus } from "../../services/enums";
import "./styles/OfferCard.css";

type OfferCardProps = {
  offer: OfferResponse;
  currency?: Currency;
  onClose: () => void;
  refetchOffers: () => void;
};

const OfferCard = ({
  offer,
  currency,
  onClose,
  refetchOffers,
}: OfferCardProps) => {
  const navigate = useNavigate();

  const handleAccept = async () => {
    if (
      window.confirm(
        "Are you sure you want to accept this offer? This action cannot be undone."
      )
    ) {
      const success = await acceptOffer(offer.id);
      if (success) {
        onClose();
        navigate("/chat", {
          state: {
            selectedUser: {
              userId: offer.userId,
              userName: offer.userName,
              profilePhoto: `/api/user/${offer.userId}/profile-photo`,
            },
            input: `Hello, I am accepting your offer of ${offer.amount.toLocaleString(
              "en-US"
            )} ${
              currency !== undefined && Currency[currency]
            } for the artwork. Please provide your payment details so we can proceed with the transaction.
             If you have already made the payment, please share the evidence of payment to confirm the transaction. 
             Thank you!`,
          },
        });
      }
    }
  };

  const handleReject = async () => {
    const success = await rejectOffer(offer.id);
    if (success) refetchOffers();
  };

  return (
    <div className="oc-container">
      <p className="oc-top-offer">🔥 Top Offer</p>
      <span className="oc-status">{OfferStatus[offer.status]}</span>
      <p>@{offer.userName}</p>
      <p>
        {offer.amount.toLocaleString("en-US")}{" "}
        {currency !== undefined && Currency[currency]}
      </p>

      <div className="oc-actions">
        <button
          id="accept-offer"
          title="Accept"
          onClick={handleAccept}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          Accept
        </button>
        <button
          id="reject-offer"
          title="Reject"
          onClick={handleReject}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          Reject
        </button>
      </div>
    </div>
  );
};

export default OfferCard;
