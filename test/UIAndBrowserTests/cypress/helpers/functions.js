export const visitHomepage =  () => {
    cy.visit("")
}

export const hexToRgb = (hex) => {
  var parsedValues = /^#?([a-f\d]{1,2})([a-f\d]{1,2})([a-f\d]{1,2})([a-f\d]{0,2})$/i.exec(hex);

  if (parsedValues) {
      const r = parseInt((parsedValues[1] + parsedValues[1]).slice(-2), 16);
      const g = parseInt((parsedValues[2] + parsedValues[2]).slice(-2), 16);
      const b = parseInt((parsedValues[3] + parsedValues[3]).slice(-2), 16);
      const a = parsedValues.length > 3 ? parseInt((parsedValues[4] + parsedValues[4]).slice(-2), 16) : null;
      const formattedAlpha = Math.round(((a/255) + Number.EPSILON) * 100) / 100;

      return `rgb${a ? "a" : ""}(${r}, ${g}, ${b}${a ? ", " + formattedAlpha + ")" : ")"}`;
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