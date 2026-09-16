
/************************************************
 * 
 * Copyright        :   © 2023 VEXIT ®, www.vexit.com
 * Author           :   Vex Tatarevic
 * Date Created     :   2023-07-13
 * Date Updated     :   2025-06-04 | Vex | Updated InsertIntoValues - added parameter returnId bool, which allows to optionally return ids from created records
 *                      2026-04-10 | Vex | Added Uuid() method
 *                      2026-05-02 | Vex | Added DefaultSchemaName, SchemaName() and TableName() methods
 *                      2026-04-30 | OUTPUT inserted: use FieldName(\"Id\") for snake_case columns
 *
 ************************************************/

using System.Text;
using Vexit.Common.Extensions.StringExtensions;

namespace Vexit.DataAccess.Sql;

/// <summary>
/// Implements Sql language constructs specific to SqlServer database
/// </summary>
public class SqlServerLang : ISqlLang
{
    public string DefaultSchemaName => "dbo";

    public string SchemaName(string schemaName)
        => string.IsNullOrWhiteSpace(schemaName)
            ? DefaultSchemaName
            : schemaName.ToSnakeCase();

    public string TableName(string tableName)
        => tableName.ToSnakeCase();

    public string FieldName(string fieldName)
        => fieldName.ToSnakeCase();


    #region [ DATA TYPES DECLARATION ]

    public string Char(int size) => $"char({size})";

    public string Varchar(int? size = null) => $"varchar({size})";

    public string Nvarchar(int? size = null) => $"nvarchar({size})";

    public string Text() => "nvarchar(max)";

    public string Decimal(int precision, int scale) => $"decimal({precision},{scale})";

    public string Uuid() => "uniqueidentifier";

    #endregion

    public string GetBool(bool value) => value ? "1" : "0";

    public string GetDate() => "getdate()";

    public string GetUtcDate() => "getutcdate()";

    public string GetUtcDateOnly() => "cast(getutcdate() as date)";


    public string InsertIntoValues(
     string schema,
     string table,
     string values,
     bool returnId = false,
     params string[] fields
 )
    {
        var sbInsert = new StringBuilder();
        sbInsert.Append(@$"insert into [{schema}].[{table}]");
        sbInsert.Append("(");
        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            var isFirst = i == 0;
            sbInsert.Append($"{(isFirst ? "" : ",")}[{FieldName(field)}]");
        }
        sbInsert.Append(")\n");
        sbInsert.Append("values \n");

        // Remove trailing semicolon if we're returning Id
        sbInsert.Append(returnId && values.EndsWith(";") ? values.TrimEnd(';') : values);

        if (returnId)
        {
            sbInsert.Append($"\n output inserted.[{FieldName("Id")}];");
        }

        return sbInsert.ToString();
    }

    public string SetIdentityInsertOn(string schema, string table)
    {
        return $"set identity_insert {schema}.{table} on \n";
    }

    public string SetIdentityInsertOff(string Schema, string Table, string IdField = "Id")
    {
        return $"set identity_insert {Schema}.{Table} off \n";
    }


    #region [ Queries ]
    public string SelectWhereFieldsEqual(
    string schema,
    string table,
    Dictionary<string, object> conditionFields,
    params string[] selectFields
)
    {
        var selectClause = selectFields.Length == 0 ? "*" : string.Join(", ", selectFields.Select(f => $"[{f}]"));
        var whereClause = string.Join(" AND ", conditionFields.Select(field => $"[{field.Key}] = '{field.Value}'"));

        return
$@"SELECT {selectClause}
FROM [{schema}].[{table}]
WHERE {whereClause}";
    }

    /// <summary>
    /// Use this for ADO.NET DbCommand with Parameters
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="table"></param>
    /// <param name="paramFields"></param>
    /// <param name="selectFields"></param>
    /// <returns></returns>
    public string SelectWhereFieldsEqual_WithParameters(
        string schema,
        string table,
        string[] paramFields,
        params string[] selectFields
    )
    {
        var selectClause = selectFields.Length == 0 ? "*" : string.Join(", ", selectFields.Select(f => $"[{f}]"));
        var whereClause = string.Join(" AND ", paramFields.Select(field => $"[{field}] = @{field}"));

        return
$@"SELECT {selectClause}
FROM [{schema}].[{table}]
WHERE {whereClause}";
    }

    public string SelectFirst(string schema, string table)
    {
        return $@"
        SELECT TOP 1 *
        FROM [{schema}].[{table}]
        ";
    }
    #endregion

}
