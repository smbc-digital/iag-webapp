import { hexToRgb } from "../../../helpers/functions";

describe("View more services button functionality", () => {
  const viewports = ["iphone-8", [1920, 1080]];
  const getMoreServicesButton = () => {
    return cy.get('[data-cy="view-more-services"]');
  };

  beforeEach(function () {
    cy.visit("");
  });

  viewports.map((size) => {
    it(`tests on ${size} screen to check the View more services button is clickable and goes to a new page location`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
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
