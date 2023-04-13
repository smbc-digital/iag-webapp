.PHONY: help
help:
		@cat ./MakefileHelp

.PHONY: test-all
test-all: test js-test

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

.PHONY: test
test: unit-test integration-test

# ---------------------------------------------------------------------------------------
# -- Unit-test
# ---------------------------------------------------------------------------------------
.PHONY: unit-test
unit-test:
	cd test/StockportWebappTests; dotnet test

# ---------------------------------------------------------------------------------------
# -- Integration-test
# ---------------------------------------------------------------------------------------
.PHONY: integration-test
integration-test:
	cd test/StockportWebappTests_Integration; dotnet test

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
	