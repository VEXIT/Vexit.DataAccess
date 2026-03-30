/***********************************
 *
 * Copyright	:	© VEXIT® 2013 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2013-04-23
 * Date Updated	:	2026-03-15 - Changed DateUpdated->UpdatedAt and DateCreated->CreatedAt, DateTime->DateTimeOffset
 *
 **********************************/

using Vexit.DataAccess.Enums;

namespace Vexit.DataAccess.Interfaces;

public interface IAuditable
{

    EntityStateEnum EntityState { get; set; }

    long? CreatorId { get; set; }

    DateTimeOffset CreatedAt { get; set; }

    long? UpdaterId { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }
}
