import { useState } from "react";
import "./styles/FAQ.css";
import { BsCaretDownFill, BsCaretRightFill } from "react-icons/bs";

const faqData: {
  question: string;
  answer: string;
}[] = [
  {
    question: "What is this platform about?",
    answer:
      "This is a social media platform for artists to share, discover, and discuss art.",
  },
  {
    question: "Is it free to use?",
    answer: "Yes, the platform is completely free to use for everyone.",
  },
  {
    question: "Can I sell my artwork here?",
    answer:
      "Absolutely! You can list your artworks for sale or auction through your profile.",
  },
  {
    question: "How do auctions work?",
    answer:
      "Auctions allow users to place bids on your artwork during a set timeframe. The highest bidder wins.",
  },
  {
    question: "Are there any fees when selling art?",
    answer:
      "We take a small transaction fee on successful sales to support platform maintenance.",
  },
  {
    question: "Do you have an app?",
    answer:
      "Coming soon! Our iOS app will be available soon, so stay tuned for updates!",
  },
];

const FAQ = () => {
  const [openIndex, setOpenIndex] = useState<number | null>(null);

  const toggleFAQ = (index: number) => {
    setOpenIndex((prev) => (prev === index ? null : index));
  };

  return (
    <div id="faq" className="faq-container">
      <h2>FAQ</h2>
      <div className="faqs">
        {faqData.map((item, index) => (
          <div
            key={index}
            className={`faq-item ${openIndex === index ? "open" : ""}`}
            onClick={() => toggleFAQ(index)}
          >
            <div className="faq-question">
              <BsCaretDownFill className="faq-icon" />
              {item.question}
            </div>
            <div className="faq-answer">{item.answer}</div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FAQ;
