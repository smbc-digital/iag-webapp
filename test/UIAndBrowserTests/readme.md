# Cypress snapshot tests

### To run Cypress

```bash
cd test
npm i
cypress start
```

### Writing snapshot tests

A component can be marked for testing by adding the `data-cy` attribute to it. This is to keep the component resilient to changes made to id or class references.

```html
<div class="grid-100" data-cy="top-tasks-wrapper">...</div>
```

The component can then be referenced within the cypress test.

A typical test test file can look something like this.

```javascript
import { desktop, mobile, tablet } from "../../../helpers/breakpoints";

describe("Home", () => {
  const breakpoints = [desktop, mobile, tablet];
  const components = [
    "top-tasks-wrapper",
    "index-search-bar",
    "navcard-list-component",
    "global-alert",
    "personalised-services",
  ];

  beforeEach(function () {
    cy.visit("");
  });

  components.map((component) => {
    it(`${component} - Compare structure snapshot`, () => {
      cy.toMatchingDOM(component);
    });

    breakpoints.map((size) => {
      it(`${component} - ${size.name} image snapshot`, () => {
        cy.takeSnapshots(component, size.value);
      });
    });
  });
});
```

A structure snapshot is taken for each component listed in the `components` array. If no structure test with the given component name is found a new one with be created, otherwise the test will pass or fail depending on weather the html structure matches.

```javascript
it(`${component} - Compare structure snapshot`, () => {
  cy.toMatchingDOM(component);
});
```

An image snapshot will be taken for each of the given size breakpoints. Like the previous test, if no snapshot is found it will generate a new one.

If the 2 images differ in some way a comparison image will be generated within a `__diff_output__` file.

```javascript
breakpoints.map((size) => {
  it(`${component} - ${size.name} image snapshot`, () => {
    cy.takeSnapshots(component, size.value);
  });
});
```

To update a structure snapshot test, simply remove the existing one within `snapshot.js` and rerun the test suite. For an image snapshot test, delete each of the desktop, mobile, and tablet snapshots before rerunning.

### Config

The the image snapshot module can be configured with a failure threshold. This can be useful for dealing with minute differences between browser versions, or where the image taken isn't exactly pixel perfect.

More information can be found [here](https://github.com/simonsmith/cypress-image-snapshot)

```javascript
addMatchImageSnapshotCommand({
  failureThresholdType: "percent",
  failureThreshold: 0.1,
});
```
