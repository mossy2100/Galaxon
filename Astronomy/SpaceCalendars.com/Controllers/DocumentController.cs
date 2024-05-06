using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Galaxon.Astronomy.SpaceCalendars.com.Repositories;
using Galaxon.Astronomy.SpaceCalendars.com.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Galaxon.Astronomy.SpaceCalendars.com.Controllers;

public class DocumentController(
    IDocumentRepository documentRepo,
    DocumentService documentService,
    MessageBoxService messageBoxService)
    : Controller
{
    public ViewResult Index()
    {
        ViewBag.PageTitle = "Document Index";

        // Remember the levels to save time.
        Dictionary<int, int> levels = new ();

        List<Document> docs = documentRepo.GetAll().ToList();
        foreach (Document doc in docs)
        {
            doc.PathAlias = documentService.GetPathAlias(doc);
            doc.IconPath = DocumentService.GetIconPath(doc);

            // Get the level.
            doc.Level = doc.FolderId == null ? 0 : levels[doc.FolderId.Value] + 1;
            levels[doc.Id] = doc.Level;
        }

        return View(docs);
    }

    public IActionResult Details(int id)
    {
        Document? doc = documentRepo.GetById(id);

        if (doc == null)
        {
            return NotFound();
        }

        doc.IconPath = DocumentService.GetIconPath(doc);

        ViewBag.PageTitle = "Document Details";
        return View(doc);
    }

    private ViewResult ViewEditForm(Document doc)
    {
        ViewBag.PageTitle = doc.Id == 0 ? "Create Document" : "Update Document";
        ViewBag.Folders = new SelectList(documentRepo.GetFolders(), "Id", "Name");

        // Get "breadcrumbs" (titles with hierarchy) for folders.
        IEnumerable<Document> folders = documentRepo.GetFolders();
        ViewBag.Folders = new List<SelectListItem>();
        foreach (Document folder in folders)
        {
            SelectListItem option =
                new (documentService.GetBreadcrumb(folder), folder.Id.ToString());
            ViewBag.Folders.Add(option);
        }

        return View("Edit", doc);
    }

    public ViewResult Edit(int? id)
    {
        Document doc = (id != null ? documentRepo.GetById(id.Value) : null) ?? new Document();

        if (doc.Id != 0)
        {
            ViewBag.PageTitle = "Update Document";
            // Get the document icon if there is one.
            doc.IconPath = DocumentService.GetIconPath(doc);
        }
        else
        {
            ViewBag.PageTitle = "Create Document";
        }

        return ViewEditForm(doc);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Document doc, string? iconAction, IFormFile? icon)
    {
        ViewBag.ModelStateIsValid = ModelState.IsValid;
        if (!ModelState.IsValid)
        {
            return ViewEditForm(doc);
        }

        // If an icon file is provided, check it's valid.
        if (icon != null)
        {
            FileInfo fi = new (icon.FileName);
            string extension = fi.Extension.ToLower();
            if (extension != ".svg" && extension != ".png")
            {
                messageBoxService.AddMessage(TempData, EMessageSeverity.Danger,
                    "Only SVG or PNG files are valid for document icons.");
                return ViewEditForm(doc);
            }
        }

        // Create or update the document in the repository.
        if (doc.Id == 0)
        {
            documentRepo.Create(doc);
            messageBoxService.AddMessage(TempData, EMessageSeverity.Success, "Document created.");
        }
        else
        {
            documentRepo.Update(doc);
            messageBoxService.AddMessage(TempData, EMessageSeverity.Success, "Document updated.");
        }

        // Delete existing icon file if requested.
        if (iconAction == "delete" || (iconAction == "update" && icon != null))
        {
            try
            {
                bool iconDeleted = documentService.DeleteIcon(doc.Id);
                if (iconDeleted)
                {
                    messageBoxService.AddMessage(TempData, EMessageSeverity.Success,
                        "Icon deleted.");
                }
            }
            catch (Exception)
            {
                messageBoxService.AddMessage(TempData, EMessageSeverity.Danger,
                    "Error deleting icon file.");
                return ViewEditForm(doc);
            }
        }

        // Upload new icon file if requested and provided.
        if (iconAction is null or "update" && icon != null)
        {
            try
            {
                await documentService.UploadIcon(doc.Id, icon);
                messageBoxService.AddMessage(TempData, EMessageSeverity.Success, "Icon uploaded.");
            }
            catch (Exception)
            {
                messageBoxService.AddMessage(TempData, EMessageSeverity.Danger,
                    "Error uploading icon file.");
                return ViewEditForm(doc);
            }
        }

        // Reorder the documents.
        documentRepo.Reorder();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        Document? doc = documentRepo.GetById(id);
        if (doc == null)
        {
            return NotFound();
        }

        ViewBag.PageTitle = "Delete Document";
        return View(doc);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        // Try to delete the icon.
        try
        {
            documentService.DeleteIcon(id);
            messageBoxService.AddMessage(TempData, EMessageSeverity.Success, "Icon deleted.");
        }
        catch (Exception)
        {
            messageBoxService.AddMessage(TempData, EMessageSeverity.Danger,
                "Error deleting icon file.");
            return Delete(id);
        }

        // Delete the document from the repository.
        documentRepo.Delete(id);
        messageBoxService.AddMessage(TempData, EMessageSeverity.Success, "Document deleted.");

        documentRepo.Reorder();

        return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public IActionResult Display(int id)
    {
        Document? doc = documentRepo.GetById(id);
        if (doc == null)
        {
            return NotFound();
        }

        ViewBag.PageTitle = doc.Title;
        return View(doc);
    }

    [AllowAnonymous]
    public IActionResult DisplayFromPathAlias(string alias)
    {
        Document? doc = documentRepo
            .GetAll()
            .FirstOrDefault(doc => documentService.GetPathAlias(doc) == $"/{alias}");

        if (doc == null)
        {
            return NotFound();
        }

        ViewBag.PageTitle = doc.Title;
        return View("Display", doc);
    }
}
