/***********************************
 *
 * Copyright	:	© VEXIT® 2013 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2013-04-23
 * Date Updated	:	2026-03-15 - Changed DateUpdated->UpdatedAt and DateCreated->CreatedAt, DateTime->DateTimeOffset
 *                 2026-05-02 - Removed EntityStateEnum EntityState property
 *
 **********************************/

namespace Vexit.DataAccess.Interfaces;

/// <summary>
/// Interface for entities that are auditable.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// The ID of the user who created the entity.
    /// </summary>
    long? CreatorId { get; set; }

    /// <summary>
    /// The date and time the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The ID of the user who updated the entity.
    /// </summary>
    long? UpdaterId { get; set; }

    /// <summary>
    /// The date and time the entity was updated.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; set; }
}
