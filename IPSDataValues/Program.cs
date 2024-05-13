using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the ValueSetId:");
        var valueSetId = Console.ReadLine();

        Console.WriteLine("Enter the SQL Query (Press Enter twice to finish):");
        var sqlQueryBuilder = new StringBuilder();
        string line;
        int emptyLineCount = 0;
        while (true)
        {
            line = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                emptyLineCount++;
                if (emptyLineCount >= 2)
                    break;
            }
            else
            {
                emptyLineCount = 0;
                sqlQueryBuilder.AppendLine(line);
            }
        }
        var sqlQuery = sqlQueryBuilder.ToString();

        var sb = new StringBuilder();

        // Find INSERT INTO statements
        var insertStatements = Regex.Matches(sqlQuery, @"INSERT INTO\s+(\w+)\s+\(([^)]+)\)\s+VALUES\s+\(([^)]+)\);");
        foreach (Match insertMatch in insertStatements)
        {
            string tableName = insertMatch.Groups[1].Value;

            string id = "", code = "", codeSystem = "", displayName = "";
            string[] insertValues = insertMatch.Groups[3].Value.Split(',');
            if (insertValues.Length == 4)
            {
                id = insertValues[0].Trim('\'');
                code = insertValues[1].Trim('\'');
                codeSystem = insertValues[2].Trim('\'');
                displayName = insertValues[3].Trim('\'');
            }
            sb.AppendLine(GenerateEntityData(tableName, valueSetId, id, code, codeSystem, displayName));
        }

        string generatedCode = sb.ToString().TrimEnd(); // Remove the extra newline at the end

        Console.WriteLine("\nGenerated Code:\n");
        Console.WriteLine(generatedCode);

        // Write the generated code to a file
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "GeneratedCode.txt"), generatedCode);
    }

    static string GenerateEntityData(string tableName, string valueSetId, string id, string code, string codeSystem, string displayName)
    {
        return $@"
        new {tableName}
        {{
            Id = {id},
            Code = ""{code.Trim(' ', '\'')}"",
            CodeSystem = ""{codeSystem.Trim(' ', '\'')}"",
            DisplayName = ""{displayName.Trim(' ', '\'')}"",
            ValueSetId = ""{valueSetId}"",
            IpsVersion = ""1.0"",
            IsActive = true
        }},
    ";
    }

}
