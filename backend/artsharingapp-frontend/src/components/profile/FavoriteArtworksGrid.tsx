import { useFavoriteArtworks } from "../../hooks/useFavoriteArtworks";
import ArtworkCard from "../ArtworkCard";
import { User } from "../../services/auth";

interface FavoriteArtworksGridProps {
  activeTab: string;
  user: User | null;
}

const FavoriteArtworksGrid = ({
  activeTab,
  user,
}: FavoriteArtworksGridProps) => {
  const { favorites, loadingFavorites } = useFavoriteArtworks({
    likedByUser: user,
    activeTab,
  });

  return (
    <div
      className={`profile-content${activeTab === "favorites" ? " active" : ""}`}
    >
      {favorites && favorites.length !== 0 ? (
        <div className="profile-artwork-grid">
          {favorites.map((favorite) => (
            <ArtworkCard
              key={favorite.artworkId}
              artwork={favorite}
              loading={loadingFavorites}
            />
          ))}
        </div>
      ) : (
        <p className="profile-content-text not-found">
          You have no favorites yet.
        </p>
      )}
    </div>
  );
};

export default FavoriteArtworksGrid;
