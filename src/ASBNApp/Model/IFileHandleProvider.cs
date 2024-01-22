// Created 2023-12-12
// Interface that allows us to get hold of the available FileSystem handles

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleProvider{
    // TODO: This used to be an IReadOnlyCollection, is there an equivalent for an object?
    FileSystemFileHandle GetFileHandle();
}