// -----------------------------------------------------------------------
// <copyright file="PaymentHashing.cs" company="Stockport Metropolitan Borough Council">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Utils
{
    /// <summary>
    /// Available Hashing Algorithms for PARIS results validation
    /// </summary>
    public enum PaymentHashingAlgorithm
    {
        /// <summary>
        /// MD5 Encryption
        /// </summary>
        MD5,

        /// <summary>
        /// SHA1 Encryption
        /// </summary>
        SHA1,

        /// <summary>
        /// SHA256 Encryption
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA384 Encryption
        /// </summary>
        SHA384,

        /// <summary>
        /// SHA512 Encryption
        /// </summary>
        SHA512
    }

    public class ParisKeys
    {
        public string PreSalt { get; set; }
        public string PostSalt { get; set; }
        public string PrivateSalt { get; set; }
    }

    /// <summary>
    /// Class for decrypting hashed query strings from PARIS
    /// </summary>
    public class ParisHashHelper
    {
        #region "Constructors"

        /// <summary>
        ///  Initializes a new instance of the <see cref="PaymentHashing" /> class.
        /// </summary>
        public ParisHashHelper(ParisKeys keys)
        {
            this.PreSalt = keys.PreSalt;
            this.PostSalt = keys.PostSalt;
            this.PrivateSalt = keys.PrivateSalt;
            this.HashType = PaymentHashingAlgorithm.SHA512;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="PaymentHashing" /> class.
        /// </summary>
        /// <param name="hashType">Hashing Algorithm specifying type of encryption used</param>
        public ParisHashHelper(ParisKeys keys, PaymentHashingAlgorithm hashType)
        {
            this.PreSalt = keys.PreSalt;
            this.PostSalt = keys.PostSalt;
            this.PrivateSalt = keys.PrivateSalt;
            this.HashType = hashType;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="PaymentHashing" /> class.
        /// </summary>
        /// <param name="preSalt">Pre Salt String</param>
        /// <param name="postSalt">Post Salt String</param>
        public ParisHashHelper(string preSalt, string postSalt, string privateSalt)
        {
            this.PreSalt = preSalt;
            this.PostSalt = postSalt;
            this.PrivateSalt = privateSalt;
            this.HashType = PaymentHashingAlgorithm.SHA512;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="PaymentHashing" /> class.
        /// </summary>
        /// <param name="preSalt">Pre Salt String</param>
        /// <param name="postSalt">Post Salt String</param>
        /// <param name="hashType">Hashing Algorithm specifying type of encryption used</param>
        public ParisHashHelper(string preSalt, string postSalt, string privateSalt, PaymentHashingAlgorithm hashType)
        {
            this.PreSalt = preSalt;
            this.PostSalt = postSalt;
            this.PrivateSalt = privateSalt;
            this.HashType = hashType;
        }

        #endregion

        #region "Properties"
        /// <summary>
        /// Gets or sets the algorithm to be used for hashing
        /// </summary>
        public PaymentHashingAlgorithm HashType { private get; set; }

        /// <summary>
        /// Gets or sets the Pre Salt.
        /// </summary>
        private string PreSalt { get; set; }

        /// <summary>
        /// Gets or sets the Post Salt
        /// </summary>
        private string PostSalt { get; set; }

        /// <summary>
        /// Gets or sets the Private Salt.
        /// </summary>
        private string PrivateSalt { get; set; }
        #endregion
        #region "Hash Checking Functions"
        /// <summary>
        /// Checks a request query string matches the returned hash querystring value. 
        /// </summary>
        /// <param name="request">HttpRequest request</param>
        /// <returns>Boolean Is Valid</returns>
        public bool IsValidRequest(HttpRequest request)
        {
            // Retrieve the hash query string parameter from the request
            string hash = string.IsNullOrEmpty(request.Query["hash"].ToString()) ? string.Empty : request.Query["hash"].ToString();
            if (string.IsNullOrEmpty(hash))
                return false;

            // Recalculate the hash
            string parameters = System.Net.WebUtility.UrlDecode(request.QueryString.ToString());
            if (string.IsNullOrEmpty(parameters))
                return false;

            // Check and Find the location of the &hash parameter
            int index = parameters.IndexOf("&hash");

            // Remove the hash parameter from querystring before recalculating the hash
            if (index > -1)
                parameters = parameters.Substring(0, index);

            // Add the two keys to the start and end of the string we are about to create a hash result from
            if (!string.IsNullOrEmpty(parameters))
                return this.IsMatchingHash(parameters, hash);

            return false;
        }

        /// <summary>
        /// Hashes the supplies URL and compares it to the supplied hash value.
        /// </summary>
        /// <param name="url">String url</param>
        /// <param name="hash">String hash</param>
        /// <returns>Boolean is matching</returns>
        public bool IsMatchingHash(string url, string hash)
        {
            string recalculatedHash = string.Empty;
            byte[] hashThis = null;
            byte[] hashedResult = null;
            HashAlgorithm algorithm = null;

            // To correctly calculate the has we need to add the relevant salt values to beginning and end of the url.
            url = this.PreSalt + url + this.PostSalt;

            // Convert string to a byte[]
            hashThis = Encoding.UTF8.GetBytes(url);

            // Instantiate the appropriate cryptography class depending on the chosen algorithm
            switch (this.HashType)
            {
                case PaymentHashingAlgorithm.MD5:
                    algorithm = MD5.Create();
                    break;
                case PaymentHashingAlgorithm.SHA1:
                    algorithm = SHA1.Create();
                    break;
                case PaymentHashingAlgorithm.SHA256:
                    algorithm = SHA256.Create();
                    break;
                case PaymentHashingAlgorithm.SHA384:
                    algorithm = SHA384.Create();
                    break;
                case PaymentHashingAlgorithm.SHA512:
                    algorithm = SHA512.Create();
                    break;
            }

            if (hashThis != null && algorithm != null)
            {
                // Now recalculate the hash
                hashedResult = algorithm.ComputeHash(hashThis);

                // Convert the resultant byte[] to a string
                if (hashedResult != null)
                    recalculatedHash = BitConverter.ToString(hashedResult);
            }

            // If hash = recalculated value hashes match and request is valid - return true
            if ((!string.IsNullOrEmpty(hash)) && (recalculatedHash == hash))
                return true;

            // All other situations - hashed do not match or are null/empty return false
            return false;
        }
        #endregion
    }
}