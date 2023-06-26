describe("Our Services functionality", () => {
    const viewports = ["iphone-8", [1920, 1080]];
    viewports.map((size) => {
      it(`tests on ${size} screen to check the View more services button is clickable and goes to a new page location`, () => {
        if (Cypress._.isArray(size)) {
          cy.viewport(size[0], size[1]);
        } else {
          cy.viewport(size);
        }
        cy.visit("");
        const location = cy.location()
        cy.get('.button').click()
        expect(cy.location()).to.not.equal(location)
      });
    });
    //Write another test to test the nav card functionality 
    //change test above to specifically go to link
  });
  