import "./styles/Search.css";
import { FiSearch } from "react-icons/fi";
import { FaLandmark } from "react-icons/fa6";
import { FaCity, FaUser } from "react-icons/fa";
import { RiDashboardFill } from "react-icons/ri";
import ArtworkSearchCard from "./ArtworkSearchCard";
import ArtistSearchCard from "./ArtistSearchCard";
import CitySearchCard from "./CitySearchCard";
import GallerySearchCard from "./GallerySearchCard";
import { useRef, useState } from "react";
import { useSearch } from "../../hooks/useSearch";
import { ArtworkSearchResponse } from "../../services/artwork";
import { useClickOutside } from "../../hooks/useClickOutside";
import { Gallery } from "../../services/gallery";
import { City } from "../../services/city";
import { UserSearchResponse } from "../../services/user";
import { useNavigate } from "react-router-dom";

const setFilterOptionColors = (
  background: string,
  fill: string
): React.CSSProperties => {
  return {
    "--search-filter-option-bg": background,
    "--search-filter-option-fill": fill,
  } as React.CSSProperties;
};

const Search = ({ onClose }: { onClose: () => void }) => {
  const [searchString, setSearchString] = useState<string>("");
  const [filter, setFilter] = useState<string>("artwork");

  const navigate = useNavigate();

  const containerRef = useRef<HTMLDivElement>(null);
  useClickOutside(containerRef, onClose);

  const { results, loadingSearchResults, clearResults } = useSearch({
    searchString: searchString,
    filter: filter,
  });

  return (
    <div className="search-page">
      <div className="search-container" ref={containerRef}>
        <div className="search-bar">
          <input
            type="text"
            id="search-input"
            placeholder="Search for artworks, artists, or galleries..."
            value={searchString}
            onChange={(e) => setSearchString(e.target.value)}
            autoFocus
          />
          <button type="submit">
            <FiSearch />
          </button>
        </div>
        <div className="search-filters">
          <p>Filters:</p>
          <div className="search-filter-options">
            <label
              htmlFor="sf-artwork"
              style={setFilterOptionColors(
                "rgba(135, 206, 235, 0.2)",
                "rgb(135, 206, 235)"
              )}
            >
              <input
                type="radio"
                name="filter"
                id="sf-artwork"
                value="artwork"
                checked={filter === "artwork"}
                onChange={(e) => {
                  clearResults();
                  setFilter(e.target.value);
                }}
              />
              <RiDashboardFill />
              Artworks
            </label>
            <label
              style={setFilterOptionColors(
                "rgba(147, 112, 219, 0.2)",
                "rgb(147, 112, 219)"
              )}
            >
              <input
                type="radio"
                name="filter"
                value="artist"
                checked={filter === "artist"}
                onChange={(e) => {
                  clearResults();
                  setFilter(e.target.value);
                }}
              />
              <FaUser />
              Artists
            </label>
            <label
              style={setFilterOptionColors(
                "rgba(30, 144, 255, 0.2)",
                "rgb(30, 144, 255)"
              )}
            >
              <input
                type="radio"
                name="filter"
                value="city"
                checked={filter === "city"}
                onChange={(e) => {
                  clearResults();
                  setFilter(e.target.value);
                }}
              />
              <FaCity />
              Cities
            </label>
            <label
              style={setFilterOptionColors(
                "rgba(255, 140, 0, 0.2)",
                "rgb(255, 140, 0)"
              )}
            >
              <input
                type="radio"
                name="filter"
                value="gallery"
                checked={filter === "gallery"}
                onChange={(e) => {
                  clearResults();
                  setFilter(e.target.value);
                }}
              />
              <FaLandmark />
              Galleries
            </label>
          </div>
        </div>

        <div className="search-results">
          {loadingSearchResults ? (
            <div className="search-no-results search-loader"></div>
          ) : results && results.length > 0 ? (
            results.map((result) => {
              switch (filter) {
                case "artwork":
                  return (
                    <ArtworkSearchCard
                      key={result.id}
                      artwork={result as ArtworkSearchResponse}
                    />
                  );
                case "artist":
                  return (
                    <ArtistSearchCard
                      key={result.id}
                      artist={result as UserSearchResponse}
                      onClick={() =>
                        navigate(`/${(result as UserSearchResponse).userName}`)
                      }
                    />
                  );
                case "city":
                  return (
                    <CitySearchCard key={result.id} city={result as City} />
                  );
                case "gallery":
                  return (
                    <GallerySearchCard
                      key={result.id}
                      gallery={result as Gallery}
                    />
                  );
                default:
                  return null;
              }
            })
          ) : (
            <p className="search-no-results">No results found.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Search;
