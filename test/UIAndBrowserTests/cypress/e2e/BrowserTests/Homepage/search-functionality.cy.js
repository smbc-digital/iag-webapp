import { setViewPort, visitHomepage } from "../../../helpers/functions";

describe("Search functionality", () => {
  const containsSearchResults = () => {
    cy.url().should("include", "/searchResults");
    cy.get("h1").contains("Search results");
  };
  const viewports = ["iphone-x", [1680, 1050]];
  const searchBarId = "#cludo-search-bar";
  const searchList = '[aria-autocomplete="list"]';
  viewports.map((size) => {

    it(`tests entering a single character and selecting an option from the list via the homepage search on ${size} screen `, () => {
      setViewPort(size);
      visitHomepage()
      cy.get(searchBarId).type("T");
      cy.get(searchList).should("be.visible");
      cy.get('[aria-label="tree preservation order map"]').click();
      containsSearchResults();
    });

    it(`tests entering full search and pressing the search button via the homepage search on ${size} screen`, () => {
      setViewPort(size);
      visitHomepage()
      cy.get(searchBarId).type("Council Tax");
      cy.get('#cludo-search-hero-form > [data-cy="search-submit"]').click();
      containsSearchResults();
    });

    it(`tests using the header search on the search results page using ${size} screen`, () => {
      setViewPort(size);
      cy.visit("/searchResults");
      if (size === "iphone-x") {
        cy.get('[data-cy="show-search"]').click();
        cy.get("#mobile-search-bar").clear().type("graffiti");
      } else {
        cy.get("#search-bar").clear().type("graffiti");
      }
      cy.get(searchList).should("be.visible");
      cy.get('[aria-label="graffiti"]').click();
      containsSearchResults();
    });
   
    it.only('tests tabbing into the search on the homepage, entering input and then using the enter key to submit search', () => {
        visitHomepage()
        cy.get('.global-alert-text-condolence > :nth-child(5) > a')
        .tab().tab() 
        .type('Council Tax')
        .tab().type('{enter}')
        containsSearchResults()
    })
  });
})
