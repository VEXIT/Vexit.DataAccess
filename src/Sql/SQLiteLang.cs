
/************************************************
 * 
 * Copyright        :   © 2023 VEXIT ®, www.vexit.com
 * Author           :   Vex Tatarevic
 * Date Created     :   2023-07-13
 * Date Updated     :   2026-04-10 | Vex | Added Uuid() method
 *                     2026-05-02 | Vex | Added DefaultSchemaName, SchemaName() and TableName() methods
 *
 ************************************************/



using System.Text;
using Vexit.Common.Extensions.StringExtensions;

namespace Vexit.DataAccess.Sql;

/// <summary>
/// Implements Sql language constructs specific to SQLite database
/// </summary>
public class SQLiteLang : ISqlLang
{
    public string DefaultSchemaName => "main";

    public string SchemaName(string schemaName)
        => string.IsNullOrWhiteSpace(schemaName)
            ? DefaultSchemaName
            : schemaName.ToSnakeCase();

    public string TableName(string tableName)
        => tableName.ToSnakeCase();

    public string FieldName(string fieldName)
        => fieldName.ToSnakeCase();


    #region [ DATA TYPES DECLARATION ]

    public string Char(int size) => $"CHARACTER({size})";
    public string Varchar(int? size = null) => size != null ? $"VARCHAR({size})" : "TEXT";
    public string Nvarchar(int? size = null) => size != null ? $"NVARCHAR({size})" : "TEXT";
    public string Text() => "TEXT";
    public string Decimal(int precision, int scale) => $"DECIMAL({precision},{scale})";
    public string Uuid() => "TEXT"; 

    #endregion

    public string GetBool(bool value) => value ? "1" : "0";
    public string GetDate() => "date('now')";
    public string GetUtcDate() => "datetime('now')";
    public string GetUtcDateOnly() => "date('now')";

    public string InsertIntoValues(string schema, string table, string values, bool returnId = false, params string[] fields)
    {
        var sbInsert = new StringBuilder();
        // SQLite doesn't use [schema].[table] syntax in the same way, usually just [table]
        sbInsert.Append(@$"insert into [{table}]"); 
        sbInsert.Append("(");
        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            var isFirst = i == 0;
            sbInsert.Append($"{(isFirst ? "" : ",")}[{FieldName(field)}]");
        }
        sbInsert.Append(")\n");
        sbInsert.Append("values \n");
        sbInsert.Append(returnId && values.EndsWith(";") ? values.TrimEnd(';') : values);
        sbInsert.Append(";");
        return sbInsert.ToString();
    }


    public string SelectWhereFieldsEqual_WithParameters(string schema, string table, string[] paramFields, params string[] selectFields)
    {
        var selectClause = selectFields.Length == 0 ? "*" : string.Join(", ", selectFields.Select(f => $"[{f}]"));
        var whereClause = string.Join(" AND ", paramFields.Select(field => $"[{field}] = @{field}"));
        
        return $@"SELECT {selectClause} FROM [{table}] WHERE {whereClause}";
    }

    public string SelectFirst(string schema, string table)
    {
        // SQLite uses LIMIT 1 at the end instead of TOP 1
        return $@"SELECT * FROM [{table}] LIMIT 1";
    }

    public string SetIdentityInsertOn(string schema, string table)
    {
        throw new NotImplementedException();
    }

    public string SetIdentityInsertOff(string schema, string table, string idField = "Id")
    {
        throw new NotImplementedException();
    }

    public string SelectWhereFieldsEqual(string schema, string table, Dictionary<string, object> conditionFields, params string[] selectFields)
    {
        throw new NotImplementedException();
    }
}

