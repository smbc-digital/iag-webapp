import {
  mobile,
  tabletLandscape,
  tabletPortrait,
  laptop,
  setViewPort,
} from "../../../helpers/viewports";

describe("Top tasks functionality", () => {
  const viewports = [mobile, tabletLandscape, tabletPortrait, laptop];
  const topTasksIcon = '[data-cy="top-task-icon"]'
  const topTaskLink = '[data-cy="top-task-link"]'
  beforeEach(() => {
    cy.visit("");
  });

  viewports.map((viewport) => {
    it(`tests on ${viewport.name} screen to check a top task can be clicked and loads new page`, () => {
      const url = cy.url();
      setViewPort(viewport.value);
      cy.get(topTasksIcon).should('be.visible')
      cy.get(topTaskLink).first().click();
      cy.url().should("not.eq", url);
    });
  });

  it("tests to see if underline decoration for icon does not exist on hover", () => {
    cy.get(topTasksIcon)
      .first()
      .realHover()
      .should("have.css", "text-decoration", "none solid rgb(255, 255, 255)");
  });

  it("tests to see if underline decoration for title exists on hover", () => {
    cy.get('[data-cy="top-task-title"]')
      .first()
      .realHover()
      .should(
        "have.css",
        "text-decoration",
        "underline 3px solid rgb(255, 255, 255)"
      );
  });
  it(`tests using keyboard only to navigate through the top tasks and selects one`, () => {
    let topTaskFirst = cy.get(topTaskLink);
    for (let i = 0; i < 5; i++) {
      topTaskFirst.tab();
    }
    topTaskFirst.click();
    cy.url().should("include", "/offer-day");
  });
});
