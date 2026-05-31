/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com, Tomorrow is today... 
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.Sql.Models;

/// <summary>
/// One WHERE clause fragment for <see cref="ISqlLang.Select"/> / <see cref="ISqlLang.SelectCount"/>.
/// </summary>
public sealed class Predicate
{
    /// <summary>Left side (column identifier, quoted by the dialect).</summary>
    public string Left { get; init; } = string.Empty;

    /// <summary>
    /// Comparison operator: <c>=</c>, <c>!=</c>, or <c>like</c> (trimmed/lowercased; <c>&lt;&gt;</c> accepted as not-equal).
    /// Validated via <see cref="Enums.ComparerStrEnum.Parse"/>.
    /// </summary>
    public string Comparer { get; init; } = "=";

    /// <summary>Right side: SQL literal when <see cref="RightIsLiteral"/> is true, otherwise a column identifier.</summary>
    public string Right { get; init; } = string.Empty;

    /// <summary>When true, <see cref="Right"/> is escaped and quoted as a string literal.</summary>
    public bool RightIsLiteral { get; init; } = true;
}
