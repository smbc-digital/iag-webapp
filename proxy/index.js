var http = require('http'),
    httpProxy = require('http-proxy');
var proxy = httpProxy.createProxyServer({});

proxy.on('proxyReq', function(proxyReq, req, res, options) {
  var host = req.headers.host;
  var id = "stockportgov";//host.split(":")[0].split(".")[0];
  console.log("The BusinessId is: " + id);
  //if (id !== "") { proxyReq.setHeader('BUSINESS-ID', id); }
});

var server = http.createServer(function(req, res) {
  var url = "http://localhost:52287";
  if (/.local:5555$/g.test(req.headers.host)) { url = "http://localhost:5000"; }

  console.log("Going to url: " + url);
  proxy.web(req, res, {
    target: url
  });
});

proxy.on('error', function (err, req, res) {
  console.log("Something went wrong - " + err);
  res.end('Something went wrong - check that the website is running.');
});

console.log("listening on port 5555");
server.listen(5555);
