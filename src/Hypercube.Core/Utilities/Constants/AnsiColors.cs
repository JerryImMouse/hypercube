﻿using JetBrains.Annotations;

namespace Hypercube.Core.Utilities.Constants;

/// <summary>
/// Provides ANSI escape codes for text colors, background colors, and styles.
/// Supports 16, 256, and RGB true-color formatting.
/// </summary>
[PublicAPI]
public static class AnsiColors
{
        // Reset all formatting
        public const string Reset = "\u001b[0m";

        // Standard 16 colors (text)
        public const string Black = "\u001b[30m";
        public const string Red = "\u001b[31m";
        public const string Green = "\u001b[32m";
        public const string Yellow = "\u001b[33m";
        public const string Blue = "\u001b[34m";
        public const string Magenta = "\u001b[35m";
        public const string Cyan = "\u001b[36m";
        public const string White = "\u001b[37m";

        // Bright text colors
        public const string BrightBlack = "\u001b[90m";
        public const string BrightRed = "\u001b[91m";
        public const string BrightGreen = "\u001b[92m";
        public const string BrightYellow = "\u001b[93m";
        public const string BrightBlue = "\u001b[94m";
        public const string BrightMagenta = "\u001b[95m";
        public const string BrightCyan = "\u001b[96m";
        public const string BrightWhite = "\u001b[97m";

        // Background colors
        public const string BackgroundBlack = "\u001b[40m";
        public const string BackgroundRed = "\u001b[41m";
        public const string BackgroundGreen = "\u001b[42m";
        public const string BackgroundYellow = "\u001b[43m";
        public const string BackgroundBlue = "\u001b[44m";
        public const string BackgroundMagenta = "\u001b[45m";
        public const string BackgroundCyan = "\u001b[46m";
        public const string BackgroundWhite = "\u001b[47m";

        // Bright background colors
        public const string BrightBackgroundBlack = "\u001b[100m";
        public const string BrightBackgroundRed = "\u001b[101m";
        public const string BrightBackgroundGreen = "\u001b[102m";
        public const string BrightBackgroundYellow = "\u001b[103m";
        public const string BrightBackgroundBlue = "\u001b[104m";
        public const string BrightBackgroundMagenta = "\u001b[105m";
        public const string BrightBackgroundCyan = "\u001b[106m";
        public const string BrightBackgroundWhite = "\u001b[107m";

        // Text styles
        public const string Bold = "\u001b[1m";
        public const string Dim = "\u001b[2m";
        public const string Italic = "\u001b[3m";
        public const string Underline = "\u001b[4m";
        public const string Blink = "\u001b[5m";
        public const string Reversed = "\u001b[7m";

        // 256-color support (standard ANSI palette)
        public static string Text256(int colorCode) => $"\u001b[38;5;{colorCode}m";
        public static string Background256(int colorCode) => $"\u001b[48;5;{colorCode}m";

        // True-color (24-bit RGB support)
        public static string TextRgb(int r, int g, int b) => $"\u001b[38;2;{r};{g};{b}m";
        public static string BackgroundRgb(int r, int g, int b) => $"\u001b[48;2;{r};{g};{b}m";

        // Utility methods for gradients and patterns (optional extension)
        public static string GradientText(string text, Func<int, (int R, int G, int B)> colorFunction)
        {
            var result = string.Empty;
            
            for (var i = 0; i < text.Length; i++)
            {
                var (r, g, b) = colorFunction(i);
                result += $"{TextRgb(r, g, b)}{text[i]}";
            }
            
            return result + Reset;
        }
}