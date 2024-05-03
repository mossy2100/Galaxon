using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Galaxon.Astronomy.SpaceCalendars.com.Models;

public class Document
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    public string Title { get; set; } = "";

    [Display(Name = "This is a folder")]
    public bool IsFolder { get; set; }

    public bool Published { get; set; }

    public string? Content { get; set; } = "";

    // Link to containing folder.
    [Display(Name = "Folder")]
    public int? FolderId { get; set; }

    // Navigation property. The folder containing this document.
    public Document? Folder { get; set; }

    // Navigation property. If this is a folder, the documents it contains.
    public List<Document>? Documents { get; set; }

    public int Order { get; set; }

    [NotMapped]
    public string? PathAlias { get; set; }

    [NotMapped]
    public bool? Expand { get; set; }

    [NotMapped]
    public string? IconPath { get; set; }

    [NotMapped]
    public int Level { get; set; }
}
