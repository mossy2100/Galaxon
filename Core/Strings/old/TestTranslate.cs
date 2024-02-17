using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.Core.Strings.old;

[TestClass]
public class TestTranslate
{
    [TestMethod]
    public void TestTranslation()
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
            "/Users/shaun/Documents/Web & software development/C#/Projects/Galaxon/Core/Strings/translation-374919-e49a70ea084f.json");

        string en = "Hello and welcome to Shanghai";
        string zh = "您好，欢迎来到上海";

        var provider = new TranslateProvider();
        string translated = provider.Execute(en, "zh-CN");

        Trace.WriteLine($"{en} => {zh}");
        Assert.AreEqual(zh, translated);
    }
}
