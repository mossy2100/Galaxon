using Galaxon.Core.Strings;

namespace Galaxon.Tests.Core.Strings;

[TestClass]
public class StringExtensionsTests
{
    #region EqualsIgnoreCase

    [TestMethod]
    public void EqualsIgnoreCase_TwoEmptyStrings_ReturnsTrue()
    {
        // Arrange
        string str1 = "";
        string str2 = "";

        // Act
        bool result = str1.EqualsIgnoreCase(str2);

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("hello", "hello")]
    [DataRow("hello", "Hello")]
    [DataRow("Hello", "hello")]
    [DataRow("hello", "HELLO")]
    [DataRow("HELLO", "hello")]
    public void EqualsIgnoreCase_TwoEqualStrings_ReturnsTrue(string str1, string str2)
    {
        // Act
        bool result = str1.EqualsIgnoreCase(str2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EqualsIgnoreCase_TwoDifferentStrings_ReturnsFalse()
    {
        // Arrange
        string str1 = "Hello";
        string str2 = "World";

        // Act
        bool result = str1.EqualsIgnoreCase(str2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsIgnoreCase_EmptyAndNull_ReturnsFalse()
    {
        // Arrange
        string str1 = "";
        string? str2 = null;

        // Act
        bool result = str1.EqualsIgnoreCase(str2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsIgnoreCase_NonEmptyAndNull_ReturnsFalse()
    {
        // Arrange
        string str1 = "Hello";
        string? str2 = null;

        // Act
        bool result = str1.EqualsIgnoreCase(str2);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion EqualsIgnoreCase

    #region ReplaceChars

    [TestMethod]
    public void ReplaceChars_WithValidCharMap_ReturnsTransformedString()
    {
        // Arrange
        string original = "Hello";
        Dictionary<char, string> charMap = new Dictionary<char, string>
        {
            { 'H', "X" },
            { 'e', "Y" },
            { 'l', "Z" },
            { 'o', "W" }
        };
        string expected = "XYZZW";

        // Act
        string result = original.ReplaceChars(charMap);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReplaceChars_WithEmptyCharMap_ReturnsOriginalString()
    {
        // Arrange
        string original = "Hello";
        Dictionary<char, string> charMap = new Dictionary<char, string>();
        string expected = "Hello";

        // Act
        string result = original.ReplaceChars(charMap);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReplaceChars_WithKeepCharsNotInMapTrue_ReturnsStringWithUnmappedChars()
    {
        // Arrange
        string original = "Hello";
        Dictionary<char, string> charMap = new Dictionary<char, string>
        {
            { 'H', "X" },
            { 'e', "Y" },
        };
        string expected = "XYllo";

        // Act
        string result = original.ReplaceChars(charMap);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReplaceChars_WithKeepCharsNotInMapFalse_ReturnsStringWithoutUnmappedChars()
    {
        // Arrange
        string original = "Hello";
        Dictionary<char, string> charMap = new Dictionary<char, string>
        {
            { 'H', "X" },
            { 'e', "Y" },
        };
        string expected = "XY";

        // Act
        string result = original.ReplaceChars(charMap, false);

        // Assert
        Assert.AreEqual(expected, result);
    }

    #endregion ReplaceChars

    #region Repeat

    [TestMethod]
    public void Repeat_WithPositiveNumber_ReturnsRepeatedString()
    {
        // Arrange
        string inputString = "abc";
        int repeatCount = 3;
        string expected = "abcabcabc";

        // Act
        string result = inputString.Repeat(repeatCount);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Repeat_WithZero_ReturnsEmptyString()
    {
        // Arrange
        string inputString = "abc";
        int repeatCount = 0;
        string expected = "";

        // Act
        string result = inputString.Repeat(repeatCount);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Repeat_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        string inputString = "";
        int repeatCount = 5;
        string expected = "";

        // Act
        string result = inputString.Repeat(repeatCount);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Repeat_WithNegativeNumber_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        string inputString = "abc";
        int repeatCount = -2;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            inputString.Repeat(repeatCount);
        });
    }

    #endregion Repeat

    #region StripBrackets

    [TestMethod]
    public void StripBrackets_ShouldRemoveRoundBracketsAndContents()
    {
        var input = "Hello (World)";
        var expected = "Hello ";

        var result = input.StripBrackets(EBracketsType.Round);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripBrackets_ShouldRemoveSquareBracketsAndContents()
    {
        var input = "Hello [World]";
        var expected = "Hello ";

        var result = input.StripBrackets(EBracketsType.Square);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripBrackets_ShouldRemoveCurlyBracketsAndContents()
    {
        var input = "Hello {World}";
        var expected = "Hello ";

        var result = input.StripBrackets(EBracketsType.Curly);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripBrackets_ShouldRemoveAngleBracketsAndContents()
    {
        var input = "Hello <World>";
        var expected = "Hello ";

        var result = input.StripBrackets(EBracketsType.Angle);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripBrackets_ShouldRemoveNothing()
    {
        var input = "Hello <World>";
        var expected = "Hello <World>";

        var result = input.StripBrackets(EBracketsType.Curly);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void StripBrackets_ShouldThrowWhenInvalidBracketType()
    {
        var input = "Hello <World>";

        // This will throw as ((EBracketsType)5) is not a valid value of the EBracketsType enum
        input.StripBrackets((EBracketsType)5);
    }

    #endregion StripBrackets

    #region StripTags

    [TestMethod]
    public void StripTags_ShouldRemoveHtmlTags()
    {
        var input = "<h1>Hello World!</h1>";
        var expected = "Hello World!";

        var result = input.StripTags();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripTags_ShouldHandleEmptyStrings()
    {
        var input = "";
        var expected = "";

        var result = input.StripTags();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void StripTags_ShouldHandleStringsWithoutHtmlTags()
    {
        var input = "Hello World!";
        var expected = "Hello World!";

        var result = input.StripTags();

        Assert.AreEqual(expected, result);
    }

    #endregion StripTags

    #region IsAscii

    [TestMethod]
    public void IsAscii_WithAsciiCharacters_ReturnsTrue()
    {
        // Arrange
        string input = "Hello, world!";

        // Act
        bool result = input.IsAscii();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsAscii_WithNonAsciiCharacters_ReturnsFalse()
    {
        // Arrange
        string input = "Привет, мир!";

        // Act
        bool result = input.IsAscii();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsAscii_WithEmptyString_ReturnsTrue()
    {
        // Arrange
        string input = "";

        // Act
        bool result = input.IsAscii();

        // Assert
        Assert.IsTrue(result);
    }

    #endregion IsAscii

    #region IsPalindrome

    [TestMethod]
    public void IsPalindrome_WithPalindromeString_ReturnsTrue()
    {
        // Arrange
        string palindrome = "racecar";

        // Act
        bool result = palindrome.IsPalindrome();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPalindrome_WithNonPalindromeString_ReturnsFalse()
    {
        // Arrange
        string nonPalindrome = "hello";

        // Act
        bool result = nonPalindrome.IsPalindrome();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsPalindrome_WithEmptyString_ReturnsTrue()
    {
        // Arrange
        string emptyString = "";

        // Act
        bool result = emptyString.IsPalindrome();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPalindrome_WithSingleCharacterString_ReturnsTrue()
    {
        // Arrange
        string singleCharString = "a";

        // Act
        bool result = singleCharString.IsPalindrome();

        // Assert
        Assert.IsTrue(result);
    }

    #endregion IsPalindrome

    #region Small caps

    [TestMethod]
    public void ToSmallCaps_ReturnsCorrectValue()
    {
        var s1 = "A quick brown fox jumps over the lazy dog.";
        var s2 = s1.ToSmallCaps();
        Assert.AreEqual("A ꞯᴜɪᴄᴋ ʙʀᴏᴡɴ ꜰᴏx ᴊᴜᴍᴘꜱ ᴏᴠᴇʀ ᴛʜᴇ ʟᴀᴢʏ ᴅᴏɢ.", s2);

        // Article title. Example with numbers and symbols.
        s1 = "10 Tips To Become A Good Programmer - C# Corner";
        s2 = s1.ToSmallCaps();
        Assert.AreEqual("10 Tɪᴘꜱ Tᴏ Bᴇᴄᴏᴍᴇ A Gᴏᴏᴅ Pʀᴏɢʀᴀᴍᴍᴇʀ - C# Cᴏʀɴᴇʀ", s2);
    }

    [TestMethod]
    public void ToSmallCaps_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        string s1 = "";

        // Act
        string s2 = s1.ToSmallCaps();

        // Assert
        Assert.AreEqual("", s2);
    }

    #endregion Small caps

    #region ToUpperFirstLetter

    [TestMethod]
    public void ToUpperFirstLetter_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.ToUpperFirstLetter();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void
        ToUpperFirstLetter_StringWithLowercaseFirstLetter_ReturnsStringWithUppercaseFirstLetter()
    {
        // Arrange
        string input = "hello world";
        string expected = "Hello world";

        // Act
        string result = input.ToUpperFirstLetter();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToUpperFirstLetter_StringWithUppercaseFirstLetter_ReturnsSameString()
    {
        // Arrange
        string input = "Hello World";

        // Act
        string result = input.ToUpperFirstLetter();

        // Assert
        Assert.AreEqual(input, result);
    }

    #endregion ToUpperFirstLetter

    #region ToLowerFirstLetter

    [TestMethod]
    public void ToLowerFirstLetter_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.ToLowerFirstLetter();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void ToLowerFirstLetter_StringWithUppercaseFirstLetter_ReturnsStringWithLowercaseFirstLetter()
    {
        // Arrange
        string input = "Hello world";
        string expected = "hello world";

        // Act
        string result = input.ToLowerFirstLetter();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToLowerFirstLetter_StringWithLowercaseFirstLetter_ReturnsSameString()
    {
        // Arrange
        string input = "hello world";

        // Act
        string result = input.ToLowerFirstLetter();

        // Assert
        Assert.AreEqual(input, result);
    }

    #endregion ToLowerFirstLetter

    #region ToProper

    [TestMethod]
    public void ToProper_GivenEmptyString_ReturnsEmptyString()
    {
        string expected = "";
        string actual = expected.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProper_OneWord_ReturnsCorrectValue()
    {
        string source = "cat";
        string expected = "Cat";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProper_Title_ReturnsCorrectValue()
    {
        string source = "How to cook dairy-free macaroni";
        string expected = "How To Cook Dairy-Free Macaroni";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't lower-case any characters, even if the words aren't acronyms.
    /// Also shows one way to solve this problem.
    /// </summary>
    [TestMethod]
    public void ToProper_AllUpperCase_ReturnsCorrectValue()
    {
        string source = "HERE IS A SIMPLE TITLE, ALL UPPER-CASE";
        string expected = "HERE IS A SIMPLE TITLE, ALL UPPER-CASE";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);

        // Use ToLower() before ToProper() in order to convert words that look like acronyms to
        // words that look like words.
        source = source.ToLower();
        expected = "here is a simple title, all upper-case";
        Assert.AreEqual(expected, source);

        expected = "Here Is A Simple Title, All Upper-Case";
        actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test handling of ASCII apostrophe.
    /// </summary>
    [TestMethod]
    public void ToProper_WithAsciiApostrophe_ReturnsCorrectValue()
    {
        string source = "I can't believe it's not Java!";
        string expected = "I Can't Believe It's Not Java!";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test handling of Unicode apostrophe.
    /// </summary>
    [TestMethod]
    public void ToProper_WithUnicodeApostrophe_ReturnsCorrectValue()
    {
        string source = "let’s cook bill’s and your friends’ meals.";
        string expected = "Let’s Cook Bill’s And Your Friends’ Meals.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Test handling of words beginning with or preceded by apostrophes.
    /// </summary>
    [TestMethod]
    public void ToProper_ApostrophesAtStartOfWords_ReturnsCorrectValue()
    {
        string source = "'don’t worry ’bout a thing,' she said.";
        string expected = "'Don’t Worry ’Bout A Thing,' She Said.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't lower-case letters after the first one in the word, thereby
    /// preserving proper nouns with apostrophes.
    /// </summary>
    [TestMethod]
    public void ToProper_NounsWithApostrophes_ReturnsCorrectValue()
    {
        string source = "Seamus O'Henry loves pretending he's T'Challa.";
        string expected = "Seamus O'Henry Loves Pretending He's T'Challa.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the ToProper method doesn't recognise lower camel case names.
    /// </summary>
    [TestMethod]
    public void ToProper_CamelCase_DoesNotRecognise()
    {
        string source = "Correct use of the JavaScript method getElementById().";
        string expected = "Correct Use Of The JavaScript Method GetElementById().";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method properly handles acronyms.
    /// </summary>
    [TestMethod]
    public void ToProper_Acronyms_ReturnsCorrect_Value()
    {
        string source = "How to work for NASA on UAVs and HLVs.";
        string expected = "How To Work For NASA On UAVs And HLVs.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method properly handles hyphens.
    /// </summary>
    [TestMethod]
    public void ToProper_Hyphens_ReturnsCorrect_Value()
    {
        string source = "Olivia Newton-John: How to thrive in a fast-changing world.";
        string expected = "Olivia Newton-John: How To Thrive In A Fast-Changing World.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    #endregion ToProper

    #region GetCase

    [TestMethod]
    public void GetCase_EmptyString_ReturnsNone()
    {
        // Arrange
        string input = "";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.None, result);
    }

    [TestMethod]
    public void GetCase_NumericString_ReturnsNone()
    {
        // Arrange
        string input = "3.1416";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.None, result);
    }

    [TestMethod]
    public void GetCase_LowerCaseString_ReturnsLower()
    {
        // Arrange
        string input = "hello world";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.Lower, result);
    }

    [TestMethod]
    public void GetCase_UpperCaseString_ReturnsUpper()
    {
        // Arrange
        string input = "HELLO WORLD";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.Upper, result);
    }

    [TestMethod]
    public void GetCase_ProperCaseString_ReturnsProper()
    {
        // Arrange
        string input = "Hello World";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.Proper, result);
    }

    [TestMethod]
    public void GetCase_UpperFirstLetterString_ReturnsUpperFirstLetter()
    {
        // Arrange
        string input = "Hello world";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.UpperFirstLetter, result);
    }

    [TestMethod]
    public void GetCase_MixedcaseString_ReturnsMixed()
    {
        // Arrange
        string input = "hElLo WoRlD";

        // Act
        EStringCase result = input.GetCase();

        // Assert
        Assert.AreEqual(EStringCase.Mixed, result);
    }

    #endregion GetCase

    #region SetCase

    [TestMethod]
    public void SetCase_ConvertToLower_Success()
    {
        // Arrange
        string input = "HELLO";
        string expected = "hello";

        // Act
        string result = input.SetCase(EStringCase.Lower);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_ConvertToUpper_Success()
    {
        // Arrange
        string input = "hello";
        string expected = "HELLO";

        // Act
        string result = input.SetCase(EStringCase.Upper);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_ConvertToProper_Success()
    {
        // Arrange
        string input = "hello world";
        string expected = "Hello World";

        // Act
        string result = input.SetCase(EStringCase.Proper);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_ConvertToUpperFirstLetter_Success()
    {
        // Arrange
        string input = "hello world";
        string expected = "Hello world";

        // Act
        string result = input.SetCase(EStringCase.UpperFirstLetter);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_InputHasNoCase_ReturnsSameString()
    {
        // Arrange
        string input = "123";
        string expected = input;

        // Act
        string result = input.SetCase(EStringCase.Lower);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_InputEmptyString_ReturnsEmptyString()
    {
        // Arrange
        string input = "";
        string expected = "";

        // Act
        string result = input.SetCase(EStringCase.Lower);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_InputHasLettersAndDesiredCaseIsNone_ThrowsException()
    {
        // Arrange
        string input = "Hello";

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => input.SetCase(EStringCase.None));
    }

    [TestMethod]
    public void SetCase_InputHasNoLettersAndDesiredCaseIsNone_ReturnsSameString()
    {
        // Arrange
        string input = "1234-5678";
        string expected = "1234-5678";

        // Act
        string result = input.SetCase(EStringCase.None);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_DesiredCaseIsMixed_ReturnsSameString()
    {
        // Arrange
        string input = "Phone 1300-TEST-CASE";
        string expected = "Phone 1300-TEST-CASE";

        // Act
        string result = input.SetCase(EStringCase.Mixed);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetCase_InvalidInput_ThrowsException()
    {
        var original = "original";
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => original.SetCase((EStringCase)10));
    }

    #endregion SetCase

    #region PadBoth

    [TestMethod]
    public void PadBoth_WithSpaces_EqualSpacing()
    {
        const string original = "Hello";
        const string expected = "  Hello  ";

        Assert.AreEqual(expected, original.PadBoth(9, ' '));
    }

    [TestMethod]
    public void PadBoth_WithSpaces_ExtraOnLeft()
    {
        const string original = "Hello";
        const string expected = "  Hello ";

        Assert.AreEqual(expected, original.PadBoth(8, ' ', false));
    }

    [TestMethod]
    public void PadBoth_WithSpaces_ExtraOnRight()
    {
        const string original = "Hello";
        const string expected = " Hello  ";

        Assert.AreEqual(expected, original.PadBoth(8, ' ', true));
    }

    [TestMethod]
    public void PadBoth_WithStringLongerThanLength()
    {
        const string original = "Hello";
        const string expected = "Hello";

        Assert.AreEqual(expected, original.PadBoth(4, ' ', true));
    }

    #endregion PadBoth

    #region ZeroPad

    [TestMethod]
    public void ZeroPad_WidthGreaterThanStringLength_ReturnsPaddedString()
    {
        // Arrange
        string input = "123";
        int width = 5;
        string expected = "00123";

        // Act
        string result = input.ZeroPad(width);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ZeroPad_WidthEqualsStringLength_ReturnsOriginalString()
    {
        // Arrange
        string input = "123";
        int width = 3;
        string expected = input;

        // Act
        string result = input.ZeroPad(width);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ZeroPad_WidthLessThanStringLength_ReturnsOriginalString()
    {
        // Arrange
        string input = "123";
        int width = 2;
        string expected = input;

        // Act
        string result = input.ZeroPad(width);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ZeroPad_WidthIsZero_ThrowsException()
    {
        // Arrange
        string input = "123";
        int width = 0;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            string _ = input.ZeroPad(width);
        });
    }

    [TestMethod]
    public void ZeroPad_WidthIsNegative_ThrowsException()
    {
        // Arrange
        string input = "123";
        int width = -5;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            string _ = input.ZeroPad(width);
        });
    }

    #endregion ZeroPad

    #region GroupDigits

    [TestMethod]
    public void GroupDigits_DefaultSeparatorAndSize()
    {
        // Arrange
        string input = "1234567890";
        string expected = "1,234,567,890";

        // Act
        string result = input.GroupDigits();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GroupDigits_CustomSeparator()
    {
        // Arrange
        string input = "1234567890";
        string expected = "1_234_567_890";

        // Act
        string result = input.GroupDigits('_');

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GroupDigits_CustomSize()
    {
        // Arrange
        string input = "1234567890";
        string expected = "12,3456,7890";

        // Act
        string result = input.GroupDigits(',', 4);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GroupDigits_HexadecimalWithZeroPad()
    {
        // Arrange
        string input = "4169e1";
        string expected = "0041_69e1";

        // Act
        string result = input.ZeroPad(8).GroupDigits('_', 4);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GroupDigits_EmptyString()
    {
        // Arrange
        string input = "";
        string expected = "";

        // Act
        string result = input.GroupDigits();

        // Assert
        Assert.AreEqual(expected, result);
    }

    #endregion GroupDigits
}
