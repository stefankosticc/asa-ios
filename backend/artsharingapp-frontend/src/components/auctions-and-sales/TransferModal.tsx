import { useState } from "react";
import { FiSearch } from "react-icons/fi";
import "./styles/TransferModal.css";
import { useSearch } from "../../hooks/useSearch";
import ArtistSearchCard from "../search/ArtistSearchCard";
import { UserSearchResponse } from "../../services/user";
import { transferArtwork } from "../../services/artwork";
import { toast } from "react-toastify";

type TransferModalProps = {
  artworkId: number;
  onClose: () => void;
  refetchArtwork: () => void;
};

const TransferModal = ({
  artworkId,
  onClose,
  refetchArtwork,
}: TransferModalProps) => {
  const [searchString, setSearchString] = useState<string>("");

  const { results } = useSearch({
    searchString: searchString,
    filter: "artist",
  });

  const handleTransfer = async (
    artworkId: number,
    user: UserSearchResponse
  ) => {
    if (
      window.confirm(
        `Are you sure you want to transfer this artwork to @${user.userName}?`
      )
    ) {
      const success = await transferArtwork(artworkId, user.id);
      if (success) {
        onClose();
        refetchArtwork();
        toast.success(`Artwork successfully transferred to @${user.userName}!`);
      }
    }
  };

  return (
    <div className="blured-page">
      <div className="modal-container transfer-modal-container">
        <div className="search-bar">
          <input
            type="text"
            placeholder="Search artist for transfer..."
            value={searchString}
            onChange={(e) => setSearchString(e.target.value)}
            autoFocus
          />
          <button type="submit">
            <FiSearch />
          </button>
        </div>

        <div className="search-results">
          {results &&
            results.map((result) => (
              <ArtistSearchCard
                key={result.id}
                artist={result as UserSearchResponse}
                onClick={() =>
                  handleTransfer(artworkId, result as UserSearchResponse)
                }
              />
            ))}
        </div>

        <button className="transfer-cancel-btn" onClick={onClose}>
          Cancel
        </button>
      </div>
    </div>
  );
};

export default TransferModal;
