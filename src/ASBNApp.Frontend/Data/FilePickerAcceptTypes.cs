using KristofferStrube.Blazor.FileSystemAccess;

namespace ASBNApp.Data
{
	/// <summary>
	/// Pre-defines allowed file types for file pickers throughout the app.
	/// </summary>
	public static class FilePickerAcceptTypes
	{
		public static FilePickerAcceptType[] fileTypePDF = new FilePickerAcceptType[]
		{
			new FilePickerAcceptType()
			{
				Description = ".pdf",
			Accept = new() { { "document/*", new string[] { ".pdf" } } }
			}
		};

		public static FilePickerAcceptType[] fileTypeJSON = new FilePickerAcceptType[]
		{
			new FilePickerAcceptType()
			{
				Description = ".json",
				Accept = new() { { "document/*", new string[] { ".json" } } }
			}
		};
	}
}
