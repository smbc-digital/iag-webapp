using Microsoft.AspNetCore.Http;
using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace StockportWebapp.Utils
{
    public interface IFavouritesHelper
    {
        void AddToFavourites<T>(string slug);
        List<string> GetFavourites<T>();
        List<T> PopulateFavourites<T>(List<T> items);
        void RemoveAllFromFavourites<T>();
        void RemoveFromFavourites<T>(string slug);
    }

    public class FavouritesHelper : IFavouritesHelper
    {
        private IHttpContextAccessor httpContextAccessor;

        public FavouritesHelper(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public List<T> PopulateFavourites<T>(List<T> items)
        {
            var allFavourites = GetFavouritesAsObject();

            if (!allFavourites.Keys.Any()) return items;

            var type = typeof(T).ToString().Replace("Processed", "");

            var favourites = allFavourites[type];

            foreach (var item in items)
            {
                PropertyInfo favProp = item.GetType().GetProperty("Favourite");
                PropertyInfo slugProp = item.GetType().GetProperty("Slug");

                if (null != favProp && null != slugProp && favProp.CanWrite)
                {
                    var exists = favourites.Any(f => f == slugProp.GetValue(item).ToString());
                    favProp.SetValue(item, exists, null);
                }
                else
                {
                    throw new Exception("The object you are adding to favourites does not have either the property 'Favourite' or the property 'Slug'");
                }
            }

            return items;
        }

        public void AddToFavourites<T>(string slug)
        {
            var favourites = GetFavouritesAsObject();

            if (!favourites.ContainsKey(typeof(T).ToString()))
            {
                favourites.Add(typeof(T).ToString(), new List<string>());
            }

            if (!favourites[typeof(T).ToString()].Any(f => f == slug))
            {
                favourites[typeof(T).ToString()].Add(slug);
            }

            UpdateFavourites(favourites);
        }

        public void RemoveFromFavourites<T>(string slug)
        {
            var favourites = GetFavouritesAsObject();

            if (!favourites.ContainsKey(typeof(T).ToString()))
            {
                favourites.Add(typeof(T).ToString(), new List<string>());
            }

            if (favourites[typeof(T).ToString()].Any(f => f == slug))
            {
                favourites[typeof(T).ToString()].Remove(slug);
            }

            UpdateFavourites(favourites);
        }

        public void RemoveAllFromFavourites<T>()
        {
            var favourites = GetFavouritesAsObject();

            if (favourites.ContainsKey(typeof(T).ToString()))
            {
                favourites.Remove(typeof(T).ToString());
            }

            UpdateFavourites(favourites);
        }

        public List<string> GetFavourites<T>()
        {
            var result = new List<string>();
            var favourites = GetFavouritesAsObject();
            favourites.TryGetValue(typeof(T).ToString(), out result);
            return result;
        }

        private Dictionary<string, List<string>> GetFavouritesAsObject()
        {
            var favourites = httpContextAccessor.HttpContext.Request.Cookies["favourites"];
            if (!string.IsNullOrEmpty(favourites))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(favourites);
            }
            else
            {
                return new Dictionary<string, List<string>>();
            }
        }

        private void UpdateFavourites(Dictionary<string, List<string>> favourites)
        {
            var data = JsonConvert.SerializeObject(favourites);
            httpContextAccessor.HttpContext.Response.Cookies.Append("favourites", data, new CookieOptions { Expires = DateTime.Now.AddYears(99) });
        }
    }
}
