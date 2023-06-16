Cypress.Commands.add('toMatchingDOM', (testId, index = 0) => {
  cy.document().then(async (win) => {
    const html = win.activeElement.querySelector(`[data-test='${testId}']`);
    cy.wrap(html).snapshot({
      json: false
    })
  })
})


Cypress.Commands.add('takeSnapshots', (testId, size = null) => {
  if (size) {
    if (Cypress._.isArray(size)) {
      cy.viewport(size[0], size[1])
    } else {
      cy.viewport(size)
    }
  }

  cy.get(`[data-test="${testId}"]`).first().matchImageSnapshot();
})