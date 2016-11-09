var methods = {
    assertStartButtonIsVisible: function () {
        this.waitForElementVisible('@startButton', this.api.globals.timeOut)
            .expect.element('@startButton').text.to.equal('Start now');
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + 'start';
    },
    elements: {
        startButton: 'a.button-call-to-action'
    }
};
