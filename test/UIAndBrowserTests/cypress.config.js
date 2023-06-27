const { defineConfig } = require("cypress");
const {addMatchImageSnapshotPlugin}  =  require('@simonsmith/cypress-image-snapshot/plugin')

module.exports = defineConfig({
  setupNodeEvents(on, config) {
    require('./cypress/plugins/index.js')(on, config)
  },

  e2e: {
    baseUrl: 'https://qa-iag-stockportgov.smbcdigital.net/',
    setupNodeEvents(on, config) {
      addMatchImageSnapshotPlugin(on)
    },
  },
});
