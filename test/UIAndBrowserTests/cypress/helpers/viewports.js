export const setViewPort = (size) => {
  if (Cypress._.isArray(size)) {
    cy.viewport(size[0], size[1]);
  } else {
    cy.viewport(size);
  }
};
export const mobile = {
  name: "Mobile",
  value: [390, 664],
};
export const tabletPortrait = {
  name: "Tablet Portrait",
  value: [834, 1075],
};

export const tabletLandscape = {
  name: "Tablet Landscape",
  value: [1075, 834],
};

export const laptop = {
  name: "Laptop",
  value: [1440, 900],
};
