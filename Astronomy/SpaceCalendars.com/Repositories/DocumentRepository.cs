using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private ApplicationDbContext _dbContext { get; }

    public DocumentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region IRepository methods

    public IEnumerable<Document> GetAll() =>
        _dbContext.Documents
            .OrderBy(doc => doc.Order);

    public Document? GetById(int id) =>
        GetAll().FirstOrDefault(doc => doc.Id == id);

    public void Create(Document doc)
    {
        _dbContext.Documents.Add(doc);
        _dbContext.SaveChanges();
    }

    public void Update(Document doc)
    {
        _dbContext.Documents.Update(doc);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        Document doc = new() { Id = id };
        _dbContext.Entry(doc).State = EntityState.Deleted;
        _dbContext.SaveChanges();
    }

    #endregion IRepository methods

    public IEnumerable<Document> GetPublished(bool published = true) =>
        _dbContext.Documents
            .Where(doc => doc.Published == published)
            .OrderBy(doc => doc.Order);

    public IEnumerable<Document> GetByFolder(int? folderId) =>
        GetAll()
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order);

    public IEnumerable<Document> GetPublishedByFolder(int? folderId, bool published = true) =>
        GetAll()
            .Where(doc => doc.FolderId == folderId && doc.Published == published)
            .OrderBy(doc => doc.Order);

    public IEnumerable<Document> GetFolders() =>
        GetAll()
            .Where(doc => doc.IsFolder)
            .OrderBy(doc => doc.Order);

    public int Reorder(int? folderId = null, int order = 0)
    {
        List<Document> docs = _dbContext.Documents
            .Where(doc => doc.FolderId == folderId)
            .OrderBy(doc => doc.Order).ToList();

        foreach (Document doc in docs)
        {
            order += 2;
            doc.Order = order;
            _dbContext.SaveChanges();

            // Reorder children, if any.
            order = Reorder(doc.Id, order);
        }

        return order;
    }
}
