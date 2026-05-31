/************************************************
 * 
 * Copyright     : © 2023 VEXIT ®, www.vexit.com
 * Author        : Vex Tatarevic
 * Date Created  : 2023-07-13 
 * Date Updated  : 2025-06-04  - Vex   - Updated InsertIntoValues - added parameter returnId bool, which allows to optionally return ids from created records
 *                 2026-04-10 | Vex | Added Uuid() method
 *                 2026-05-02 | Vex | Added DefaultSchemaName, SchemaName() and TableName() methods
 *                 2026-04-30 | RETURNING / setval: use FieldName(id) so snake_case columns match inserts
 *
 ************************************************/

using System.Text;
using Vexit.Common.Extensions.StringExtensions;

namespace Vexit.DataAccess.Sql;

/// <summary>
///  Implements Sql language constructs specific to Postgres database
/// </summary>
public class PostgresLang : ISqlLang
{
    public string DefaultSchemaName => "public";

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
    public string Varchar(int? size) => size != null ? $"varchar({size})" : "varchar";
    public string Nvarchar(int? size) => size != null ? $"varchar({size})" : "varchar";
    public string Text() => "text";
    public string Decimal(int precision, int scale) => $"numeric({precision}, {scale})";
    public string Uuid() => "uuid";

    #endregion

    public string GetBool(bool value) => value ? "true" : "false";
    public string GetDate() => "now()";

    public string GetUtcDate() => "now() at time zone 'utc'";

    public string InsertIntoValues(
    string schema,
    string table,
    string values,
    bool returnId = false,
    params string[] fields
)
    {
        var sbInsert = new StringBuilder();
        sbInsert.Append($"insert into \"{schema}\".\"{table}\"");
        sbInsert.Append("(");
        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            var isFirst = i == 0;
            var fieldName = FieldName(field);
            sbInsert.Append($"{(isFirst ? "" : ",")}\"{fieldName}\"");
        }
        sbInsert.Append(")\n");
        sbInsert.Append("values \n");

        // Remove trailing semicolon and line endings so RETURNING is not after a statement terminator (e.g. .sqlv with \r\n)
        var valuesForInsert = returnId ? values.TrimEnd().TrimEnd(';') : values;
        sbInsert.Append(valuesForInsert);

        if (returnId)
        {
            var idCol = FieldName("Id");
            sbInsert.Append($"\nreturning \"{idCol}\";");
        }

        return sbInsert.ToString();
    }

    public string SetIdentityInsertOn(string schema, string table)
    {
        // NOTE: Postgres allows you to manually set identity without any special command.
        // therefore we return empty string
        return $"";
    }

    public string SetIdentityInsertOff(string schema, string table, string idField = "Id")
    {
        // In Postgres we have to update identity sequency after having inserted records with manual id.
        var idFieldName = FieldName(idField);
        return $"select setval(pg_get_serial_sequence('\"{schema}\".\"{table}\"', '{idFieldName}'), (select max(\"{idFieldName}\") from \"{schema}\".\"{table}\")); \n";
    }

    #region [ Queries ]
    public string SelectWhereFieldsEqual(
    string schema,
    string table,
    Dictionary<string, object> conditionFields,
    params string[] selectFields
)
    {
        var selectClause = selectFields.Length == 0 ? "*" : string.Join(", ", selectFields.Select(f => $"\"{f}\""));
        var whereClause = string.Join(" AND ", conditionFields.Select(field => $"\"{field.Key}\" = '{field.Value}'"));

        return
$@"SELECT {selectClause}
FROM ""{schema}"".""{table}""
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
        var selectClause = selectFields.Length == 0 ? "*" : string.Join(", ", selectFields.Select(f => $"\"{f}\""));
        var whereClause = string.Join(" AND ", paramFields.Select(field => $"\"{field}\" = @{field}"));

        return
$@"SELECT {selectClause}
FROM ""{schema}"".""{table}""
WHERE {whereClause}";
    }

    public string SelectFirst(string schema, string table)
    {
        return
$@"SELECT *
FROM ""{schema}"".""{table}""
LIMIT 1";
    }
    #endregion


}
