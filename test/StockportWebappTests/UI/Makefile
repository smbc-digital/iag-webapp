
# UI tests
# ---------------------------------------------------------------------------------------
# @if [[ -z "$$BUSINESS_ID" ]]; then echo "[FAIL] BUSINESS_ID not set" && exit 1; fi

# @if [[ -z "$$BUSINESS_ID" ]]; then echo "[FAIL] BUSINESS_ID not set" && exit 1; fi

.PHONY: ui-test
ui-test:
	node configureGlobals.js host=$(UI_TEST_HOST)	
	node nightwatch.js --group $(BUSINESS_ID)

.PHONY: ui-test-specific
ui-test-specific:
	node configureGlobals.js host=$(UI_TEST_HOST)	
	node_modules/nightwatch/bin/nightwatch --group $(BUSINESS_ID)	--test specs/stockportgov/userjourney.tests.js --testcase "$(testname)"
