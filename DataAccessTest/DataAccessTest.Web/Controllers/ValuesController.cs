namespace DataAccessTest.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;

    public class ValuesController : ApiController
    {
        private readonly IGenericRepository<ValueModel> valuesRepository;

        public ValuesController(IGenericRepository<ValueModel> valuesRepository)
        {
            this.valuesRepository = valuesRepository;
        }

        // GET api/values
        public IEnumerable<ValueModel> Get()
        {
            return this.valuesRepository.GetAll();
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            var valueModel = valuesRepository.FindBy(vm => vm.Id == id).SingleOrDefault();
            if (valueModel != null)
            {
                return this.Request.CreateResponse(valueModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]ValueModel value)
        {
            var valueModel = valuesRepository.FindBy(vm => vm.Id == value.Id).SingleOrDefault();
            if (valueModel == null)
            {
                this.valuesRepository.Create(value);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }

            return this.Request.CreateResponse(HttpStatusCode.Conflict);
        }

        // PUT api/values/5
        public HttpResponseMessage Put([FromBody]ValueModel value)
        {
            var valueModel = valuesRepository.FindBy(vm => vm.Id == value.Id).SingleOrDefault();
            if (valueModel != null)
            {
                return this.Request.CreateResponse(valueModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            var valueModel = valuesRepository.FindBy(vm => vm.Id == id).SingleOrDefault();
            if (valueModel != null)
            {
                this.valuesRepository.Delete(valueModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
