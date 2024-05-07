using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.SpaceCalendars.com.Repositories;

public interface IDocumentRepository : IRepository<DocumentRecord>
{
    public DocumentRecord? GetByTitle(string title);

    public IEnumerable<DocumentRecord> GetPublished(bool published = true);

    public IEnumerable<DocumentRecord> GetByFolder(int? folderId);

    public IEnumerable<DocumentRecord> GetPublishedByFolder(int? folderId, bool published = true);

    public IEnumerable<DocumentRecord> GetFolders();

    public int Reorder(int? folderId = null, int order = 0);
}
