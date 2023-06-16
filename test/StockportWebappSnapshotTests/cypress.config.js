const { defineConfig } = require("cypress");
const {addMatchImageSnapshotPlugin}  =  require('@simonsmith/cypress-image-snapshot/plugin')

module.exports = defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      addMatchImageSnapshotPlugin(on)
    },
    baseUrl: 'http://localhost:5001/',
    defaultCommandTimeout: 20000,
    pageLoadTimeout: 90000,
    responseTimeout: 60000,
    experimentalRunAllSpecs: true
  },
});
