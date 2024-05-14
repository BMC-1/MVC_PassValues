using System;
using System.IO;

class Program
{
    static void Main()
    {
         string filePath = @"./sample.txt"; // Path to your input file
        string outputPath = @"./GenerateSqlCommand.txt"; // Path to save the modified file


        try
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("INSERT INTO"))
                    {
                        // Append new columns to the INSERT INTO clause correctly
                        writer.WriteLine(line.TrimEnd(')') + ", value_set_id, ips_version, is_active)");
                    }
                    else if (line.Trim().StartsWith("VALUES"))
                    {
                        // append VALUES to the new columns
                        writer.WriteLine(line.TrimEnd(';', ')') + ", '2.16.840.1.113883.11.22.5', '1.0', true);");
                    }
                    else
                    {
                        // Write other lines unchanged
                        writer.WriteLine(line);
                    }
                }
            }

            Console.WriteLine("File has been processed and saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
