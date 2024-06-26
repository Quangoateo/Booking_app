﻿using NetUtility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingApp.Helpers
{
    public class JWTExtensions
    {
        /// <summary>
        /// Get token JWT "string token = Request.Headers["Authorization"]"
        /// </summary>
        /// <param name="token">token from header</param>
        /// <returns>IEnumerable<Claim> . Follow sample to get value in Claim List
        /// Ex: DecodeToken().First(claim => claim.Type == "Name").Value
        /// </returns>
        public static IEnumerable<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = token.ToSafetyString();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            return tokenS.Claims;
        }

        public static int GetDecodeTokenByID(string token) // extract ID from token
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = token.ToSafetyString();
            if (authHeader == string.Empty)
                return 0;
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            return tokenS.Claims.First(x => x.Type.Equals("nameid")).Value.ToInt();
        }
      
        public static object GetDecodeTokenByProperty(string token, string propertyType)
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = token.ToSafetyString();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            return tokenS.Claims.First(x => x.Type.Equals(propertyType)).Value;
        }

        public static IEnumerable<object> GetValues<T>(IEnumerable<T> items, string propertyName)
        {
            Type type = typeof(T);
            var prop = type.GetProperty(propertyName);
            foreach (var item in items)
                yield return prop.GetValue(item, null);
        }
    }
}
