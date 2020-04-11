using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using FlyCenterProject;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;

namespace FlyCenterWebAPI.Controllers
{
    [BasicAuthentication]
    public class AirlineCompanyController : ApiController
    {

        private FlyingCenterSystem FlyCenter;
        private LoginToken<AirlineCompany> airlineCompanyLoginToken;
        private void GetLoginToken()
        {
            Request.Properties.TryGetValue("token", out object logintoken);
            airlineCompanyLoginToken = logintoken as LoginToken<AirlineCompany>;
        }

        // Get All Tickets
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AirlineCompanyFacade/GetAllTickets")]
        [HttpGet]
        public IHttpActionResult GetAllTickets()
        {
            GetLoginToken();
            if (airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineCompanyFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            IList<Ticket> tickets = airlineCompanyFacade.GetAllTicketsByAirline(airlineCompanyLoginToken);

            if (tickets.Count == 0)
            {
                return NotFound();
            }
            return Ok(tickets);

        }
        // Get All Flights
        [ResponseType(typeof(Flight))]
        [Route("api/AirlineCompanyFacade/GetAllFlights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            GetLoginToken();
            if (airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            IList<Flight> flights = airlineFacade.GetAllFlightsByAirline(airlineCompanyLoginToken);

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        //Create New Flight
        [ResponseType(typeof(Flight))]
        [Route("api/AirlineCompanyFacade/CreateNewFlight", Name = "createflight")]
        [HttpPost]
        public IHttpActionResult CreateNewFlight([FromBody] Flight flight)
        {
            GetLoginToken();
            if (airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            long flightId = airlineFacade.CreateFlight(airlineCompanyLoginToken, flight);
            return CreatedAtRoute("createFlight", new { id = flightId }, flight);
        }
        //Update Flight
        [ResponseType(typeof(string))]
        [Route("api/AirlineCompanyFacade/UpdateFlight/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateFlight(int id, [FromBody] Flight flight)
        {
            GetLoginToken();
            if (airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            flight = airlineFacade.GetFlightById(airlineCompanyLoginToken, id);
            if (flight == null)
            {
                return BadRequest("Id not Found");
            }
            else
            {
                airlineFacade.UpdateFlight(airlineCompanyLoginToken, flight);
                return Ok($"Flight ID = {id} updated");
            }
        }
        //Change My Password
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AirlineCompanyFacade/ChangeMyPassword/{newPassword}")]
        [HttpPut]
        public IHttpActionResult ChangeMyPassword(string newPassword)
        {
            if (newPassword == string.Empty)
                return Content(HttpStatusCode.NotAcceptable, "new password have not been completed properly");
            try
            {
                ChangeMyPassword(newPassword);
                return Ok($"changed his password");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotFound, $"{e.Message}");
            }

        }
        //Cancel Flight
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AirlineCompanyFacade/CancelFlight/{flightId}")]
        [HttpDelete]
        public IHttpActionResult CancelFlight([FromUri]long flightId)
        {
            GetLoginToken();

            if (flightId <= 0)
                return Content(HttpStatusCode.NotAcceptable, $"{flightId} not valid");
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            IHttpActionResult result = null;
            Flight flight = null;
            try
            {
                flight = airlineFacade.GetFlightById(airlineCompanyLoginToken, flightId);
                if (flight != null)
                {
                    airlineFacade.CancelFlight(airlineCompanyLoginToken, flight);

                }
                return Content(HttpStatusCode.NotAcceptable, $"Flight with ID = {flightId} not found");
            }
            catch (Exception e)
            {
                result = BadRequest("Flight hasn't been  deleted " + e.Message);
            }
            return result;

        }
        //ModifyAirlineDetails
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AirlineCompanyFacade/ModifyAirlineDetails")]
        [HttpPut]
        public IHttpActionResult ModifyAirlineDetails([FromBody] AirlineCompany airlineCompany)
        {
            GetLoginToken();
            if (airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            try
            {
                airlineFacade.ModifyAirlineDetails(airlineCompanyLoginToken, airlineCompany);
                return Ok("Airline company details had been modified");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        //Get Customer By UserName
        [ResponseType(typeof(Customer))]
        [Route("api/AirlineCompanyFacade/GetCustomerByUserName/search")]
        [HttpGet]
        public IHttpActionResult GetCustomerByUserName(string username = "")
        {
            IHttpActionResult result = null;
            Customer customer = null;
            GetLoginToken();
            if(airlineCompanyLoginToken == null)
            {
                return Unauthorized();
            }
            FlyCenter = FlyingCenterSystem.GetInstance();
            ILoggedInAirlineFacade airlineFacade = FlyCenter.GetFacede(airlineCompanyLoginToken) as ILoggedInAirlineFacade;
            IList<Flight> flights = airlineFacade.GetAllFlightsByAirline(airlineCompanyLoginToken);
            if(username != "")
            {
                customer = airlineFacade.GetCustomerByUserName(airlineCompanyLoginToken, username);
                result = Ok(customer);
            }
            else if((username != "" && customer == null) || username == "")
            {
                result  = NotFound();
            }
            return result;
        }
    }
}