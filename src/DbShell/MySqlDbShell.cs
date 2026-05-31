/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.DbShell;

/// <summary>
/// MySQL via <c>mysql</c> CLI (not yet used by VXS DbMgmt).
/// </summary>
public sealed class MySqlDbShell : DbShellBase
{
    public MySqlDbShell(string databaseName) : base(databaseName)
    {
    }

    public override string BuildCommand(string sql)
    {
        var normalizedSql = (sql ?? string.Empty).Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        var escapedSql = normalizedSql.Replace("\"", "\\\"").Replace("'", "\\'");

        return $"""mysql -D {DatabaseName} -N -e "{escapedSql}" """;
    }
}
