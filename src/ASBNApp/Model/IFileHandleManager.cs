/// <summary>
/// Interface that aims to help with managing the fileHandle
/// </summary>

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleManager{
    void AssignFileHandle(FileSystemFileHandle fileHandle);
}