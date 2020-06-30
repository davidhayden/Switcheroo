using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;

namespace Switcheroo.OrchardCore.Tests {
    public class RequestCookies : IRequestCookieCollection {
        private IDictionary<string, string> _cookies;

        public RequestCookies() : this(new Dictionary<string,string>()) {}
        public RequestCookies(IDictionary<string, string> cookies) {
            _cookies = cookies;
        }
        public string this[string key] => _cookies[key];

        public int Count => _cookies.Count;

        public ICollection<string> Keys => _cookies.Keys;

        public bool ContainsKey(string key) => _cookies.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _cookies.GetEnumerator();

        public bool TryGetValue(string key, out string value) => _cookies.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _cookies.GetEnumerator();
    }
}
