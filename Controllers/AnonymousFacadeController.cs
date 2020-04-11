using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyCenterProject;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;

namespace FlyCenterWebAPI.Controllers
{
    public class AnonymousFacadeController : ApiController
    {
        private FlyingCenterSystem FlyingCenter;

        //Get all flights
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/AllFlights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousUserFacade.GetAllFlights();
            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        //Get All AirlineCompanies
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/AnonymousFacade/GetAllAirlineCompanies")]
        [HttpGet]
        public IHttpActionResult GetAllAirlinesCompanies()
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<AirlineCompany> airlineCompanies = anonymousUserFacade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0)
            {
                return NotFound();
            }
            return Ok(airlineCompanies);
        }

        //Get flights by vacancy
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetAllFlightsVacancy/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            Dictionary<Flight, int> flights = anonymousUserFacade.GetAllFlightsVacancy();

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        //Get flight by id
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetFlightById/{id}")]
        [HttpGet]
        public IHttpActionResult GetFlightById(int flightId)
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            Flight flight = anonymousUserFacade.GetFlightById(flightId);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
        //Get flihgts by origin country code
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetFlightsByOriginCountryCode/{countryCode}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountryCode(int countryCode)
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousUserFacade.GetFlightsByOriginCountry(countryCode);

            if (flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        // Get flights by destination country code
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetFlightsByDestinationCountry/{countryCode}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountry(int countryCode)
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousUserFacade.GetFlightsByDestinationCountry(countryCode);

            if (countryCode == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }

        //Get Flights By Departure Date
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetFlightsByDepartureDate/{departureDate}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDepartureDate(DateTime departureDate)
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousUserFacade.GetFlightsByDepatruteDate(departureDate);

            if (flights.Count == 0)
            {
                return NotFound();

            }
            return Ok(flights);
        }
        // Get Flights By Landing Date
        [ResponseType(typeof(Flight))]
        [Route("api/AnonymousFacade/GetFlightsByLandingDate/{landingDate}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            FlyingCenter = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade anonymousUserFacade = FlyingCenter.GetFacede(null) as IAnonymousUserFacade;
            IList<Flight> flights = anonymousUserFacade.GetFlightsByLandingDate(landingDate);

            if(flights.Count == 0)
            {
                return NotFound();
            }
            return Ok(flights);
        }
    }
}