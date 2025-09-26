import { FaCity } from "react-icons/fa";
import "./styles/CitySearchCard.css";
import { City } from "../../services/city";
import { useNavigate } from "react-router-dom";

type CitySearchCardProps = {
  city: City;
};

const CitySearchCard = ({ city }: CitySearchCardProps) => {
  const navigate = useNavigate();

  return (
    <div
      className="city-sc-container"
      onClick={() => navigate(`/city/${city.id}`)}
    >
      <div className="city-sc-icon-container">
        <FaCity />
      </div>
      <div className="city-sc-details">
        <p>{city.name + ", " + city.country}</p>
      </div>
    </div>
  );
};

export default CitySearchCard;
