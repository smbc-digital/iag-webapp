using Jose;
using Newtonsoft.Json;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using System;
using StockportWebapp.Config;

namespace StockportWebapp.Utils
{
    public interface IJwtDecoder
    {
        LoggedInPerson Decode(string token);
    }

    public class JwtDecoder : IJwtDecoder
    {
        private readonly GroupAuthenticationKeys _keys;

        public JwtDecoder(GroupAuthenticationKeys keys)
        {
            _keys = keys;
        }

        public LoggedInPerson Decode(string token)
        {
            try
            {
                // valid tokens are split into three sections by .'s
                if (token.Split('.').Length != 3) throw new InvalidJwtException("Invalid JWT token");

                return JWT.Decode<LoggedInPerson>(token, Convert.FromBase64String(_keys.Key), JwsAlgorithm.HS256);
            }
            catch (Exception ex)
            {
                if (ex is IntegrityException)
                {
                    // TODO: Log exception rather than throwing it
                }

                if (ex is JsonReaderException)
                {
                    // TODO: Log exception rather than throwing it
                }

                throw;
            }
        }
    }
}
