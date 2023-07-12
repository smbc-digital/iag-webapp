import { hexToRgb, visitHomepage } from "../../../helpers/functions";

import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";

describe("Search functionality", () => {
  const containsSearchResults = () => {
    cy.url().should("include", "/searchResults");
    cy.get("h1").contains("Search results");
  };
  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];
  const searchBarId = "#cludo-search-bar";
  const searchList = '[aria-autocomplete="list"]';
  const searchButton = '#cludo-search-hero-form > [data-cy="search-submit"]';

  viewports.map((viewport) => {
    it(`tests entering a single character and selecting an option from the list via the homepage search on ${viewport.name} screen `, () => {
      setViewPort(viewport.value);
      visitHomepage();
      cy.get(searchBarId).type("T");
      cy.get(searchList).should("be.visible");
      cy.get('[aria-label="tree preservation order map"]').click();
      containsSearchResults();
    });

    it(`tests entering full search and pressing the search button via the homepage search on ${viewport.name} screen`, () => {
      setViewPort(viewport.value);
      visitHomepage();
      cy.get(searchBarId).type("Council Tax");
      cy.get(searchButton).click();
      containsSearchResults();
    });

    it(`tests using the header search on the search results page using ${viewport.name} screen`, () => {
      setViewPort(viewport.value);
      cy.visit("/searchResults");
      if (viewport.name === "Mobile" || viewport.name === "Tablet Portrait") {
        cy.get('[data-cy="show-search"]').click();
        cy.get("#mobile-search-bar").clear().type("graffiti");
      } else {
        cy.get("#search-bar").clear().type("graffiti");
      }
      cy.get(searchList).should("be.visible");
      cy.get('[aria-label="graffiti"]').click();
      containsSearchResults();
    });
  });

  it("tests tabbing into the search on the homepage, entering input and then using the enter key to submit search", () => {
    visitHomepage();
    cy.get('[data-cy="global-alert"]')
      .last()
      .tab()
      .type("Council Tax")
      .tab()
      .type("{enter}");
    containsSearchResults();
  });

  it("tests the search button colour changes on hover and focus", () => {
    const color = hexToRgb("#066");
    visitHomepage();
    cy.get(searchButton)
      .realHover()
      .should("have.css", "background-color", color)
      .focus()
      .should("have.css", "background-color", color);
  });
});
