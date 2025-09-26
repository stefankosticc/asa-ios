import { NavLink } from "react-router-dom";
import Dock from "../components/Dock";
import "../styles/FollowingPage.css";
import { useDiscoverArtworks } from "../hooks/useDiscoverArtworks";
import { useRef } from "react";
import { useScroll } from "../hooks/useScroll";
import ArtworkFeedCard from "../components/discover-page/ArtworkFeedCard";

const FollowingPage = () => {
  const { artworks, loadingArtworks, loadMoreArtworks } =
    useDiscoverArtworks("following");

  const followedUsersArtworksRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: followedUsersArtworksRef,
    storageKey: "followedUsersArtworksScrollY",
    onReachBottom: loadMoreArtworks,
  });

  return (
    <div className="fixed-page">
      <div className="following-page" ref={followedUsersArtworksRef}>
        <div className="discover-nav-container">
          <NavLink to={"/discover"}>Discover</NavLink>
          <NavLink to={"/following"} className={"active"}>
            Following
          </NavLink>
        </div>

        <h1>Following</h1>
        <div className="following-feed">
          {artworks.map((artwork) => (
            <ArtworkFeedCard artwork={artwork} key={artwork.id} />
          ))}

          {!loadingArtworks && artworks.length === 0 && (
            <p className="fp-no-results">No artworks found.</p>
          )}

          {loadingArtworks && <div className="fp-loader" />}
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default FollowingPage;
