using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Galaxon.Astronomy.SpaceCalendars.com.Models;

/// <summary>
/// Represents a document or a folder within the application.
/// </summary>
public class Document
{
    /// <summary>
    /// Unique identifier for the Document.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title of the document. Required and limited to 255 characters.
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Title { get; set; } = "";

    /// <summary>
    /// Indicates whether the document is a folder.
    /// </summary>
    [Display(Name = "This is a folder")]
    public bool IsFolder { get; set; }

    /// <summary>
    /// Indicates whether the document is published.
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// The content of the document. Stored as TEXT in MySQL.
    /// </summary>
    [Column(TypeName = "text")]
    public string? Content { get; set; } = "";

    /// <summary>
    /// Optional ID of the containing folder.
    /// </summary>
    [Display(Name = "Folder")]
    public int? FolderId { get; set; }

    /// <summary>
    /// Navigation property for the folder containing this document.
    /// </summary>
    public Document? Folder { get; set; }

    /// <summary>
    /// If this document is a folder, contains the documents within it.
    /// </summary>
    public List<Document>? Documents { get; set; }

    /// <summary>
    /// The order of the document within its folder.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// An alias for the path, not mapped to the database.
    /// </summary>
    [NotMapped]
    public string? PathAlias { get; set; }

    /// <summary>
    /// Whether the document/folder is expanded in the UI, not mapped to the database.
    /// </summary>
    [NotMapped]
    public bool? Expand { get; set; }

    /// <summary>
    /// Path to an icon representing the document, not mapped to the database.
    /// </summary>
    [NotMapped]
    public string? IconPath { get; set; }

    /// <summary>
    /// The level of the document within the hierarchy, not mapped to the database.
    /// </summary>
    [NotMapped]
    public int Level { get; set; }
}
