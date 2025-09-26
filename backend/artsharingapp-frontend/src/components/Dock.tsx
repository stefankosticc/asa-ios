import { FiSearch } from "react-icons/fi";
import { FaMap, FaUser, FaPlusSquare } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import "../styles/Dock.css";
import { FaWindowMaximize } from "react-icons/fa6";
import { MdOutlineChatBubble } from "react-icons/md";
import { IoNotifications } from "react-icons/io5";
import { useEffect, useState } from "react";
import Search from "./search/Search";
import Notifications from "./Notifications";
import { useLoggedInUser } from "../hooks/useLoggedInUser";

const Dock = () => {
  const [isSearchOpen, setIsSearchOpen] = useState<boolean>(false);
  const [isNotificationsOpen, setIsNotificationsOpen] =
    useState<boolean>(false);

  const { loggedInUser } = useLoggedInUser();

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        setIsSearchOpen(false);
      }
    };

    if (isSearchOpen) {
      window.addEventListener("keydown", handleKeyDown);
    }

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [isSearchOpen]);

  return (
    <>
      {isSearchOpen && <Search onClose={() => setIsSearchOpen(false)} />}
      <div className="dock">
        <div className="notifications-container">
          <IoNotifications
            id="notifications-icon"
            onClick={(e) => {
              e.stopPropagation();
              if (isSearchOpen) setIsSearchOpen(false);
              setIsNotificationsOpen(!isNotificationsOpen);
            }}
          />
          {isNotificationsOpen && (
            <Notifications onClose={() => setIsNotificationsOpen(false)} />
          )}
        </div>
        <NavLink to={"/map"} title="Map">
          <FaMap />
        </NavLink>
        <NavLink to={"/discover"} title="Discover">
          <FaWindowMaximize />
        </NavLink>
        <div
          onClick={(e) => {
            e.stopPropagation();
            if (isNotificationsOpen) setIsNotificationsOpen(false);
            setIsSearchOpen(!isSearchOpen);
          }}
        >
          <FiSearch id="search-icon" />
        </div>
        <NavLink to={`/${loggedInUser?.userName}`} title="Profile">
          <FaUser />
        </NavLink>
        <NavLink to={"/artwork/new"} title="New Artwork">
          <FaPlusSquare />
        </NavLink>
        <NavLink to={"/chat"} title="Chat">
          <MdOutlineChatBubble />
        </NavLink>
      </div>
    </>
  );
};

export default Dock;
