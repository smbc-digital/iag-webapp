describe("View more services button functionality", () => {
  const viewports = ["iphone-8", [1920, 1080]];
  viewports.map((size) => {
    it(`tests on ${size} screen to check the View more services button is clickable and goes to a new page location`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.visit("");
      cy.get('[data-cy="view-more-services"]').click();
      cy.url().should("include", "topic/our-council-services");
    });
  });
});
