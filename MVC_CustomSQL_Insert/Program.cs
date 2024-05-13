// Get user input for table name
Console.Write("Enter table name: ");
string tableName = Console.ReadLine();

// Get user input for value_set_id
Console.Write("Enter value_set_id: ");
string valueSetId = Console.ReadLine();

// Console.Write("Enter allergy_category_id: ");
// string allergyCategoryId = Console.ReadLine();

// Get user input for entities
Console.WriteLine("Enter entities (format: code_system_version, concept_code, description):");
Console.WriteLine("Enter an empty line to finish.");

string entityInput;
var entities = new System.Collections.Generic.List<string>();
while (!string.IsNullOrWhiteSpace(entityInput = Console.ReadLine()))
{
    entities.Add(entityInput);
}

// Specify the output file path (in the current directory)
string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "GenerateSQLCommand.txt");

int i = 1;
// Write SQL statements to the file
using (StreamWriter writer = new StreamWriter(outputPath))
{
    writer.WriteLine($"INSERT INTO {tableName}(id, code_system_id, code_system_version, concept_code, description, value_set_id, mvc_version, is_active) VALUES");

    foreach (string entity in entities)
    {
        string[] values = entity.Split('\t'); // Use tab as the delimiter

        if (values.Length >= 3)
        {
            string id = i.ToString();
            string codeSystemId = values[0];
            string codeSystemVersion = values[1];
            string conceptCode = values[2];
            string description = values.Length > 3 ? values[3].Replace("'", "\"") : string.Empty;
            string formattedValues = $" ('{id}', '{codeSystemId}', '{codeSystemVersion}', '{conceptCode}', '{description}', '{valueSetId}', '7.1.0', true),";
            writer.WriteLine(formattedValues);
        }
        else
        {
            Console.WriteLine($"Invalid input: {entity}");
        }

        i++;
    }
    // double quotes on " {'} " ,, fixed by doing double quotes " {''} "
}

Console.WriteLine($"Output written to {outputPath}");