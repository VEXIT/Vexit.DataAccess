/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20 - Resolves ISqlLang + IShell by database engine
 *
 *************************************************************/

using Vexit.DataAccess.Enums;
using Vexit.DataAccess.DbShell;
using Vexit.DataAccess.Sql;

namespace Vexit.DataAccess;

/// <summary>
/// Factory for dialect-specific SQL builders and remote shell runners.
/// </summary>
public static class DbAccess
{
    public static ISqlLang CreateSqlLang(DbTypeEnum dbType) =>
        dbType switch
        {
            DbTypeEnum.Postgres => new PostgresLang(),
            DbTypeEnum.SqlServer => new SqlServerLang(),
            DbTypeEnum.MySql => throw new NotSupportedException($"SQL lang for {dbType} is not implemented yet."),
            DbTypeEnum.SQLite => new SQLiteLang(),
            _ => throw new NotSupportedException($"Unknown database type: {dbType}"),
        };

    public static IDbShell CreateDbShell(DbTypeEnum dbType, string? databaseName = null) =>
        dbType switch
        {
            DbTypeEnum.Postgres => new PsqlDbShell(databaseName ?? GetSystemDatabaseName(dbType)),
            DbTypeEnum.SqlServer => new SqlCmdDbShell(databaseName ?? GetSystemDatabaseName(dbType)),
            DbTypeEnum.MySql => new MySqlDbShell(databaseName ?? GetSystemDatabaseName(dbType)),
            DbTypeEnum.SQLite => throw new NotSupportedException($"Shell for {dbType} is not implemented yet."),
            _ => throw new NotSupportedException($"Unknown database type: {dbType}"),
        };

    public static (ISqlLang Sql, IDbShell Shell) Create(DbTypeEnum dbType, string databaseName) =>
        (CreateSqlLang(dbType), CreateDbShell(dbType, databaseName));


    public static string GetSystemDatabaseName(DbTypeEnum dbType) =>
        dbType switch
        {
            DbTypeEnum.Postgres => "postgres",
            DbTypeEnum.SqlServer => "master",
            DbTypeEnum.MySql => "mysql",
            DbTypeEnum.SQLite => "sqlite",
            _ => throw new NotSupportedException($"Unknown database type: {dbType}"),
        };
}
