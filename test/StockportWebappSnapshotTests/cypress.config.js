const { defineConfig } = require("cypress");
const {addMatchImageSnapshotPlugin}  =  require('@simonsmith/cypress-image-snapshot/plugin') // Is this something we want? there are many snapshot plugins, this is just the first one that worked

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
