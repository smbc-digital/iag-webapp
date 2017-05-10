# ---------------------------------------------------------------------------------------
# -- ui-test-all
# -- USAGE: UI_TEST_HOST=http://localhost:5000 Make ui-test-all
# ---------------------------------------------------------------------------------------
.PHONY: ui-test-all
ui-test-all:
	node configureGlobals.js host=$(UI_TEST_HOST)	
	node nightwatch.js --env stockportgov --group stockportgov
	node nightwatch.js --env healthystockport --group healthystockport

	
# ---------------------------------------------------------------------------------------
# -- ui-test
# -- USAGE: BUSINESS_ID=stockportgov UI_TEST_HOST=http://stockportgov.local:5555 Make ui-test
# ---------------------------------------------------------------------------------------
.PHONY: ui-test
ui-test:
	node configureGlobals.js host=$(UI_TEST_HOST)
	node nightwatch.js --env $(TEST_OS) --group $(BUSINESS_ID)

	
# ---------------------------------------------------------------------------------------
# -- ui-test
# -- USAGE: BUSINESS_ID=stockportgov UI_TEST_HOST=http://stockportgov.local:5555 Make ui-test-specific testname="Find article for About the Hat Works"
# ---------------------------------------------------------------------------------------
.PHONY: ui-test-specific
ui-test-specific:
	node configureGlobals.js host=$(UI_TEST_HOST)
	node nightwatch.js --env $(TEST_OS) --group $(BUSINESS_ID) --test specs/$(BUSINESS_ID)/userjourney.tests.js --testcase '$(testname)'
	
