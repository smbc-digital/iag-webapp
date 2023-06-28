import { hexToRgb } from "../../../helpers/functions";
describe("View more services button functionality", () => {
  const viewports = ["iphone-8", [1920, 1080]];

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
      cy.get('[data-cy="view-more-services"]').click();
      cy.url().should("include", "topic/our-council-services");
    });
  });

  it("view more services button should change color on focus and hover", () => {
    const moreServicesButton = cy.get('[data-cy="view-more-services"]');
    const color = hexToRgb('#066');

    moreServicesButton.realHover();
    moreServicesButton.should('have.css', 'background-color', color);

    moreServicesButton.focus();
    moreServicesButton.should('have.css', 'background-color', color);
  });
});
