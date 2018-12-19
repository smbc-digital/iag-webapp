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
	cd src/StockportWebapp; dotnet run

.PHONY: restore
dotnet-restore:
	dotnet restore

.PHONY: test
test:
	cd test/StockportWebappTests; dotnet test
	
# ---------------------------------------------------------------------------------------
# -- Ui-test
# ---------------------------------------------------------------------------------------
.PHONY: ui-test
ui-test:
	cd test/StockportWebappTests_UI/ && ./runtests.cmd

# ---------------------------------------------------------------------------------------
# -- JavaScript
# ---------------------------------------------------------------------------------------
.PHONY: js-build
js-build:
	@echo Installing, cleaning and building JavaScript files
	make npm-install
	cd ./src/StockportWebapp && npm run js:clean 
	cd ./src/StockportWebapp && npm run js:compile

.PHONY: js-tests
js-tests: js-test

.PHONY: js-test
js-test:
	cd test/StockportWebappTests/JSTests && npm install && cd node_modules/karma/bin && node karma start ../../../karma.conf.js --single-run

# ---------------------------------------------------------------------------------------
# -- Gulp tasks - Are these needed?
# ---------------------------------------------------------------------------------------
.PHONY: css
css:
	cd src/StockportWebapp && gulp css

.PHONY: js
js:
	cd src/StockportWebapp && gulp min:js

.PHONY: js-config
js-config:
	cd src/StockportWebapp && gulp min:config:all

.PHONY: js-all
js-all:
	cd src/StockportWebapp && gulp min:js && gulp min:config:all
