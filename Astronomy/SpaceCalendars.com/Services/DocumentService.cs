using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.SpaceCalendars.com.Repositories;

namespace Galaxon.Astronomy.SpaceCalendars.com.Services;

public class DocumentService(
    IDocumentRepository documentRepo,
    BufferedFileUploadService fileUploadService)
{
    public const string DocumentIconDirUri = "images/icons";

    public const string DocumentIconDirApp = $"wwwroot/{DocumentIconDirUri}";

    public Document? GetFolder(Document doc)
    {
        if (doc.Folder != null)
        {
            return doc.Folder;
        }
        if (doc.FolderId == null)
        {
            return null;
        }
        doc.Folder = documentRepo.GetById(doc.FolderId.Value);
        return doc.Folder;
    }

    /// <summary>
    /// Convert a string (like a name or title) into a URL part.
    /// Removes non-letters and returns string in lower-kebab-case.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ProcessUrlPart(string name) =>
        Regex.Replace(name, "[^a-zA-Z]+", "-").Trim('-').ToLower();

    public string GetPathAlias(Document doc)
    {
        Document? folder = GetFolder(doc);
        return (folder == null ? "" : GetPathAlias(folder)) + "/" + ProcessUrlPart(doc.Title);
    }

    public string GetBreadcrumb(Document doc)
    {
        Document? folder = GetFolder(doc);
        return (folder == null ? "" : GetBreadcrumb(folder) + " / ") + doc.Title;
    }

    public static string? GetIconPath(Document doc, bool absolute = false)
    {
        string iconBaseName = $"document-icon-{ProcessUrlPart(doc.Title)}";

        string[] files = Directory.GetFiles(DocumentIconDirApp);
        foreach (string path in files)
        {
            FileInfo fi = new (path);
            string fileBaseName = fi.Name[..^fi.Extension.Length];
            if (fileBaseName == iconBaseName)
            {
                return absolute
                    ? Path.GetFullPath($"{DocumentIconDirApp}/{fi.Name}")
                    : $"/{DocumentIconDirUri}/{fi.Name}";
            }
        }

        return null;
    }

    public bool DeleteIcon(int docId)
    {
        Document? doc = documentRepo.GetById(docId);
        if (doc == null)
        {
            return false;
        }

        string? iconPath = GetIconPath(doc, true);
        if (iconPath == null || !File.Exists(iconPath))
        {
            return false;
        }

        File.Delete(iconPath);
        return true;
    }

    public async Task UploadIcon(int docId, IFormFile icon)
    {
        FileInfo fi = new FileInfo(icon.FileName);
        string iconFileName = $"document-icon-{docId}{fi.Extension.ToLower()}";
        await fileUploadService.UploadFile(icon, DocumentIconDirApp, iconFileName);
    }

    public bool FolderContainsCurrentDocument(int folderId, HttpRequest request)
    {
        IEnumerable<Document> docs = documentRepo.GetByFolder(folderId);
        return docs.Any(doc =>
            doc.IsFolder && FolderContainsCurrentDocument(doc.Id, request)
            || !doc.IsFolder && request.Path == GetPathAlias(doc));
    }
}
