/*************************************************************
 * 
 *  Copyright       :   © VEXIT® 2021
 *  Company         :   VEXIT Pty Ltd
 *  URL             :   www.vexit.com
 *  
 *  Author          :   Vex Tatarevic
 *  Date Created    :   2021-03-21
 *  
 *  Description     :   Base class used by DataSeeder classes that insert initial data to database during the system set-up phase.
 *  
 *  Updates         :    
 *  
 ************************************************************/

using Vexit.DataAccess.Interfaces;

namespace Vexit.DataAccess.BaseClasses;


/// <summary>
///     Base class used by DataSeeder classes that insert initial data to database during the system set-up phase.
///     NOTE: You must inject DBContext into constructor of the inheriting class.
/// </summary>
/// <example>
///     <code>
///         public class EncryptionSettingsDataSeeder : DataSeederBase
///         {
///             
///             public EncryptionSettingsDataSeeder(DBContext ctx) : base(ctx)
///             {
///             
///             }
///         
///             public override Task Seed()
///             {
///                 throw new NotImplementedException();
///             }
///         }
///     </code>
/// </example>
public abstract class DataSeederBase<TDbContext> where TDbContext : IDbContext
{

    //------------------------------------------------------------
    //  Fields
    //------------------------------------------------------------

    /// <summary>
    ///  Database context instance variable. Use this to make LINQ queries to database.
    /// </summary>
    protected readonly TDbContext _ctx;




    //------------------------------------------------------------
    //  Constructor
    //------------------------------------------------------------

    public DataSeederBase(TDbContext ctx)
    {
        _ctx = ctx;
    }



    //------------------------------------------------------------
    //  Methods
    //------------------------------------------------------------

    /// <summary>
    ///  Should implement inserting seed-data into database, using _ctx (database context instance) variable
    /// </summary>
    /// <returns></returns>
    public abstract Task Seed();

}
