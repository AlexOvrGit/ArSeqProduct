using System.Text.RegularExpressions;
namespace ArSeqProduct
{
    internal partial class Program
    {
        //   public static string tail = string.Empty;
        public static Dictionary<string, string> ParseCommand(string input, out string tail)
        {
            int index = input.IndexOf(";");
            tail = index >= 0 ? input.Substring(index + 1).Trim() : string.Empty;
            // If a semicolon is found, trim the input up to that point
            string trimmedInput = index >= 0 ? input.Substring(0, index).Trim() : input.Trim();
            var result = ParseArguments(trimmedInput);
            return result;
        }

        public static Dictionary<string, string> ParseArguments(string input)
        {
            var result = new Dictionary<string, string>();
            var pattern = @"\s*([^=,\s]+)\s*=\s*(\d+)\s*";
            var matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;
                Params.SetValue(key, value);
                result[key] = value;
            }

            return result;
        }
    }
}


