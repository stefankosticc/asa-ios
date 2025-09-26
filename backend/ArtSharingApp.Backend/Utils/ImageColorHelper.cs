using ArtSharingApp.Backend.Exceptions;
using SkiaSharp;

namespace ArtSharingApp.Backend.Utils;

public static class ImageColorHelper
{
    /// <summary>
    /// Extracts saturation-weighted average color from image bytes.
    /// </summary>
    /// <param name="imageBytes">Image byte array.</param>
    /// <param name="resizeWidth">Resize width for faster processing (default 100px).</param>
    /// <returns>HEX string of weighted average color (e.g. "#AABBCC").</returns>
    public static string? ExtractSaturationWeightedAverageColor(byte[] imageBytes, int resizeWidth = 100)
    {
        using var ms = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(ms);

        if (original == null)
            throw new ArgumentException("Invalid image data.");

        int height = (int)((float)original.Height / original.Width * resizeWidth);

        using var resized = original.Resize(
            new SKImageInfo(resizeWidth, height),
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));

        if (resized == null)
            throw new InvalidOperationException("Failed to resize image.");

        double rSum = 0;
        double gSum = 0;
        double bSum = 0;
        double totalWeight = 0;

        for (int y = 0; y < resized.Height; y++)
        {
            for (int x = 0; x < resized.Width; x++)
            {
                var pixel = resized.GetPixel(x, y);
                if (pixel.Alpha < 128) // ignore transparent
                    continue;

                // Convert RGB to HSL to get saturation
                RgbToHsl(pixel.Red, pixel.Green, pixel.Blue, out double h, out double s, out double l);

                // Weight by saturation (saturation between 0 and 1)
                // You can multiply by lightness l too if you want brightness influence
                double weight = s;

                if (weight > 0)
                {
                    rSum += pixel.Red * weight;
                    gSum += pixel.Green * weight;
                    bSum += pixel.Blue * weight;
                    totalWeight += weight;
                }
            }
        }

        if (totalWeight == 0)
            throw new InvalidOperationException("No visible colorful pixels found.");

        byte avgR = (byte)(rSum / totalWeight);
        byte avgG = (byte)(gSum / totalWeight);
        byte avgB = (byte)(bSum / totalWeight);

        return $"#{avgR:X2}{avgG:X2}{avgB:X2}";
    }

    /// <summary>
    /// Converts RGB (0-255) to HSL (all 0-1).
    /// </summary>
    private static void RgbToHsl(byte r, byte g, byte b, out double h, out double s, out double l)
    {
        double dr = r / 255.0;
        double dg = g / 255.0;
        double db = b / 255.0;

        double max = Math.Max(dr, Math.Max(dg, db));
        double min = Math.Min(dr, Math.Min(dg, db));
        h = 0;
        s = 0;
        l = (max + min) / 2.0;

        if (max == min)
        {
            h = 0;
            s = 0; // achromatic
        }
        else
        {
            double d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == dr)
                h = (dg - db) / d + (dg < db ? 6 : 0);
            else if (max == dg)
                h = (db - dr) / d + 2;
            else
                h = (dr - dg) / d + 4;

            h /= 6;
        }
    }
}

