import { setViewPort } from "../../../helpers/functions";

describe("Top tasks functionality", () => {
    const viewports = ["iphone-x", [1440, 900], ["ipad-2", "portrait"], ["ipad-2", "landscape"]];

    beforeEach(function () {
      cy.visit("");
    });
  
    viewports.map((size) => {
      it(`tests on ${size} screen to check a top task can be clicked and loads new page`, () => {
        const url = cy.url()

        setViewPort(size)

        cy.get('[data-cy="top-task-link"]')
        .first()
        .click()
        cy.url().should('not.eq',url)
      });
    });
    
    it("tests to see if underline decoration for icon does not exists on hover", () => {
        cy.get('[data-cy="top-task-link"]').first().realHover();
        cy.get('[data-cy="top-task-icon"]').first().should('have.css', 'text-decoration', 'none solid rgb(255, 255, 255)')
    });

    it("tests to see if underline decoration for title exists on hover", () => {
        cy.get('[data-cy="top-task-link"]').first().realHover();
        cy.get('[data-cy="top-task-title"]').first().should('have.css', 'text-decoration', 'underline solid rgb(255, 255, 255)')
    });
  });
  