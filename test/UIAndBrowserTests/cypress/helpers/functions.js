export const closeAlerts = () => {
    cy.get('[data-cy="global-alert"]')
        .not('a[title="Close Test condolence alert alert"]')
        .click({multiple: true})
        .should('not.be.visible')
}

export const hexToRgb = (hex) => {
    var parsedValues = /^#?([a-f\d]{1,2})([a-f\d]{1,2})([a-f\d]{1,2})$/i.exec(hex);

    if (parsedValues) {
        const r = parseInt((parsedValues[1] + parsedValues[1]).slice(-2), 16);
        const g = parseInt((parsedValues[2] + parsedValues[2]).slice(-2), 16);
        const b = parseInt((parsedValues[3] + parsedValues[3]).slice(-2), 16);
        return `rgb(${r}, ${g}, ${b})`;
    }
    return null;
}

export const setViewPort = (size) => {
    if (Cypress._.isArray(size)) {
        cy.viewport(size[0], size[1]);
      } else {
        cy.viewport(size);
      }
}