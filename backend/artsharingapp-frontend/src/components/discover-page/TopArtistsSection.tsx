import { useNavigate } from "react-router-dom";
import {
  ARTIST_FALLBACK_IMAGE,
  BACKEND_BASE_URL,
} from "../../config/constants";
import { TopArtistResponse } from "../../services/discover";
import "./styles/TopArtistsSection.css";

type TopArtistsSectionProps = {
  artists: TopArtistResponse[];
};

const TopArtistsSection = ({ artists }: TopArtistsSectionProps) => {
  const navigate = useNavigate();

  return (
    <div className="top-artists-container">
      {artists.map((artist) => (
        <div
          className="top-artist"
          key={artist.id}
          onClick={() => navigate(`/${artist.userName}`)}
        >
          <img
            src={
              artist.profilePhoto
                ? `${BACKEND_BASE_URL}${artist.profilePhoto}`
                : ARTIST_FALLBACK_IMAGE
            }
            alt={artist.name}
            onError={(e) => {
              e.currentTarget.src = ARTIST_FALLBACK_IMAGE;
            }}
          />
          <p>{artist.name}</p>
        </div>
      ))}
    </div>
  );
};

export default TopArtistsSection;
