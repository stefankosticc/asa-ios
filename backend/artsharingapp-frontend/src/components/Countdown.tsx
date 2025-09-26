import { useEffect, useState } from "react";
import "../styles/Countdown.css";

type CountdownProps = {
  endTime: Date;
};

const Countdown = ({ endTime }: CountdownProps) => {
  const calculateTimeLeft = () => {
    const end = new Date(endTime);
    const now = new Date();
    const diffMs = end.getTime() - now.getTime();

    if (diffMs <= 0) return null;

    const totalSeconds = Math.floor(diffMs / 1000);
    const days = Math.floor(totalSeconds / (3600 * 24));
    const hours = Math.floor((totalSeconds % (3600 * 24)) / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    return { days, hours, minutes, seconds, totalSeconds };
  };

  const [timeLeft, setTimeLeft] = useState(calculateTimeLeft());

  useEffect(() => {
    setTimeLeft(calculateTimeLeft());
  }, [endTime]);

  useEffect(() => {
    if (!timeLeft) return;

    const intervalMs = timeLeft.totalSeconds > 3600 ? 60_000 : 1000;

    const interval = setInterval(() => {
      setTimeLeft(calculateTimeLeft());
    }, intervalMs);

    return () => clearInterval(interval);
  }, [timeLeft?.totalSeconds, endTime]);

  if (!timeLeft) return <span className="countdown-expired">Expired</span>;

  const { days, hours, minutes, seconds, totalSeconds } = timeLeft;
  const isUrgent = totalSeconds <= 5 * 60;
  const showSeconds = totalSeconds <= 3600;

  return (
    <span className={`${isUrgent ? "countdown-urgent" : ""}`}>
      {days > 0 && `${days}d `}
      {hours}h {minutes}m{showSeconds && ` ${seconds}s`}
    </span>
  );
};

export default Countdown;
