const {desktop, mobile, tablet} = require("../../../helpers/breakpoints");

describe('Services', () => {

  cy.toMatchingDOM("navcard-list-component");

  [desktop, mobile, tablet].map(size => {
    it (`${size.name} snapshot`, () => {
      cy.visit("");
      cy.takeSnapshots("navcard-wrapper", "navcard-list-component", size.value);
    })
  })
})