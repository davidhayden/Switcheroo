using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;
using OrchardCore.DisplayManagement.Theming;
using OrchardCore.Themes.Services;
using Xunit;

namespace Switcheroo.OrchardCore.Tests {
    public class SwitcherooThemeSelectorTests {
        private const string CurrentActiveTheme = "Editorial";

        [Theory]
        [InlineData("Author")]
        [InlineData("Contributor")]
        [InlineData("Editor")]
        [InlineData("Moderator")]
        [InlineData(SwitcherooThemeSelector.RequiredRole)]
        public async void OnlyRequiredRoleCanChangeTheme(string userRole) {
            // Arrange
            const string selectedThemeName = "TheAgencyTheme";

            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
                {{SwitcherooThemeSelector.QueryStringKey, new StringValues(selectedThemeName)}});
            var requestCookieCollection = new RequestCookieCollection();
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(userRole))
                .Returns((string p) => p.Equals(SwitcherooThemeSelector.RequiredRole));
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(CurrentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            if (userRole.Equals(SwitcherooThemeSelector.RequiredRole)) {
                Assert.NotNull(result);
                Assert.Equal(selectedThemeName, result.ThemeName);
                Assert.Equal(SwitcherooThemeSelector.Priority, result.Priority);
                Assert.Single(responseCookies.Cookies);
                Assert.Equal(SwitcherooThemeSelector.CookieName, responseCookies.Cookies.First().Key);
                Assert.Equal(selectedThemeName, responseCookies.Cookies.First().Value);
            }
            else {
                Assert.Null(result);
                Assert.Empty(responseCookies.Cookies);
            }
        }

        [Theory]
        [InlineData("TheBlogTheme")]
        [InlineData("ComingSoon")]
        [InlineData("Editorial")]
        [InlineData("Freelancer")]
        [InlineData("SafeMode")]
        [InlineData(null)]
        [InlineData("")]
        public async void CurrentActiveThemeRequired(string currentActiveTheme) {
            // Arrange
            const string selectedThemeName = "TheAgencyTheme";

            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
                {{SwitcherooThemeSelector.QueryStringKey, new StringValues(selectedThemeName)}});
            var requestCookieCollection = new RequestCookieCollection();
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(SwitcherooThemeSelector.RequiredRole)).Returns(true);
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(currentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            if (!string.IsNullOrWhiteSpace(currentActiveTheme)) {
                Assert.NotNull(result);
                Assert.Equal(selectedThemeName, result.ThemeName);
                Assert.Equal(SwitcherooThemeSelector.Priority, result.Priority);
                Assert.Single(responseCookies.Cookies);
                Assert.Equal(SwitcherooThemeSelector.CookieName, responseCookies.Cookies.First().Key);
                Assert.Equal(selectedThemeName, responseCookies.Cookies.First().Value);
            }
            else {
                Assert.Null(result);
                Assert.Empty(responseCookies.Cookies);
            }
        }

        [Fact]
        public async void SelectorResultNullWhenNoQueryParameterOrCookie() {
            // Arrange
            var queryCollection = new QueryCollection(); // No Query Parameter
            var requestCookieCollection = new RequestCookieCollection(); // No Cookie
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(SwitcherooThemeSelector.RequiredRole)).Returns(true);
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(CurrentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            Assert.Null(result);
            Assert.Empty(responseCookies.Cookies);
        }

        [Theory]
        [InlineData("TheAgencyTheme")]
        [InlineData("TheBlogTheme")]
        [InlineData("ComingSoon")]
        public async void SelectorSetsThemeNameFromQuery(string selectedThemeName) {
            // Arrange
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
                {{SwitcherooThemeSelector.QueryStringKey, new StringValues(selectedThemeName)}});
            var requestCookieCollection = new RequestCookieCollection();
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(SwitcherooThemeSelector.RequiredRole)).Returns(true);
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(CurrentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(selectedThemeName, result.ThemeName);
            Assert.Equal(SwitcherooThemeSelector.Priority, result.Priority);
            Assert.Single(responseCookies.Cookies);
            Assert.Equal(SwitcherooThemeSelector.CookieName, responseCookies.Cookies.First().Key);
            Assert.Equal(selectedThemeName, responseCookies.Cookies.First().Value);
        }

        [Theory]
        [InlineData("TheAgencyTheme")]
        [InlineData("TheBlogTheme")]
        [InlineData("ComingSoon")]
        public async void SelectorSetsThemeNameFromCookie(string selectedThemeName) {
            // Arrange
            var queryCollection = new QueryCollection();
            var requestCookieCollection = new RequestCookieCollection(new Dictionary<string, string>
                {{SwitcherooThemeSelector.CookieName, selectedThemeName}});
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(SwitcherooThemeSelector.RequiredRole)).Returns(true);
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(CurrentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(selectedThemeName, result.ThemeName);
            Assert.Equal(SwitcherooThemeSelector.Priority, result.Priority);
            Assert.Single(responseCookies.Cookies);
            Assert.Equal(SwitcherooThemeSelector.CookieName, responseCookies.Cookies.First().Key);
            Assert.Equal(selectedThemeName, responseCookies.Cookies.First().Value);
        }

        [Theory]
        [InlineData("TheAgencyTheme")]
        [InlineData("TheBlogTheme")]
        [InlineData("ComingSoon")]
        public async void QueryTakesPrecedenceOverCookie(string selectedThemeName) {
            // Arrange
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
                {{SwitcherooThemeSelector.QueryStringKey, new StringValues(selectedThemeName)}});
            var requestCookieCollection = new RequestCookieCollection(new Dictionary<string, string>
                {{SwitcherooThemeSelector.CookieName, "AnyThemeName"}});
            var responseCookies = new ResponseCookies();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(p => p.User.IsInRole(SwitcherooThemeSelector.RequiredRole)).Returns(true);
            mockContext.SetupGet(p => p.Request.Query).Returns(queryCollection);
            mockContext.SetupGet(p => p.Request.Cookies).Returns(requestCookieCollection);
            mockContext.SetupGet(p => p.Response.Cookies).Returns(responseCookies);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor.SetupGet(p => p.HttpContext).Returns(mockContext.Object);

            var mockThemeService = new Mock<ISiteThemeService>();
            mockThemeService.Setup(p => p.GetCurrentThemeNameAsync()).ReturnsAsync(CurrentActiveTheme);

            IThemeSelector selector = new SwitcherooThemeSelector(mockThemeService.Object, mockContextAccessor.Object);

            // Act
            var result = await selector.GetThemeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(selectedThemeName, result.ThemeName);
            Assert.Equal(SwitcherooThemeSelector.Priority, result.Priority);
            Assert.Single(responseCookies.Cookies);
            Assert.Equal(SwitcherooThemeSelector.CookieName, responseCookies.Cookies.First().Key);
            Assert.Equal(selectedThemeName, responseCookies.Cookies.First().Value);
        }
    }
}