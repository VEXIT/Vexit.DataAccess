/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.DbShell;

/// <summary>
/// SQL Server via <c>sqlcmd</c> (not yet used by VXS DbMgmt).
/// </summary>
public sealed class SqlCmdDbShell : DbShellBase
{
    public SqlCmdDbShell(string databaseName) : base(databaseName)
    {
    }

    public override string BuildCommand(string sql)
    {
        var normalizedSql = (sql ?? string.Empty).Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        var escapedSql = normalizedSql.Replace("\"", "\\\"");

        return $"""sqlcmd -d {DatabaseName} -Q "{escapedSql}" -h -1 -W""";
    }
}
