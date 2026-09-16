/************************************************
 *  
 * Copyright        :   © 2023 VEXIT ®, www.vexit.com
 * Author           :   Vex Tatarevic
 * Date Created     :   2023-07-13
 * Date Updated     :   2026-04-10 | Vex | Added Uuid() method
 *                      2026-05-02 | Vex | Added GetSchemaName() and GetTableName() methods
 *                  
 ************************************************/

namespace Vexit.DataAccess.Sql;

/// <summary>
/// Interface for implementation of Sql language constructs. 
/// Should be inherited by classes for specific database technology e..g.SqlServer and Postgres
/// </summary>
public interface ISqlLang
{
    /// <summary>
    /// Default schema name when none is provided
    /// </summary>
    string DefaultSchemaName { get; }


    /// <summary>
    /// Converts schema name in PascalCase to naming convention case of the selected database technology. <br />
    /// For example: Postgresql schema name "MySchema" will be converted to "my_schema"
    /// </summary>
    /// <param name="schemaName"></param>
    /// <returns></returns>
    string SchemaName(string schemaName);

    /// <summary>Schema from module/type name, e.g. <c>SchemaName&lt;Log&gt;()</c> → <c>log</c> (Postgres).</summary>
    string SchemaName<TSchema>() => SchemaName(typeof(TSchema).Name);



    /// <summary>
    /// Converts table name in PascalCase to naming convention case of the selected database technology. <br />
    /// For example: Postgresql table name "MyTable" will be converted to "my_table"
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string TableName(string tableName);

    /// <summary>Table from entity type name, e.g. <c>TableName&lt;LogEntry&gt;()</c> → <c>log_entry</c>.</summary>
    string TableName<TEntity>() where TEntity : class => TableName(typeof(TEntity).Name);

    /// <summary>
    /// Converts field name in PascalCase to naming convention case of the selected database technology. <br />
    /// For example: Postgresql column name "MyColumn" will be converted to "my_column"
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    string FieldName(string fieldName);
    string FieldName<TField>() => FieldName(typeof(TField).Name);



    #region [ DATA TYPES DECLARATION ]

    string Char(int size);
    string Varchar(int? size = null);
    string Nvarchar(int? size = null);
    string Text();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="precision">Maximum total number of decimal digits that will be stored, counting both - digits to the left and to the right of the decimal point. The precision has a range from 1 to 38. The default precision is 38.... For example: 123.45 has precision 5 </param>
    /// <param name="scale">Scale which is the number of decimal digits that will be stored to the right of the decimal point. The scale has a range from 0 to p (precision). The scale can be specified only if the precision is specified. By default, the scale is zero... For example: 123.45 has scale 2</param>
    /// <returns></returns>
    string Decimal(int precision, int scale);

    string Uuid();

    #endregion

    /// <summary>Returns a SQL literal for a boolean suitable for this dialect (defaults, CHECK, raw fragments).</summary>
    string GetBool(bool value);

    string GetDate();

    /// <summary>
    /// 
    /// </summary>
    string GetUtcDate();

    /// <summary>
    /// Gets date part of the utc date
    /// </summary>
    string GetUtcDateOnly();

    /// <summary>
    /// INSERT template; <paramref name="values"/> is the VALUES clause body (literals or <c>@param</c> placeholders).
    /// <paramref name="fields"/> are CLR property names (<c>nameof(LogEntry.X)</c>).
    /// </summary>
    string InsertIntoValues<TSchema, TEntity>(string values, bool returnId = false, params string[] fields)
        where TEntity : class
        => InsertIntoValues(SchemaName<TSchema>(), TableName<TEntity>(), values, returnId, fields);

    string InsertIntoValues(string schema, string table, string values, bool returnId = false, params string[] fields);


    /// <summary>
    /// Builds a single-row <c>INSERT</c> statement for ADO.NET (<see cref="System.Data.Common.DbCommand"/>)
    /// with named parameters, using the same column naming as <see cref="InsertIntoValues"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each entry in <paramref name="fields"/> is a CLR property name (e.g. <c>nameof(LogEntry.TenantId)</c>).
    /// Column identifiers are produced with <see cref="FieldName(string)"/>; parameter names use the same
    /// snake_case (Postgres) or bracketed names (SQL Server / SQLite) as <c>@tenant_id</c>, matching
    /// <c>command.Parameters.AddWithValue("tenant_id", value)</c> on Npgsql, SqlClient, and Microsoft.Data.Sqlite.
    /// </para>
    /// <para>
    /// Use <see cref="InsertIntoValues"/> when <paramref name="values"/> are SQL literals or multi-row seed
    /// blocks in <c>.sqlv</c> scripts. Use this method from repositories/services that bind parameters in code.
    /// </para>
    /// <para>
    /// When <paramref name="returnId"/> is <see langword="true"/>, the dialect appends its identity-return
    /// clause (<c>RETURNING id</c> on Postgres, <c>OUTPUT inserted.[id]</c> on SQL Server). SQLite does not
    /// support returning the new key in the current <see cref="InsertIntoValues"/> implementation.
    /// </para>
    /// <para>
    /// Example (Postgres + Npgsql):
    /// <code>
    /// var sql = lang.InsertIntoValues_WithParameters&lt;Log, LogEntry&gt;(
    ///     returnId: true,
    ///     nameof(LogEntry.Type),
    ///     nameof(LogEntry.Environment),
    ///     nameof(LogEntry.TenantId));
    ///
    /// await using var cmd = new NpgsqlCommand(sql, connection);
    /// cmd.Parameters.AddWithValue("type", (int)entry.Type);
    /// cmd.Parameters.AddWithValue("environment", entry.Environment);
    /// cmd.Parameters.AddWithValue("tenant_id", (object?)entry.TenantId ?? DBNull.Value);
    /// var id = await cmd.ExecuteScalarAsync(ct);
    /// </code>
    /// </para>
    /// </remarks>
    /// <param name="schema">Schema already in dialect form, or from <see cref="SchemaName(string)"/> / <c>SchemaName&lt;TSchema&gt;()</c>.</param>
    /// <param name="table">Table already in dialect form, or from <see cref="TableName(string)"/> / <c>TableName&lt;TEntity&gt;()</c>.</param>
    /// <param name="returnId">When <see langword="true"/>, append the dialect-specific clause to read the new primary key (typically <c>Id</c>).</param>
    /// <param name="fields">CLR property names for columns to insert, in order.</param>
    /// <returns>Complete <c>INSERT</c> SQL text; bind one parameter per field using <see cref="FieldName(string)"/> as the parameter name (without <c>@</c> in the ADO API).</returns>
    /// <exception cref="ArgumentException"><paramref name="fields"/> is empty.</exception>
    string InsertIntoValues_WithParameters(
        string schema,
        string table,
        bool returnId = false,
        params string[] fields)
    {
        if (fields is null || fields.Length == 0)
            throw new ArgumentException("At least one field is required.", nameof(fields));

        var placeholders = string.Join(", ", fields.Select(f => "@" + FieldName(f)));
        return InsertIntoValues(schema, table, $"({placeholders})", returnId, fields);
    }

    /// <summary>
    /// Parameterized INSERT for <typeparamref name="TEntity"/>, with schema from the module
    /// namespace segment (e.g. <c>nameof(Log)</c> passed as <paramref name="schema"/>).
    /// </summary>
    /// <param name="schema">Module schema fragment in PascalCase (e.g. <c>"Log"</c>); converted via <see cref="SchemaName(string)"/>.</param>
    string InsertIntoValues_WithParameters<TEntity>(
        string schema,
        bool returnId = false,
        params string[] fields)
        where TEntity : class
        => InsertIntoValues_WithParameters(
            SchemaName(schema),
            TableName<TEntity>(),
            returnId,
            fields);


    /// <summary>
    ///  Used inside seeding script when we want to manually specify identity column values
    /// </summary>
    /// <returns></returns>
    string SetIdentityInsertOn(string schema, string table);

    /// <summary>
    ///  Used inside seeding script at the end of insert execution to set back identity column to autoincrement starting from current max value
    /// </summary>
    /// <returns></returns>
    string SetIdentityInsertOff(string schema, string table, string idField = "Id");


    #region [ Queries ]

    string SelectWhereFieldsEqual(
        string schema,
        string table,
        Dictionary<string, object> conditionFields,
        params string[] selectFields
    );

    /// <summary>
    /// Use this for ADO.NET DbCommand with Parameters
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="table"></param>
    /// <param name="paramFields"></param>
    /// <param name="selectFields"></param>
    /// <returns></returns>
    string SelectWhereFieldsEqual_WithParameters(
        string schema,
        string table,
        string[] paramFields,
        params string[] selectFields
    );

    string SelectFirst(string schema, string table);
    #endregion

}
