describe("Navcard functionality", () => {
  const viewports = ["iphone-x", [1440, 900]];

  beforeEach(function () {
    cy.visit("");
  });

  viewports.map((size) => {
    it(`tests on ${size} screen to check a Navcard can be clicked and loads new page`, () => {
      if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
      cy.get('[data-cy="nav-card-link"]').first().click();
      cy.url().should("not.eq", "https://qa-iag-stockportgov.smbcdigital.net/");
    });
  });

  it.only("tests using keyboard only to navigate through the nav cards and then select one", () => {
    let navCardFirst = cy.get(':nth-child(1) > [data-cy="nav-card-link"]')
    for (let i = 0; i < 8; i++) {
    navCardFirst.tab()
    }
    navCardFirst.click()
    cy.get('h1').contains('Business Stockport')
  })

  it("tests to see if underline thickness increases on hover", () => {
    cy.get('[data-cy="navcard-list"]').within(() => {
      cy.get("a").first().realHover();
      cy.get("h3")
        .first()
        .should("have.css", "text-decoration-thickness", "3px")
  });
});
})
