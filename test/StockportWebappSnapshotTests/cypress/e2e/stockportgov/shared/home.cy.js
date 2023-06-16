import { desktop, mobile, tablet } from "../../../helpers/breakpoints";

describe("Home", () => {
  const breakpoints = [desktop, mobile, tablet];
  const components = [
    "search-and-top-tasks",
    "navcard-list",
    "global-alert",
    "personalised-services",
  ];

  beforeEach(function () {
    cy.visit("");
  });

  components.map((component) => {
    it(`${component} - Compare structure snapshot`, () => {
      cy.toMatchingDOM(component);
    });

    breakpoints.map((size) => {
      it(`${component} - ${size.name} image snapshot`, () => {
        cy.takeSnapshots(component, size.value);
      });
    });
  });
});
