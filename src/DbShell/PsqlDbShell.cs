/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.DbShell;

/// <summary>
/// Postgres shell command builder using <c>psql</c> with SQL passed through a quoted heredoc.
/// </summary>
public sealed class PsqlDbShell : DbShellBase
{
    private const string HereDocDelimiter = "__VEXIT_PSQL__";

    public PsqlDbShell(string databaseName) : base(databaseName)
    {
    }

    public override string BuildCommand(string sql)
    {
        var normalizedSql = (sql ?? string.Empty).Replace("\r\n", "\n").Replace("\r", "\n").Trim();
        var delimiter = CreateHereDocDelimiter(normalizedSql);

        return $"""
            sudo -u postgres psql -X -v ON_ERROR_STOP=1 -d {QuoteShellArg(DatabaseName)} -t -A <<'{delimiter}'
            {normalizedSql}
            {delimiter}
            """;
    }

    private static string QuoteShellArg(string value) =>
        $"'{value.Replace("'", "'\\''", StringComparison.Ordinal)}'";

    private static string CreateHereDocDelimiter(string sql)
    {
        var delimiter = HereDocDelimiter;

        while (ContainsLine(sql, delimiter))
            delimiter += "_";

        return delimiter;
    }

    private static bool ContainsLine(string value, string lineToMatch)
    {
        foreach (var line in value.Split('\n'))
        {
            if (line == lineToMatch)
                return true;
        }

        return false;
    }
}
