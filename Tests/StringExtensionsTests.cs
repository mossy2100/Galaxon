using Galaxon.Core.Strings;

namespace Galaxon.Tests;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void MakeDigitsSuperscript_ReturnsCorrectValues()
    {
        string s1;
        string s2;

        s1 = "x2";
        s2 = s1.MakeDigitsSuperscript();
        Assert.AreEqual("x²", s2);

        s1 = "m/s2";
        s2 = s1.MakeDigitsSuperscript();
        Assert.AreEqual("m/s²", s2);

        s1 = "23";
        s2 = "6.02 * 10" + s1.MakeDigitsSuperscript();
        Assert.AreEqual("6.02 * 10²³", s2);
    }

    [TestMethod]
    public void MakeDigitsSubscript_ReturnsCorrectValues()
    {
        string s1;
        string s2;

        s1 = "CH4";
        s2 = s1.MakeDigitsSubscript();
        Assert.AreEqual("CH₄", s2);

        s1 = "CH3OH";
        s2 = s1.MakeDigitsSubscript();
        Assert.AreEqual("CH₃OH", s2);

        s1 = "v0";
        s2 = s1.MakeDigitsSubscript();
        Assert.AreEqual("v₀", s2);
    }

    [TestMethod]
    public void TestStringToSmallCaps()
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
    public void ToProperReturnsEmptyStringGivenEmptyString()
    {
        string expected = "";
        string actual = expected.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProperReturnsWordWithFirstLetterUpperCase()
    {
        string source = "cat";
        string expected = "Cat";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToProperMakesFirstLetterOfEachWordUpperCase()
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
    public void ToProperMakesCorrectLettersLowerCase()
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
    public void ToProperHandlesAsciiApostrophe()
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
    public void ToProperHandlesUnicodeApostrophe()
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
    public void ToProperHandlesApostrophesAtStartOfWords()
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
    public void ToProperHandlesProperNounsWithApostrophes()
    {
        string source = "Seamus O'Henry loves pretending he's T'Challa.";
        string expected = "Seamus O'Henry Loves Pretending He's T'Challa.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Show how the method doesn't properly handle camel case variable, method (etc.) names,
    /// because it doesn't know they aren't normal words.
    /// </summary>
    [TestMethod]
    public void ToProperDoesNotRecogniseCamelCase()
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
    public void ToProperHandlesAcronyms()
    {
        string source = "How to work for NASA on UAVs and HLVs.";
        string expected = "How To Work For NASA On UAVs And HLVs.";
        string actual = source.ToProper();
        Assert.AreEqual(expected, actual);
    }
}
