/***********************************
 *
 * Copyright	:	© VEXIT® 2025 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2025-09-05
 *
 **********************************/


using Vexit.DataAccess.Enums;

namespace Vexit.DataAccess.BaseClasses;

/// <summary>
///  Base class that is used by Entity Framework Entity classes. <br></br>
///  It adds frequently used standard properties: Id, DateCreated, DateUpdated, CreatorId, UpdaterId, EntityState
/// </summary>
public abstract class EntityBase
{
    public long Id { get; set; }

    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public long CreatorId { get; set; }
    public long UpdaterId { get; set; }

    public EntityStateEnum EntityState { get; set; } = EntityStateEnum.Active; 

    protected EntityBase()
    {
        DateCreated = DateTime.UtcNow;
        DateUpdated = DateTime.UtcNow;
    }
}
