/// <summary>
/// Interface that allows us to get hold of the available FileSystem handle
/// </summary>

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleProvider{
    FileSystemFileHandle GetFileHandle();
}