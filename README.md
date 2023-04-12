# Stockport Web App

***
## Running the app
**First;** you need to run the [ContentAPI](https://github.com/smbc-digital/iag-contentapi) so that WebApp will have content to serve. This, by default, runs on http://localhost:5000 and is set in the appsettings of the WebApp, if you want to change the port for some reason. You can launch the ContentAPI on either the command line or IIS Express profiles.

**Second.** The application runs using a BUSINESS-ID header to decide which specific business to
run per request. Currently set in BusinessIdMiddleware.cs. The business ids currently supported are:
* healthystockport
* stockportgov

VS Code -or- Command line: *`Make run`
Visual Studio *2022: as usual with launch settings to adjust.

**Other **"Make"** commands in the Makefile*
**this project is now on NET 6 and requires a minimum of Visual Studio 2022*

***
## Configuration
The app has "appsettings" files which are kept in `src/StockportWebapp/app-config/`
```json
{
  "healthystockport": {
    "setting": "value"
  },
  "stockportgov": {
    "setting": "value"
  }

}
```
This allows for the settings to be related to each businessId.

***
## Secrets - inside the /app-config/ folder
There are no secrets held in this repository.
See seperate Wiki on GitLab for decrypting and encrypting secrets for IAG-WebApp & ContentAPI.

***
## Feature Toggles
The `featureToggles.yml` file will be read on startup to initialise `FeatureToggles`. To add a new feature toggle to the app:
1. Create a new property in `FeatureToggles` class with the name of the feature. A bool is false by default.

```CSharp
    Class FeatureToggles
    {
        public bool ShowAlertInArticleView { get; set; }
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
  Class NewClassWithFeatureToggle
  {
      private readonly FeatureToggles _featureToggles;

      public NewClassWithFeatureToggle(string someParam, FeatureToggles featureToggles)
      {
          _featureToggles = featureToggles
      }

      public void SomeMethod()
      {
          if (_featureToggles.ShowAlertInArticleView)
          {
              // do something
          }
      }
  }
```

***
## Style
This is an ongoing area of work - 2023

Currently, to install the node packages required for the gulp tasks, you need to run `npm install`.  
Once you've ran that command inside src/StockportWebApp you will be able to see the Gulp tasks in the Visual Studio Task Runner Explorer, or use the Gulp cli.

Sass is located in ``/wwwroot/assets/sass/../``

The final minified css is "compiled" to the ~/assets/stylesheets/ folder through gulp sass from the import statements in the following files:
* /assets/sass/`site-hs.scss` for healthystockport
* /assets/sass/`site-sg.scss` for stockportgov

These will automatically compile into minified `.css` files using the gulp task "css" and this will be running automatically on project open through the watch task. (Also it will run on build)

***
## Javascript
All the Javascript is located in ``/assets/javascript/``.
The JS Modules are loaded on to the page with require.js.

The JavaScript minify task is set up run automatically through a gulp task "watch".
Although it does take a while to run for SCSS changes, so you can just run the gulp "js/css" task if you edit a .js/.scss file and want to see or push your changes.

***
## UI Tests
Will be looked at and updated in the near future ( said on the 27th January 2023 ).
