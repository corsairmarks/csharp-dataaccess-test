namespace DataAccessTest.Web.Controllers
{
    using System.Web.Http;
    using DataAccessTest.Repository;

    /// <summary>
    /// The base class of <see cref="ApiController"/> implementations that need an <see cref="IUnitOfWork"/>.
    /// </summary>
    public abstract class UnitOfWorkApiController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkApiController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The request-level unit of work.</param>
        public UnitOfWorkApiController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the unit of work for the data store for this request.
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.UnitOfWork != null)
                {
                    this.UnitOfWork.Dispose();
                    this.UnitOfWork = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}