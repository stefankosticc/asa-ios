import { useEffect, useRef, useState } from "react";
import Dock from "../components/Dock";
import TopArtistsSection from "../components/discover-page/TopArtistsSection";
import { useScroll } from "../hooks/useScroll";
import "../styles/DiscoverPage.css";
import { NavLink } from "react-router-dom";
import { useDiscoverArtworks } from "../hooks/useDiscoverArtworks";
import ArtworkFeedCard from "../components/discover-page/ArtworkFeedCard";
import { DiscoverData, getDiscoverData } from "../services/discover";
import HighStakesAuctionsSection from "../components/discover-page/HighStakesAuctionSection";
import TrendingArtworksSection from "../components/discover-page/TrendingArtworksSection";

const DiscoverPage = () => {
  const [discoverData, setDiscoverData] = useState<DiscoverData | null>(null);
  const [loadingDiscoverData, setLoadingDiscoverData] =
    useState<boolean>(false);

  const { artworks, loadingArtworks, loadMoreArtworks } =
    useDiscoverArtworks("discover");

  const popularArtworksRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: popularArtworksRef,
    storageKey: "followedUsersArtworksScrollY",
    onReachBottom: loadMoreArtworks,
  });

  useEffect(() => {
    let isCancelled = false;

    const fetchDiscoverData = async () => {
      try {
        setLoadingDiscoverData(true);
        const response = await getDiscoverData();
        if (!isCancelled) {
          setDiscoverData(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch discover data.");
          setDiscoverData(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingDiscoverData(false);
        }
      }
    };

    fetchDiscoverData();

    return () => {
      isCancelled = true;
    };
  }, []);

  return (
    <div className="fixed-page">
      <div className="discover-page">
        <div className="discover-nav-container">
          <NavLink to={"/discover"} className={"active"}>
            Discover
          </NavLink>
          <NavLink to={"/following"}>Following</NavLink>
        </div>

        {loadingDiscoverData ? (
          <div className="loading-discover-data">
            <div className="discover-loader" />
          </div>
        ) : (
          <>
            {discoverData?.topArtistsByLikes && (
              <>
                <h1>🧑‍🎨 Top Artists</h1>
                <TopArtistsSection artists={discoverData.topArtistsByLikes} />
              </>
            )}

            {discoverData?.highStakeAuctions && (
              <>
                <h2>🔥 High Stakes Auctions</h2>
                <HighStakesAuctionsSection
                  auctions={discoverData.highStakeAuctions}
                />
              </>
            )}

            {discoverData?.trendingArtworks && (
              <>
                <h2>✨ On The Rise</h2>
                <TrendingArtworksSection
                  artworks={discoverData.trendingArtworks}
                />
              </>
            )}

            <h2>Fresh Finds</h2>
            <div className="discover-feed">
              {artworks.map((artwork) => (
                <ArtworkFeedCard artwork={artwork} key={artwork.id} />
              ))}

              {!loadingArtworks && artworks.length === 0 && (
                <p className="discover-no-results">No artworks found.</p>
              )}

              {loadingArtworks && <div className="discover-loader" />}
            </div>
          </>
        )}
      </div>

      <Dock />
    </div>
  );
};

export default DiscoverPage;
