// Created 2023-12-12
// Interface that allows us to get hold of the available FileSystem handles

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleProvider{
    IReadOnlyCollection<FileSystemFileHandle> GetFileHandles();
}