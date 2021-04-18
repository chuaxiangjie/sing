using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using sing.Models;

namespace sing
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            //    ClientId = "759634598595-qhvii1hl33c885o9cedta8k64p6ghojc.apps.googleusercontent.com",
            //    ClientSecret = "6ic7_wG1O9_7yoM0tV-yEFrx"

            IdentityModelEventSource.ShowPII = true;

            app.UseOpenIdConnectAuthentication(
             new OpenIdConnectAuthenticationOptions
             {
                // Sets the ClientId, authority, RedirectUri as obtained from web.config
                 ClientId = "759634598595-qhvii1hl33c885o9cedta8k64p6ghojc.apps.googleusercontent.com",
                 
                 Authority = "https://accounts.google.com",
                 ClientSecret = "6ic7_wG1O9_7yoM0tV-yEFrx",
                 RedirectUri = "https://localhost:44312",
                 Scope = OpenIdConnectScope.OpenIdProfile,
                 AuthenticationMode = AuthenticationMode.Active,
                 RedeemCode = true,
                 SaveTokens = true,
                // ResponseType is set to request the code id_token - which contains basic information about the signed-in user
                ResponseType = OpenIdConnectResponseType.Code,
                // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
                // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter
                TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = false // This is a simplification
                },
           
                // OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
                Notifications = new OpenIdConnectAuthenticationNotifications
                 {
                     AuthenticationFailed = OnAuthenticationFailed
                 }
             }
         );


            app.UseGoogleAuthentication(
        clientId: "759634598595-qhvii1hl33c885o9cedta8k64p6ghojc.apps.googleusercontent.com",
        clientSecret: "6ic7_wG1O9_7yoM0tV-yEFrx");
        }

        /// <summary>
        /// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }
    }
}