using ClosedXML.Excel;

namespace Galaxon.Astronomy.DataImport.Services;

public static class ExcelReader
{
    public static List<List<string>> ReadXlsxFile(string xlsxFilePath)
    {
        // Prepare the result data structure.
        List<List<string>> data = new ();

        // Load the workbook.
        using XLWorkbook workbook = new (xlsxFilePath);

        // Get the first worksheet.
        IXLWorksheet? worksheet = workbook.Worksheet(1);

        // Get the rows.
        IXLRows? rows = worksheet.RowsUsed();

        // Read the data in each row.
        foreach (IXLRow? row in rows)
        {
            // Prepare the data structure to contain the row data.
            List<string> rowData = new ();

            // Read the data in each cell in the row.
            foreach (IXLCell? cell in row.Cells())
            {
                rowData.Add(cell.GetValue<string>());
            }

            data.Add(rowData);
        }

        return data;
    }
}
