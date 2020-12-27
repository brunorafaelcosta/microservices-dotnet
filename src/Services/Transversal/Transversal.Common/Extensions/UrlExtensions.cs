using System;
using System.Text.RegularExpressions;

namespace Transversal.Common.Extensions
{
	/// <summary>
	/// Provides additional methods to Urls.
	/// </summary>
	public static class UrlExtensions
    {
		/// <summary>
		/// Basically a Path.Combine for URLs. Ensures exactly one '/' separates each segment,
		/// and exactly on '&amp;' separates each query parameter
		/// <para>URL-encodes illegal characters but not reserved characters</para>
		/// </summary>
		/// <param name="baseUrl">URL base path</param>
		/// <param name="parts">URL parts to combine</param>
		public static string Combine(string baseUrl, params string[] parts)
		{
			if (baseUrl == null)
				throw new ArgumentNullException(nameof(baseUrl));

			string result = baseUrl;
			bool inQuery = false, inFragment = false;

			string CombineEnsureSingleSeparator(string a, string b, char separator)
			{
				if (string.IsNullOrEmpty(a)) return b;
				if (string.IsNullOrEmpty(b)) return a;
				return a.TrimEnd(separator) + separator + b.TrimStart(separator);
			}

			foreach (var part in parts)
			{
				if (string.IsNullOrEmpty(part))
					continue;

				if (result.EndsWith("?") || part.StartsWith("?"))
					result = CombineEnsureSingleSeparator(result, part, '?');
				else if (result.EndsWith("#") || part.StartsWith("#"))
					result = CombineEnsureSingleSeparator(result, part, '#');
				else if (inFragment)
					result += part;
				else if (inQuery)
					result = CombineEnsureSingleSeparator(result, part, '&');
				else
					result = CombineEnsureSingleSeparator(result, part, '/');

				if (part.Contains("#"))
				{
					inQuery = false;
					inFragment = true;
				}
				else if (!inFragment && part.Contains("?"))
				{
					inQuery = true;
				}
			}
			return EncodeIllegalCharacters(result);
		}

		/// <summary>
		/// URL-encodes characters in a string that are neither reserved nor unreserved
		/// <para>Avoids encoding reserved characters such as '/' and '?'</para>
		/// <para>Avoids encoding '%' if it begins a %-hex-hex sequence (i.e. avoids double-encoding)</para>
		/// </summary>
		/// <param name="url">Url to encode</param>
		/// <param name="encodeSpaceAsPlus">If true, spaces will be encoded as + signs. Otherwise, they'll be encoded as %20</param>
		/// <returns>The encoded URL</returns>
		public static string EncodeIllegalCharacters(string url, bool encodeSpaceAsPlus = false)
		{
			if (string.IsNullOrEmpty(url))
				return url;

			if (encodeSpaceAsPlus)
				url = url.Replace(" ", "+");

			// Uri.EscapeUriString mostly does what we want - encodes illegal characters only - but it has a quirk
			// in that % isn't illegal if it's the start of a %-encoded sequence https://stackoverflow.com/a/47636037/62600

			// no % characters, so avoid the regex overhead
			if (!url.Contains("%"))
				return Uri.EscapeUriString(url);

			// pick out all %-hex-hex matches and avoid double-encoding 
			return Regex.Replace(url, "(.*?)((%[0-9A-Fa-f]{2})|$)", c => {
				var a = c.Groups[1].Value; // group 1 is a sequence with no %-encoding - encode illegal characters
				var b = c.Groups[2].Value; // group 2 is a valid 3-character %-encoded sequence - leave it alone!
				return Uri.EscapeUriString(a) + b;
			});
		}
	}
}
