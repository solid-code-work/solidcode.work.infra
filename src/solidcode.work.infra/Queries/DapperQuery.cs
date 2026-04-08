using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;

public class DapperQuery
{
    private readonly DbContext _context;

    public DapperQuery(DbContext context) => _context = context;

    private IDbConnection Connection => _context.Database.GetDbConnection();

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        => await Connection.QueryAsync<T>(sql, param);

    public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null)
        => await Connection.QueryFirstOrDefaultAsync<T>(sql, param);
}