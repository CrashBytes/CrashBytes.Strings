namespace CrashBytes.Strings.Tests;

public class TruncateTests
{
    [Theory]
    [InlineData("Hello, World!", 5, "...", "He...")]
    [InlineData("Hello, World!", 10, "...", "Hello, ...")]
    [InlineData("Hello", 10, "...", "Hello")]
    [InlineData("Hello", 5, "...", "Hello")]
    [InlineData("Hello, World!", 8, "..", "Hello,..")]
    public void Truncate_WithValidInput_ReturnsExpected(string input, int maxLength, string suffix, string expected)
    {
        Assert.Equal(expected, input.Truncate(maxLength, suffix));
    }

    [Fact]
    public void Truncate_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.Truncate(10));
    }

    [Fact]
    public void Truncate_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.Truncate(10));
    }

    [Theory]
    [InlineData("Hello", 0, "...")]
    [InlineData("Hello", 1, "...")]
    [InlineData("Hello", 2, "...")]
    [InlineData("Hello", 3, "...")]
    public void Truncate_MaxLengthLessThanOrEqualSuffixLength_ReturnsTruncatedWithoutSuffix(string input, int maxLength, string suffix)
    {
        var result = input.Truncate(maxLength, suffix);
        Assert.Equal(input.Substring(0, maxLength), result);
    }

    [Fact]
    public void Truncate_NegativeMaxLength_TreatedAsZero()
    {
        Assert.Equal("", "Hello".Truncate(-1));
    }

    [Fact]
    public void Truncate_DefaultSuffix_UsesEllipsis()
    {
        Assert.Equal("He...", "Hello, World!".Truncate(5));
    }

    [Fact]
    public void Truncate_ExactLength_NoTruncation()
    {
        Assert.Equal("Hello", "Hello".Truncate(5));
    }
}

public class TruncateWordsTests
{
    [Theory]
    [InlineData("The quick brown fox", 2, "...", "The quick...")]
    [InlineData("The quick brown fox", 4, "...", "The quick brown fox")]
    [InlineData("The quick brown fox", 10, "...", "The quick brown fox")]
    [InlineData("Hello World", 1, "...", "Hello...")]
    public void TruncateWords_WithValidInput_ReturnsExpected(string input, int maxWords, string suffix, string expected)
    {
        Assert.Equal(expected, input.TruncateWords(maxWords, suffix));
    }

    [Fact]
    public void TruncateWords_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.TruncateWords(5));
    }

    [Fact]
    public void TruncateWords_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.TruncateWords(5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TruncateWords_ZeroOrNegativeMaxWords_ReturnsSuffix(int maxWords)
    {
        Assert.Equal("...", "Hello World".TruncateWords(maxWords));
    }

    [Fact]
    public void TruncateWords_DefaultSuffix_UsesEllipsis()
    {
        Assert.Equal("Hello...", "Hello World Foo".TruncateWords(1));
    }

    [Fact]
    public void TruncateWords_SingleWord_NoTruncation()
    {
        Assert.Equal("Hello", "Hello".TruncateWords(1));
    }

    [Fact]
    public void TruncateWords_MultipleSpaces_HandledCorrectly()
    {
        Assert.Equal("The quick...", "The  quick   brown fox".TruncateWords(2));
    }
}

public class ToSlugTests
{
    [Theory]
    [InlineData("Hello World", "hello-world")]
    [InlineData("Héllo Wörld!", "hello-world")]
    [InlineData("  Hello   World  ", "hello-world")]
    [InlineData("Hello---World", "hello-world")]
    [InlineData("Hello_World", "hello-world")]
    [InlineData("Already-slug", "already-slug")]
    [InlineData("Special!@#$%Characters", "special-characters")]
    public void ToSlug_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToSlug());
    }

    [Fact]
    public void ToSlug_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToSlug());
    }

    [Fact]
    public void ToSlug_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToSlug());
    }

    [Fact]
    public void ToSlug_WithNumbers_PreservesNumbers()
    {
        Assert.Equal("hello-world-123", "Hello World 123".ToSlug());
    }

    [Fact]
    public void ToSlug_OnlySpecialChars_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "!@#$%".ToSlug());
    }
}

public class ToSnakeCaseTests
{
    [Theory]
    [InlineData("HelloWorld", "hello_world")]
    [InlineData("helloWorld", "hello_world")]
    [InlineData("Hello World", "hello_world")]
    [InlineData("hello_world", "hello_world")]
    [InlineData("Hello-World", "hello_world")]
    [InlineData("XMLParser", "xml_parser")]
    [InlineData("getHTTPResponse", "get_http_response")]
    public void ToSnakeCase_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToSnakeCase());
    }

    [Fact]
    public void ToSnakeCase_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToSnakeCase());
    }

    [Fact]
    public void ToSnakeCase_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToSnakeCase());
    }

    [Fact]
    public void ToSnakeCase_SingleWord_ReturnsLowercase()
    {
        Assert.Equal("hello", "Hello".ToSnakeCase());
    }
}

public class ToCamelCaseTests
{
    [Theory]
    [InlineData("hello_world", "helloWorld")]
    [InlineData("Hello World", "helloWorld")]
    [InlineData("hello-world", "helloWorld")]
    [InlineData("HelloWorld", "helloWorld")]
    [InlineData("HELLO_WORLD", "helloWorld")]
    [InlineData("first_second_third", "firstSecondThird")]
    public void ToCamelCase_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToCamelCase());
    }

    [Fact]
    public void ToCamelCase_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToCamelCase());
    }

    [Fact]
    public void ToCamelCase_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToCamelCase());
    }

    [Fact]
    public void ToCamelCase_SingleWord_ReturnsLowercase()
    {
        Assert.Equal("hello", "Hello".ToCamelCase());
    }
}

public class ToPascalCaseTests
{
    [Theory]
    [InlineData("hello_world", "HelloWorld")]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("hello-world", "HelloWorld")]
    [InlineData("helloWorld", "HelloWorld")]
    [InlineData("HELLO_WORLD", "HelloWorld")]
    [InlineData("first_second_third", "FirstSecondThird")]
    public void ToPascalCase_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToPascalCase());
    }

    [Fact]
    public void ToPascalCase_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToPascalCase());
    }

    [Fact]
    public void ToPascalCase_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToPascalCase());
    }

    [Fact]
    public void ToPascalCase_SingleWord_ReturnsCapitalized()
    {
        Assert.Equal("Hello", "hello".ToPascalCase());
    }
}

public class ToKebabCaseTests
{
    [Theory]
    [InlineData("HelloWorld", "hello-world")]
    [InlineData("hello_world", "hello-world")]
    [InlineData("Hello World", "hello-world")]
    [InlineData("helloWorld", "hello-world")]
    [InlineData("XMLParser", "xml-parser")]
    public void ToKebabCase_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToKebabCase());
    }

    [Fact]
    public void ToKebabCase_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToKebabCase());
    }

    [Fact]
    public void ToKebabCase_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToKebabCase());
    }

    [Fact]
    public void ToKebabCase_SingleWord_ReturnsLowercase()
    {
        Assert.Equal("hello", "Hello".ToKebabCase());
    }
}

public class MaskTests
{
    [Theory]
    [InlineData("4111111111111111", 4, '*', "************1111")]
    [InlineData("1234567890", 4, '*', "******7890")]
    [InlineData("1234567890", 0, '*', "**********")]
    [InlineData("secret", 2, '#', "####et")]
    public void Mask_WithValidInput_ReturnsExpected(string input, int visibleChars, char maskChar, string expected)
    {
        Assert.Equal(expected, input.Mask(visibleChars, maskChar));
    }

    [Fact]
    public void Mask_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.Mask());
    }

    [Fact]
    public void Mask_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.Mask());
    }

    [Fact]
    public void Mask_StringShorterThanVisibleChars_ReturnsOriginal()
    {
        Assert.Equal("abc", "abc".Mask(5));
    }

    [Fact]
    public void Mask_StringEqualToVisibleChars_ReturnsOriginal()
    {
        Assert.Equal("abcd", "abcd".Mask(4));
    }

    [Fact]
    public void Mask_DefaultParameters_MasksAllButLast4()
    {
        Assert.Equal("************1111", "4111111111111111".Mask());
    }

    [Fact]
    public void Mask_NegativeVisibleChars_TreatedAsZero()
    {
        Assert.Equal("*****", "Hello".Mask(-1));
    }
}

public class BetweenTests
{
    [Theory]
    [InlineData("Hello [World] Foo", "[", "]", "World")]
    [InlineData("<div>content</div>", "<div>", "</div>", "content")]
    [InlineData("start-middle-end", "start-", "-end", "middle")]
    [InlineData("abc(xyz)def", "(", ")", "xyz")]
    public void Between_WithValidInput_ReturnsExpected(string input, string start, string end, string expected)
    {
        Assert.Equal(expected, input.Between(start, end));
    }

    [Fact]
    public void Between_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.Between("[", "]"));
    }

    [Fact]
    public void Between_EmptyInput_ReturnsNull()
    {
        Assert.Null(string.Empty.Between("[", "]"));
    }

    [Fact]
    public void Between_StartNotFound_ReturnsNull()
    {
        Assert.Null("Hello World".Between("[", "]"));
    }

    [Fact]
    public void Between_EndNotFound_ReturnsNull()
    {
        Assert.Null("Hello [World".Between("[", "]"));
    }

    [Fact]
    public void Between_EmptyStart_ReturnsNull()
    {
        Assert.Null("Hello".Between("", "]"));
    }

    [Fact]
    public void Between_EmptyEnd_ReturnsNull()
    {
        Assert.Null("Hello".Between("[", ""));
    }

    [Fact]
    public void Between_NullStart_ReturnsNull()
    {
        Assert.Null("Hello".Between(null!, "]"));
    }

    [Fact]
    public void Between_NullEnd_ReturnsNull()
    {
        Assert.Null("Hello".Between("[", null!));
    }

    [Fact]
    public void Between_EmptyResultBetweenDelimiters_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, "[]".Between("[", "]"));
    }
}

public class BeforeTests
{
    [Theory]
    [InlineData("Hello-World", "-", "Hello")]
    [InlineData("foo@bar.com", "@", "foo")]
    [InlineData("one::two::three", "::", "one")]
    public void Before_WithValidInput_ReturnsExpected(string input, string separator, string expected)
    {
        Assert.Equal(expected, input.Before(separator));
    }

    [Fact]
    public void Before_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.Before("-"));
    }

    [Fact]
    public void Before_SeparatorNotFound_ReturnsOriginal()
    {
        Assert.Equal("Hello World", "Hello World".Before("xyz"));
    }

    [Fact]
    public void Before_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.Before("-"));
    }

    [Fact]
    public void Before_EmptySeparator_ReturnsOriginal()
    {
        Assert.Equal("Hello", "Hello".Before(""));
    }

    [Fact]
    public void Before_SeparatorAtStart_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "-Hello".Before("-"));
    }
}

public class AfterTests
{
    [Theory]
    [InlineData("Hello-World", "-", "World")]
    [InlineData("foo@bar.com", "@", "bar.com")]
    [InlineData("one::two::three", "::", "two::three")]
    public void After_WithValidInput_ReturnsExpected(string input, string separator, string expected)
    {
        Assert.Equal(expected, input.After(separator));
    }

    [Fact]
    public void After_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.After("-"));
    }

    [Fact]
    public void After_SeparatorNotFound_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "Hello World".After("xyz"));
    }

    [Fact]
    public void After_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.After("-"));
    }

    [Fact]
    public void After_EmptySeparator_ReturnsOriginal()
    {
        Assert.Equal("Hello", "Hello".After(""));
    }

    [Fact]
    public void After_SeparatorAtEnd_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "Hello-".After("-"));
    }
}

public class ContainsAllTests
{
    [Fact]
    public void ContainsAll_AllPresent_ReturnsTrue()
    {
        Assert.True("Hello World Foo Bar".ContainsAll("Hello", "World", "Foo"));
    }

    [Fact]
    public void ContainsAll_CaseInsensitiveByDefault_ReturnsTrue()
    {
        Assert.True("Hello World".ContainsAll("hello", "WORLD"));
    }

    [Fact]
    public void ContainsAll_OneMissing_ReturnsFalse()
    {
        Assert.False("Hello World".ContainsAll("Hello", "Missing"));
    }

    [Fact]
    public void ContainsAll_NullInput_ReturnsFalse()
    {
        string? value = null;
        Assert.False(value.ContainsAll("test"));
    }

    [Fact]
    public void ContainsAll_EmptyValues_ReturnsFalse()
    {
        Assert.False("Hello".ContainsAll());
    }

    [Fact]
    public void ContainsAll_WithStringComparison_CaseSensitive()
    {
        Assert.False("Hello World".ContainsAll(StringComparison.Ordinal, "hello"));
        Assert.True("Hello World".ContainsAll(StringComparison.Ordinal, "Hello"));
    }

    [Fact]
    public void ContainsAll_EmptyInput_ReturnsFalse()
    {
        Assert.False(string.Empty.ContainsAll("test"));
    }

    [Fact]
    public void ContainsAll_NullValues_ReturnsFalse()
    {
        Assert.False("Hello".ContainsAll(StringComparison.Ordinal, null!));
    }
}

public class ContainsAnyTests
{
    [Fact]
    public void ContainsAny_OnePresent_ReturnsTrue()
    {
        Assert.True("Hello World".ContainsAny("Missing", "World"));
    }

    [Fact]
    public void ContainsAny_CaseInsensitiveByDefault_ReturnsTrue()
    {
        Assert.True("Hello World".ContainsAny("hello"));
    }

    [Fact]
    public void ContainsAny_NonePresent_ReturnsFalse()
    {
        Assert.False("Hello World".ContainsAny("Missing", "NotHere"));
    }

    [Fact]
    public void ContainsAny_NullInput_ReturnsFalse()
    {
        string? value = null;
        Assert.False(value.ContainsAny("test"));
    }

    [Fact]
    public void ContainsAny_EmptyValues_ReturnsFalse()
    {
        Assert.False("Hello".ContainsAny());
    }

    [Fact]
    public void ContainsAny_WithStringComparison_CaseSensitive()
    {
        Assert.False("Hello World".ContainsAny(StringComparison.Ordinal, "hello"));
        Assert.True("Hello World".ContainsAny(StringComparison.Ordinal, "Hello"));
    }

    [Fact]
    public void ContainsAny_EmptyInput_ReturnsFalse()
    {
        Assert.False(string.Empty.ContainsAny("test"));
    }

    [Fact]
    public void ContainsAny_NullValues_ReturnsFalse()
    {
        Assert.False("Hello".ContainsAny(StringComparison.Ordinal, null!));
    }
}

public class CountOccurrencesTests
{
    [Theory]
    [InlineData("hello hello hello", "hello", 3)]
    [InlineData("aaaa", "aa", 2)]
    [InlineData("Hello World", "xyz", 0)]
    [InlineData("abcabc", "abc", 2)]
    [InlineData("aaa", "a", 3)]
    public void CountOccurrences_WithValidInput_ReturnsExpected(string input, string substring, int expected)
    {
        Assert.Equal(expected, input.CountOccurrences(substring));
    }

    [Fact]
    public void CountOccurrences_NullInput_ReturnsZero()
    {
        string? value = null;
        Assert.Equal(0, value.CountOccurrences("test"));
    }

    [Fact]
    public void CountOccurrences_EmptyInput_ReturnsZero()
    {
        Assert.Equal(0, string.Empty.CountOccurrences("test"));
    }

    [Fact]
    public void CountOccurrences_NullSubstring_ReturnsZero()
    {
        Assert.Equal(0, "Hello".CountOccurrences(null!));
    }

    [Fact]
    public void CountOccurrences_EmptySubstring_ReturnsZero()
    {
        Assert.Equal(0, "Hello".CountOccurrences(""));
    }
}

public class RemoveDiacriticsTests
{
    [Theory]
    [InlineData("héllo wörld", "hello world")]
    [InlineData("café", "cafe")]
    [InlineData("naïve", "naive")]
    [InlineData("résumé", "resume")]
    [InlineData("über", "uber")]
    public void RemoveDiacritics_WithDiacritics_ReturnsCleanString(string input, string expected)
    {
        Assert.Equal(expected, input.RemoveDiacritics());
    }

    [Fact]
    public void RemoveDiacritics_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.RemoveDiacritics());
    }

    [Fact]
    public void RemoveDiacritics_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.RemoveDiacritics());
    }

    [Fact]
    public void RemoveDiacritics_NoDiacritics_ReturnsOriginal()
    {
        Assert.Equal("Hello World", "Hello World".RemoveDiacritics());
    }
}

public class StripHtmlTests
{
    [Theory]
    [InlineData("<p>Hello</p>", "Hello")]
    [InlineData("<div><b>Hello</b> World</div>", "Hello World")]
    [InlineData("No tags here", "No tags here")]
    [InlineData("<br/>", "")]
    [InlineData("<a href=\"url\">Link</a>", "Link")]
    public void StripHtml_WithHtml_ReturnsStrippedString(string input, string expected)
    {
        Assert.Equal(expected, input.StripHtml());
    }

    [Fact]
    public void StripHtml_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.StripHtml());
    }

    [Fact]
    public void StripHtml_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.StripHtml());
    }

    [Fact]
    public void StripHtml_WithEntities_PreservesEntities()
    {
        Assert.Equal("&amp; &lt;", "<p>&amp; &lt;</p>".StripHtml());
    }

    [Fact]
    public void StripHtml_SelfClosingTags_Removed()
    {
        Assert.Equal("Hello World", "Hello<br />World".StripHtml());
    }
}

public class IsValidEmailTests
{
    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.co.uk", true)]
    [InlineData("user+tag@example.com", true)]
    [InlineData("user@sub.domain.com", true)]
    [InlineData("notanemail", false)]
    [InlineData("@domain.com", false)]
    [InlineData("user@", false)]
    [InlineData("user@domain", false)]
    [InlineData("", false)]
    [InlineData("user @domain.com", false)]
    public void IsValidEmail_WithInput_ReturnsExpected(string input, bool expected)
    {
        Assert.Equal(expected, input.IsValidEmail());
    }

    [Fact]
    public void IsValidEmail_NullInput_ReturnsFalse()
    {
        string? value = null;
        Assert.False(value.IsValidEmail());
    }

    [Fact]
    public void IsValidEmail_WhitespaceInput_ReturnsFalse()
    {
        Assert.False("   ".IsValidEmail());
    }
}

public class IsValidUrlTests
{
    [Theory]
    [InlineData("https://example.com", true)]
    [InlineData("http://example.com", true)]
    [InlineData("https://example.com/path?query=1", true)]
    [InlineData("ftp://example.com", false)]
    [InlineData("not-a-url", false)]
    [InlineData("", false)]
    [InlineData("example.com", false)]
    public void IsValidUrl_WithInput_ReturnsExpected(string input, bool expected)
    {
        Assert.Equal(expected, input.IsValidUrl());
    }

    [Fact]
    public void IsValidUrl_NullInput_ReturnsFalse()
    {
        string? value = null;
        Assert.False(value.IsValidUrl());
    }

    [Fact]
    public void IsValidUrl_WhitespaceInput_ReturnsFalse()
    {
        Assert.False("   ".IsValidUrl());
    }
}

public class IsNullOrEmptyTests
{
    [Fact]
    public void IsNullOrEmpty_NullInput_ReturnsTrue()
    {
        string? value = null;
        Assert.True(value.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyInput_ReturnsTrue()
    {
        Assert.True(string.Empty.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_WhitespaceInput_ReturnsFalse()
    {
        Assert.False("   ".IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_ValidInput_ReturnsFalse()
    {
        Assert.False("Hello".IsNullOrEmpty());
    }
}

public class IsNullOrWhiteSpaceTests
{
    [Fact]
    public void IsNullOrWhiteSpace_NullInput_ReturnsTrue()
    {
        string? value = null;
        Assert.True(value.IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_EmptyInput_ReturnsTrue()
    {
        Assert.True(string.Empty.IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_WhitespaceInput_ReturnsTrue()
    {
        Assert.True("   ".IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_TabAndNewline_ReturnsTrue()
    {
        Assert.True("\t\n".IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_ValidInput_ReturnsFalse()
    {
        Assert.False("Hello".IsNullOrWhiteSpace());
    }
}

public class RepeatTests
{
    [Theory]
    [InlineData("ab", 3, "ababab")]
    [InlineData("x", 5, "xxxxx")]
    [InlineData("Hello", 1, "Hello")]
    [InlineData("ab", 0, "")]
    public void Repeat_WithValidInput_ReturnsExpected(string input, int count, string expected)
    {
        Assert.Equal(expected, input.Repeat(count));
    }

    [Fact]
    public void Repeat_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.Repeat(3));
    }

    [Fact]
    public void Repeat_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.Repeat(5));
    }

    [Fact]
    public void Repeat_NegativeCount_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "Hello".Repeat(-1));
    }

    [Fact]
    public void Repeat_ZeroCount_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "Hello".Repeat(0));
    }
}

public class ToTitleCaseTests
{
    [Theory]
    [InlineData("hello world", "Hello World")]
    [InlineData("HELLO WORLD", "Hello World")]
    [InlineData("hello", "Hello")]
    [InlineData("a tale of two cities", "A Tale Of Two Cities")]
    public void ToTitleCase_WithValidInput_ReturnsExpected(string input, string expected)
    {
        Assert.Equal(expected, input.ToTitleCase());
    }

    [Fact]
    public void ToTitleCase_NullInput_ReturnsNull()
    {
        string? value = null;
        Assert.Null(value.ToTitleCase());
    }

    [Fact]
    public void ToTitleCase_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, string.Empty.ToTitleCase());
    }

    [Fact]
    public void ToTitleCase_SingleChar_ReturnsCapitalized()
    {
        Assert.Equal("A", "a".ToTitleCase());
    }
}
