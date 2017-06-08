defaultConfiguration = {
  testUri : 'https://int-windows-iag-origin.smbcdigital.net',
  timeOut : 10000
  // for development environment have a different emailAlertsUrl set
};

var hostRegExp = /host=(.*)/;

process.argv.forEach(function (val, index) {
    var hostValue = hostRegExp.exec(val);
    if (hostValue && hostValue[1] !== '')
        defaultConfiguration.testUri = hostValue[1];

});

console.log(defaultConfiguration);
var fs = require('fs');
fs.writeFile('global.js', "module.exports=" + JSON.stringify(defaultConfiguration) + ";");
