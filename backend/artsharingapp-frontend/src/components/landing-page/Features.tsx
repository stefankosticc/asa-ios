import "./styles/Features.css";

const featuresData: {
  title: string;
  image: string;
  description: string;
}[] = [
  {
    title: "Share your work",
    image: "/pictures/ArtwokPage.png",
    description: "Showcase your projects and connect with a global community.",
  },
  {
    title: "Get inspired",
    image: "/pictures/Discover.png",
    description: "Inspire and be inspired by the creativity of others.",
  },
  {
    title: "Chat with others",
    image: "/pictures/Chat.png",
    description: "Engage in real-time discussions with like-minded creators.",
  },
  {
    title: "Participate in auctions",
    image: "/pictures/Auction.png",
    description: "Bid on unique items or list your own in exciting auctions.",
  },
];

const Features = () => {
  return (
    <div className="lp-features-container" id="features">
      <h2>Features</h2>
      <div className="lp-features">
        {featuresData.map((feature) => (
          <div className="feature-card" key={feature.title}>
            <div>
              <h3>{feature.title}</h3>
              <p className="feature-description">{feature.description}</p>
            </div>
            <img
              src={feature.image}
              alt="Feature Image"
              className="feature-img"
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Features;
