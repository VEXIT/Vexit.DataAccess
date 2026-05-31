/**********************************************************************
 *
 * Copyright	:	© VEXIT® 2026, www.vexit.com, Tomorrow is today...
 * Author		:	Vex Tatarevic
 * Date Created	:	2026-05-02
 * Date Updated	:	
 *                 
 **********************************************************************/

namespace Vexit.DataAccess.Interfaces;

/// <summary>
/// Interface for entities that can be soft deleted. <br />
/// IMPORTANT: VModBase.DbContext_OnModelCreating method AUTOMATICALLY applies the soft delete filter to all the entities that implement the ISoftDeletable interface.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// The date and time the entity was deleted.
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}