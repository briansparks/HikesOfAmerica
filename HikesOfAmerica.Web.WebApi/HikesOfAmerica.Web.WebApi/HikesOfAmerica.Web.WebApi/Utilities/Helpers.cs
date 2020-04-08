using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HikesOfAmerica.Web.WebApi.Utilities
{
    public static class Helpers
    {
        public static IEnumerable<T> GetListParam<T>(HttpRequest request, string param)
        {
            request.Form.TryGetValue(param, out var result);

            var obj = JsonConvert.DeserializeObject<IEnumerable<T>>(result);

            return obj;
        }

        public static T GetFormParam<T>(HttpRequest request, string param)
        {
            request.Form.TryGetValue(param, out var result);

            var obj = JsonConvert.DeserializeObject<T>(result);

            return obj;
        }
    }
}
