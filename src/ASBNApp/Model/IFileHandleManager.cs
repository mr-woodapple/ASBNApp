// Created 2023-12-12
// Interface that should help manage the available FileSystem handles 

using KristofferStrube.Blazor.FileSystem;

public interface IFileHandleManager{
    void AddFileHandle(FileSystemFileHandle fileHandle);
}