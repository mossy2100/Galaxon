using Galaxon.Astronomy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public class DocumentRepository(ApplicationDbContext astroDbContext) : IDocumentRepository
{
    public Document? GetByTitle(string title)
    {
        return astroDbContext.Documents.FirstOrDefault(doc => doc.Title == title);
    }

    public IEnumerable<Document> GetPublished(bool published = true)
    {
        return astroDbContext.Documents
            .Where(doc => doc.Published == published)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<Document> GetByFolder(int? folderId)
    {
        return GetAll()
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<Document> GetPublishedByFolder(int? folderId, bool published = true)
    {
        return GetAll()
            .Where(doc => doc.FolderId == folderId && doc.Published == published)
            .OrderBy(doc => doc.Order);
    }

    public IEnumerable<Document> GetFolders()
    {
        return GetAll()
            .Where(doc => doc.IsFolder)
            .OrderBy(doc => doc.Order);
    }

    public int Reorder(int? folderId = null, int order = 0)
    {
        List<Document> docs = astroDbContext.Documents
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order).ToList();

        foreach (Document doc in docs)
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

    public IEnumerable<Document> GetAll()
    {
        return astroDbContext.Documents.OrderBy(doc => doc.Order);
    }

    public Document? GetById(int id)
    {
        return astroDbContext.Documents.FirstOrDefault(doc => doc.Id == id);
    }

    public void Create(Document doc)
    {
        astroDbContext.Documents.Add(doc);
        astroDbContext.SaveChanges();
    }

    public void Update(Document doc)
    {
        astroDbContext.Documents.Update(doc);
        astroDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        Document doc = new () { Id = id };
        astroDbContext.Entry(doc).State = EntityState.Deleted;
        astroDbContext.SaveChanges();
    }

    #endregion IRepository methods
}
