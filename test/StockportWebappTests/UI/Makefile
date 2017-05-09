
# UI tests
# ---------------------------------------------------------------------------------------

.PHONY: ui-test-all
ui-test-all:
	node configureGlobals.js host=$(UI_TEST_HOST)	
	node nightwatch.js --env stockportgov --group stockportgov
	node nightwatch.js --env healthystockport --group healthystockport

.PHONY: ui-test
ui-test:
	node configureGlobals.js host=$(UI_TEST_HOST)
	node nightwatch.js --env $(TEST_OS) --group $(BUSINESS_ID)

.PHONY: ui-test-specific
ui-test-specific:
	node configureGlobals.js host=$(UI_TEST_HOST)
	node nightwatch.js --env $(TEST_OS) --group $(BUSINESS_ID) --test test/StockportWebappTests/UI/specs/stockportgov/userjourney.tests.js --testcase '$(testname)'
	
