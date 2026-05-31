/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-27
 *
 *************************************************************/

namespace Vexit.DataAccess.DbShell;

public class DbShellBase : IDbShell
{
    public string DatabaseName { get; set; } = string.Empty;

    public DbShellBase(string databaseName)
    {
        DatabaseName = databaseName;
    }

    public virtual string BuildCommand(string sql)
    {
        return string.Empty;
    }
}