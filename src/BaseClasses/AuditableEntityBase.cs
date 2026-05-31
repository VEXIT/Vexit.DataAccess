/***********************************
 *
 * Copyright	:	© VEXIT® 2025 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2025-09-05
 *
 **********************************/



using Vexit.DataAccess.Interfaces;

namespace Vexit.DataAccess.BaseClasses;

/// <summary>
///  Base class that implements the IAuditable and ISoftDeletable interfaces.
/// </summary>
public abstract class AuditableEntityBase : IAuditable, ISoftDeletable
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public long? CreatorId { get; set; }
    public long? UpdaterId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }


    protected AuditableEntityBase()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = null;
        DeletedAt = null;
    }
}
