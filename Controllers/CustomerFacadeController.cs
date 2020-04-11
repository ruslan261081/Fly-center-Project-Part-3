using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FlyCenterProject;

namespace FlyCenterWebAPI.Controllers
{
    [BasicAuthentication]
    public class CustomerFacadeController : ApiController
    {
        private FlyingCenterSystem FlyingCenter;
        private LoginToken<Customer> customerLoginToken;

        private void GetLoginToken()
        {
            Request.Properties.TryGetValue("token", out object logintoken);
            customerLoginToken = logintoken as LoginToken<Customer>;
        }
        //Get All Customer Flights
        [ResponseType(typeof(Flight))]
        [Route("api/CustomerFacade/GetAllMyFlights")]
        [HttpGet]
        public IHttpActionResult GetAllMyFlights()
        {
            IList<Flight> flights = null;

            GetLoginToken();

            if (customerLoginToken == null)
            {
                return Unauthorized();
            }
            FlyingCenter = FlyingCenterSystem.GetInstance();
            ILoggedInCustomerFacade customerFacade = FlyingCenter.GetFacede(customerLoginToken) as ILoggedInCustomerFacade;
            if (flights.Count > 0)
                return Ok(flights);
            else
                return StatusCode(HttpStatusCode.NotFound);

        }
        // PurchaseTicket
        [ResponseType(typeof(Ticket))]
        [Route("api/CustomerFacade/PurchaseTicket")]
        [HttpPost]
        public IHttpActionResult PurchaseTicket([FromBody] Flight flight)
        {
            GetLoginToken();
            if (customerLoginToken == null)
            {
                return Unauthorized();
            }
            FlyingCenter = FlyingCenterSystem.GetInstance();
            ILoggedInCustomerFacade customerFacade = FlyingCenter.GetFacede(customerLoginToken) as ILoggedInCustomerFacade;

            try
            {
                Ticket ticket = customerFacade.PurchaseTicket(customerLoginToken, flight);
                return CreatedAtRoute("PurchaseTicket", new { id = ticket.ID }, ticket);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //Remove Ticket
        [ResponseType(typeof(Ticket))]
        [Route("api/CustomerFacade/RemoveTicket/{customerId}")]
        [HttpDelete]
        public IHttpActionResult RemoveTicket(int customerId)
        {

            Ticket ticket = null;
            GetLoginToken();
            if (customerId > 0)
            {

                if (customerLoginToken == null)
                {
                    return Unauthorized();
                }
                FlyingCenter = FlyingCenterSystem.GetInstance();
                ILoggedInCustomerFacade customerFacade = FlyingCenter.GetFacede(customerLoginToken) as ILoggedInCustomerFacade;

                if (ticket != null)
                {
                    ((ILoggedInCustomerFacade)customerFacade).CancelTicket((LoginToken<Customer>)customerLoginToken, ticket);
                    return Ok($"{ticket} canceles by {((LoginToken<Customer>)customerLoginToken).User.UserName}");
                }
                else
                {
                    return NotFound();
                }
            }
            return Content(HttpStatusCode.NotAcceptable, $"{customerId} is not valid");

        }
    }
}
