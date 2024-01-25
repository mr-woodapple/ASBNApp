using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.AcroForms;
using ASBNApp.Enums;


/// <summary>
/// TODO: tba
/// </summary>
/// 
public class PDFExportHandler
{
    /// <summary>
    /// Handles opening then pdf, linking data and finalizing the pdf
    /// </summary>
    /// <param name="src">Bytestream making the template PDF available</param>
    /// <param name="dest">Stream handling the edited PDF</param>
    /// <param name="rows">EntryRowModel with the data to export</param>
    /// <returns></returns>
    public async Task GeneratePDF(byte[] src, Stream dest, IEnumerable<EntryRowModel> rows)
    {
        try
        {
            using(var srcstream = new MemoryStream(src)){

                // Open PDF document from memory stream
                var document = PdfReader.Open(new MemoryStream(src));

                // Write data for the individual days
                WriteData(ASBNPdfFields.Date1, ASBNPdfFields.Note1, ASBNPdfFields.Hours1, ASBNPdfFields.Location1, rows.ElementAt(0), document);
                WriteData(ASBNPdfFields.Date2, ASBNPdfFields.Note2, ASBNPdfFields.Hours2, ASBNPdfFields.Location2, rows.ElementAt(1), document);
                WriteData(ASBNPdfFields.Date3, ASBNPdfFields.Note3, ASBNPdfFields.Hours3, ASBNPdfFields.Location3, rows.ElementAt(2), document);
                WriteData(ASBNPdfFields.Date4, ASBNPdfFields.Note4, ASBNPdfFields.Hours4, ASBNPdfFields.Location4, rows.ElementAt(3), document);
                WriteData(ASBNPdfFields.Date5, ASBNPdfFields.Note5, ASBNPdfFields.Hours5, ASBNPdfFields.Location5, rows.ElementAt(4), document);


                // TODO: Add data for the top row

                // TODO: Add data for the bottom row


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
        }
    }

    /// <summary>
    /// Links user content to the matching fields in the pdf template, 
    /// then calls FillField to actually handle writing
    /// </summary>
    /// <param name="fieldDate">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldNote">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldHours">Int that links the enum to the field in the pdf</param>
    /// <param name="fieldLocation">Int that links the enum to the field in the pdf</param>
    /// <param name="row">EntryModelRow to get data from</param>
    /// <param name="document">PdfDocument to edit</param>
    private void WriteData(ASBNPdfFields fieldDate, ASBNPdfFields fieldNote, ASBNPdfFields fieldHours, ASBNPdfFields fieldLocation, EntryRowModel row, PdfDocument document)
    {
        FillField(document, fieldDate, row.Date.ToShortDateString());
        FillField(document, fieldNote, row.Note);
        FillField(document, fieldHours, row.Hours.ToString());
        FillField(document, fieldLocation, row.Location);
    }

    /// <summary>
    /// Handles the actual writing of content to their respective fields in the PDF
    /// </summary>
    /// <param name="document">PdfDocument to edit</param>
    /// <param name="field">PdfTextField to edit</param>
    /// <param name="content">String with the content to write</param>
    private void FillField(PdfDocument document, ASBNPdfFields field, string content)
    {
        PdfTextField pdfField = (PdfTextField)document.AcroForm.Fields[((int)field)];
        pdfField.Value = new PdfString(content);
    }

}