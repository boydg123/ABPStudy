using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Hangfire;
using Abp.Owin;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Twitter;
using Microsoft.Owin.Security.WsFederation;
using Derrick.Authorization;
using Derrick.Authorization.Users;
using Derrick.Configuration;
using Derrick.Web;
using Derrick.Web.Auth;
using Derrick.WebApi.Controllers;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Derrick.Web
{
    public class Startup
    {
        private static ISettingManager _settingManager;

        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.RegisterDataProtectionProvider();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //You can remove these lines if you don't like to use two factor auth (while it has no problem if you don't remove)
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            if (IsTrue("ExternalAuth.Facebook.IsEnabled"))
            {
                app.UseFacebookAuthentication(CreateFacebookAuthOptions());
            }

            if (IsTrue("ExternalAuth.Twitter.IsEnabled"))
            {
                app.UseTwitterAuthentication(CreateTwitterAuthOptions());
            }

            if (IsTrue("ExternalAuth.Google.IsEnabled"))
            {
                app.UseGoogleAuthentication(CreateGoogleAuthOptions());
            }

            if (IsTrue("ExternalAuth.WsFederation.IsEnabled"))
            {
                app.UseWsFederationAuthentication(CreateWsFederationAuthOptions());
            }

            if (IsTrue("ExternalAuth.OpenId.IsEnabled"))
            {
                app.UseOpenIdConnectAuthentication(CreateOpenIdOptions());
            }

            app.MapSignalR();

            //Enable it to use HangFire dashboard (uncomment only if it's enabled in AbpZeroTemplateWebModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
            //});
        }

        private static OpenIdConnectAuthenticationOptions CreateOpenIdOptions()
        {
            _settingManager = IocManager.Instance.Resolve<ISettingManager>();
            var websiteAddress = _settingManager.GetSettingValue(AppSettings.General.WebSiteRootAddress);

            var options = new OpenIdConnectAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["ExternalAuth.OpenId.Authority"],
                ClientId = ConfigurationManager.AppSettings["ExternalAuth.OpenId.ClientId"],
                PostLogoutRedirectUri = websiteAddress + "Account/Logout",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        var email = notification.AuthenticationTicket.Identity.Name;
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email, email));
                        return Task.FromResult(0);
                    }
                }
            };

            var clientSecret = ConfigurationManager.AppSettings["ExternalAuth.OpenId.ClientSecret"];
            if (!clientSecret.IsNullOrEmpty())
            {
                options.ClientSecret = clientSecret;
            }

            return options;
        }

        private static FacebookAuthenticationOptions CreateFacebookAuthOptions()
        {
            var options = new FacebookAuthenticationOptions
            {
                AppId = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppId"],
                AppSecret = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppSecret"]
            };

            options.Scope.Add("email");
            options.Scope.Add("public_profile");

            return options;
        }

        private static TwitterAuthenticationOptions CreateTwitterAuthOptions()
        {
            var consumerKey = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerSecret"];

            return new TwitterAuthenticationOptions
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Provider = new TwitterAuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("urn:twitter:access_token", context.AccessToken));
                        context.Identity.AddClaim(new Claim("urn:twitter:access_secret", context.AccessTokenSecret));

                        var emailRetriever = new TwitterEmailRetriever();
                        var email = emailRetriever.Get(context.AccessToken, context.AccessTokenSecret, consumerKey, consumerSecret);
                        context.Identity.AddClaim(new Claim(ClaimTypes.Email, email));

                        return Task.FromResult(0);
                    }
                },
                BackchannelCertificateValidator = new Microsoft.Owin.Security.CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "‎add53f6680fe66e383cbac3e60922e3b4c412bed", // Symantec Class 3 EV SSL CA - G3
                    "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                    "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                })
            };
        }

        private static GoogleOAuth2AuthenticationOptions CreateGoogleAuthOptions()
        {
            return new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientSecret"]
            };
        }

        private static WsFederationAuthenticationOptions CreateWsFederationAuthOptions()
        {
            _settingManager = IocManager.Instance.Resolve<ISettingManager>();
            var websiteAddress = _settingManager.GetSettingValue(AppSettings.General.WebSiteRootAddress);

            var wtrealm = ConfigurationManager.AppSettings["ExternalAuth.WsFederation.Wtrealm"];
            var metaDataAddress = ConfigurationManager.AppSettings["ExternalAuth.WsFederation.MetaDataAddress"];

            return new WsFederationAuthenticationOptions
            {
                Wtrealm = wtrealm,
                MetadataAddress = metaDataAddress,
                AuthenticationType = "adfs",
                Notifications = new WsFederationAuthenticationNotifications
                {
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.IsSignOutMessage)
                        {
                            notification.HandleResponse();
                        }

                        notification.ProtocolMessage.Wreply = websiteAddress.EnsureEndsWith('/') + "Account/ExternalLoginCallback";

                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = notification =>
                    {
                        var email = notification.AuthenticationTicket.Identity.Name;
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email, email));
                        return Task.FromResult(0);
                    }
                }
            };
        }

        private static bool IsTrue(string appSettingName)
        {
            return string.Equals(
                ConfigurationManager.AppSettings[appSettingName],
                "true",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}