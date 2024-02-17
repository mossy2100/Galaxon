using Google.Cloud.Translate.V3;
using Nito.AsyncEx;
using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Strings.Translate;

public class TranslateProvider : ITranslateProvider
{
    private const string _ProjectId = "translation-374919";

    private static readonly AsyncLazy<TranslationServiceClient> s_client =
        new (() => TranslationServiceClient.CreateAsync());

    public async Task<string> ExecuteAsync(string text, string targetLangCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        if (string.IsNullOrWhiteSpace(targetLangCode))
        {
            throw new ArgumentInvalidException(
                nameof(targetLangCode), "A target language code must be specified.");
        }

        TranslationServiceClient client = await s_client;

        TranslateTextResponse? response = await client.TranslateTextAsync(
            $"projects/{_ProjectId}", targetLangCode, new [] { text }, cancellationToken);

        // Will always contain a single entry.
        return response.Translations[0].TranslatedText;
    }

    public string Execute(string text, string targetLangCode)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        if (string.IsNullOrWhiteSpace(targetLangCode))
        {
            throw new ArgumentInvalidException(
                nameof(targetLangCode), "A target language code must be specified.");
        }

        TranslationServiceClient client = s_client.Task.Result;

        TranslateTextResponse? response = client.TranslateText(
            $"projects/{_ProjectId}", targetLangCode, new [] { text });

        // Will always contain a single entry.
        return response.Translations[0].TranslatedText;
    }

    public string GetSupportedLanguages()
    {
        TranslationServiceClient client = s_client.Task.Result;
        SupportedLanguages? supportedLangs = client.GetSupportedLanguages($"projects/{_ProjectId}", "", "en-AU");
        // Console.WriteLine(supportedLangs);
        return supportedLangs.ToString();
    }
}
