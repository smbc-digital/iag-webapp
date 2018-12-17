.PHONY: help
help:
		@cat ./MakefileHelp

require-envs-%:
	@for i in $$(echo $* | sed "s/,/ /g"); do \
          if [[ -z $${!i} ]]; then echo "[FAIL] Environment variable $$i is not set." && FAIL=yes; fi \
        done; \
        if [[ -n $$FAIL ]]; then echo "Aborting..." && exit 1; fi

# Project automation targets:
# ---------------------------------------------------------------------------------------

APPLICATION_ROOT_PATH = ./src/StockportWebapp
APPLICATION_PUBLISH_PATH = $(APPLICATION_ROOT_PATH)/publish/

PROJECT_NAME = StockportWebapp
APP_VERSION ?= $(BUILD_NUMBER)

.PHONY: build
build: clean dotnet-restore dotnet-test version publish-app package-app

.PHONY: run
run:
	cd src/StockportWebapp; dotnet run

.PHONY: clean
clean:
	rm -rf $(APPLICATION_ROOT_PATH)/bin

.PHONY: build-and-run 
build-and-run:         
	cd src/StockportWebapp; dotnet build; dotnet run

.PHONY: dotnet-restore
dotnet-restore:
	dotnet restore

.PHONY: dotnet-test
dotnet-test:
	cd test/StockportWebappTests; dotnet test

.PHONY: test
test:
	cd test/StockportWebappTests; dotnet test

.PHONY: test-all
test-all: test js-tests

.PHONY: publish-app
publish-app:
	@echo Publishing application
	cd ./src/StockportWebapp && dotnet publish -c Release -o publish

.PHONY: version
version:
	git rev-parse HEAD > src/$(PROJECT_NAME)/sha.txt
	echo $(APP_VERSION) > src/$(PROJECT_NAME)/version.txt

.PHONY: package-app
package-app:
	rm -f iag-webapp.zip
	cd $(APPLICATION_PUBLISH_PATH); zip -r ../../../iag-webapp.zip ./*

.PHONY: start-proxy
start-proxy:
	cd proxy ; npm install ; node index.js

# ---------------------------------------------------------------------------------------
# -- UI Tests
# ---------------------------------------------------------------------------------------


# ---------------------------------------------------------------------------------------
# -- ui-test
# ---------------------------------------------------------------------------------------
.PHONY: ui-test
ui-test:
	cd test/StockportWebappTests_UI/ && ./runtests.cmd

# ---------------------------------------------------------------------------------------
# -- js-tests
# -- USAGE: make js-tests
# ---------------------------------------------------------------------------------------
.PHONY: js-tests
js-tests:
	cd test/StockportWebappTests/JSTests && npm install && cd node_modules/karma/bin && node karma start ../../../karma.conf.js --single-run


# ---------------------------------------------------------------------------------------
# -- Gulp tasks
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
