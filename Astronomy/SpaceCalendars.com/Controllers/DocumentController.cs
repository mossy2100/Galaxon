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
    private IDocumentRepository _DocumentRepo { get; } = documentRepo;

    private DocumentService _DocumentService { get; } = documentService;

    private MessageBoxService _MessageBoxService { get; } = messageBoxService;

    public ViewResult Index()
    {
        ViewBag.PageTitle = "Document Index";

        // Remember the levels to save time.
        Dictionary<int, int> levels = new ();

        List<Document> docs = _DocumentRepo.GetAll().ToList();
        foreach (Document doc in docs)
        {
            doc.PathAlias = _DocumentService.GetPathAlias(doc);
            doc.IconPath = DocumentService.GetIconPath(doc.Id);

            // Get the level.
            doc.Level = doc.FolderId == null ? 0 : levels[doc.FolderId.Value] + 1;
            levels[doc.Id] = doc.Level;
        }

        return View(docs);
    }

    public IActionResult Details(int id)
    {
        Document? doc = _DocumentRepo.GetById(id);

        if (doc == null)
        {
            return NotFound();
        }

        doc.IconPath = DocumentService.GetIconPath(id);

        ViewBag.PageTitle = "Document Details";
        return View(doc);
    }

    private ViewResult ViewEditForm(Document doc)
    {
        ViewBag.PageTitle = doc.Id == 0 ? "Create Document" : "Update Document";
        ViewBag.Folders = new SelectList(_DocumentRepo.GetFolders(), "Id", "Name");

        // Get "breadcrumbs" (titles with hierarchy) for folders.
        IEnumerable<Document> folders = _DocumentRepo.GetFolders();
        ViewBag.Folders = new List<SelectListItem>();
        foreach (Document folder in folders)
        {
            SelectListItem option =
                new (_DocumentService.GetBreadcrumb(folder), folder.Id.ToString());
            ViewBag.Folders.Add(option);
        }

        return View("Edit", doc);
    }

    public ViewResult Edit(int? id)
    {
        Document doc = (id != null ? _DocumentRepo.GetById(id.Value) : null) ?? new Document();

        if (doc.Id != 0)
        {
            ViewBag.PageTitle = "Update Document";
            // Get the document icon if there is one.
            doc.IconPath = DocumentService.GetIconPath(doc.Id);
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
                MessageBoxService.AddMessage(TempData, "danger",
                    "Only SVG or PNG files are valid for document icons.");
                return ViewEditForm(doc);
            }
        }

        // Create or update the document in the repository.
        if (doc.Id == 0)
        {
            _DocumentRepo.Create(doc);
            MessageBoxService.AddMessage(TempData, "success", "Document created.");
        }
        else
        {
            _DocumentRepo.Update(doc);
            MessageBoxService.AddMessage(TempData, "success", "Document updated.");
        }

        // Delete existing icon file if requested.
        if (iconAction == "delete" || (iconAction == "update" && icon != null))
        {
            try
            {
                bool iconDeleted = _DocumentService.DeleteIcon(doc.Id);
                if (iconDeleted)
                {
                    MessageBoxService.AddMessage(TempData, "success", "Icon deleted.");
                }
            }
            catch (Exception)
            {
                MessageBoxService.AddMessage(TempData, "danger", "Error deleting icon file.");
                return ViewEditForm(doc);
            }
        }

        // Upload new icon file if requested and provided.
        if (iconAction is null or "update" && icon != null)
        {
            try
            {
                await _DocumentService.UploadIcon(doc.Id, icon);
                MessageBoxService.AddMessage(TempData, "success", "Icon uploaded.");
            }
            catch (Exception)
            {
                MessageBoxService.AddMessage(TempData, "danger", "Error uploading icon file.");
                return ViewEditForm(doc);
            }
        }

        // Reorder the documents.
        _DocumentRepo.Reorder();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        Document? doc = _DocumentRepo.GetById(id);
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
            _DocumentService.DeleteIcon(id);
            MessageBoxService.AddMessage(TempData, "success", "Icon deleted.");
        }
        catch (Exception)
        {
            MessageBoxService.AddMessage(TempData, "danger", "Error deleting icon file.");
            return Delete(id);
        }

        // Delete the document from the repository.
        _DocumentRepo.Delete(id);
        MessageBoxService.AddMessage(TempData, "success", "Document deleted.");

        _DocumentRepo.Reorder();

        return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public IActionResult Display(int id)
    {
        Document? doc = _DocumentRepo.GetById(id);
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
        Document? doc = _DocumentRepo
            .GetAll()
            .FirstOrDefault(doc => _DocumentService.GetPathAlias(doc) == $"/{alias}");

        if (doc == null)
        {
            return NotFound();
        }

        ViewBag.PageTitle = doc.Title;
        return View("Display", doc);
    }
}
