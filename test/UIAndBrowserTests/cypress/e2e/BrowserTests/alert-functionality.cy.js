import { closeAlerts } from "../../helpers/functions";

describe("Alert functionality", () => {
  const viewports = ["iphone-x", [1920, 1080]];
  viewports.map((size) => {
    it(`tests on ${size} screen to check alerts close and reappear on reload`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.visit("");
      cy.get(".global-alert").as("alert").should("have.length", 5);
      closeAlerts();
      cy.visit("");
      cy.get("@alert").should("have.length", 5).should("be.visible");
    });
  });
});
