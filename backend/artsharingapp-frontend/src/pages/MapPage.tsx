import Dock from "../components/Dock";

const MapPage = () => {
  return (
    <div className="fixed-page">
      <div
        className="map-page"
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          height: "100%",
        }}
      >
        <h2>Comming soon... ⏳</h2>

        <Dock />
      </div>
    </div>
  );
};

export default MapPage;
