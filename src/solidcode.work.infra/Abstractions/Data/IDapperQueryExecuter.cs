namespace Solidcode.Work.Infra.Abstractions;

public interface IDapperQueryExecuter
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);

    Task<T?> QuerySingleAsync<T>(string sql, object? param = null);
}