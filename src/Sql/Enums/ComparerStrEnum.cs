/*************************************************************
 *
 *  Copyright    : © VEXIT 2026, www.vexit.com, Tomorrow is today... 
 *  Author       : Vex Tatarevic
 *  Date Created : 2026-05-20
 *
 *************************************************************/

namespace Vexit.DataAccess.Sql.Enums;

/// <summary>
/// Canonical SQL comparer tokens for <see cref="Models.Predicate"/> (lowercase: <c>=</c>, <c>!=</c>, <c>like</c>).
/// </summary>
public sealed class ComparerStrEnum
{
    public static readonly ComparerStrEnum Equal = new("=");
    public static readonly ComparerStrEnum NotEqual = new("!=");
    public static readonly ComparerStrEnum Like = new("like");

    private static readonly IReadOnlyDictionary<string, ComparerStrEnum> CanonicalByValue =
        new Dictionary<string, ComparerStrEnum>(StringComparer.Ordinal)
        {
            [Equal.Value] = Equal,
            [NotEqual.Value] = NotEqual,
            [Like.Value] = Like,
        };

    private static readonly IReadOnlyDictionary<string, ComparerStrEnum> Aliases =
        new Dictionary<string, ComparerStrEnum>(StringComparer.Ordinal)
        {
            ["="] = Equal,
            ["!="] = NotEqual,
            ["<>"] = NotEqual,
            ["like"] = Like,
        };

    public static IReadOnlyList<ComparerStrEnum> All { get; } = [Equal, NotEqual, Like];

    private ComparerStrEnum(string value) => Value = value;

    /// <summary>Canonical token: <c>=</c>, <c>!=</c>, or <c>like</c>.</summary>
    public string Value { get; }

    /// <summary>
    /// SQL operator keyword to emit (<c>LIKE</c> is uppercased; <c>!=</c> and <c>=</c> pass through).
    /// </summary>
    public string ToSqlOperator() =>
        Value switch
        {
            "like" => "LIKE",
            _ => Value,
        };

    /// <summary>
    /// Parses a comparer string (trimmed, lowercased). Accepts <c>&lt;&gt;</c> as alias for <c>!=</c>.
    /// Does not accept <c>~</c> (regex / bitwise NOT in SQL).
    /// </summary>
    public static ComparerStrEnum Parse(string comparer)
    {
        var normalized = Normalize(comparer);
        if (Aliases.TryGetValue(normalized, out var match))
            return match;

        throw new ArgumentException(
            $"Invalid comparer '{comparer}'. Must be one of: {string.Join(", ", CanonicalByValue.Keys)} " +
            $"(not equal also accepts '<>').",
            nameof(comparer));
    }

    /// <summary>Returns true when <paramref name="comparer"/> is a known token after normalize.</summary>
    public static bool TryParse(string comparer, out ComparerStrEnum? result)
    {
        var normalized = Normalize(comparer);
        if (Aliases.TryGetValue(normalized, out var match))
        {
            result = match;
            return true;
        }

        result = null;
        return false;
    }

    public static string Normalize(string comparer) =>
        (comparer ?? string.Empty).Trim().ToLowerInvariant();

    public override string ToString() => Value;
}
