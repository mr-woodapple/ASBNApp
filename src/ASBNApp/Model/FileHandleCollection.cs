// Created 2023-12-12
// Micro service to manage fileHandles for the local files we're editing

using KristofferStrube.Blazor.FileSystem;

public class FileHandleCollection : IFileHandleManager, IFileHandleProvider, IAsyncDisposable{
    private List<FileSystemFileHandle> fileHandles = new List<FileSystemFileHandle>();

    /// <summary>
    /// Adds or replaces the fileHandle available, because we can only have one at a time
    /// </summary>
    /// <param name="fileHandle">fileHandle from the loaded file</param>
    public void AddReplaceFileHandle(FileSystemFileHandle fileHandle){
        // if fileHandles == null, call Clear() on the fileHandles list
        fileHandles?.Clear();
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
