/***********************************
 *
 * Copyright	:	© VEXIT® 2026, www.vexit.com, Tomorrow is today...
 * Author		:	Vex Tatarevic
 * Date Created	:	2026-05-02
 * Date Updated	:	
 *                 
 **********************************/

namespace Vexit.DataAccess.Interfaces;

/// <summary>
/// Interface for entities that have a status.
/// </summary>
/// <typeparam name="TStatus">The type of the status.</typeparam>
public interface IHasStatus<TStatus>
{
    TStatus Status { get; set; }
}