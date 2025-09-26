import { FaLandmark } from "react-icons/fa6";
import { Gallery } from "../services/gallery";
import "../styles/GalleryCard.css";
import { useNavigate } from "react-router-dom";

type GalleryCardProps = {
  gallery: Gallery;
  loading?: boolean;
};

const GalleryCard = ({ gallery, loading = false }: GalleryCardProps) => {
  const navigate = useNavigate();

  if (loading) {
    return <div className="gallery-card-container gallery-card-skeleton"></div>;
  }

  return (
    <div
      className="gallery-card-container"
      onClick={() => navigate(`/gallery/${gallery.id}`)}
    >
      <FaLandmark className="gallery-card-icon" />
      <div className="gallery-card-name">
        <p>{gallery?.name ?? "-"}</p>
      </div>
    </div>
  );
};

export default GalleryCard;
