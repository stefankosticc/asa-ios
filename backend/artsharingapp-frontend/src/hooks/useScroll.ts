import { useEffect } from "react";

export function useScroll({
  ref,
  storageKey,
  onReachBottom,
  isReversed = false, // Whether the scroll direction is reversed (false -> on bottom reached, true -> on top reached)
}: {
  ref: React.RefObject<HTMLDivElement | null>;
  storageKey: string;
  onReachBottom: () => void;
  isReversed?: boolean;
}) {
  useEffect(() => {
    const container = ref.current;
    if (!container) return;

    // Restore scroll position
    const savedScroll = sessionStorage.getItem(storageKey);
    if (savedScroll) {
      container.scrollTop = parseInt(savedScroll, 10);
    }

    let hasReachedEdge = false;

    const handleScroll = () => {
      const scrollTop = container.scrollTop;
      const clientHeight = container.clientHeight;
      const scrollHeight = container.scrollHeight;

      // Trigger function if scrolled near the edge
      const nearEdge = isReversed
        ? scrollHeight - clientHeight + scrollTop <= 5 // Near top BUT for containers that have flex-direction: column-reverse
        : scrollTop + clientHeight >= scrollHeight - 5; // Near bottom

      if (nearEdge && !hasReachedEdge) {
        hasReachedEdge = true;
        onReachBottom();
      }

      // Save scroll position
      sessionStorage.setItem(storageKey, scrollTop.toString());
    };

    container.addEventListener("scroll", handleScroll);

    return () => {
      container.removeEventListener("scroll", handleScroll);
      sessionStorage.removeItem(storageKey); // when component unmounts reset scroll position
    };
  }, [onReachBottom]);
}
