using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicTokenSample.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //split the username. Here I am doing it against my machine, hence I just need the username. If AD is required, switch to Domain.
            var user = context.UserName.Split('\\')[1];

            using (var pc = new PrincipalContext(ContextType.Machine))
            {
                if (pc.ValidateCredentials(user, context.Password))
                {
                    //generate claims and add it to claimsidentity object
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                            "pbi:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                        },
                        {
                            "pbi:userName", context.UserName
                        }
                    });

                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                }
            }
        }
    }
}