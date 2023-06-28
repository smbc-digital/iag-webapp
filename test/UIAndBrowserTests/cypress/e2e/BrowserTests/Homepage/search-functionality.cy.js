describe("Search functionality", () => {
  const viewports = ["iphone-x", [1680, 1050]];
  viewports.map((size) => {
    it(`tests the search functionality on ${size} screen `, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.visit("");
      cy.get("#cludo-search-bar").type("T");
      cy.get('[aria-autocomplete="list"]').should("be.visible");
      cy.get('[aria-label="tree preservation order map"]').click();

      cy.visit("");
      cy.get("#cludo-search-bar").type("Council Tax");
      cy.get("#cludo-search-hero-form > .search-button").click();
      cy.url().should("include", "/searchResults");
      cy.get('h1').contains('Search results')

      if (size === "iphone-x"){
        cy.get('.show-search-button > .fa').click()
        cy.get('#mobile-search-bar')
        .clear()
        .type('Taxi')
      }else {
        cy.get('#search-bar')
        .clear()
       .type('Taxi')
      }
      cy.get('[aria-autocomplete="list"]').should("be.visible");
      cy.get('[aria-label="taxi licence"]').click();
      cy.url().should("include", "/searchResults");
      cy.get('h1').contains('Search results')
    });
  });
});
