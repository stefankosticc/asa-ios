import { useState, useEffect } from "react";
import chatService, { ChatUser } from "../services/chat";

export const useChatUsers = (refetch: boolean = false) => {
  const [chatUsers, setChatUsers] = useState<ChatUser[]>([]);
  const [loadingChatUsers, setLoadingChatUsers] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 30;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchChatUsers = async () => {
      try {
        setLoadingChatUsers(true);
        const initialData = await chatService.getConversations(0, take);
        if (!isCancelled) {
          setChatUsers(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch conversations.");
          setChatUsers([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingChatUsers(false);
        }
      }
    };

    fetchChatUsers();

    return () => {
      isCancelled = true;
    };
  }, [refetch]);

  const loadMoreChatUsers = async () => {
    if (loadingChatUsers || !hasMore) return;

    setLoadingChatUsers(true);
    try {
      const olderConversations = await chatService.getConversations(skip, take);
      setChatUsers((prev) => [...prev, ...olderConversations]);
      setSkip((prev) => prev + olderConversations.length);
      if (olderConversations.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more conversations.");
    } finally {
      setLoadingChatUsers(false);
    }
  };

  return {
    chatUsers,
    loadingChatUsers,
    loadMoreChatUsers,
  };
};
