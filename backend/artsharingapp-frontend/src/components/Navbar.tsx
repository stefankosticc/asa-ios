import { NavLink } from "react-router-dom";
import "../styles/Navbar.css";

const Navbar = () => {
  const handleScroll = (e: React.MouseEvent, id: string) => {
    e.preventDefault();
    const element = document.getElementById(id);
    const scrollContainer = document.querySelector(".landing-page");
    if (element && scrollContainer) {
      const offset = 8 * 16;
      const elementPosition =
        element.getBoundingClientRect().top + scrollContainer.scrollTop;
      scrollContainer.scrollTo({
        top: element.offsetTop - offset,
        behavior: "smooth",
      });
    }
  };

  return (
    <div className="navbar">
      <div className="navbar-links">
        <a href="#lp-header" onClick={(e) => handleScroll(e, "lp-header")}>
          Home
        </a>
        <a href="#features" onClick={(e) => handleScroll(e, "features")}>
          Features
        </a>
        <a href="#faq" onClick={(e) => handleScroll(e, "faq")}>
          FAQ
        </a>
        <a href="#contact" onClick={(e) => handleScroll(e, "contact")}>
          Contact
        </a>
      </div>
      <b className="navbar-logo navbar-center">
        <img src="/pictures/logo.svg" alt="" />
        ASA
      </b>
      <div className="navbar-login">
        <NavLink className="navbar-btn" to="/login">
          Log in
        </NavLink>
        <NavLink className="navbar-btn" to="/sign-up">
          Sign up
        </NavLink>
      </div>
    </div>
  );
};

export default Navbar;
