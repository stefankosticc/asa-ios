import { JSX, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import Loading from "../pages/Loading";
import authAxios from "../services/authAxios";

function PrivateRoute({ children }: { children: JSX.Element }) {
  const [auth, setAuth] = useState<boolean | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      authAxios
        .get("/auth/loggedin-user")
        .then(() => {
          setAuth(true);
          setLoading(false);
        })
        .catch(() => {
          setAuth(false);
          setLoading(false);
        });
    } else {
      setAuth(false);
      setLoading(false);
    }
  }, []);

  if (loading) {
    return <Loading />;
  }

  return auth ? children : <Navigate to="/login" />;
}

export default PrivateRoute;
