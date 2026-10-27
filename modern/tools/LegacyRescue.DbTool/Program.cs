using System.Text;
using LegacyRescue.DbTool;

// Usage:
//   dotnet run --project modern/tools/LegacyRescue.DbTool -- gen-seed [--data <dir>]
//   dotnet run --project modern/tools/LegacyRescue.DbTool -- create-db <db-path> [--data <dir>] [--force]

var positional = args.Where(a => !a.StartsWith("--", StringComparison.Ordinal)).ToList();
var force = args.Contains("--force");
var dataIndex = Array.IndexOf(args, "--data");
string? dataDir = dataIndex >= 0 && dataIndex + 1 < args.Length ? args[dataIndex + 1] : null;
if (dataDir is not null)
{
    positional.Remove(dataDir);
}

dataDir ??= DataDirectory.Find();
if (dataDir is null)
{
    Console.Error.WriteLine("data/schema.sql not found above the current directory. Pass --data <dir>.");
    return 2;
}

switch (positional.FirstOrDefault())
{
    case "gen-seed":
    {
        var path = Path.Combine(dataDir, "seed.sql");
        var sql = new SeedGenerator().Generate();
        File.WriteAllText(path, sql.ReplaceLineEndings("\n"), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        Console.WriteLine($"Wrote {path}");
        return 0;
    }

    case "create-db" when positional.Count >= 2:
    {
        var dbPath = Path.GetFullPath(positional[1]);
        if (File.Exists(dbPath))
        {
            if (!force)
            {
                Console.Error.WriteLine($"{dbPath} already exists. Use --force to recreate it.");
                return 1;
            }

            File.Delete(dbPath);
        }

        var counts = DatabaseBuilder.Create(dbPath, dataDir);
        Console.WriteLine($"Created {dbPath}");
        foreach (var (table, count) in counts)
        {
            Console.WriteLine($"  {table,-18}{count,6}");
        }

        return 0;
    }

    default:
        Console.Error.WriteLine("Usage: gen-seed [--data <dir>] | create-db <db-path> [--data <dir>] [--force]");
        return 2;
}
