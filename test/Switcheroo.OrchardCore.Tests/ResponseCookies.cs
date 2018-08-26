using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Switcheroo.OrchardCore.Tests {
    public class ResponseCookies : IResponseCookies {
        public ResponseCookies() {
            Cookies = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Cookies { get; }

        public void Append(string key, string value) {
            Cookies.Add(key, value);
        }

        public void Append(string key, string value, CookieOptions options) {
            throw new System.NotImplementedException();
        }

        public void Delete(string key) {
            throw new System.NotImplementedException();
        }

        public void Delete(string key, CookieOptions options) {
            throw new System.NotImplementedException();
        }
    }
}