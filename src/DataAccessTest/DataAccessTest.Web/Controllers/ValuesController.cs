namespace DataAccessTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;

    /// <summary>
    /// A sample value editing controller.
    /// </summary>
    public class ValuesController : UnitOfWorkApiController
    {
        /// <summary>
        /// The values repository.
        /// </summary>
        private readonly IGenericRepository<ValueModel> valuesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The request-level unit of work.</param>
        /// <param name="valuesRepository">The values repository.</param>
        public ValuesController(IUnitOfWork unitOfWork, IGenericRepository<ValueModel> valuesRepository)
            : base(unitOfWork)
        {
            this.valuesRepository = valuesRepository;
        }

        /// <summary>
        /// Get all of the existing values.
        /// </summary>
        /// <returns>The collection of values.</returns>
        public IEnumerable<ValueModel> Get()
        {
            return this.valuesRepository.GetAll();
        }

        /// <summary>
        /// Retrieve an existing value by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The response message.</returns>
        public HttpResponseMessage Get(int id)
        {
            var valueModel = this.valuesRepository.FindBy(vm => vm.Id == id).SingleOrDefault();
            if (valueModel != null)
            {
                return this.Request.CreateResponse(valueModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Create a new value.
        /// </summary>
        /// <param name="value">The updated information.</param>
        /// <returns>The response message.</returns>
        public HttpResponseMessage Post([FromBody]ValueModel value)
        {
            var valueModel = this.valuesRepository.FindBy(vm => vm.Id == value.Id).SingleOrDefault();
            if (valueModel == null)
            {
                this.valuesRepository.Create(value);
                this.UnitOfWork.Complete();
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }

            return this.Request.CreateResponse(HttpStatusCode.Conflict);
        }

        /// <summary>
        /// Update a value based on the request body.
        /// </summary>
        /// <param name="value">The updated information.</param>
        /// <returns>The response message.</returns>
        public HttpResponseMessage Put([FromBody]ValueModel value)
        {
            var valueModel = this.valuesRepository.FindBy(vm => vm.Id == value.Id).SingleOrDefault();
            if (valueModel != null)
            {
                valueModel.Description = value.Description;
                this.valuesRepository.Update(valueModel);
                this.UnitOfWork.Complete();
                return this.Request.CreateResponse(valueModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Delete a value by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier of the value to delete.</param>
        /// <returns>The response message.</returns>
        public HttpResponseMessage Delete(int id)
        {
            var valueModel = this.valuesRepository.FindBy(vm => vm.Id == id).SingleOrDefault();
            if (valueModel != null)
            {
                this.valuesRepository.Delete(valueModel);
                this.UnitOfWork.Complete();
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}