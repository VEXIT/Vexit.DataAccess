/*****************************************
 * 
 *  Copyright       :   © VEXIT® 2025, www.vexit.com
 *  Author          :   Vex Tatarevic
 *  Date Created    :   2010-06-09 
 *  
 *  Description     :   This utility class provides 
 *  
 *  Updates         :   
 * 
 *****************************************/


/**
  
1. Takes a DbDataReader in the constructor
2. Caches column ordinals for faster lookups
3. Implements an indexer that:
    - Looks up the column ordinal
    - Gets the value
    - Converts DBNull to null
    - Returns the value
 * 
 */

using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace Vexit.DataAccess.Utils;



/// <summary>
/// Used with ADO.NET queries <br/>
/// Wrapper for DbDataReader that provides property-like access to reader columns. <br/>
/// Used in conjunction with DataMapper to map database results to entities. <br/>
/// 
/// 1. Takes a DbDataReader in the constructor <br/>
/// 2. Caches column ordinals for faster lookups <br/>
/// 3. Implements an indexer that: <br/>
///     - Looks up the column ordinal <br/>
///     - Gets the value <br/>
///     - Converts DBNull to null <br/>
///     - Returns the value <br/> <br/>
///     
/// Use it with DataMapper like this:
/// <code>
/// using var reader = await command.ExecuteReaderAsync();
/// if (await reader.ReadAsync())
/// {
///     var dataReader = new DataReaderWrapper(reader);
///     return DataMapper.Map&lt;DataReaderWrapper, TEntity&gt;(dataReader);
/// }
/// </code>
/// </summary>
public class DataReaderWrapper
{
    private readonly DbDataReader _reader;
    private readonly Dictionary<string, int> _columnOrdinals;

    /// <summary>
    /// Initializes a new instance of the DataReaderWrapper class.
    /// </summary>
    /// <param name="reader">The DbDataReader to wrap</param>
    public DataReaderWrapper(DbDataReader reader)
    {
        _reader = reader;
        _columnOrdinals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Cache column ordinals for faster lookups
        for (int i = 0; i < reader.FieldCount; i++)
        {
            _columnOrdinals[reader.GetName(i)] = i;
        }
    }

    /// <summary>
    /// Gets the value of the specified column by property name.
    /// Returns null if the column doesn't exist or contains DBNull.
    /// </summary>
    /// <param name="propertyName">The name of the column to get</param>
    /// <returns>The column value, or null if the column doesn't exist or contains DBNull</returns>
    public object? this[string propertyName]
    {
        get
        {
            if (!_columnOrdinals.TryGetValue(propertyName, out int ordinal))
            {
                return null;
            }

            var value = _reader.GetValue(ordinal);
            return value == DBNull.Value ? null : value;
        }
    }
}
