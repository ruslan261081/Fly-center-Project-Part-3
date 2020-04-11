using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FlyCenterProject;
using System.Text;

namespace FlyCenterWebAPI.Controllers
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
           //Got UserName + Password
           if(actionContext.Request.Headers.Authorization == null)
           {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Please Enter your UserName and Password, in basic autehtication");
                return;
           }
            string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];

            if(username == "admin" && password == "9999")
            {
                ILoginToken LoginUser = FlyingCenterSystem.GetInstance().Login(username, password);
                actionContext.Request.Properties["login-token"] = LoginUser;
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"You are Not Authorized");
            }
        }
    }
}
