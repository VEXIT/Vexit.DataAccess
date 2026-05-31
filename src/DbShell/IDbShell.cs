/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.DbShell;

/// <summary>
/// Builds a remote shell command that runs SQL against a database on the server.
/// </summary>
public interface IDbShell
{
    /// <summary>
    /// The database name to run the SQL against.
    /// </summary>
    string DatabaseName { get; set; }

    /// <summary>
    /// Builds the full command line to execute on the remote host (e.g. via SSH).
    /// </summary>
    string BuildCommand(string sql);
}
