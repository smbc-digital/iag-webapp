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


# UI tests
# ---------------------------------------------------------------------------------------
ifeq ($(OS),Windows_NT)
TEST_OS = default
else
TEST_OS = linux
endif

.PHONY: ui-test
ui-test:
	node test/StockportWebappTests/UI/configureGlobals.js host=$(UI_TEST_HOST)
	@if [[ -z "$$BUSINESS_ID" ]]; then echo "[FAIL] BUSINESS_ID not set" && exit 1; fi
	test/StockportWebappTests/UI/node_modules/nightwatch/bin/nightwatch --env $(TEST_OS) --group $(BUSINESS_ID)

.PHONY: ui-test-specific
ui-test-specific:
	node test/StockportWebappTests/UI/configureGlobals.js host=$(UI_TEST_HOST)
	@if [[ -z "$$BUSINESS_ID" ]]; then echo "[FAIL] BUSINESS_ID not set" && exit 1; fi
	test/StockportWebappTests/UI/node_modules/nightwatch/bin/nightwatch --env $(TEST_OS) --group $(BUSINESS_ID)	--test test/StockportWebappTests/UI/specs/stockportgov/userjourney.tests.js --testcase "$(testname)"


