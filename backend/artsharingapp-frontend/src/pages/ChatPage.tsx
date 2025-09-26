import { useEffect, useRef, useState } from "react";
import chatService, { ChatMessage, ChatUser } from "../services/chat";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import "../styles/ChatPage.css";
import Dock from "../components/Dock";
import { ARTIST_FALLBACK_IMAGE, BACKEND_BASE_URL } from "../config/constants";
import { AiOutlineSend } from "react-icons/ai";
import { useScroll } from "../hooks/useScroll";
import { useChatUsers } from "../hooks/useChatUserConversations";
import { useLocation } from "react-router-dom";

const ChatPage = () => {
  const { loggedInUser } = useLoggedInUser();
  const location = useLocation();

  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [input, setInput] = useState(location.state?.input || "");
  const [isConnected, setIsConnected] = useState(false);

  const initialSelectedUser = location.state?.selectedUser as ChatUser | null;
  const [selectedUser, setSelectedUser] = useState<ChatUser | null>(
    initialSelectedUser || null
  );
  const [loading, setLoading] = useState(false);
  const [refetchChatUsers, setRefetchChatUsers] = useState(false);
  const [hasMoreMessages, setHasMoreMessages] = useState(true);

  const [skipMessages, setSkipMessages] = useState(0);
  const takeMessages = 50; // Number of messages to load

  const selectedUserRef = useRef<ChatUser | null>(null);
  const chatUsersContainerRef = useRef<HTMLDivElement>(null);
  const messagesContainerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    selectedUserRef.current = selectedUser;
  }, [selectedUser]);

  // Fetch users the logged-in user has messaged
  const { chatUsers, loadingChatUsers, loadMoreChatUsers } =
    useChatUsers(refetchChatUsers);

  useScroll({
    ref: chatUsersContainerRef,
    storageKey: "chatUsersScrollY",
    onReachBottom: loadMoreChatUsers,
  });

  // SignalR connection and listeners
  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken") || "";
    if (!accessToken) return;

    chatService.start(accessToken).then(() => {
      setIsConnected(true);
      chatService.onReceiveMessage(async (msg: ChatMessage) => {
        try {
          const currentSelectedUser = selectedUserRef.current;

          // Update messages if the sender is the selected user
          if (
            currentSelectedUser &&
            msg.senderId === currentSelectedUser.userId
          ) {
            setMessages((prev) => [msg, ...prev]);
            await chatService.markAsRead(msg.id);
          }
          setRefetchChatUsers((prev) => !prev);
        } catch (err) {
          console.error("Error processing received message:", err);
        }
      });

      chatService.onMessageSent((msg: ChatMessage) => {
        const currentSelectedUser = selectedUserRef.current;
        if (
          currentSelectedUser &&
          msg.receiverId === currentSelectedUser.userId
        ) {
          setMessages((prev) => [msg, ...prev]);
        }
        setRefetchChatUsers((prev) => !prev);
      });
    });

    return () => {
      chatService.stop();
      setIsConnected(false);
    };
  }, []);

  // Load chat history when user is selected
  useEffect(() => {
    const loadHistory = async () => {
      if (!selectedUser || !isConnected) return;

      setHasMoreMessages(true);
      setSkipMessages(0);
      setLoading(true);
      const history = await chatService.getChatHistory(
        selectedUser.userId,
        0,
        takeMessages
      );
      setMessages(history ? history.reverse() : []);
      setSkipMessages(history.length);
      setHasMoreMessages(history.length === takeMessages);
      setLoading(false);

      // Mark all unread messages from selectedUser as read
      if (history && loggedInUser) {
        const unreadMessages = history.filter(
          (msg: ChatMessage) =>
            msg.receiverId === loggedInUser.id && !msg.isRead
        );
        for (const msg of unreadMessages) {
          await chatService.markAsRead(msg.id);
        }
        setRefetchChatUsers((prev) => !prev);
      }
    };
    loadHistory();
  }, [selectedUser, isConnected]);

  const handleSend = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedUser || !input.trim() || !isConnected) return;
    try {
      await chatService.sendMessage(selectedUser.userId, input);
      setInput("");
    } catch (error) {
      console.error("Error sending message:", error);
    }
  };

  const loadMoreMessages = async () => {
    if (!selectedUser || loading || !hasMoreMessages || !isConnected) return;

    setLoading(true);
    try {
      const newMessages = await chatService.getChatHistory(
        selectedUser.userId,
        skipMessages,
        takeMessages
      );
      console.log(newMessages);
      setMessages((prev) => [...prev, ...(newMessages || []).reverse()]);
      setSkipMessages((prev) => prev + newMessages.length);
      if (newMessages.length < takeMessages) setHasMoreMessages(false);
    } catch (error) {
      console.error("Error loading more messages:", error);
    } finally {
      setLoading(false);
    }
  };

  // Load more messages when scrolled to top
  useScroll({
    ref: messagesContainerRef,
    storageKey: `chatMessagesScrollY-${selectedUser?.userId || ""}`,
    onReachBottom: loadMoreMessages,
    isReversed: true, // Scroll to top to load older messages
  });

  return (
    <div className="fixed-page">
      <div className="chat-page">
        <div className="chat-sidebar" ref={chatUsersContainerRef}>
          <h3>Your chats</h3>
          <div className="chat-users-list">
            {chatUsers.length === 0 && (
              <div className="chat-not-found">No conversations yet.</div>
            )}
            {chatUsers.map((user) => (
              <div
                key={user.userId}
                className={`chat-user-item${
                  selectedUser?.userId === user.userId ? " selected" : ""
                }`}
                onClick={() => setSelectedUser(user)}
              >
                <img
                  src={
                    `${BACKEND_BASE_URL}${user.profilePhoto}` ||
                    ARTIST_FALLBACK_IMAGE
                  }
                  alt={user.userName}
                  className="chat-user-profile-photo"
                  onError={(e) => {
                    (e.target as HTMLImageElement).src = ARTIST_FALLBACK_IMAGE;
                  }}
                />
                <div>
                  <p className="chat-user-name">{user.name}</p>
                  <p className="chat-user-username">@{user.userName}</p>
                </div>
                {user.unreadMessageCount > 0 && (
                  <p className="chat-unread-count">{user.unreadMessageCount}</p>
                )}
              </div>
            ))}
          </div>
        </div>
        <div className="chat-main">
          {selectedUser ? (
            <>
              <div className="chat-header">
                <img
                  src={
                    `${BACKEND_BASE_URL}${selectedUser.profilePhoto}` ||
                    ARTIST_FALLBACK_IMAGE
                  }
                  alt={selectedUser.userName}
                  className="chat-user-profile-photo"
                  onError={(e) => {
                    (e.target as HTMLImageElement).src = ARTIST_FALLBACK_IMAGE;
                  }}
                />
                <div>
                  <p className="chat-user-name">{selectedUser.name}</p>
                  <p className="chat-user-username">@{selectedUser.userName}</p>
                </div>
              </div>
              <div className="chat-messages" ref={messagesContainerRef}>
                {loading ? (
                  <div className="chat-loading loading-spinner"></div>
                ) : (
                  messages.map((msg) => (
                    <div
                      key={msg.id}
                      className={`chat-message${
                        msg.senderId === loggedInUser?.id
                          ? " sent"
                          : " received"
                      }`}
                    >
                      <span>{msg.message}</span>
                      <span className="chat-message-time">
                        {new Date(msg.sentAt).toLocaleTimeString([], {
                          hour: "2-digit",
                          minute: "2-digit",
                        })}
                      </span>
                    </div>
                  ))
                )}
              </div>
              <form className="chat-input-form" onSubmit={handleSend}>
                <input
                  type="textarea"
                  name="message"
                  value={input}
                  onChange={(e) => setInput(e.target.value)}
                  placeholder="Type a message..."
                  disabled={!selectedUser}
                />
                <button
                  type="submit"
                  title="Send"
                  disabled={!input.trim() || loading}
                >
                  <AiOutlineSend />
                </button>
              </form>
            </>
          ) : (
            <div className="chat-not-found">
              Select a conversation to start chatting.
            </div>
          )}
        </div>

        <Dock />
      </div>
    </div>
  );
};

export default ChatPage;
