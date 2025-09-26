import { FaInstagram, FaFacebookSquare, FaLinkedin } from "react-icons/fa";
import { FaXTwitter } from "react-icons/fa6";
import "./styles/Contact.css";

const Contact = () => {
  return (
    <div className="landing-page-contact" id="contact">
      <div className="contact-follow-us contact-content">
        <h2>Follow Us</h2>
        <div className="social-icons">
          <a
            href="https://www.instagram.com/"
            target="_blank"
            className="social-icon"
          >
            <FaInstagram size={24} />
          </a>
          <a
            href="https://www.facebook.com/"
            target="_blank"
            className="social-icon"
          >
            <FaFacebookSquare size={24} />
          </a>
          <a href="https://www.x.com/" target="_blank" className="social-icon">
            <FaXTwitter size={24} />
          </a>
          <a
            href="https://www.linkedin.com/"
            target="_blank"
            className="social-icon"
          >
            <FaLinkedin size={24} />
          </a>
        </div>
      </div>

      <div className="contact-us contact-content">
        <h2>Contact Us</h2>
        <p>Have questions or need assistance? Reach out to us at</p>
        <p className="contact-info">info@asa.com</p>
        <p className="contact-info">+44 123 456 7890</p>
        <p className="contact-info">West Street 123, London, UK</p>
      </div>
    </div>
  );
};

export default Contact;
