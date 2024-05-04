using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public class DocumentRepository(ApplicationDbContext dbContext) : IDocumentRepository
{
    private ApplicationDbContext _DbContext { get; } = dbContext;

    public IEnumerable<Document> GetPublished(bool published = true)
    {
        return _DbContext.Documents
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
        List<Document> docs = _DbContext.Documents
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order).ToList();

        foreach (Document doc in docs)
        {
            order += 2;
            doc.Order = order;
            _DbContext.SaveChanges();

            // Reorder children, if any.
            order = Reorder(doc.Id, order);
        }

        return order;
    }

    #region IRepository methods

    public IEnumerable<Document> GetAll()
    {
        return _DbContext.Documents.OrderBy(doc => doc.Order);
    }

    public Document? GetById(int id)
    {
        return GetAll().FirstOrDefault(doc => doc.Id == id);
    }

    public void Create(Document doc)
    {
        _DbContext.Documents.Add(doc);
        _DbContext.SaveChanges();
    }

    public void Update(Document doc)
    {
        _DbContext.Documents.Update(doc);
        _DbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        Document doc = new () { Id = id };
        _DbContext.Entry(doc).State = EntityState.Deleted;
        _DbContext.SaveChanges();
    }

    #endregion IRepository methods
}
