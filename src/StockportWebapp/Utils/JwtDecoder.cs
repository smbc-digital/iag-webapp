using System;
using System.Text;
using Jose;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public interface IJwtDecoder
    {
        LoggedInPerson Decode(string token);
    }

    public class JwtDecoder : IJwtDecoder
    {
        private readonly GroupAuthenticationKeys _keys;
        private readonly ILogger<JwtDecoder> _logger;

        public JwtDecoder(GroupAuthenticationKeys keys, ILogger<JwtDecoder> logger)
        {
            _keys = keys;
            _logger = logger;
        }

        public LoggedInPerson Decode(string token)
        {
            try
            {
                // valid tokens are split into three sections by .'s
                if (token.Split('.').Length != 3)
                {
                    _logger.LogWarning($"InvalidJwtException was thrown from jwt decoder for token {token}");
                    throw new InvalidJwtException("Invalid JWT token");
                }

                return JWT.Decode<LoggedInPerson>(token, Encoding.ASCII.GetBytes(_keys.Key), JwsAlgorithm.HS256);
            }
            catch (Exception ex)
            {
                if (ex is IntegrityException) _logger.LogWarning($"IntegrityException was thrown from jwt decoder for token {token}");
                if (ex is JsonReaderException) _logger.LogWarning($"JsonReaderException was thrown for jwt decoder for token {token}");

                throw;
            }
        }
    }
}
