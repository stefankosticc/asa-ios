import { useState, useEffect } from "react";
import {
  getNotifications,
  NotificationResponse,
} from "../services/notifications";
import { NotificationStatus } from "../services/enums";

export const useNotifications = (refetch: boolean = false) => {
  const [notifications, setNotifications] = useState<NotificationResponse[]>(
    []
  );
  const [loadingNotifications, setLoadingNotifications] =
    useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 10;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchNotifications = async () => {
      try {
        setLoadingNotifications(true);
        const initialData = await getNotifications(0, take);
        if (!isCancelled) {
          setNotifications(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch notifications.");
          setNotifications([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingNotifications(false);
        }
      }
    };

    fetchNotifications();

    return () => {
      isCancelled = true;
    };
  }, [refetch]);

  const loadMoreNotifications = async () => {
    if (loadingNotifications || !hasMore) return;

    setLoadingNotifications(true);
    try {
      const olderNotifications = await getNotifications(skip, take);
      setNotifications((prev) => [...prev, ...olderNotifications]);
      setSkip((prev) => prev + olderNotifications.length);
      if (olderNotifications.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more notifications.");
    } finally {
      setLoadingNotifications(false);
    }
  };

  const markAsReadLocally = (notificationId: number) => {
    setNotifications((prev) =>
      prev.map((n) =>
        n.id === notificationId ? { ...n, status: NotificationStatus.READ } : n
      )
    );
  };

  const deleteLocally = (notificationId: number) => {
    setNotifications((prev) => prev.filter((n) => n.id !== notificationId));
  };

  return {
    notifications,
    loadingNotifications,
    loadMoreNotifications,
    markAsReadLocally,
    deleteLocally,
  };
};
