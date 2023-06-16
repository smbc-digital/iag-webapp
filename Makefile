b.PHONY: help
help:
		@cat ./MakefileHelp

# ---------------------------------------------------------------------------------------
# -- Dotnet commands
# ---------------------------------------------------------------------------------------
.PHONY: build
build:
	cd src/StockportWebapp; dotnet build

.PHONY: run
run:
	dotnet run --project ./src/StockportWebapp/StockportWebapp.csproj --urls="http://localhost:5002;https://localhost:5003"

.PHONY: dotnet-restore
dotnet-restore: restore

.PHONY: restore
restore:
	dotnet restore

.PHONY: publish
publish:
	@echo Publishing application
	cd ./src/StockportWebapp && dotnet publish -c Release -o publish

# ---------------------------------------------------------------------------------------
# -- Unit-test
# ---------------------------------------------------------------------------------------
.PHONY: unit-test
unit-test:
	cd test/StockportWebappTests; dotnet test

# ---------------------------------------------------------------------------------------
# -- Unit tests coverage
# ---------------------------------------------------------------------------------------
.PHONY: coverage
coverage:
	cd test/StockportWebappTests;rm TestResults -r -f
	dotnet build
	dotnet test -l "console;verbosity=normal" -p:CollectCoverage=true -p:CoverletOutputFormat=\"opencover\" -p:CoverletOutput=TestResults/Coverage.xml -p:SkipAutoProps=true /p:Exclude=\"[*]StockportWebapp.ContentFactory*,**/Constants/*,[*]StockportWebapp.Models.Config*,[*]StockportWebapp.Models.Emails*,[*]StockportWebapp.Models.Enums*,[*]StockportWebapp.Models.Exceptions*,[*]StockportWebapp.Models.Groups*,[*]StockportWebapp.Models.Responses*\" /p:ExcludeByFile=\"**/StockportWebapp/Views/**/*.cshtml,**/StockportWebapp/EmailTemplates/*.cshtml\" -p:ExcludeByAttribute="ExcludeFromCodeCoverage"

# ---------------------------------------------------------------------------------------
# -- Unit tests coverage with threshold
# ---------------------------------------------------------------------------------------
.PHONY: coverage-threshold
coverage-threshold:
	cd test/StockportWebappTests;rm TestResults -r -f
	dotnet test -l "console;verbosity=normal" -p:CollectCoverage=true -p:CoverletOutputFormat=\"opencover\" -p:CoverletOutput=TestResults/Coverage.xml -p:SkipAutoProps=true /p:Exclude=\"[*]StockportWebapp.ContentFactory*,**/Constants/*,[*]StockportWebapp.Models.Config*,[*]StockportWebapp.Models.Emails*,[*]StockportWebapp.Models.Enums*,[*]StockportWebapp.Models.Exceptions*,[*]StockportWebapp.Models.Groups*,[*]StockportWebapp.Models.Responses*\" /p:ExcludeByFile=\"**/StockportWebapp/Views/**/*.cshtml,**/StockportWebapp/EmailTemplates/*.cshtml\" -p:ExcludeByAttribute="ExcludeFromCodeCoverage" -p:Threshold=$(threshold)

# ---------------------------------------------------------------------------------------
# -- Unit tests coverage report, opens index.html in Chrome
# ---------------------------------------------------------------------------------------
.PHONY: report
report:
	cd test/StockportWebappTests; rm TestCoverageResults -r -f && \
	reportgenerator -reports:TestResults/Coverage.xml -targetdir:TestCoverageResults -reporttypes:Html && \
	start Chrome $$PWD/TestCoverageResults/index.html


.PHONY: report-teamcity
report-teamcity:
	cd test/StockportWebappTests
	reportgenerator -reports:TestResults/coverage.xml -reporttypes:"TeamCitySummary;HtmlSummary" -targetdir:TestResults

# ---------------------------------------------------------------------------------------
# -- Unit tests coverage tools
# ---------------------------------------------------------------------------------------
.PHONY: coverage-tools
coverage-tools: 
	cd test/StockportWebappTests && \
    dotnet tool install -g dotnet-reportgenerator-globaltool && \
    echo "Please ensure that the path environment variable us updated with `C:\Users\{​​​​​​​​​​​​​​​​​​​​​​your-user}​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​​\.dotnet\tools`" && \
    reportgenerator

# ---------------------------------------------------------------------------------------
# -- Unit tests clear coverage report
# ---------------------------------------------------------------------------------------
.PHONY: clear-report
clear-report:
	cd test/StockportWebappTests;rm TestResults -r -f; rm TestCoverageResults -r -f

# ---------------------------------------------------------------------------------------
# -- Ui-test
# ---------------------------------------------------------------------------------------
.PHONY: ui-test
ui-test:
	cd test/StockportWebappTests_UI/ && ./runtests.cmd

# ---------------------------------------------------------------------------------------
# -- Gulp tasks
# ---------------------------------------------------------------------------------------
.PHONY: css
css:
	cd src/StockportWebapp && gulp css

.PHONY: js
js:
	cd src/StockportWebapp && gulp js

.PHONY: lint
css:
	cd src/StockportWebapp && npm run lint

.PHONY: build
js:
	cd src/StockportWebapp && gulp build

.PHONY: watch
watch:
	cd src/StockportWebapp && gulp watch
	