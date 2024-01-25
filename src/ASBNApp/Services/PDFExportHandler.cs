using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.AcroForms;


/// <summary>
/// TODO: tba
/// </summary>
/// 
public class PDFExportHandler
{

    public async Task GeneratePDF(byte[] src, Stream dest)
    {
        try
        {
            using(var srcstream = new MemoryStream(src)){

                // Load PDF document
                var document = PdfReader.Open(new MemoryStream(src));

                // Get fields from the pdf
                var fieldNames = document.AcroForm.Fields.Names;
                PdfTextField field = (PdfTextField)document.AcroForm.Fields[11];
                field.Value = new PdfString("ajglkdajgkl");

                field = (PdfTextField)document.AcroForm.Fields[12];
                field.Value = new PdfString("ajglkdajgkl");

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
}