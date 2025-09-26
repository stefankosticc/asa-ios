import { useState } from "react";
import { NavLink } from "react-router-dom";
import "../styles/Login.css";
import "../styles/Auth.css";
import { IoChevronBackOutline } from "react-icons/io5";
import { login } from "../services/auth";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await login({ email, password });
      localStorage.setItem("accessToken", response.accessToken);
      localStorage.setItem("refreshToken", response.refreshToken);
      navigate("/discover");
    } catch (err: any) {
      const message: string =
        err?.response?.data?.error || "Invalid email or password";
      setError(message);
      setPassword("");
    }
  };

  return (
    <div className="fixed-page">
      <div className="login-page">
        <NavLink to="/" className="auth-back-link">
          <IoChevronBackOutline />
        </NavLink>

        <div className="auth-card">
          <h1>Log in</h1>

          <div className="third-party-auth">
            <button>
              <img
                src="/pictures/google-icon.png"
                alt="Google Icon"
                className="third-party-auth-icon"
              />
              Log in with Google
            </button>
          </div>

          <div className="auth-or-divider">
            <hr className="auth-divider" />
            <span className="auth-or-text">or</span>
            <hr className="auth-divider" />
          </div>

          <form className="auth-form" onSubmit={handleSubmit}>
            <div className="auth-form-field">
              <label htmlFor="email">Email</label>
              <input
                type="email"
                id="email"
                name="email"
                required
                placeholder=" "
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="username"
              />
            </div>

            <div className="auth-form-field">
              <label htmlFor="password">Password</label>
              <input
                type="password"
                id="password"
                name="password"
                required
                placeholder=" "
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
              />
              <p id="login-forgot-password">Forgot password?</p>
            </div>

            {error && <p className="login-error">{error}</p>}

            <p id="login-go-to-signup">
              Don't have an account?{" "}
              <NavLink to="/sign-up" className="login-signup-link">
                Sign up
              </NavLink>
            </p>

            <button type="submit">Log in</button>
          </form>
        </div>

        <div className="login-placeholder"></div>
      </div>
    </div>
  );
};

export default Login;
