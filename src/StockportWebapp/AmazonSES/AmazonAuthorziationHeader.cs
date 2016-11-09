using System.Text;
using StockportWebapp.Utils;

namespace StockportWebapp.AmazonSES
{
    public class AmazonAuthorizationHeader
    {
        private readonly string _service = "email";
        private readonly string _terminationString = "aws4_request";
        private readonly string _signingAlgorithm = "AWS4-HMAC-SHA256";
        private readonly string _requestMethod = "POST";
        private readonly string _contentType = "application/x-www-form-urlencoded";
        private readonly string _headersToSign = "content-type;host;x-amz-date";

        public virtual string Create(AmazonSesClientConfiguration sesClientConfig, string payload, string dateStamp, string amzDate)
        {
            // Task 1: Create a Canonical Request for Signature Version 4
            var canonicalRequest = CanonicalRequest(amzDate, payload, _headersToSign, sesClientConfig.Host);

            // TASK 2: Create the string to sign
            var hashedCanonicalRequest = Cryptography.Sha256(canonicalRequest);
            var credentialScope = $"{dateStamp}/{sesClientConfig.Region}/{_service}/{_terminationString}";
            var stringToSign = $"{_signingAlgorithm}\n{amzDate}\n{credentialScope}\n{hashedCanonicalRequest}";

            // TASK 3: Calculate the Signature
            var signature = CalculateSignature(sesClientConfig, dateStamp, stringToSign);

            // TASK 4: Add signing information to the request
            return AssembleAuthorisationHeader(sesClientConfig, credentialScope, _headersToSign, signature);
        }

        public string CanonicalRequest(string amzDate, string requestPayload, string signedHeaders, string host)
        {
            var canonicalUri = "/";
            var canonicalQueryString = "";
            var canonicalHeaders = $"content-type:{_contentType}; charset=utf-8" + "\n" +
                                   "host:" + host + "\n" +
                                   "x-amz-date:" + amzDate + "\n";

            var lowerCasePayloadHash = Cryptography.Sha256(requestPayload);

            var canonicalRequest = _requestMethod + "\n" +
                                   canonicalUri + "\n" +
                                   canonicalQueryString + "\n" +
                                   canonicalHeaders + "\n" +
                                   signedHeaders + "\n" +
                                   lowerCasePayloadHash;
            return canonicalRequest;
        }

        private string CalculateSignature(AmazonSesClientConfiguration sesClientConfig, string dateStamp,
            string stringToSign)
        {
            var signatureKey = CalculateSignatureKey(sesClientConfig.AwsSecretAccessKey, dateStamp, sesClientConfig.Region);
            var signature = Cryptography.HmacSha256(stringToSign, signatureKey);
            return Cryptography.ByteArrayToHexaString(signature);
        }

        private string AssembleAuthorisationHeader(AmazonSesClientConfiguration sesClientConfig, string credentialScope,
            string signedHeaders, string signature)
        {
            return
                $"{_signingAlgorithm} Credential={sesClientConfig.AwsAccessKeyId}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";
        }

        public byte[] CalculateSignatureKey(string key, string dateStamp, string regionName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = Cryptography.HmacSha256(dateStamp, kSecret);
            byte[] kRegion = Cryptography.HmacSha256(regionName, kDate);
            byte[] kService = Cryptography.HmacSha256(_service, kRegion);
            byte[] kSigning = Cryptography.HmacSha256("aws4_request", kService);

            return kSigning;
        }
    }
}