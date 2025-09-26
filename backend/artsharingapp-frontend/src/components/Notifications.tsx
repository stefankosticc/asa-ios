import { useEffect, useRef, useState } from "react";
import { useNotifications } from "../hooks/useNotifications";
import { NotificationStatus } from "../services/enums";
import "../styles/Notifications.css";
import { BsCheck2All } from "react-icons/bs";
import { FaTrashAlt } from "react-icons/fa";
import {
  deleteNotification,
  markNotificationAsRead,
} from "../services/notifications";
import { useClickOutside } from "../hooks/useClickOutside";
import { useScroll } from "../hooks/useScroll";
const Notifications = ({ onClose }: { onClose: () => void }) => {
  const [refetchNotifications, setRefetchNotifications] =
    useState<boolean>(false);

  const {
    notifications,
    loadingNotifications,
    loadMoreNotifications,
    markAsReadLocally,
    deleteLocally,
  } = useNotifications(refetchNotifications);

  const notificationMenuRef = useRef<HTMLDivElement>(null);
  useClickOutside(notificationMenuRef, onClose);

  useScroll({
    ref: notificationMenuRef,
    storageKey: "notificationsScrollY",
    onReachBottom: loadMoreNotifications,
  });

  const persistScrollPosition = () => {
    const container = notificationMenuRef.current;
    if (container) {
      sessionStorage.setItem(
        "notificationsScrollY",
        container.scrollTop.toString()
      );
    }
  };

  const handleRead = async (notificationId: number) => {
    persistScrollPosition();
    const success = await markNotificationAsRead(notificationId);
    if (success) markAsReadLocally(notificationId);
  };

  const handleDelete = async (notificationId: number) => {
    persistScrollPosition();
    const success = await deleteNotification(notificationId);
    if (success) deleteLocally(notificationId);
  };

  return (
    <div className="notifications-menu" ref={notificationMenuRef}>
      {notifications.length === 0 && !loadingNotifications ? (
        <p className="notifications-no-results">No notifications found.</p>
      ) : (
        <>
          {notifications.map((notification) => (
            <div
              key={notification.id}
              className={`notification ${
                notification.status === NotificationStatus.UNREAD
                  ? "notification-unread"
                  : ""
              }`}
            >
              <p>{notification.text}</p>
              <div className="notification-actions">
                <div
                  className="notification-icon-wrapper"
                  onClick={() => handleRead(notification.id)}
                >
                  <BsCheck2All title="Mark as read" />
                </div>
                <div
                  className="notification-icon-wrapper"
                  onClick={() => handleDelete(notification.id)}
                >
                  <FaTrashAlt title="Delete" />
                </div>
              </div>

              {notification.status === NotificationStatus.UNREAD && (
                <div className="notification-dot"></div>
              )}
            </div>
          ))}

          {loadingNotifications && (
            <div className="notifications-no-results notifications-loader" />
          )}
        </>
      )}
    </div>
  );
};

export default Notifications;
