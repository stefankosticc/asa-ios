import { useEffect, useState } from "react";
import "../styles/Profile.css";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import Dock from "../components/Dock";
import TextEditor from "../components/TextEditor";
import { MdEdit } from "react-icons/md";
import { getUserByUsername, updateUserBiography } from "../services/user";
import UserInfo from "../components/profile/UserInfo";
import MyArtworksGrid from "../components/profile/MyArtworksGrid";
import FavoriteArtworksGrid from "../components/profile/FavoriteArtworksGrid";
import { useParams } from "react-router-dom";
import NotFound from "./NotFound";
import { User } from "../services/auth";
import Loading from "./Loading";

const TABS: {
  key: string;
  label: string;
}[] = [
  { key: "artworks", label: "Artworks" },
  { key: "favorites", label: "Favorites" },
  { key: "biography", label: "Biography" },
];

const Profile = () => {
  const { username } = useParams();
  const [activeTab, setActiveTab] = useState<string>("artworks");
  const [isEditingBiography, setIsEditingBiography] = useState<boolean>(false);
  const [biography, setBiography] = useState<string>("");
  const [profileUser, setProfileUser] = useState<User | null>(null);
  const [loadingProfileUser, setLoadingProfileUser] = useState<boolean>(true);
  const [error, setError] = useState<unknown>(null);
  const [refetchUser, setRefetchUser] = useState(0);

  const { loggedInUser, loading: loadingLoggedInUser } = useLoggedInUser();

  useEffect(() => {
    const fetchProfileUser = async () => {
      if (!username) {
        setError("No username provided");
        setLoadingProfileUser(false);
        return;
      }
      try {
        setLoadingProfileUser(true);
        const user = await getUserByUsername(username);
        if (!user) {
          setError("User not found");
          setProfileUser(null);
        } else {
          setProfileUser(user);
          setBiography(user?.biography || "");
        }
      } catch (err) {
        setError(err);
        setProfileUser(null);
      } finally {
        setLoadingProfileUser(false);
      }
    };

    fetchProfileUser();
  }, [username, refetchUser]);

  const triggerRefetchUser = () => setRefetchUser((prev) => prev + 1);

  // Determine if the profile belongs to the logged-in user
  const isMyProfile = loggedInUser?.userName === username;

  if (!loadingProfileUser && (!profileUser || error)) {
    return <NotFound />;
  }

  if (loadingProfileUser || loadingProfileUser) {
    return <Loading />;
  }

  return (
    <div className="profile-page page">
      {/* USER INFO */}
      <UserInfo
        user={profileUser}
        setUser={setProfileUser}
        triggerRefetchUser={triggerRefetchUser}
        loading={loadingProfileUser}
        loggedInUser={loggedInUser}
        isMyProfile={isMyProfile}
      />

      <div className="profile-divider"></div>

      {/* TABS */}
      <div className="profile-content-container">
        <div className="profile-tabs">
          {TABS.map((tab) => (
            <button
              key={tab.key}
              className={`profile-tab${activeTab === tab.key ? " active" : ""}`}
              onClick={() => setActiveTab(tab.key)}
              type="button"
              title={tab.label + " tab"}
            >
              {tab.label}
            </button>
          ))}
        </div>

        {/* MY ARTWORKS */}
        <MyArtworksGrid
          activeTab={activeTab}
          user={profileUser}
          isMyProfile={isMyProfile}
        />

        {/* FAVORITES */}
        <FavoriteArtworksGrid activeTab={activeTab} user={profileUser} />

        {/* BIOPGRAPHY */}
        <div
          className={`profile-content${
            activeTab === "biography" ? " active" : ""
          }`}
        >
          {(biography !== "" && biography !== "<p></p>") ||
          isEditingBiography ? (
            <div className="biography-container">
              <TextEditor
                content={biography}
                editable={isEditingBiography && isMyProfile}
                className={`profile-content-biography ${
                  isEditingBiography ? "text-editor-editing" : ""
                }`}
                onUpdate={({ editor }) => {
                  if (isEditingBiography) setBiography(editor.getHTML());
                }}
              />
              {isMyProfile && (
                <MdEdit
                  className="biography-edit-icon"
                  onClick={() => setIsEditingBiography(!isEditingBiography)}
                  title="Edit"
                />
              )}
            </div>
          ) : isMyProfile ? (
            <p className="profile-content-text not-found">
              You haven't added a biography yet. <br />
              To add one, click the "Edit" button below. <br />
              <button
                className="biography-editing-button"
                onClick={() => setIsEditingBiography(true)}
              >
                Edit
              </button>
            </p>
          ) : (
            <p className="profile-content-text not-found">
              Biography has not been added yet.
            </p>
          )}
          {isEditingBiography && isMyProfile && (
            <div className="biography-buttons-container">
              <button
                className="biography-editing-button"
                id="biography-cancel-button"
                onClick={() => {
                  setBiography(profileUser?.biography || "");
                  setIsEditingBiography(false);
                }}
                title="Cancel"
              >
                Cancel
              </button>
              <button
                className="biography-editing-button"
                id="biography-save-button"
                onClick={async () => {
                  await updateUserBiography({ biography });
                  setIsEditingBiography(false);
                }}
                title="Save changes"
              >
                Save
              </button>
            </div>
          )}
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default Profile;
