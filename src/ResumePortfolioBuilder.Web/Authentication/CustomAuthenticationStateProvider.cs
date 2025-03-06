using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ResumePortfolioBuilder.Web.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _storageKey = "user_session";
        private UserSession _currentUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _storageKey);
                
                if (string.IsNullOrEmpty(userSession))
                {
                    return CreateAnonymousState();
                }

                _currentUser = System.Text.Json.JsonSerializer.Deserialize<UserSession>(userSession);
                
                // Create claims for the authenticated user
                var claimsPrincipal = GetClaimsPrincipalFromUser(_currentUser);
                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return CreateAnonymousState();
            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                // Store user session in local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _storageKey, 
                    System.Text.Json.JsonSerializer.Serialize(userSession));
                
                _currentUser = userSession;
                claimsPrincipal = GetClaimsPrincipalFromUser(userSession);
            }
            else
            {
                // Remove user session from local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _storageKey);
                
                _currentUser = null;
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private ClaimsPrincipal GetClaimsPrincipalFromUser(UserSession userSession)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userSession.Username),
            }, "apiauth_type");

            return new ClaimsPrincipal(identity);
        }

        private AuthenticationState CreateAnonymousState()
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
} 