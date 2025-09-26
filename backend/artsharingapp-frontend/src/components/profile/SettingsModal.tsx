import { IoCloseCircleOutline } from "react-icons/io5";
import "./styles/SettingsModal.css";
import { useState } from "react";
import { logout, User } from "../../services/auth";
import { useNavigate } from "react-router-dom";
import AccountSettings from "./AccountSettings";

const TABS: { key: string }[] = [{ key: "Account" }];

type SettingsModalProps = {
  user: User;
  triggerRefetchUser: () => void;
  onClose: () => void;
};

const SettingsModal = ({
  user,
  triggerRefetchUser,
  onClose,
}: SettingsModalProps) => {
  const [selectedTab, setSelectedTab] = useState<string>(TABS[0].key);

  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await logout();
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      navigate("/login");
    } catch (error: any) {
      const errorMessage =
        error?.response?.data?.error || error?.message || "Log out failed.";
      console.error("Log out error:", errorMessage);
    }
  };

  const renderTabContent = () => {
    switch (selectedTab) {
      case "Account":
        return (
          <AccountSettings
            user={user}
            triggerRefetchUser={triggerRefetchUser}
            onClose={onClose}
          />
        );
      default:
        return null;
    }
  };
  return (
    <div className="blured-page">
      <div className="modal-container sm-container">
        <IoCloseCircleOutline
          className="sm-close"
          title="Close"
          onClick={onClose}
        />

        <div className="sm-content">
          <div className="sm-sidebar">
            {TABS.map((tab) => (
              <div
                key={tab.key}
                className={`sm-tab ${tab.key === selectedTab ? "active" : ""}`}
                onClick={() => setSelectedTab(tab.key)}
              >
                {tab.key}
              </div>
            ))}

            <div className="sm-tab logout-tab" onClick={handleLogout}>
              Log out
            </div>
          </div>

          <div className="sm-tab-content">{renderTabContent()}</div>
        </div>
      </div>
    </div>
  );
};

export default SettingsModal;
