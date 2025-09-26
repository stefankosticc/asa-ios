import { NavLink, useNavigate } from "react-router-dom";
import "../styles/SignUp.css";
import "../styles/Auth.css";
import { IoChevronBackOutline } from "react-icons/io5";
import { useState } from "react";
import { signUp, SignUpRequest } from "../services/auth";
import { toast } from "react-toastify";

const SignUp = () => {
  const [name, setName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  type SignUpErrors = {
    confirmPassword?: string;
    general?: string;
  };

  const [errors, setErrors] = useState<SignUpErrors>({});
  const navigate = useNavigate();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setErrors({ confirmPassword: "Passwords do not match" });
      return;
    }

    const requestData: SignUpRequest = {
      name,
      email,
      userName,
      password,
    };

    try {
      await signUp(requestData);
      navigate("/login");
      toast.success("Registration successful! Please log in.", {
        position: "top-center",
      });
    } catch (error: any) {
      const errorMessage =
        error?.response?.data?.error ||
        error?.message ||
        "Registration failed.";
      console.error("Registration error:", errorMessage);
      setErrors({ general: errorMessage });
    }
  };

  return (
    <div className="fixed-page">
      <div className="sign-up-page">
        <NavLink to="/" className="auth-back-link">
          <IoChevronBackOutline />
        </NavLink>

        <div className="signup-placeholder"></div>

        <div className="auth-card" onSubmit={handleRegister}>
          <h1>Sign up</h1>

          <div className="third-party-auth">
            <button>
              <img
                src="/pictures/google-icon.png"
                alt="Google Icon"
                className="third-party-auth-icon"
              />
              Sign up with Google
            </button>
          </div>

          <div className="auth-or-divider">
            <hr className="auth-divider" />
            <span className="auth-or-text">or</span>
            <hr className="auth-divider" />
          </div>

          <form className="auth-form">
            <div className="auth-form-field">
              <label htmlFor="name">Name</label>
              <input
                type="text"
                id="name"
                name="name"
                required
                placeholder=" "
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </div>

            <div className="auth-form-field">
              <label htmlFor="username">Username</label>
              <input
                type="text"
                id="username"
                name="username"
                required
                placeholder=" "
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                autoComplete="username"
              />
            </div>

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
                autoComplete="new-password"
              />
            </div>

            <div className="auth-form-field">
              <label htmlFor="confirm-password">Confirm Password</label>
              <input
                type="password"
                id="confirm-password"
                name="confirm-password"
                required
                placeholder=" "
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                autoComplete="new-password"
              />
              {errors.confirmPassword && (
                <p className="sign-up-error">{errors.confirmPassword}</p>
              )}
            </div>

            <div>
              <p id="signup-go-to-login">
                Already have an account?{" "}
                <NavLink to="/login" className="sign-up-login-link">
                  Log in
                </NavLink>
              </p>
            </div>

            {errors.general && (
              <p className="sign-up-error">{errors.general}</p>
            )}

            <button type="submit">Sign up</button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default SignUp;
