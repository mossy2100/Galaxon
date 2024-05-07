using Galaxon.Astronomy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public class DocumentRepository(ApplicationDbContext astroDbContext) : IDocumentRepository
{
    public DocumentRecord? GetByTitle(string title)
    {
        return astroDbContext.Documents.FirstOrDefault(doc => doc.Title == title);
    }

    public IEnumerable<DocumentRecord> GetPublished(bool published = true)
    {
        return astroDbContext.Documents
            .Where(doc => doc.Published == published)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<DocumentRecord> GetByFolder(int? folderId)
    {
        return GetAll()
            .Where(doc => doc.ParentId == folderId)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<DocumentRecord> GetPublishedByFolder(int? folderId, bool published = true)
    {
        return GetAll()
            .Where(doc => doc.ParentId == folderId && doc.Published == published)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<DocumentRecord> GetFolders()
    {
        return GetAll()
            .Where(doc => doc.IsFolder)
            .OrderBy(doc => doc.Order);
    }

    public int Reorder(int? folderId = null, int order = 0)
    {
        List<DocumentRecord> docs = astroDbContext.Documents
            .Where(doc => doc.ParentId == folderId)
            .OrderBy(doc => doc.Order).ToList();

        foreach (DocumentRecord doc in docs)
        {
            order += 2;
            doc.Order = order;
            astroDbContext.SaveChanges();

            // Reorder children, if any.
            order = Reorder(doc.Id, order);
        }

        return order;
    }

    #region IRepository methods

    public IEnumerable<DocumentRecord> GetAll()
    {
        return astroDbContext.Documents.OrderBy(doc => doc.Order);
    }

    public DocumentRecord? GetById(int id)
    {
        return astroDbContext.Documents.FirstOrDefault(doc => doc.Id == id);
    }

    public void Create(DocumentRecord doc)
    {
        astroDbContext.Documents.Add(doc);
        astroDbContext.SaveChanges();
    }

    public void Update(DocumentRecord doc)
    {
        astroDbContext.Documents.Update(doc);
        astroDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        DocumentRecord doc = new () { Id = id };
        astroDbContext.Entry(doc).State = EntityState.Deleted;
        astroDbContext.SaveChanges();
    }

    #endregion IRepository methods
}
