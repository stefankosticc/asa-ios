import "../styles/NotFound.css";

const NotFound = () => {
  return (
    <div className="not-found-page fixed-page">
      <h1 className="not-found-title">404</h1>
      <p className="not-found-message">Page Not Found</p>
      <p className="not-found-description">
        The page you are looking for does not exist or has been moved.
      </p>
      <a className="not-found-link" onClick={() => window.history.back()}>
        Go Back
      </a>
    </div>
  );
};

export default NotFound;
