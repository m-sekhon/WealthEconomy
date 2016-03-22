//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace forCrowd.WealthEconomy.WebApi.Controllers.OData
{
    using forCrowd.WealthEconomy.BusinessObjects;
    using forCrowd.WealthEconomy.Facade;
    using Results;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.OData;

    public abstract class BaseUsersController : BaseODataController
    {
        public BaseUsersController()
		{
			MainUnitOfWork = new UserUnitOfWork();		
		}

		protected UserUnitOfWork MainUnitOfWork { get; private set; }

        // GET odata/User
        //[Queryable]
        public virtual IQueryable<User> Get()
        {
			var list = MainUnitOfWork.AllLive;
            return list;
        }

        // GET odata/User(5)
        //[Queryable]
        public virtual SingleResult<User> Get([FromODataUri] int id)
        {
            return SingleResult.Create(MainUnitOfWork.AllLive.Where(user => user.Id == id));
        }

        // PUT odata/User(5)
        public virtual async Task<IHttpActionResult> Put([FromODataUri] int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await MainUnitOfWork.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await MainUnitOfWork.All.AnyAsync(item => item.Id == user.Id))
                {
                    return Conflict();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok(user);
        }

        // POST odata/User
        public virtual async Task<IHttpActionResult> Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await MainUnitOfWork.InsertAsync(user);
            }
            catch (DbUpdateException)
            {
                if (await MainUnitOfWork.All.AnyAsync(item => item.Id == user.Id))
                {
					return new UniqueKeyConflictResult(Request, "Id", user.Id.ToString());
                }
                else throw;
            }

            return Created(user);
        }

        // PATCH odata/User(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] int id, Delta<User> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await MainUnitOfWork.AllLive.SingleOrDefaultAsync(item => item.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var patchEntity = patch.GetEntity();

            // TODO How is passed ModelState.IsValid?
            if (patchEntity.RowVersion == null)
                throw new InvalidOperationException("RowVersion property of the entity cannot be null");

            if (!user.RowVersion.SequenceEqual(patchEntity.RowVersion))
            {
                return Conflict();
            }

            patch.Patch(user);

            try
            {
                await MainUnitOfWork.UpdateAsync(user);
            }
            catch (DbUpdateException)
            {
                if (patch.GetChangedPropertyNames().Any(item => item == "Id"))
                {
                    object idObject = null;
                    patch.TryGetPropertyValue("Id", out idObject);

                    if (idObject != null && await MainUnitOfWork.All.AnyAsync(item => item.Id == (int)idObject))
                    {
                        return new UniqueKeyConflictResult(Request, "Id", idObject.ToString());
                    }
                    else throw;
                }
                else throw;
            }

            return Ok(user);
        }

        // DELETE odata/User(5)
        public virtual async Task<IHttpActionResult> Delete([FromODataUri] int id)
        {
            var user = await MainUnitOfWork.AllLive.SingleOrDefaultAsync(item => item.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            await MainUnitOfWork.DeleteAsync(user.Id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }

    public partial class UsersController : BaseUsersController
    {
	}
}
