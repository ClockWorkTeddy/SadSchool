namespace SadSchool.Services
{
    public class MixedNumericStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int numX, numY;
            string alphaX, alphaY;

            // Separate numeric and non-numeric parts
            SplitNumericAndAlpha(x, out numX, out alphaX);
            SplitNumericAndAlpha(y, out numY, out alphaY);

            // Compare numeric parts first
            int numComparison = numX.CompareTo(numY);

            if (numComparison == 0)
                return string.Compare(alphaX, alphaY, StringComparison.Ordinal);

            return numComparison;
        }

        private void SplitNumericAndAlpha(string input, out int numericPart, out string alphaPart)
        {
            numericPart = 0;
            alphaPart = string.Empty;

            int index = 0;

            while (index < input.Length && char.IsDigit(input[index]))
                numericPart = numericPart * 10 + (input[index++] - '0');

            alphaPart = input.Substring(index);
        }
    }

}
