// <copyright file="MixedNumericStringComparer.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services
{
    using Serilog;

    /// <summary>
    /// Compares strings that contain a mix of numeric and non-numeric characters.
    /// </summary>
    public class MixedNumericStringComparer : IComparer<string?>
    {
        /// <summary>
        /// Compares two strings that contain a mix of numeric and non-numeric characters.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>Comparation result: -1, 0 or 1.</returns>
        public int Compare(string? x, string? y)
        {
            Log.Information($"MixedNumericStringComparer.Compare(): " +
                $"method called with parameters: x = {x} and y = {y}");

            int numX, numY;
            string alphaX, alphaY;

            // Separate numeric and non-numeric parts
            this.SplitNumericAndAlpha(x, out numX, out alphaX);
            this.SplitNumericAndAlpha(y, out numY, out alphaY);

            // Compare numeric parts first
            int numComparison = numX.CompareTo(numY);

            if (numComparison == 0)
            {
                return string.Compare(alphaX, alphaY, StringComparison.Ordinal);
            }

            return numComparison;
        }

        private void SplitNumericAndAlpha(string? input, out int numericPart, out string alphaPart)
        {
            Log.Debug($"MixedNumericStringComparer.SplitNumericAndAlpha(): " +
                $"method called with parameter: input = {input}");

            alphaPart = string.Empty;
            numericPart = 0;

            if (input != null)
            {
                int index = 0;

                while (index < input.Length && char.IsDigit(input[index]))
                {
                    numericPart = (numericPart * 10) + (input[index++] - '0');
                }

                alphaPart = input.Substring(index);
            }
        }
    }
}
