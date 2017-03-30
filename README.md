# StockportWebapp

## Building the app
To build and run tests:

```
make build
```

## Running the app
The application runs using a BUSINESS-ID header to decide which specific business to
run per request. In order for this to be simplified, you need to run a local proxy which will add the corresponding header, and route to the correct url. For example, `[business_id].local:5555` (e.g. `stockportgov.local:5555`) will add header `BUSINESS-ID` and set it to `business_id`, and redirect to `localhost:5000`. If you just call `[business_id]:5555` it will redirect to `192.168.99.100:5000`.
The hosts file may also need to be amended with the following lines.
`127.0.0.1		[business_id]`
`127.0.0.1		[business_id].local`

### Configuration
The app has three config files which are kept in `src/StockportWebapp/app-config/`

1. `appsettings.json` - this is for generic application config.
2. `appsettings.{ASPNETCORE_ENVIRONMENT}.json` - this is for environment specific config, i.e. external service settings.
3. `injected/appsettings.{ASPNETCORE_ENVIRONMENT}.secrets.json` - this is for secret config, i.e. the SES email credentials ([template found here](src/StockportWebapp/app-config/injected/readme.md)).

### How to run
To start the proxy:
```
make start-proxy
```

To start the application:
```
make run
```

## Business IDs:
The Business Id corresponds to a specific business area for which you are requesting content. The business ids currently supported are:
* healthystockport
* stockportgov

## Environment
To change the environment the app is running under, set the `ASPNETCORE_ENVIRONMENT` environment variable

## Updating the styleguide shared CSS
To update the CSS within this project we are pulling the files from a shared repository "StockportStyleGuides".

We are running gulp tasks through Visual Studio. You can find the task runner in View -> Other Windows -> Task Runner Explorer.

* To pull in the styleguide artifacts, run the "pull-styleguide" gulp task from the runner window above
  - The .css files will be placed in `/wwwroot/assets/stylesheets/styleguide-hs.min.css` and `/wwwroot/assets/stylesheets/styleguide-sg.min.css`
  - The .scss files will be placed in `/wwwroot/assets/sass/styleguide/_ devices.scss` and `/wwwroot/assets/sass/styleguide/_ colors-sg.scss` (these are used in the gulp sass minify task for the webapp css)
  - This task is not set up to be automatically run and when you want new styleguide artifacts, if you have updated the styleguide you will need to manually run this task
  - This task is set up to pull from master (latest) and not via a tag.
* To pull in the styleguide artifacts from a tag, run the "pull-styleguide-from-tag" gulp task from the runner window above
  - This task will pull from the tag specified in the `StyleGuideVersion.lock` file.

## CSS in the webapp
If you are adding in specific css that will be used in either healthystockport or stockportgov then these css and sass changes are to be added into the webapp itself and not the shared styleguide as they will not be used elsewhere.
Sass is located in ``/wwwroot/assets/sass/``
* `site-hs.scss` for healthystockport
* `site-sg.scss` for stockportgov

These will automatically compile into minified `.css` files using the gulp task "css" and this will be running automatically on project open through the watch task. (Also it will run on build)

## Javascript in the webapp
Javascript is located in ``/assets/javascript/`` and will automatically concat and minify site specific javascript into two minified files
  * `healthystockport.min.js`
  * `stockportgov.min.js`

site.js is shared between both websites and will compile into both files.
The javascript minify task is set up run automatically through a gulp watch that runs "js:min".

## AppSettings configuration
The `appsettings.json` file will get transformed depending on the environment of the app that is running. All that needs to change is to define what environment the app is running under is the `ASPNETCORE_ENVIRONMENT` environment variable.
Currently there are transformations for `INT`, `QA`, `STAGING` and `PROD`. If no setting is set in a transformation, the site will use the default `appsettings.json`

The appsettings are organised into businessId specific settings.

```
{

  ...

  "businessId": {
    "setting": "a setting"
  },
  "another": {
    "setting": "the setting"
  }

}
```
This allows for the settings to be related to each businessId.

## AppSettings Feature Toggles:
The `featureToggles.yml` file will be read on startup to initialise `FeatureToggles`. To add a new feature toggle to the app:
1. Create a new property in `FeatureToggles` class with the name of the feature. Ensure you set it to `false` as default.
    - Here is a code snippet:

    ``` CSharp
        Class FeatureToggles {
            public bool ShowAlertInArticleView { get; set; } = false;
        }
    ```

2. Add the new property in the `featureToggles.yml` file. When adding a new feature toggle, always set it in prod environment first, then overwrite it where you need. The application loads the feature toggles based on the value of `ASPNETCORE_ENVIRONMENT`. Here is an example:

    ```Ruby
    prod: &default
        ShowAlertInArticleView: false
        SearchBar: true
    qa:
        <<: *default                # Inherits features from prod(aka default alias)
    int:
        <<: *default
        ShowAlertInArticleView: true    # Overwrite the value for int environment
    ```

3. Inject the `FeatureToggles` class where you need to use it (`Startup` does that for you, just introduce the param in the class constructor).

    ```CSharp
        Class NewClassWithFeatureToggle {
            private readonly FeatureToggles _featureToggles;

            public NewClassWithFeatureToggle(string someParam, FeatureToggles featureToggles) {
                _featureToggles = featureToggles
            }

            public void SomeMethod() {
                if (_featureToggles.ShowAlertInArticleView) {
                    // do something when feature toggle is on
                } else {
                    // do something else when feature toggle is off
                }
            }
        }
     ```

## UI Tests
The webapp has UI tests to ensure that the UI is expected. In order for this to happen the tests are written in a manner to represent a *users
journey* through the app.

### How to run
1. The UI tests run using nightwatch.js:

    ```
    npm install -g nightwatch
    ```

2. Within the test project is a UI test folder. Within it install the projects dependencies:

    ```
    cd test/StockportWebappTests/UI; npm install
    ```

3. In order to run the app, use the make command:

    ```
    make ui-test
    ```

    This runs the tests using the environment variable `BUSINESS_ID` in order to run the related UI tests. It also defaults to testing the
    application on the host `192.168.99.100:5000`. In order to change it set the environment variable `UI_TEST_HOST`.

    ```
    BUSINESS_ID=stockportgov UI_TEST_HOST=192.168.99.100:5000 make ui-test
    ```

- This will default to run using headless phantom js, however if you want to view the tests being ran, change the nightwatch.json to run with
firefox instead (this must be version 44 of firefox).

```
...
 "desiredCapabilities": {
        "browserName": "phantomjs", // change here to firefox
        "javascriptEnabled": true,
        "acceptSslCerts": true,
...
```

## How to write a test

The tests are written within `test/StockportWebappTests/UI/specs` folder (there is a `BUSINESS_ID` specific folder depending on which one you want
to test), and the page_objects that represent the specific page types are within `test/StockportWebappTests/UI/page_objects`. This means that you
define the tests within the `specs` folder, and set the page specific interactions in the `page_objects` folder.

### Prerequisites:
This app runs using at least .Net Core SDK version 1.0.0-preview2-003121.

## BrowserStack Selenium cloud testing:
<a href='https://www.browserstack.com/'><img src='https://www.browserstack.com/images/layout/browserstack-logo-600x315.png'/></a>
<br/>
Having used Browserstack for the trial period, we are satisfied that Browserstack has provided us with a quick solution to achieve the coverage of cross browser compatibility testing which was previously limited to browser capabilities available on our personal computers, tablets and mobile phones within the development team. We have quickly seen the benefits in quality assurance for our web applications as it provides us with a simple tool to test usability across all the major browsers we support like Safari, Opera, Firefox, IE, and Chrome all can be tested seamlessly using one tool. It has also helped with the challenge of testing of the web applications on latest browser version during development and maintaining compatibility across previous versions of the browser as well. Browserstack experience has been seamless as if the user is in a native environment with native browser behavior and development tools available to assist in troubleshooting any cross compatibility issues.
