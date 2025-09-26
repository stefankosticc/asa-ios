type Threshold = {
  value: number;
  suffix: string;
  decimals: number;
  divisor?: number;
};

export const formatFollowCount = (count: number | undefined): string => {
  if (count === undefined) return "0";

  const thresholds: Threshold[] = [
    { value: 1_000_000_000, suffix: "B", decimals: 2 },
    { value: 1_000_000, suffix: "M", decimals: 1 },
    { value: 10_000, suffix: "K", decimals: 1, divisor: 1_000 },
  ];

  for (const threshold of thresholds) {
    if (count >= threshold.value) {
      const divisor = threshold.divisor ?? threshold.value;
      const factor = Math.pow(10, threshold.decimals);
      const num = Math.trunc((count / divisor) * factor) / factor;
      return num.toString().replace(/\.0+$/, "") + threshold.suffix;
    }
  }

  return count.toString();
};
