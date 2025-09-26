import { FaLandmark } from "react-icons/fa6";
import "../styles/GalleryPage.css";
import Dock from "../components/Dock";
import { useNavigate, useParams } from "react-router-dom";
import { useGallery } from "../hooks/useGallery";
import { useGalleryArtworks } from "../hooks/useGalleryArtworks";
import ArtworkCard from "../components/ArtworkCard";

const GalleryPage = () => {
  const { galleryId } = useParams();
  const navigate = useNavigate();

  const { gallery } = useGallery(galleryId ? parseInt(galleryId) : -1);
  const { galleryArtworks, loadingGalleryArtworks } = useGalleryArtworks(
    galleryId ? parseInt(galleryId) : -1
  );

  return (
    <div className="fixed-page">
      <div className="gallery-page">
        <div className="gp-gallery-header">
          <div className="gp-icon-wrapper">
            <FaLandmark />
            <span>{gallery?.name}</span>
          </div>
        </div>

        <div className="gp-content">
          <div className="gp-info">
            <p
              className="gp-city-name"
              onClick={() => {
                if (gallery?.cityId) navigate(`/city/${gallery.cityId}`);
              }}
            >
              {gallery?.cityName}
            </p>
            <p>{gallery?.address}</p>
          </div>

          <div className="gp-artwork-grid">
            {galleryArtworks?.map((artwork) => (
              <ArtworkCard
                key={artwork.id}
                artwork={artwork}
                loading={loadingGalleryArtworks}
              />
            ))}
          </div>
        </div>

        <Dock />
      </div>
    </div>
  );
};

export default GalleryPage;
