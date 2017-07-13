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

.PHONY: clean
clean:
	rm -rf $(APPLICATION_ROOT_PATH)/bin

.PHONY: dotnet-restore
dotnet-restore:
	dotnet restore

.PHONY: dotnet-test
dotnet-test:
	cd test/StockportWebappTests; dotnet test

.PHONY: publish-app
publish-app:
	cd src/StockportWebapp; dotnet publish --configuration Release -o publish;

.PHONY: version
version:
	git rev-parse HEAD > src/$(PROJECT_NAME)/sha.txt
	echo $(APP_VERSION) > src/$(PROJECT_NAME)/version.txt

.PHONY: package-app
package-app:
	rm -f iag-webapp.zip
	cd $(APPLICATION_PUBLISH_PATH); zip -r ../../../iag-webapp.zip ./*

# .PHONY: build
# build:
# 	git rev-parse HEAD > src/$(PROJECT_NAME)/sha.txt
# 	echo $(APP_VERSION) > src/$(PROJECT_NAME)/version.txt
# 	./docker.sh build \
# 			$(IMAGE) \
# 			$(TAG) \
# 			Dockerfile

.PHONY: start-proxy
start-proxy:
	cd proxy ; npm install ; node index.js

# ---------------------------------------------------------------------------------------
# -- UI Tests
# ---------------------------------------------------------------------------------------


# ---------------------------------------------------------------------------------------
# -- ui-test-all
# -- USAGE: UI_TEST_HOST=http://localhost:5000 Make ui-test-all
# ---------------------------------------------------------------------------------------
.PHONY: ui-test-all
ui-test-all:
	cd test/StockportWebappTests/UI && make ui-test-all


# ---------------------------------------------------------------------------------------
# -- ui-test
# -- USAGE: BUSINESS_ID=stockportgov UI_TEST_HOST=http://stockportgov.local:5555 Make ui-test
# ---------------------------------------------------------------------------------------
.PHONY: ui-test
ui-test:
	cd test/StockportWebappTests/UI && make ui-test


# ---------------------------------------------------------------------------------------
# -- ui-test
# -- USAGE: BUSINESS_ID=stockportgov UI_TEST_HOST=http://stockportgov.local:5555 Make ui-test-specific testname="Find article for About the Hat Works"
# ---------------------------------------------------------------------------------------
.PHONY: ui-test-specific
ui-test-specific:
	cd test/StockportWebappTests/UI && make ui-test-specific testname="$(testname)"


# ---------------------------------------------------------------------------------------
# -- js-tests
# -- USAGE: make js-tests
# ---------------------------------------------------------------------------------------
.PHONY: js-tests
js-tests:
	cd test\StockportWebappTests\JSTests ; npm install ; npm install -g karma-cli ; karma start karma.conf.js --single-run
