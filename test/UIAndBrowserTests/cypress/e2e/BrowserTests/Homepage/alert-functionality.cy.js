import { setViewPort } from "../../../helpers/functions";

describe("Alert functionality", () => {
  const viewports = ["iphone-x", [1920, 1080]];
  const closeAlertIcon = '[data-cy="global-alert"]';
  const closeAlerts = () => {
    cy.get('[data-cy="global-alert"]')
      .not('a[title="Close Test condolence alert alert"]')
      .click({ multiple: true })
      .should("not.be.visible");
  };
  beforeEach(() => {
    cy.visit("");
  });
  viewports.map((size) => {
    it(`tests on ${size} screen to check alerts close and reappear on reload`, () => {
      setViewPort(size);
      closeAlerts();
      cy.visit("");
      cy.get(closeAlertIcon).filter(":visible").should("have.length", 4);
    });

    it(`tests on ${size} screen to check the alert icons show on desktop and are hidden on mobile`, () => {
      setViewPort(size);
      if (size === "iphone-x") {
        cy.get('[data-cy="alert-icon"]').should("not.be.visible");
      } else {
        cy.get('[data-cy="alert-icon"]').should("be.visible");
      }
    });
  });

  it("tests dismissing an alert using keyboard", () => {
    cy.get(closeAlertIcon).first().focus().type("{enter}");
    cy.get('[title="Close Alert error alert"]').should("not.be.visible");
    cy.reload();
    cy.get(closeAlertIcon).filter(":visible").should("have.length", 4);
  });
});
