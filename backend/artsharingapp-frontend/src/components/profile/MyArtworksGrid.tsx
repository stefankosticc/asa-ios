import { useEffect, useState } from "react";
import { getUserArtworks, UserArtworksResponse } from "../../services/artwork";
import ArtworkCard from "../ArtworkCard";
import "./styles/MyArtworksGrid.css";
import { User } from "../../services/auth";
import { CiLock, CiUnlock } from "react-icons/ci";

interface MyArtworksGridProps {
  activeTab: string;
  user: User | null;
  isMyProfile: boolean;
}

const MyArtworksGrid = ({
  activeTab,
  user,
  isMyProfile,
}: MyArtworksGridProps) => {
  const [artworks, setArtworks] = useState<UserArtworksResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [showPublicArtworks, setShowPublicArtworks] = useState<boolean>(true);

  useEffect(() => {
    if (!user) return;

    const fetchArtworks = async () => {
      try {
        setLoading(true);
        const response = await getUserArtworks(user.id);
        setArtworks(response);
      } catch (err) {
        setLoading(true);
      } finally {
        setLoading(false);
      }
    };

    fetchArtworks();
  }, [user]);

  if (loading) {
    return <div className="profile-artwork-grid-loader"></div>;
  }

  return (
    <div
      className={`profile-content${activeTab === "artworks" ? " active" : ""}`}
    >
      {artworks &&
      (artworks.publicArtworks.length !== 0 ||
        artworks.privateArtworks.length !== 0) ? (
        <div className="profile-artwork-grid">
          {isMyProfile && (
            <div
              className="visibility-artworks-card"
              onClick={() => setShowPublicArtworks((prev) => !prev)}
            >
              {showPublicArtworks ? (
                <CiLock title="Show Private Artworks" />
              ) : (
                <CiUnlock
                  title="Show Public Artwroks"
                  id="public-artworks-card"
                />
              )}
            </div>
          )}
          {(showPublicArtworks
            ? artworks.publicArtworks
            : artworks.privateArtworks
          ).map((artwork) => (
            <ArtworkCard key={artwork.id} artwork={artwork} loading={loading} />
          ))}
        </div>
      ) : (
        <p className="profile-content-text not-found">
          You have no artworks yet.
        </p>
      )}
    </div>
  );
};

export default MyArtworksGrid;
