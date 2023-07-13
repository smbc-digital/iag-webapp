import { hexToRgb } from "../../../helpers/functions";

import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";

describe("View more services button functionality", () => {
  const getMoreServicesButton = () => {
    return cy.get('[data-cy="view-more-services"]');
  };

  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];

  beforeEach(() => {
    cy.visit("");
  });

  viewports.map((viewport) => {
    it(`tests on ${viewport.name} screen to check the View more services button is clickable and goes to a new page location`, () => {
      setViewPort(viewport.value);
      getMoreServicesButton().click();
      cy.url().should("include", "topic/our-council-services");
    });
  });

  it("tests using keyboard only to navigate and press the view more services button", () => {
    cy.get('[data-cy="nav-card-link"]').last().tab().type("{enter}");
    cy.url().should("include", "topic/our-council-services");
  });

  it("tests that the view more services button should change colour on focus and hover", () => {
    const color = hexToRgb("#066");
    getMoreServicesButton()
      .realHover()
      .should("have.css", "background-color", color)
      .focus()
      .should("have.css", "background-color", color);
  });
});
