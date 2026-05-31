/***********************************
 *
 * Copyright	:	© VEXIT® 2026 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2026-04-10
 *
 **********************************/

using Vexit.DataAccess.Interfaces;

namespace Vexit.DataAccess.BaseClasses;

/// <summary>
/// Base class for entities that have a public identifier.
/// </summary>
public abstract class PublicEntityBase : IHasPublicId
{
    public long Id { get; set; } // Internal Primary Key (Clustered)

    /// <summary>
    /// The public identifier of the entity. <br />
    /// Automatically generated <br />
    /// Uses UUID v7 for better index performance if on .NET 9+
    /// </summary>
    public Guid PublicId { get; init; } = CreatePublicId(); 

    /// <summary>
    /// Creates a new public identifier.
    /// </summary>
    /// <returns>A new public identifier.</returns>
    public static Guid CreatePublicId() => Guid.CreateVersion7();
}