import { useState, useEffect } from "react";
import { getLoggedInUser, User } from "../services/auth";

export const useLoggedInUser = () => {
  const [loggedInUser, setLoggedInUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLoggedInUser = async () => {
      try {
        const loggedInUser = await getLoggedInUser();
        setLoggedInUser(loggedInUser);
      } catch (err) {
        setError("Failed to fetch logged-in user");
      } finally {
        setTimeout(() => {
          setLoading(false);
        }, 2000); //TODO: Simulate a delay for loading state DELETE THIS LATER
      }
    };

    fetchLoggedInUser();
  }, []);

  return { loggedInUser, loading, error };
};
