using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using StockportWebapp.Config;

namespace StockportWebapp.Builders
{
    public interface IParisLinkBuilder
    {
        string Build(IApplicationConfiguration config);
        ParisLinkBuilder ReturnText(string returnText);
        ParisLinkBuilder IgnoreConfirmation(string ignoreConfirmation);
        ParisLinkBuilder PayForBasketMode(string payForbasketMode);
        ParisLinkBuilder Data(string data);
        ParisLinkBuilder ParisRecordXML(ParisRecordXML parisRecordXML);
        ParisLinkBuilder ReturnUrl(string returnUrl);
    }

    [XmlRoot("record")]
    public class ParisRecordXML
    {
        public string reference;
        public string fund;
        public string amount;
        public string text6;
        public string memo;
    }

    public class ParisLinkBuilder : IParisLinkBuilder
    {
        public string _returnText = "?returntext=";
        public string _ignoreConfirmation = "&ignoreconfirmation=";
        public string _payForBasketMode = "&payforbasketmode=";
        public string _data = "&data=";
        public string _parisRecordXML = "&recordxml=";
        public string _returnUrl = "&returnurl=";

        public string Build(IApplicationConfiguration config)
        {
            StringBuilder parisLinkQueryString = new StringBuilder();

            parisLinkQueryString.Append(config.GetParisPamentLink("Stockportgov"));

            parisLinkQueryString.Append(_returnText);

            parisLinkQueryString.Append(_ignoreConfirmation);

            parisLinkQueryString.Append(_payForBasketMode);

            parisLinkQueryString.Append(_data);

            parisLinkQueryString.Append(_parisRecordXML);

            parisLinkQueryString.Append(_returnUrl);

            return parisLinkQueryString.ToString();
        }

        public ParisLinkBuilder ReturnText(string returnText)
        {
            _returnText = _returnText + returnText;
            return this;
        }

        public ParisLinkBuilder IgnoreConfirmation(string ignoreConfirmation)
        {
            _ignoreConfirmation = _ignoreConfirmation + ignoreConfirmation;
            return this;
        }

        public ParisLinkBuilder PayForBasketMode(string payForbasketMode)
        {
            _payForBasketMode = _payForBasketMode + payForbasketMode;

            return this;
        }

        public ParisLinkBuilder Data(string data)
        {
            _data = _data + data;

            return this;
        }

        public ParisLinkBuilder ParisRecordXML(ParisRecordXML parisRecordXML)
        {
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");

            var xmlserializer = new XmlSerializer(typeof(ParisRecordXML));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
            {
                xmlserializer.Serialize(writer, parisRecordXML, ns);
                _parisRecordXML = _parisRecordXML + "<records>" + stringWriter.ToString() + "</records>";
            }

            return this;
        }
        public ParisLinkBuilder ReturnUrl(string returnUrl)
        {
            _returnUrl = _returnUrl + returnUrl;

            return this;
        }
    }
}
