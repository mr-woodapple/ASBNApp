/// <summary>
/// Interface that allows us to get hold of the available FileSystem handle
/// </summary>

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleProvider{
    // TODO: This used to be an IReadOnlyCollection, is there an equivalent for an object?
    FileSystemFileHandle GetFileHandle();
}