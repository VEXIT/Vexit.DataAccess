/***********************************
 *
 * Copyright	:	© VEXIT® 2013 - www.vexit.com
 * Author		:	Vex Tatarevic
 * Date Created	:	2013-04-23
 * Date Updated	:	2026-03-15 - Smart enum (EntityStateSE) added
 *
 **********************************/

using Vexit.Common.BaseClasses;

namespace Vexit.DataAccess.Enums;

/// <summary>
/// Smart enum wrapper for <see cref="EntityStateEnum"/> providing string helpers.
/// Instances are auto-generated from enum values.
/// </summary>
public sealed class EntityStateSE : SmartEnumBase<EntityStateEnum, EntityStateSE>
{
    public static EntityStateSE Active => FromEnum(EntityStateEnum.Active);
    public static EntityStateSE Inactive => FromEnum(EntityStateEnum.Inactive);
    public static EntityStateSE Deleted => FromEnum(EntityStateEnum.Deleted);

    private EntityStateSE(EntityStateEnum value, string name) : base(value, name) { }
}

/// <summary>
/// Entity lifecycle state (e.g. for soft delete / active flag).
/// </summary>
public enum EntityStateEnum
{
    Active = 1,
    Inactive = 2,
    Deleted = 3
}
