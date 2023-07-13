import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";

describe("Alert functionality", () => {
  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];
  const closeAlertIcon = '[data-cy="global-alert"]';
  const closeAlerts = () => {
    cy.get('[data-cy="global-alert"]')
      .click({ multiple: true })
      .should("not.be.visible");
  };
  beforeEach(() => {
    cy.visit("");
  });
  viewports.map((viewport) => {
    it(`tests on ${viewport.name} screen to check alerts close and reappear on reload`, () => {
      setViewPort(viewport.value);
      closeAlerts();
      cy.visit("");
      cy.get(closeAlertIcon).filter(":visible").should("have.length", 4);
    });

    it(`tests on ${viewport.name} screen to check the alert icons show on desktop and are hidden on mobile`, () => {
      setViewPort(viewport.value);
      if (viewport.name === "Mobile" || viewport.name === "Tablet Portrait") {
        cy.get('[data-cy="alert-icon"]').should("not.be.visible");
      } else cy.get('[data-cy="alert-icon"]').should("be.visible");
    });
  });

  it("tests dismissing an alert using keyboard", () => {
    cy.get(closeAlertIcon).first().focus().type("{enter}");
    cy.get('[aria-label="Close Alert error alert"]').should("not.be.visible");
    cy.reload();
    cy.get(closeAlertIcon).filter(":visible").should("have.length", 4);
  });
});
