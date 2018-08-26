using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OrchardCore.DisplayManagement.Theming;
using OrchardCore.Themes.Services;

namespace Switcheroo.OrchardCore {
    /// <summary>
    /// Provides the theme provided in query parameter for the current scope (request).
    /// The same <see cref="ThemeSelectorResult"/> is returned if called multiple times
    /// during the same scope.
    /// </summary>
    public class SwitcherooThemeSelector : IThemeSelector {
        public const string RequiredRole = "Administrator";
        public const string CookieName = "theme";
        public const string QueryStringKey = "theme";
        public const int Priority = 50;

        private readonly ISiteThemeService _siteThemeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SwitcherooThemeSelector(ISiteThemeService siteThemeService,
            IHttpContextAccessor httpContextAccessor) {
            _siteThemeService = siteThemeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ThemeSelectorResult> GetThemeAsync() {
            var context = _httpContextAccessor.HttpContext;

            if (!context.User.IsInRole(RequiredRole))
                return null;

            var currentThemeName = await _siteThemeService.GetCurrentThemeNameAsync();
            if (string.IsNullOrEmpty(currentThemeName))
                return null;

            // Try cookies.
            context.Request.Cookies.TryGetValue(CookieName, out var themeName);

            // Try query string. Query string takes precedence.
            context.Request.Query.TryGetValue(QueryStringKey, out var values);
            if (!StringValues.IsNullOrEmpty(values))
                themeName = values.First();

            if (string.IsNullOrWhiteSpace(themeName))
                return null;

            // Non-persistent cookie. Available only during session.
            context.Response.Cookies.Append(CookieName, themeName);

            // Priority should override current theme priority, but not admin theme priority.
            // Admin Theme Priority = 100
            // Current Theme Priority = 0
            return new ThemeSelectorResult {
                Priority = Priority,
                ThemeName = themeName
            };
        }
    }
}