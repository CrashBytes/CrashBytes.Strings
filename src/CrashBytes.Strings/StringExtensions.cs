using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CrashBytes.Strings;

/// <summary>
/// Extension methods for <see cref="string"/> providing common string manipulation operations
/// including truncation, case conversion, slug generation, masking, and validation.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates the string to the specified maximum length, appending a suffix if truncated.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the resulting string (including suffix).</param>
    /// <param name="suffix">The suffix to append when truncation occurs. Defaults to "...".</param>
    /// <returns>The truncated string with suffix, or the original string if no truncation is needed.</returns>
    public static string? Truncate(this string? value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (maxLength < 0)
            maxLength = 0;

        if (value.Length <= maxLength)
            return value;

        if (maxLength <= suffix.Length)
            return value.Substring(0, maxLength);

        return value.Substring(0, maxLength - suffix.Length) + suffix;
    }

    /// <summary>
    /// Truncates the string at a word boundary to the specified maximum number of words, appending a suffix if truncated.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxWords">The maximum number of words to retain.</param>
    /// <param name="suffix">The suffix to append when truncation occurs. Defaults to "...".</param>
    /// <returns>The truncated string with suffix, or the original string if no truncation is needed.</returns>
    public static string? TruncateWords(this string? value, int maxWords, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (maxWords <= 0)
            return suffix;

        var words = value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (words.Length <= maxWords)
            return value;

        return string.Join(" ", words.Take(maxWords)) + suffix;
    }

    /// <summary>
    /// Converts the string to a URL-safe slug by lowercasing, removing diacritics,
    /// replacing non-alphanumeric characters with hyphens, collapsing multiple hyphens,
    /// and trimming hyphens from the ends.
    /// </summary>
    /// <param name="value">The string to convert to a slug.</param>
    /// <returns>A URL-safe slug representation of the string.</returns>
    public static string? ToSlug(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Remove diacritics
        var normalized = value.RemoveDiacritics()!;

        // Lowercase
        normalized = normalized.ToLowerInvariant();

        // Replace non-alphanumeric characters with hyphens
        normalized = Regex.Replace(normalized, @"[^a-z0-9]", "-");

        // Collapse multiple hyphens
        normalized = Regex.Replace(normalized, @"-{2,}", "-");

        // Trim hyphens from ends
        normalized = normalized.Trim('-');

        return normalized;
    }

    /// <summary>
    /// Converts the string to snake_case (e.g., "HelloWorld" becomes "hello_world").
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in snake_case format.</returns>
    public static string? ToSnakeCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var words = SplitIntoWords(value);
        return string.Join("_", words.Select(w => w.ToLowerInvariant()));
    }

    /// <summary>
    /// Converts the string to camelCase (e.g., "hello_world" becomes "helloWorld").
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in camelCase format.</returns>
    public static string? ToCamelCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var words = SplitIntoWords(value);
        if (words.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.Append(words[0].ToLowerInvariant());
        for (int i = 1; i < words.Count; i++)
        {
            sb.Append(CapitalizeFirst(words[i]));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts the string to PascalCase (e.g., "hello_world" becomes "HelloWorld").
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in PascalCase format.</returns>
    public static string? ToPascalCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var words = SplitIntoWords(value);
        if (words.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var word in words)
        {
            sb.Append(CapitalizeFirst(word));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts the string to kebab-case (e.g., "HelloWorld" becomes "hello-world").
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in kebab-case format.</returns>
    public static string? ToKebabCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var words = SplitIntoWords(value);
        return string.Join("-", words.Select(w => w.ToLowerInvariant()));
    }

    /// <summary>
    /// Masks all characters except the last N characters with the specified mask character.
    /// </summary>
    /// <param name="value">The string to mask.</param>
    /// <param name="visibleChars">The number of characters to leave visible at the end. Defaults to 4.</param>
    /// <param name="maskChar">The character to use for masking. Defaults to '*'.</param>
    /// <returns>The masked string, or the original string if it is shorter than or equal to visibleChars.</returns>
    public static string? Mask(this string? value, int visibleChars = 4, char maskChar = '*')
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (visibleChars < 0)
            visibleChars = 0;

        if (value.Length <= visibleChars)
            return value;

        var maskLength = value.Length - visibleChars;
        return new string(maskChar, maskLength) + value.Substring(maskLength);
    }

    /// <summary>
    /// Extracts the substring between the first occurrence of <paramref name="start"/> and the
    /// first occurrence of <paramref name="end"/> after that start position.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="start">The starting delimiter.</param>
    /// <param name="end">The ending delimiter.</param>
    /// <returns>The substring between the delimiters, or <c>null</c> if either delimiter is not found.</returns>
    public static string? Between(this string? value, string start, string end)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            return null;

        var startIndex = value.IndexOf(start, StringComparison.Ordinal);
        if (startIndex < 0)
            return null;

        var contentStart = startIndex + start.Length;
        var endIndex = value.IndexOf(end, contentStart, StringComparison.Ordinal);
        if (endIndex < 0)
            return null;

        return value.Substring(contentStart, endIndex - contentStart);
    }

    /// <summary>
    /// Returns the substring before the first occurrence of the specified separator.
    /// If the separator is not found, the original string is returned.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="separator">The separator to search for.</param>
    /// <returns>The substring before the separator, or the original string if the separator is not found.</returns>
    public static string? Before(this string? value, string separator)
    {
        if (value is null)
            return null;

        if (string.IsNullOrEmpty(separator))
            return value;

        var index = value.IndexOf(separator, StringComparison.Ordinal);
        if (index < 0)
            return value;

        return value.Substring(0, index);
    }

    /// <summary>
    /// Returns the substring after the first occurrence of the specified separator.
    /// If the separator is not found, an empty string is returned.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="separator">The separator to search for.</param>
    /// <returns>The substring after the separator, or an empty string if the separator is not found.</returns>
    public static string? After(this string? value, string separator)
    {
        if (value is null)
            return null;

        if (string.IsNullOrEmpty(separator))
            return value;

        var index = value.IndexOf(separator, StringComparison.Ordinal);
        if (index < 0)
            return string.Empty;

        return value.Substring(index + separator.Length);
    }

    /// <summary>
    /// Determines whether the string contains all of the specified values using case-insensitive comparison.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="values">The values to search for.</param>
    /// <returns><c>true</c> if the string contains all of the specified values; otherwise, <c>false</c>.</returns>
    public static bool ContainsAll(this string? value, params string[] values)
    {
        return value.ContainsAll(StringComparison.OrdinalIgnoreCase, values);
    }

    /// <summary>
    /// Determines whether the string contains all of the specified values using the given comparison type.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="comparison">The type of string comparison to use.</param>
    /// <param name="values">The values to search for.</param>
    /// <returns><c>true</c> if the string contains all of the specified values; otherwise, <c>false</c>.</returns>
    public static bool ContainsAll(this string? value, StringComparison comparison, params string[] values)
    {
        if (value is null || values is null || values.Length == 0)
            return false;

        return values.All(v => value.IndexOf(v, comparison) >= 0);
    }

    /// <summary>
    /// Determines whether the string contains any of the specified values using case-insensitive comparison.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="values">The values to search for.</param>
    /// <returns><c>true</c> if the string contains any of the specified values; otherwise, <c>false</c>.</returns>
    public static bool ContainsAny(this string? value, params string[] values)
    {
        return value.ContainsAny(StringComparison.OrdinalIgnoreCase, values);
    }

    /// <summary>
    /// Determines whether the string contains any of the specified values using the given comparison type.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="comparison">The type of string comparison to use.</param>
    /// <param name="values">The values to search for.</param>
    /// <returns><c>true</c> if the string contains any of the specified values; otherwise, <c>false</c>.</returns>
    public static bool ContainsAny(this string? value, StringComparison comparison, params string[] values)
    {
        if (value is null || values is null || values.Length == 0)
            return false;

        return values.Any(v => value.IndexOf(v, comparison) >= 0);
    }

    /// <summary>
    /// Counts the number of non-overlapping occurrences of the specified substring within the string.
    /// </summary>
    /// <param name="value">The string to search within.</param>
    /// <param name="substring">The substring to count.</param>
    /// <returns>The number of non-overlapping occurrences of the substring.</returns>
    public static int CountOccurrences(this string? value, string substring)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(substring))
            return 0;

        int count = 0;
        int index = 0;

        while ((index = value.IndexOf(substring, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }

    /// <summary>
    /// Removes diacritical marks (accents) from the string using Unicode normalization.
    /// </summary>
    /// <param name="value">The string from which to remove diacritics.</param>
    /// <returns>The string with diacritical marks removed.</returns>
    public static string? RemoveDiacritics(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Removes HTML tags from the string. Entity references (e.g., &amp;amp;) are left as-is.
    /// </summary>
    /// <param name="value">The string from which to strip HTML tags.</param>
    /// <returns>The string with HTML tags removed.</returns>
    public static string? StripHtml(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(value, @"<[^>]*>", string.Empty);
    }

    /// <summary>
    /// Determines whether the string is a valid email address using a basic regex pattern.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns><c>true</c> if the string is a valid email address; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Determines whether the string is a valid absolute HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns><c>true</c> if the string is a valid absolute HTTP/HTTPS URL; otherwise, <c>false</c>.</returns>
    public static bool IsValidUrl(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Determines whether the string is null or empty.
    /// This is a fluent-style extension wrapper around <see cref="string.IsNullOrEmpty"/>.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether the string is null, empty, or consists only of white-space characters.
    /// This is a fluent-style extension wrapper around <see cref="string.IsNullOrWhiteSpace"/>.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is null, empty, or consists only of white-space; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Repeats the string the specified number of times.
    /// </summary>
    /// <param name="value">The string to repeat.</param>
    /// <param name="count">The number of times to repeat the string.</param>
    /// <returns>A new string consisting of the original string repeated the specified number of times.</returns>
    public static string? Repeat(this string? value, int count)
    {
        if (value is null)
            return null;

        if (count <= 0 || value.Length == 0)
            return string.Empty;

        var sb = new StringBuilder(value.Length * count);
        for (int i = 0; i < count; i++)
        {
            sb.Append(value);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts the string to title case (capitalizes the first letter of each word) using the current culture.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string in title case.</returns>
    public static string? ToTitleCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(value.ToLower(CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// Splits a string into individual words based on camelCase/PascalCase boundaries,
    /// separators (underscore, hyphen, space), and consecutive uppercase runs.
    /// </summary>
    private static List<string> SplitIntoWords(string value)
    {
        // First, split on explicit separators: underscores, hyphens, spaces
        var parts = Regex.Split(value, @"[\s_\-]+")
            .Where(p => p.Length > 0)
            .ToList();

        var words = new List<string>();

        foreach (var part in parts)
        {
            // Split camelCase / PascalCase:
            // Insert boundary before: a lowercase followed by an uppercase,
            // or an uppercase followed by an uppercase then lowercase (e.g., "XMLParser" -> "XML", "Parser")
            var camelWords = Regex.Replace(part, @"([a-z])([A-Z])", "$1 $2");
            camelWords = Regex.Replace(camelWords, @"([A-Z]+)([A-Z][a-z])", "$1 $2");

            words.AddRange(camelWords.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        return words;
    }

    /// <summary>
    /// Capitalizes the first letter of a word and lowercases the rest.
    /// </summary>
    private static string CapitalizeFirst(string word)
    {
        if (word.Length == 0)
            return word;

        return char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant();
    }
}
