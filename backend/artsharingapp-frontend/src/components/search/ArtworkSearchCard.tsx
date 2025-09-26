import "./styles/ArtworkSearchCard.css";
import { FaLandmark } from "react-icons/fa6";
import { FaCity } from "react-icons/fa";
import { ArtworkSearchResponse } from "../../services/artwork";
import { useNavigate } from "react-router-dom";
import { BACKEND_BASE_URL } from "../../config/constants";

const fallbackImage =
  "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500";

type ArtworkSearchCardProps = {
  artwork: ArtworkSearchResponse;
};

const ArtworkSearchCard = ({ artwork }: ArtworkSearchCardProps) => {
  const navigate = useNavigate();

  if (!artwork) return null;

  return (
    <div
      className="asc-container"
      onClick={() => navigate(`/artwork/${artwork.id}`)}
    >
      <div className="asc-img-container">
        <img
          src={`${BACKEND_BASE_URL}${artwork.image}` || fallbackImage}
          alt={artwork.title}
          onError={(e) => {
            (e.target as HTMLImageElement).src = fallbackImage;
          }}
          className="asc-img"
        />
      </div>
      <div className="asc-details">
        <p>{artwork.title}</p>
        <div className="asc-info">
          <span>{"@" + artwork.postedByUserName}</span>
          {artwork.cityName && (
            <span className="asc-info-with-icon">
              <FaCity />
              {artwork.cityName + ", " + artwork.country}
            </span>
          )}
          {artwork.galleryName && (
            <span className="asc-info-with-icon">
              <FaLandmark />
              {artwork.galleryName}
            </span>
          )}
        </div>
      </div>
      {artwork.isOnSale && <div className="asc-on-sale">ON SALE</div>}
    </div>
  );
};

export default ArtworkSearchCard;
