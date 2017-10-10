using Microsoft.AspNetCore.Http;
using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace StockportWebapp.Utils
{
    public interface ICookiesHelper
    {
        void AddToCookies<T>(string slug, string cookieType);
        List<string> GetCookies<T>(string cookieType);
        List<T> PopulateCookies<T>(List<T> items, string cookieType);
        void RemoveAllFromCookies<T>(string cookieType);
        void RemoveFromCookies<T>(string slug, string cookieType);
    }

    public class CookiesHelper : ICookiesHelper
    {
        private IHttpContextAccessor httpContextAccessor;

        public CookiesHelper(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public List<T> PopulateCookies<T>(List<T> items, string cookieType)
        {
            var cookiesAsObject = GetCookiesAsObject(cookieType);

            if (!cookiesAsObject.Keys.Any()) return items;

            var type = typeof(T).ToString().Replace("Processed", "");

            var cookies = cookiesAsObject[type];

            foreach (var item in items)
            {
                PropertyInfo cookieProp = item.GetType().GetProperty("Favourite");
                PropertyInfo slugProp = item.GetType().GetProperty("Slug");

                if (null != cookieProp && null != slugProp && cookieProp.CanWrite)
                {
                    var exists = cookies.Any(f => f == slugProp.GetValue(item).ToString());
                    cookieProp.SetValue(item, exists, null);
                }
                else
                {
                    throw new Exception("The object you are adding to favourites does not have either the property 'Favourite' or the property 'Slug'");
                }
            }

            return items;
        }

        public void AddToCookies<T>(string slug, string cookieType)
        {
            var cookiesAsObject = GetCookiesAsObject(cookieType);

            if (!cookiesAsObject.ContainsKey(typeof(T).ToString()))
            {
                cookiesAsObject.Add(typeof(T).ToString(), new List<string>());
            }

            if (cookiesAsObject[typeof(T).ToString()].All(f => f != slug))
            {
                cookiesAsObject[typeof(T).ToString()].Add(slug);
            }

            UpdateCookies(cookiesAsObject);
        }

        public void RemoveFromCookies<T>(string slug, string cookieType)
        {
            var cookiesAsObject = GetCookiesAsObject(cookieType);

            if (!cookiesAsObject.ContainsKey(typeof(T).ToString()))
            {
                cookiesAsObject.Add(typeof(T).ToString(), new List<string>());
            }

            if (cookiesAsObject[typeof(T).ToString()].Any(f => f == slug))
            {
                cookiesAsObject[typeof(T).ToString()].Remove(slug);
            }

            UpdateCookies(cookiesAsObject);
        }

        public void RemoveAllFromCookies<T>(string cookieType)
        {
            var cookiesAsObject = GetCookiesAsObject(cookieType);

            if (cookiesAsObject.ContainsKey(typeof(T).ToString()))
            {
                cookiesAsObject.Remove(typeof(T).ToString());
            }

            UpdateCookies(cookiesAsObject);
        }

        public List<string> GetCookies<T>(string cookieType)
        {
            List<string> result;
            var cookiesAsObject = GetCookiesAsObject(cookieType);
            cookiesAsObject.TryGetValue(typeof(T).ToString(), out result);
            return result;
        }

        private Dictionary<string, List<string>> GetCookiesAsObject(string cookieType)
        {
            var cookies = httpContextAccessor.HttpContext.Request.Cookies["favourites"];
            return !string.IsNullOrEmpty(cookies) 
                ? JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies) 
                : new Dictionary<string, List<string>>();
        }

        private void UpdateCookies(Dictionary<string, List<string>> cookies)
        {
            var data = JsonConvert.SerializeObject(cookies);
            httpContextAccessor.HttpContext.Response.Cookies.Append("favourites", data, new CookieOptions { Expires = DateTime.Now.AddYears(99) });
        }
    }
}
