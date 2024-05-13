using System.Diagnostics;
using Galaxon.Development.Application;

namespace Galaxon.Tests.Development.WebTest;

[TestClass]
public class UriToolsTests
{
    [TestMethod]
    public void TestMakeSlug()
    {
        string s1;
        string s2;

        // English.
        s1 = "This is a normal, everyday title, written in English.";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("this-is-a-normal-everyday-title-written-in-english", s2);

        // Maths.
        s1 = "Einstein's famous equation is e=mc²";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("einsteins-famous-equation-is-e-mc2", s2);

        // Spanish.
        // Tests removal of diacritics but also trimming non-alphanumeric characters from the ends
        // of the string.
        s1 = "¡Hola! ¿Cómo estás hoy?";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("hola-como-estas-hoy", s2);

        // German.
        s1 = "Der Kurfürstendamm ist eine berühmte Straße in Berlin.";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("der-kurfurstendamm-ist-eine-beruhmte-strasse-in-berlin", s2);

        // Polish.
        s1 = "10 rzeczy, które warto wiedzieć o historii Polski.";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("10-rzeczy-ktore-warto-wiedziec-o-historii-polski", s2);

        // Japanese. "Hello, and welcome to Tokyo!"
        s1 = "こんにちは、東京へようこそ！";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("konnichiha-dongjingheyoukoso", s2);
        // The transliteration is not great for Asian languages.
        // It works better if you Romanize the text first using Google translate, e.g.
        s1 = "Kon'nichiwa, Tōkyō e yōkoso!";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("konnichiwa-tokyo-e-yokoso", s2);

        // Chinese. "We hope you enjoy your stay in Shanghai."
        s1 = "我们希望您在上海过得愉快。";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("womenxiwangninzaishanghaiguodeyukuai", s2);
        // With initial manual Romanization.
        s1 = "Wǒmen xīwàng nín zài shànghǎiguò dé yúkuài.";
        s2 = UriTools.MakeSlug(s1);
        Trace.WriteLine(s2);
        Assert.AreEqual("women-xiwang-nin-zai-shanghaiguo-de-yukuai", s2);
    }
}
