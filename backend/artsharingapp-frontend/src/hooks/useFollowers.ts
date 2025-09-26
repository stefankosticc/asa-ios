import { useState, useEffect } from "react";
import { UserSearchResponse } from "../services/user";
import { getFollowers, getFollowing } from "../services/followers";

export function useFollowers(userId: number, type: "followers" | "following") {
  const [users, setUsers] = useState<UserSearchResponse[]>([]);
  const [loadingUsers, setLoadingUsers] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 30;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchFollowers = async () => {
      try {
        setLoadingUsers(true);
        console.log("first");
        const initialData =
          type === "followers"
            ? await getFollowers(userId, 0, take)
            : await getFollowing(userId, 0, take);
        if (!isCancelled) {
          setUsers(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch followers/following.");
          setUsers([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingUsers(false);
        }
      }
    };

    fetchFollowers();

    return () => {
      isCancelled = true;
    };
  }, [userId, type]);

  const loadMoreUsers = async () => {
    console.log("scroll");
    if (loadingUsers || !hasMore) return;

    setLoadingUsers(true);
    try {
      console.log("second");
      const newUsers =
        type === "followers"
          ? await getFollowers(userId, skip, take)
          : await getFollowing(userId, skip, take);
      setUsers((prev) => [...prev, ...newUsers]);
      setSkip((prev) => prev + newUsers.length);
      if (newUsers.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more users.");
    } finally {
      setLoadingUsers(false);
    }
  };

  return {
    users,
    loadingUsers,
    loadMoreUsers,
  };
}
