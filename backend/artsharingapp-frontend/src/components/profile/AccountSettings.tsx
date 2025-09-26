import { useState } from "react";
import { User } from "../../services/auth";
import {
  updateUserProfile,
  UpdateUserProfileRequest,
} from "../../services/user";
import {
  ARTIST_FALLBACK_IMAGE,
  BACKEND_BASE_URL,
} from "../../config/constants";
import { HiArrowPathRoundedSquare } from "react-icons/hi2";
import "./styles/AccountSettings.css";
import "./styles/SettingsModal.css";

type AccountSettingsProps = {
  user: User;
  triggerRefetchUser: () => void;
  onClose: () => void;
};

const AccountSettings = ({
  user,
  triggerRefetchUser,
  onClose,
}: AccountSettingsProps) => {
  const [profilePhoto, setProfilePhoto] = useState<string>(
    `${BACKEND_BASE_URL}${user.profilePhoto}?t=${Date.now()}`
  ); // for img tag
  const [profilePhotoFile, setProfilePhotoFile] = useState<File | null>(null); // for backend
  const [editingUserData, setEditingUserData] =
    useState<UpdateUserProfileRequest>({
      name: user.name,
      removePhoto: false,
    });

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setProfilePhotoFile(file);
    if (file) {
      const url = URL.createObjectURL(file);
      setProfilePhoto(url);
      setEditingUserData((prev) => ({ ...prev, removePhoto: false }));
    }
  };

  const handleInputChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    const { name, value } = e.target;
    setEditingUserData((prev) => ({ ...prev, [name]: value }));
  };

  const handleCancel = () => {
    setEditingUserData({
      name: user.name,
      removePhoto: false,
    });
    setProfilePhoto(`${BACKEND_BASE_URL}${user.profilePhoto}?t=${Date.now()}`);
    onClose();
  };

  const handleSave = async () => {
    if (!editingUserData.name.trim()) {
      alert("Name is required.");
      return;
    }
    if (editingUserData.name.length > 50) {
      alert("Name must be under 50 characters.");
      return;
    }

    await updateUserProfile(editingUserData, profilePhotoFile);
    triggerRefetchUser();
    onClose();
  };

  return (
    <div className="tab-panel">
      <h5>Account Settings</h5>
      <p>Update your profile here.</p>

      <div className="account-settings-container">
        <div className="settings-img-wrapper">
          <img
            src={profilePhoto}
            alt={""}
            onError={() => setProfilePhoto(ARTIST_FALLBACK_IMAGE)}
            className="settings-image"
          />
          <div
            className="settings-image settings-replace-image-overlay"
            title="Replace profile photo"
          >
            <HiArrowPathRoundedSquare />
            <input
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              className="settings-file-input-overlay"
            />
          </div>
        </div>

        <button
          id="remove-profile-photo"
          title="Remove profile photo"
          onClick={() => {
            setProfilePhoto(ARTIST_FALLBACK_IMAGE);
            setProfilePhotoFile(null);
            setEditingUserData((prev) => ({ ...prev, removePhoto: true }));
          }}
        >
          Remove
        </button>

        <div className="account-info">
          <label htmlFor="name" className="account-info-field">
            Name:{" "}
            <input
              type="text"
              name="name"
              id="name"
              value={editingUserData.name}
              placeholder={user.name}
              onChange={handleInputChange}
            />
          </label>
          <p className="account-info-field">
            Username: <span>@{user.userName}</span>
          </p>
          <p className="account-info-field">
            Email: <span>{user.email}</span>
          </p>

          <div className="account-btns">
            <button onClick={handleCancel}>Cancel</button>
            <button id="account-info-save-btn" onClick={handleSave}>
              Save
            </button>
          </div>
        </div>
      </div>

      <button id="delete-account-btn">Delete Account</button>
      <p>Permanently delete your account</p>
    </div>
  );
};

export default AccountSettings;
