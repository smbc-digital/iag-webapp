describe("Navcard functionality", () => {
  const viewports = ["iphone-x", [1440, 900]];
  viewports.map((size) => {
    it(`tests on ${size} screen to check a Navcard can be clicked and loads new page`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.visit("")
      //Content for the Navcards is content managed, only testing the componant is clickable. We can't control errors caused by CMS.
      cy.get('[data-cy="nav-card-link"]')
      .should("have.length", 9)
      .first()
      .click()
      cy.url().should('not.eq','https://qa-iag-stockportgov.smbcdigital.net/')
    });
  });
});
