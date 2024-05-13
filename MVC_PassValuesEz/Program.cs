using System.Text;

Console.WriteLine("Enter the Table Name -- :");
var initialData = Console.ReadLine();

Console.WriteLine("Enter the ValueSetId -- :");
var valueSetId = Console.ReadLine();

Console.WriteLine("Paste the data (Code,Code System,Description) separated by tabs. Press Enter twice to generate code:");

var sb = new StringBuilder();
sb.AppendLine(initialData);
sb.AppendLine(valueSetId);

int i = 1;
string line;
while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
{
    var data = line.Split('\t');
    sb.AppendLine(GenerateEntityData(initialData, valueSetId, i,data[0], data[1], data[2]));
    i++;
}

string generatedCode = sb.ToString().TrimEnd(); // Remove the extra newline at the end

Console.WriteLine("\nGenerated Code:\n");
Console.WriteLine(generatedCode);

// Write the generated code to a file
File.WriteAllText( Path.Combine(Directory.GetCurrentDirectory(),"GeneratedCode.txt"), generatedCode);

static string GenerateEntityData(string tableName, string valueSetId,int i, string code, string codeSystem, string displayName)
{
    return $@"
            new {tableName}
            {{
                Id = {i},
                Code = ""{code}"",
                CodeSystem = ""{codeSystem}"",
                DisplayName = ""{displayName}"",
                ValueSetId = ""{valueSetId}"",
                MvcVersion = ""1.0"",
                IsActive = true
            }},";
}