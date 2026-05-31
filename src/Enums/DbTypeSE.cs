/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20 - Shared database engine type (lifted from Vexit.VMod)
 *
 *************************************************************/

using Vexit.Common.BaseClasses;

namespace Vexit.DataAccess.Enums;

public sealed class DbTypeSE : SmartEnumBase<DbTypeEnum, DbTypeSE>
{
    public static DbTypeSE Postgres => FromEnum(DbTypeEnum.Postgres);
    public static DbTypeSE SqlServer => FromEnum(DbTypeEnum.SqlServer);
    public static DbTypeSE MySql => FromEnum(DbTypeEnum.MySql);
    public static DbTypeSE SQLite => FromEnum(DbTypeEnum.SQLite);
    
    private DbTypeSE(DbTypeEnum enumValue, string name) : base(enumValue, name) { }
}

/// <summary>
/// Database engine used by VMod apps, VXS DbMgmt, and related tooling.
/// </summary>
public enum DbTypeEnum
{
    Postgres,
    SqlServer,
    MySql,
    SQLite,
}
