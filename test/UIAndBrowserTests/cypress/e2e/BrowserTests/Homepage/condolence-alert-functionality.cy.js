import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";

describe("Condolence alert functionality", () => {
  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];
  const closeAlert = '[data-cy="condolence-alert"]';

  beforeEach(() => {
    cy.visit("");
  });
  viewports.map((viewport) => {
    it(`tests on ${viewport.name} screen to check the condolence alert closes and then appaears on page reload`, () => {
      setViewPort(viewport.value);
      cy.get(closeAlert).click({ force: true }).should("not.be.visible");
      cy.visit("");
      cy.get(closeAlert).should("be.visible");
    });
  });

  it("tests dismissing an alert using keyboard", () => {
    cy.get('[title="test link"]').contains("Read more about this").tab();
    cy.get(closeAlert).type("{enter}");
    cy.get(closeAlert).should("not.be.visible");
    cy.reload();
    cy.get(closeAlert).should("be.visible");
  });

  it("tests to see if underline decoration for the links show on hover", () => {
    cy.get(".condolence_text > :nth-child(3) > a")
      .realHover()
      .should(
        "have.css",
        "text-decoration",
        "underline 3px solid rgb(255, 255, 255)"
      );
  });
});
