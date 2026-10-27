using Microsoft.Data.Sqlite;

namespace LegacyRescue.DbTool;

internal static class DatabaseBuilder
{
    private static readonly string[] Tables = ["Item", "DefectCode", "WorkOrder", "ProductionResult"];

    /// <summary>Creates a SQLite database from schema.sql and seed.sql and returns the row count per table.</summary>
    public static IReadOnlyList<(string Table, long Count)> Create(string dbPath, string dataDir)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        using var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString());
        connection.Open();
        Execute(connection, File.ReadAllText(Path.Combine(dataDir, "schema.sql")));
        Execute(connection, File.ReadAllText(Path.Combine(dataDir, "seed.sql")));

        var counts = new List<(string, long)>();
        foreach (var table in Tables)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM {table}";
            counts.Add((table, (long)command.ExecuteScalar()!));
        }

        return counts;
    }

    private static void Execute(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}

internal static class DataDirectory
{
    /// <summary>Walks up from the current directory to the first folder containing data/schema.sql.</summary>
    public static string? Find()
    {
        for (var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "data");
            if (File.Exists(Path.Combine(candidate, "schema.sql")))
            {
                return candidate;
            }
        }

        return null;
    }
}
