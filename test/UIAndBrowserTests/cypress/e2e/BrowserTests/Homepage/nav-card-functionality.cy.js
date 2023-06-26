describe("Navcard functionality", () => {
  const viewports = ["iphone-x", [1440, 900]];
  viewports.map((size) => {
    it(`tests on ${size} screen to check the Navcards are clickable and go to a new page location`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.visit("")
      cy.get('[data-cy="nav-card-link"]')
      .should("have.length", 9)
      .click({ multiple: true });
    });
  });
});
