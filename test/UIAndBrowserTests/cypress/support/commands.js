// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

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
