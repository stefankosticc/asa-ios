import { useNavigate } from "react-router-dom";
import { Artwork } from "../../services/artwork";
import { Currency } from "../../services/enums";
import "./styles/AuctionSection.css";
import "./styles/FixedSaleSection.css";
import { AiOutlineSend } from "react-icons/ai";

type FixedSaleSectionProps = {
  artwork: Artwork;
};

const FixedSaleSection = ({ artwork }: FixedSaleSectionProps) => {
  const navigation = useNavigate();

  return (
    <div className="auction-section-container fixed-sale-section-container">
      <h4>On Sale</h4>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>Price</p>
          <p className="auction-section-column-info">
            {artwork.price?.toLocaleString("en-US")}{" "}
            {Currency[artwork.currency]}
          </p>
        </div>
        <div className="auction-section-column">
          <p>Send a message</p>
          <div className="auction-section-offer fixed-sale-section-message">
            <button
              title="Send a message"
              onClick={() =>
                navigation("/chat", {
                  state: {
                    selectedUser: {
                      userId: artwork.postedByUserId,
                      userName: artwork.postedByUserName,
                      profilePhoto: `/api/user/${artwork.postedByUserId}/profile-photo`,
                    },
                    input: `Hello, I am interested in purchasing your artwork "${artwork.title}". Could you please provide more details?`,
                  },
                })
              }
            >
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FixedSaleSection;
