namespace FileFlower.Core;

/// <summary>
/// Represents a context for a file, providing basic metadata.
/// </summary>
public sealed class FileContext(FileInfo fileInfo, FileModificationType operation)
{
    /// <summary>
    /// Gets a value indicating whether the file exists and is marked as read-only.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if the file represented by <see cref="FileInfo"/> both exists and is set to read-only; otherwise, <c>false</c>.
    /// </remarks>
    public bool ExistsAsReadonly => FileInfo is not null && FileInfo.Exists && FileInfo.IsReadOnly;

    /// <summary>
    /// Gets or sets the <see cref="FileInfo"/> object that represents the file associated with this context.
    /// </summary>
    public FileInfo FileInfo { get; init; } = fileInfo;

    /// <summary>
    /// Gets or sets the specification of the file modification type, e.g. Created, Deleted, etc.
    /// </summary>
    public FileModificationType FileModificationType { get; init; } = operation;
}
