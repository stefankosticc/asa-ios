import { FaCity } from "react-icons/fa";
import "../styles/CityPage.css";
import Dock from "../components/Dock";
import { useParams } from "react-router-dom";
import ArtworkCard from "../components/ArtworkCard";
import { useCity } from "../hooks/useCity";
import GalleryCard from "../components/GalleryCard";
import { useCityArtworks } from "../hooks/useCityArtworks";
import { useCityGalleries } from "../hooks/useCityGalleries";

const CityPage = () => {
  const { cityId } = useParams();

  const { city } = useCity(cityId ? parseInt(cityId) : -1);
  const { cityArtworks, loadingCityArtworks } = useCityArtworks(
    cityId ? parseInt(cityId) : -1
  );
  const { cityGalleries, loadingCityGalleries } = useCityGalleries(
    cityId ? parseInt(cityId) : -1
  );

  return (
    <div className="fixed-page">
      <div className="city-page">
        <div className="cp-city-header">
          <div className="cp-icon-wrapper">
            <FaCity />
            <span className="cp-city-name">
              {city?.name}
              <span className="cp-country">
                {city?.country ? `, ${city.country}` : ""}
              </span>
            </span>
          </div>
        </div>

        <div className="cp-content">
          {/* GALLERIES */}
          <h3>Galleries in {city?.name}</h3>
          <div className="cp-city-galleries">
            {cityGalleries ? (
              cityGalleries?.map((gallery) => (
                <GalleryCard
                  key={gallery.id}
                  gallery={gallery}
                  loading={loadingCityGalleries}
                />
              ))
            ) : (
              <p className="cp-city-galleries-not-found">
                No galleries have been added for this city yet.
              </p>
            )}
          </div>

          {/* ARTWORKS */}
          <div className="cp-artwork-grid">
            {cityArtworks?.map((artwork) => (
              <ArtworkCard
                key={artwork.id}
                artwork={artwork}
                loading={loadingCityArtworks}
              />
            ))}
          </div>
        </div>

        <Dock />
      </div>
    </div>
  );
};

export default CityPage;
