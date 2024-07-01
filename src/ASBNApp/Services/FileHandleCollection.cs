/// <summary>
/// Micro service to manage fileHandles for the local files we're editing
/// </summary>


using KristofferStrube.Blazor.FileSystem;

public class FileHandleCollection : IFileHandleManager, IFileHandleProvider, IAsyncDisposable
{
    private FileSystemFileHandle? fileHandle = null;

    /// <summary>
    /// Assigns the handle variable to our fileHandle variable
    /// </summary>
    /// <param name="handle">fileHandle from the loaded file</param>
    public async Task AssignFileHandle(FileSystemFileHandle handle)
    {
        if (fileHandle != null)
        {
            await fileHandle.DisposeAsync();
        }
        fileHandle = handle;
        
        // TODO: save to local storage
    }

    public FileSystemFileHandle GetFileHandle()
    {
        // TODO: Check if fileHandle is available in local storage, if so assign it

        if (fileHandle == null)
        {
            throw new NullReferenceException("No fileHandle available, please load a file first.");
        }
        return fileHandle;
    }

    // Called by the system when it's time to dispose (for example when shutting down the app)
    public async ValueTask DisposeAsync()
    {
        await fileHandle.DisposeAsync();
    }
}
