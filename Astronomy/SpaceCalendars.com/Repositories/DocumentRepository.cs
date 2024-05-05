using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public class DocumentRepository(ApplicationDbContext dbContext) : IDocumentRepository
{
    public Document? GetByTitle(string title)
    {
        return dbContext.Documents.FirstOrDefault(doc => doc.Title == title);
    }

    public IEnumerable<Document> GetPublished(bool published = true)
    {
        return dbContext.Documents
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
        List<Document> docs = dbContext.Documents
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order).ToList();

        foreach (Document doc in docs)
        {
            order += 2;
            doc.Order = order;
            dbContext.SaveChanges();

            // Reorder children, if any.
            order = Reorder(doc.Id, order);
        }

        return order;
    }

    #region IRepository methods

    public IEnumerable<Document> GetAll()
    {
        return dbContext.Documents.OrderBy(doc => doc.Order);
    }

    public Document? GetById(int id)
    {
        return dbContext.Documents.FirstOrDefault(doc => doc.Id == id);
    }

    public void Create(Document doc)
    {
        dbContext.Documents.Add(doc);
        dbContext.SaveChanges();
    }

    public void Update(Document doc)
    {
        dbContext.Documents.Update(doc);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        Document doc = new () { Id = id };
        dbContext.Entry(doc).State = EntityState.Deleted;
        dbContext.SaveChanges();
    }

    #endregion IRepository methods
}
