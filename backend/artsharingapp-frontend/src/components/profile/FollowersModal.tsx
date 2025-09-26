import { IoCloseCircleOutline } from "react-icons/io5";
import "./styles/FollowersModal.css";
import { useEffect, useRef, useState } from "react";
import { useFollowers } from "../../hooks/useFollowers";
import ArtistSearchCard from "../search/ArtistSearchCard";
import { useNavigate } from "react-router-dom";
import { useScroll } from "../../hooks/useScroll";

type FollowersModalProps = {
  userId: number;
  tab?: "followers" | "following";
  onClose: () => void;
};

const FollowersModal = ({
  userId,
  tab = "followers",
  onClose,
}: FollowersModalProps) => {
  const [activeTab, setActiveTab] = useState<"followers" | "following">(tab);

  const navigate = useNavigate();

  const { users, loadingUsers, loadMoreUsers } = useFollowers(
    userId,
    activeTab
  );

  const followersModalRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: followersModalRef,
    storageKey: "followedUsersScrollY",
    onReachBottom: loadMoreUsers,
  });

  return (
    <div className="blured-page">
      <div className="modal-container fm-container" ref={followersModalRef}>
        <IoCloseCircleOutline
          className="fm-close"
          title="Close"
          onClick={onClose}
        />

        <div className="fm-nav-container">
          <button
            className={`${activeTab === "followers" && "active"}`}
            onClick={() => setActiveTab("followers")}
          >
            Followers
          </button>
          <button
            className={`${activeTab === "following" && "active"}`}
            onClick={() => setActiveTab("following")}
          >
            Following
          </button>
        </div>

        <div className="fm-users">
          {users && users.length !== 0 ? (
            users.map((user) => (
              <ArtistSearchCard
                key={user.id}
                artist={user}
                onClick={() => navigate(`/${user.userName}`)}
              />
            ))
          ) : loadingUsers ? (
            <div className="loading-spinner fm-no-results"></div>
          ) : (
            <p className="fm-no-results">
              {activeTab === "followers"
                ? "No followers yet."
                : "Not following anyone yet."}
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default FollowersModal;
