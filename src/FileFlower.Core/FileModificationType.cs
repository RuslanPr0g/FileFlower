namespace FileFlower.Core;

/// <summary>
/// Specifies the file modification type, e.g. Created, Deleted, etc.
/// </summary>
public enum FileModificationType : byte
{
    /// <summary>Operation is not specified.</summary>
    NotSpecified = 0,

    /// <summary>File was created.</summary>
    Created = 1,

    /// <summary>File was changed.</summary>
    Changed = 2,

    /// <summary>File was deleted.</summary>
    Deleted = 3,

    /// <summary>File was renamed.</summary>
    Renamed = 4,
}
