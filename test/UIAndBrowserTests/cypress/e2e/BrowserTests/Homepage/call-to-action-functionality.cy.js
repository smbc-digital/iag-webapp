import { setViewPort, hexToRgb } from "../../../helpers/functions";

describe("Call to action functionality", () => {
  const viewports = ["iphone-x", [1920, 1080]];

  beforeEach(() => {
    cy.visit("");
  });
  
  viewports.map((size) => {
    it(`tests on ${size} screen to check Call to action button can be clicked and loads new page`, () => {
      const currenturl = cy.url();
      setViewPort(size);

      cy.get('[data-cy="call-to-action-link"]').first().click();
      cy.url().should("not.eq", currenturl);
    });
  });

  it("tests that the call to action button should change colour on focus and hover", () => {
    const hoverColor = hexToRgb("#ffffffe6");
    cy.get('[data-cy="call-to-action-link"]').first()
      .realHover()
      .should("have.css", "background-color", hoverColor)
      .focus()
      .should("have.css", "background-color", hoverColor);
  });
});
