using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Galaxon.Astronomy.SpaceCalendars.com.Repositories;
using Galaxon.Astronomy.SpaceCalendars.com.Services;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.SpaceCalendars.com.ViewComponents;

public class MenuViewComponent : ViewComponent
{
    private IDocumentRepository _documentRepo { get; }
    private DocumentService _documentService { get; }

    public MenuViewComponent(IDocumentRepository documentRepo, DocumentService documentService)
    {
        _documentRepo = documentRepo;
        _documentService = documentService;
    }

    public IViewComponentResult Invoke(int? folderId = null, int level = 0)
    {
        ViewBag.FolderId = folderId ?? 0;
        ViewBag.Level = level;

        // Get the documents in this folder.
        // If folderId is null, these will be the top-level items.
        List<Document> docs = _documentRepo.GetPublishedByFolder(folderId).ToList();

        foreach (Document doc in docs)
        {
            // Get the path alias for each document.
            doc.PathAlias = _documentService.GetPathAlias(doc);

            // Get the id for the current document.
            if (Request.Path == doc.PathAlias)
            {
                ViewBag.CurrentDocumentId = doc.Id;
            }

            // For folders, Determine if the folder should be collapsed or expanded.
            if (doc.IsFolder)
            {
                doc.Expand = _documentService.FolderContainsCurrentDocument(doc.Id, Request);
            }

            // Get the path to the icon if there is one.
            doc.IconPath = DocumentService.GetIconPath(doc);
        }

        return View(docs);
    }
}
