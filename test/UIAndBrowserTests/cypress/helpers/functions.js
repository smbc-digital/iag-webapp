export const closeAlerts = () => {
    cy.get('[data-cy="global-alert"]')
        .not('a[title="Close Test condolence alert alert"]')
        .click({multiple: true})
        .should('not.be.visible')
}