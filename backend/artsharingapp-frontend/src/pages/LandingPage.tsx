import "../styles/LandingPage.css";
import Navbar from "../components/Navbar";
import { NavLink } from "react-router-dom";
import Contact from "../components/landing-page/Contact";
import FAQ from "../components/landing-page/FAQ";
import Features from "../components/landing-page/Features";

const LandingPage = () => {
  return (
    <div className="fixed-page landing-fixed-page">
      <div className="landing-page">
        <Navbar />

        <div className="landing-page-header" id="lp-header">
          <div className="lp-header-content grid-background">
            <h1>
              Turning creativity <br /> into meaningful moments
            </h1>
            <NavLink to="/login" id="get-started-btn">
              Get started
            </NavLink>
          </div>
        </div>

        <div className="lp-transition"></div>

        <div className="lp-quote-container">
          <img
            src="https://cdn.britannica.com/63/59963-050-C03F29B9/Pablo-Picasso.jpg"
            alt="Pablo Picasso"
          />
          <div>
            <p className="lp-quote">
              “There are painters who transform the sun into a yellow spot, but
              there are others who, thanks to their art and intelligence,
              transform a yellow spot into the sun.”
            </p>
            <p className="lp-quote-author">- Pablo Picasso</p>
          </div>
        </div>

        <Features />
        <FAQ />
        <Contact />
      </div>
    </div>
  );
};

export default LandingPage;
