import { useState, useEffect } from "react";
import { ArtworkSearchResponse, searchArtworks } from "../services/artwork";
import { searchArtists, UserSearchResponse } from "../services/user";
import { Gallery, searchGalleries } from "../services/gallery";
import { City, searchCities } from "../services/city";

type UseSearchOptions = {
  searchString: string;
  filter?: string;
};

type SearchResult =
  | ArtworkSearchResponse
  | UserSearchResponse
  | City
  | Gallery
  | null
  | any;

export const useSearch = ({ searchString, filter }: UseSearchOptions) => {
  const [results, setResults] = useState<
    (ArtworkSearchResponse | UserSearchResponse | City | Gallery)[] | null
  >(null);
  const [loadingSearchResults, setLoadingSearchResults] =
    useState<boolean>(false);

  useEffect(() => {
    if (searchString.trim() === "") {
      setResults(null);
      return;
    }

    let isCancelled = false;
    const delayDebounce = setTimeout(() => {
      const search = async () => {
        try {
          setLoadingSearchResults(true);
          let response: SearchResult[] = [];

          switch (filter) {
            case "artwork":
              response = await searchArtworks(searchString.trim());
              break;
            case "artist":
              response = await searchArtists(searchString.trim());
              break;
            case "city":
              response = await searchCities(searchString.trim());
              break;
            case "gallery":
              response = await searchGalleries(searchString.trim());
              break;
            default:
              response = [];
              break;
          }

          if (!isCancelled) {
            setResults(response);
          }
        } catch (err) {
          if (!isCancelled) {
            console.error("Failed to fetch search results");
            setResults([]);
          }
        } finally {
          if (!isCancelled) {
            setLoadingSearchResults(false);
          }
        }
      };

      search();
    }, 500);

    return () => {
      isCancelled = true;
      clearTimeout(delayDebounce);
    };
  }, [filter, searchString]);

  return {
    results,
    loadingSearchResults,
    clearResults: () => setResults(null),
  };
};
