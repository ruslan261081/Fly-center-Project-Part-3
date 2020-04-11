using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Web.Http.Description;
using FlyCenterProject;


namespace FlyCenterWebAPI.Controllers
{
    [BasicAuthentication]

    public class AdministratorController : ApiController
    {
        private AuthenticationDetails _authentication;


        public AdministratorController()
        {
            _authentication = new AuthenticationDetails();
        }

        //PostAirlineCompany: api/AdministratorFacade/CreateNewAirline
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/administratorFacade/AddNewAirline")]
        [HttpPost]
        public IHttpActionResult CreateNewAirline([FromBody] AirlineCompany airline)
        {
            GetLoginToken();
            if (airline == null)
                return Content(HttpStatusCode.NotAcceptable, "You didn't send airline to post");
            try
            {
                _authentication.administratorFacade.CreateNewAirline(_authentication.admin, airline);
                return Ok($"{airline} Added by {_authentication.admin.User.UserName}");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }

        //UpdateAirlineCompany: api/AdministratorFacade/UpdateAirlineDetails
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/administratorFacade/UpdateDetails")]
        [HttpPut]
        public IHttpActionResult UpdateAirlineDetails([FromBody]AirlineCompany airlne)
        {
            GetLoginToken();
            if (airlne == null || airlne.ID == 0)
                return Content(HttpStatusCode.NotAcceptable, $"{airlne} ");
            try
            {
                _authentication.administratorFacade.UpdateAirlineDetails(_authentication.admin, airlne);
                return Ok($"{airlne} update by {_authentication.admin.User.UserName}");
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.NotFound, $"ID {airlne.ID} was not found");
            }


        }

        //RemoveAirlineCompany: api/AdministratorFacade/RemoveAirlinecompany
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AdministartorFacade/RemoveAirline/{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveAirline(int id)
        {
            GetLoginToken();
            if (id <= 0)
                return Content(HttpStatusCode.NotAcceptable, "Id is not correct");
            AirlineCompany airline = _authentication.administratorFacade.GetAllAirlineCompanies().ToList().Find(a => a.ID == id);
            if (airline == null)
                return Content(HttpStatusCode.NotFound, $"{id} not found");
            _authentication.administratorFacade.RemoveAirline(_authentication.admin, airline);
            return Ok($"{airline} deleted by {_authentication.admin.User.UserName}");

        }

        //PostCustomer: api/AdministratorFacade/Createcustomer
        [ResponseType(typeof(Customer))]
        [Route("api/administratorFacade/CreateNewCustomer")]
        [HttpPost]
        public IHttpActionResult CreateNewCustomer([FromBody] Customer customer)
        {
            GetLoginToken();
            if (customer == null)
                return Content(HttpStatusCode.NotAcceptable, "Send customer to Post");
            try
            {
                _authentication.administratorFacade.CreateNewCustomer(_authentication.admin, customer);
                return Ok($"{customer} Added by {_authentication.admin.User.UserName}");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }

        //UpdateCustomer: api/AdministratorFacade/UpdateCustomer
        [ResponseType(typeof(Customer))]
        [Route("api/administratorFacade/UpdateCustomer")]
        [HttpPut]
        public IHttpActionResult UpdateCustomerDetails([FromBody]Customer customer)
        {
            GetLoginToken();
            if (customer == null || customer.Id <= 0)
                return Content(HttpStatusCode.NotAcceptable, $"{customer} details have not been completed");
            try
            {
                _authentication.administratorFacade.UpdateCustomer(_authentication.admin, customer);
                return Ok($"{customer} update by {_authentication.admin.User.UserName}");
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.NotFound, $"ID{customer.Id} was not found");
            }
        }

        //RemoveCustomer: api/AdministratorFacade/RemoveCustomer
        [ResponseType(typeof(Customer))]
        [Route("api/administratorFacade/RemoveCustomer/{id}")]
        [HttpDelete]
        public IHttpActionResult RemoveCustomer(int id)
        {
            GetLoginToken();
            if (id <= 0)
                return Content(HttpStatusCode.NotAcceptable, "Id is not valid");
            Customer customer = _authentication.administratorFacade.GetAllCustomer().ToList().Find(c => c.Id == id);
            if (customer == null)
                return Content(HttpStatusCode.NotFound, $"{id} was not found");
            _authentication.administratorFacade.RemoveCustomer(_authentication.admin, customer);
            return Ok($"{customer} Remove by {_authentication.admin.User.UserName}");

        }


        private AuthenticationDetails GetLoginToken()
        {
            Request.Properties.TryGetValue("AdminUser", out object token);
            Request.Properties.TryGetValue("AdminFacade", out object facade);
            LoginToken<Administrator> adminToken = (LoginToken<Administrator>)token;
            LoggedInAdministratorFacade adminFacede = (LoggedInAdministratorFacade)facade;
            _authentication.admin = adminToken;
            _authentication.administratorFacade = adminFacede;
            return _authentication;
        }

    }
}