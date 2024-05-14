using System;
using System.IO;
using System.Text;
using ExcelDataReader;

// Path to your Excel file
string filePath = "your_excel_file.xlsx";

// Read Excel file using ExcelDataReader
using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        // Read the data from the Excel file
        var result = reader.AsDataSet();

        // Assuming the data is in the first worksheet
        var dataTable = result.Tables[0];

        // Generate output
        StringBuilder output = new StringBuilder();
        output.AppendLine("using System;");
        output.AppendLine("public class ValueSetGenerator");
        output.AppendLine("{");
        output.AppendLine("    public static void Main()");
        output.AppendLine("    {");

        foreach (System.Data.DataRow row in dataTable.Rows)
        {
            string id = row[0].ToString();
            string name = row[1].ToString();
            output.AppendLine($"        Console.WriteLine(\"new ValueSet\");");
            output.AppendLine($"        Console.WriteLine(\"{{\");");
            output.AppendLine($"        Console.WriteLine(\"    Id = \\\"{id}\\\",\");");
            output.AppendLine($"        Console.WriteLine(\"    Name = \\\"{name}\\\"\");");
            output.AppendLine($"        Console.WriteLine(\"}},\");");
        }

        output.AppendLine("    }");
        output.AppendLine("}");

        // Write output to a file
        File.WriteAllText("output.cs", output.ToString());
    }
}

Console.WriteLine("Output generated successfully!");
    
