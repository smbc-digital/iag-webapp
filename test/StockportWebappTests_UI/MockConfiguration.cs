using System;
using Newtonsoft.Json.Linq;
using WireMock.Handlers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace StockportWebappTests_UI
{
    public class MockConfiguration
    {
        public static FluentMockServer Server;
        public static bool IsRecordMode;

        public MockConfiguration(bool isRecordMode)
        {
            if (Server == null)
            {
                IsRecordMode = isRecordMode;
                Start();
            }
        }

        private static void Start()
        {
            if (IsRecordMode)
            {
                Server = FluentMockServer.Start(new FluentMockServerSettings
                {
                    StartAdminInterface = true,
                    ProxyAndRecordSettings = new ProxyAndRecordSettings
                    {
                        Url = "http://localhost:5001/",
                        SaveMapping = true,
                        BlackListedHeaders = new[] {"X-ClientId", "Request-Id", "Authorization", "Host"},
                    },
                    Port = 8080
                });
            }
            else
            {
                Server = FluentMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://localhost:8080/" }
                });

                var path = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Remove(path.IndexOf("bin", StringComparison.Ordinal));

                Server.ReadStaticMappings(path + "SavedResponses");
            }
        }
    }
}
