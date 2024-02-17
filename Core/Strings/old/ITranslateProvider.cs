namespace Galaxon.Core.Strings.Translate;

public interface ITranslateProvider
{
    Task<string> ExecuteAsync(
        string text, string targetLangCode, CancellationToken cancellationToken);
}
