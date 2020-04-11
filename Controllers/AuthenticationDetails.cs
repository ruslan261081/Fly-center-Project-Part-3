using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyCenterProject;


namespace FlyCenterWebAPI.Controllers
{
    public class AuthenticationDetails
    {
        public LoginToken<Administrator> admin { get; set; }
        public LoggedInAdministratorFacade administratorFacade { get; set; }
        public LoginToken<AirlineCompany> airline { get; set; }
        public LoggedInAirlineFacade airlineFacade { get; set; }
        public LoginToken<Customer> customer { get; set; }
        public LoggedInCustomerFacade customerFacade { get; set; }

        public AuthenticationDetails()
        {
            admin = new LoginToken<Administrator>();
            administratorFacade = new LoggedInAdministratorFacade();
            airline = new LoginToken<AirlineCompany>();
            airlineFacade = new LoggedInAirlineFacade();
            customer = new LoginToken<Customer>();
            customerFacade = new LoggedInCustomerFacade();
        }

    }
}