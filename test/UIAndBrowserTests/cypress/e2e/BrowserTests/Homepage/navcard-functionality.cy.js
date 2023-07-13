import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";
describe("Navcard functionality", () => {
  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];

  beforeEach(() => {
    cy.visit("");
  });

  viewports.map((viewport) => {
    it(`tests on ${viewport.name} screen to check a navcard can be clicked and loads a new page`, () => {
      setViewPort(viewport.value);
      cy.get('[data-cy="nav-card-link"]').first().click();
      cy.url().should("not.eq", "https://qa-iag-stockportgov.smbcdigital.net/");
    });
  });

  it("tests using keyboard only to navigate through the navcards and then select one", () => {
    let navCardFirst = cy.get(':nth-child(1) > [data-cy="nav-card-link"]');
    for (let i = 0; i < 8; i++) {
      navCardFirst.tab();
    }
    navCardFirst.click();
    cy.get("h1").contains("Business Stockport");
  });

  it("tests to see if underline thickness increases on hover", () => {
    cy.get('[data-cy="navcard-list"]').within(() => {
      cy.get("a").first().realHover();
      cy.get("h3")
        .first()
        .should("have.css", "text-decoration-thickness", "3px");
    });
  });
});
