// Created 2023-12-12
// Micro service to manage fileHandles for the local files we're editing

using KristofferStrube.Blazor.FileSystem;

public class FileHandleCollection : IFileHandleManager, IFileHandleProvider, IAsyncDisposable{
    private List<FileSystemFileHandle> fileHandles = new List<FileSystemFileHandle>();

    // TODO: Check if:
    // Might wanna change that to AddReplaceFileHandle, because we can only have one at a time??
    // Otherwise add a check to ensure a file isn't in the list already?
    public void AddFileHandle(FileSystemFileHandle fileHandle){
        fileHandles.Add(fileHandle);
    }

    public IReadOnlyCollection<FileSystemFileHandle> GetFileHandles(){
        if(fileHandles.Count == 0){
            throw new NotSupportedException("Please load first a file.");
        }
        return fileHandles.AsReadOnly();
    }

    // Called by the system when it's time to dispose (for example when shutting down the app)
    public async ValueTask DisposeAsync()
    {
        foreach(var fileHanlde in fileHandles){
            await fileHanlde.DisposeAsync();
        }
    }
}
