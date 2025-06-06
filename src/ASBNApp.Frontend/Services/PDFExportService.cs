using ASBNApp.Enums;
using ASBNApp.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.AcroForms;

namespace ASBNApp.Frontend.Services;

/// <summary>
/// PDF handler, allows to generate pdfs from user data.
/// </summary>
public class PDFExportService
{

    public PDFExportService(DateHandler dateHandler)
    {
        this.dateHandler = dateHandler;
    }

    private readonly DateHandler dateHandler;


    /// <summary>
    /// Handles opening then pdf, linking data and finalizing the pdf.
    /// </summary>
    /// <param name="src">Bytestream making the template PDF available</param>
    /// <param name="dest">Stream handling the edited PDF</param>
    /// <param name="rows">EntryRow with the data to export</param>
    /// <param name="week">Int holding the week number to export data for</param>
    /// <param name="year">Int holding the year number to export data for</param>
    /// <returns>Task</returns>
    public async Task GeneratePDF(byte[] src, Stream dest, IEnumerable<Entry> rows, IEnumerable<WorkLocation> locations, Settings settings, int? week, int? year)
    {
        try
        {
            using(var srcstream = new MemoryStream(src))
            {
                // Open PDF document from memory stream
                var document = PdfReader.Open(new MemoryStream(src));

                // Write data for the individual days
                WriteData(ASBNPdfFields.Date1, ASBNPdfFields.Note1, ASBNPdfFields.Hours1, ASBNPdfFields.Location1, rows.ElementAt(0), ResolveWorkLocation(rows.ElementAt(0), locations), document);
                WriteData(ASBNPdfFields.Date2, ASBNPdfFields.Note2, ASBNPdfFields.Hours2, ASBNPdfFields.Location2, rows.ElementAt(1), ResolveWorkLocation(rows.ElementAt(1), locations), document);
                WriteData(ASBNPdfFields.Date3, ASBNPdfFields.Note3, ASBNPdfFields.Hours3, ASBNPdfFields.Location3, rows.ElementAt(2), ResolveWorkLocation(rows.ElementAt(2), locations), document);
                WriteData(ASBNPdfFields.Date4, ASBNPdfFields.Note4, ASBNPdfFields.Hours4, ASBNPdfFields.Location4, rows.ElementAt(3), ResolveWorkLocation(rows.ElementAt(3), locations), document);
                WriteData(ASBNPdfFields.Date5, ASBNPdfFields.Note5, ASBNPdfFields.Hours5, ASBNPdfFields.Location5, rows.ElementAt(4), ResolveWorkLocation(rows.ElementAt(4), locations), document);

                // Additional data (header, footer, etc.)
                WriteAdditionalData(document, settings, week, year);


                // Sets a value that prevents text being hidden behind form fields
                // (Would only get visible after clicking into that field)
                if (document.AcroForm.Elements.ContainsKey("/NeedAppearances")) {
                    document.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);
                } else {
                    document.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);
                }

                // Save the edited document to a MemoryStream
                // Hacky workaround because document.Save(dest) does not support async saving
                // -> we need asnyc, because the underlying JS streams are only implemented in an asynchronos way
                using (var memoryStream = new MemoryStream())
                {
                    document.Save(memoryStream);
                    await memoryStream.FlushAsync();
                    memoryStream.Position = 0;

                    // Copy the content of the MemoryStream to the destination stream asynchronously
                    await memoryStream.CopyToAsync(dest);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception thrown while trying to generate the PDF. " + e);
            throw;
        }
    }


    /// <summary>
    /// Links user content to the matching fields in the pdf template, 
    /// then calls FillField to actually handle writing.
    /// </summary>
    /// <param name="fieldDate">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldNote">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldHours">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldLocation">Int that links the enum to the field in the pdf</param>
    /// <param name="row"><see cref="Entry"/> to write to the pdf</param>
    /// <param name="location"><see cref="WorkLocation"/> to write to the pdf</param>
    /// <param name="document">PdfDocument to edit</param>
    private void WriteData(ASBNPdfFields fieldDate, ASBNPdfFields fieldNote, ASBNPdfFields fieldHours, ASBNPdfFields fieldLocation, Entry row, WorkLocation? location, PdfDocument document)
    {
        FillField(document, fieldDate, row.Date.ToString("dd.MM.yyyy"));
        FillField(document, fieldNote, row.Note);
        FillField(document, fieldHours, row.Hours.ToString());
        FillField(document, fieldLocation, location?.LocationName);
    }

    /// <summary>
    /// Prepare data that we can't write with WriteData(). Then calls FillField to 
    /// actually write data.
    /// </summary>
    /// <param name="document">PdfDocument to edit</param>
    /// <param name="settings">Settings object to get data from</param>
    /// <param name="week">Int for the week of year to export from</param>
    /// <param name="year">Int for the year to export from</param>
    private void WriteAdditionalData(PdfDocument document, Settings settings, int? week, int? year)
    {
        // Add information to the header
        FillField(document, ASBNPdfFields.Username, $"{settings.GivenName} {settings.Surname}");
        FillField(document, ASBNPdfFields.HeaderProfession, settings.Profession);
        FillField(document, ASBNPdfFields.HeaderApprenticeYear, settings.ApprenticeshipStartDate != null ? dateHandler.CalculateApprenticeshipYear(settings.ApprenticeshipStartDate).ToString() : "");
        FillField(document, ASBNPdfFields.HeaderTimeperiod, dateHandler.GetFirstDateOfWeek((int)week, (int)year).ToString("dd.MM.") + " - " + dateHandler.GetLastDateOfWeek((int)week, (int)year).ToString("dd.MM.yyyy"));
        FillField(document, ASBNPdfFields.HeaderCalendarWeek, week.ToString());

        // Add data for the bottom row
        FillField(document, ASBNPdfFields.FooterUserSignatureDate, DateTime.Today.ToString("dd.MM.yyyy"));
        FillField(document, ASBNPdfFields.FooterRepresentativeName, settings.LegalRepresentitive);
        FillField(document, ASBNPdfFields.FooterCompanyName, settings.Company);
        FillField(document, ASBNPdfFields.FooterSchoolName, settings.School);
    }

    /// <summary>
    /// Handles the actual writing of content to their respective fields in the PDF.
    /// </summary>
    /// <param name="document">PdfDocument to edit</param>
    /// <param name="field">PdfTextField to edit</param>
    /// <param name="content">String with the content to write</param>
    private void FillField(PdfDocument document, ASBNPdfFields field, string content)
    {
        PdfTextField pdfField = (PdfTextField)document.AcroForm.Fields[((int)field)];
        pdfField.Value = new PdfString(content);
    }

    /// <summary>
    /// Enable easy handling when an entry doesn't specify a <see cref="Entry.LocationId"/>.
    /// </summary>
    /// <param name="entry"><see cref="Entry"/> to find the location for.</param>
    /// <param name="locations">A list of all available <see cref="WorkLocation"/> for the user.</param>
    /// <returns>The <see cref="WorkLocation"/> or null if no <see cref="Entry.LocationId"/> is present on the entry.</returns>
    private WorkLocation? ResolveWorkLocation(Entry entry, IEnumerable<WorkLocation> locations)
    {
        try { return locations.Single(l => l.Id == entry.LocationId); }
        catch { return null; }
    }
}