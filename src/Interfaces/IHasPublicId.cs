/***********************************
 *
 * Copyright	:	© VEXIT® 2026 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2026-04-10
 *
 **********************************/

namespace Vexit.DataAccess.Interfaces;

/// <summary>
/// Interface for entities that have a public identifier.
/// </summary>
public interface IHasPublicId
{
    /// <summary>
    /// The public identifier of the entity.
    /// </summary>
    Guid PublicId { get; init; }
}