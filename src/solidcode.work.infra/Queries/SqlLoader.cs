public static class SqlLoader
{
    public static async Task<string> LoadAsync(string filename)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", filename);
        return await File.ReadAllTextAsync(path);
    }
}