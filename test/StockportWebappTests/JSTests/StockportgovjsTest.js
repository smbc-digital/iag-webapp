define(['utils', 'events', 'modernizr'], function (utils, events, modernizr) {

    describe("Very simple test", function () {
        it("is a simple test", function () {
            expect("A").toEqual("A");
        });
    });

    describe("Date tests", function () {
        // inject the HTML fixture for the tests
        beforeEach(function () {
            var fixture = '<div id="fixture"><input id="input1" type="text" value="2" /><input id="input2" type="text" value="" /></div>';

            document.body.insertAdjacentHTML(
                'afterbegin',
                fixture);
        });

        it("sets end date to be start date", function () {
            events.SetEndDateToStartDate("input2", "input1");
            expect($("#input2").val()).toEqual($("#input1").val());
        });

        // remove the html fixture from the DOM
        afterEach(function () {
            document.body.removeChild(document.getElementById('fixture'));
        });
    });

    describe("Logo tests", function () {
        // inject the HTML fixture for the tests
        beforeEach(function () {
            var fixture = '<div id="fixture"><div id="header"><img class="logo-main-image" data-mobile-image="mobileimage.jpg" data-desktop-image="desktopimage.jpg"></div></div></div>';

            document.body.insertAdjacentHTML(
                'afterbegin',
                fixture);
        });

        it("will set the logo to be mobile",
            function () {
                // arrage
                spyOn($.prototype, 'width').and.callFake(function () {
                    if (this[0] === window) {
                        return 200;
                    }
                });
                // act
                utils.SwapLogo();
                // assert
                expect($(".logo-main-image").attr("src")).toEqual("mobileimage.jpg");
            });

        it("will set the logo to be desktop",
            function () {
                // arrage
                spyOn($.prototype, 'width').and.callFake(function () {
                    if (this[0] === window) {
                        return 1000;
                    }
                });
                // act
                utils.SwapLogo();
                // assert
                expect($(".logo-main-image").attr("src")).toEqual("desktopimage.jpg");
            });

        // remove the html fixture from the DOM
        afterEach(function () {
            document.body.removeChild(document.getElementById('fixture'));
        });
    });

    describe("Utils tests", function () {

            var tests = [
                {
                    url: "http://www.test.com?param1=test",
                    param: "param1",
                    result: "http://www.test.com"
                },
                {
                    url: "http://www.test.com?param1=test&param2=testest",
                    param: "param1",
                    result: "http://www.test.com?param2=testest"
                }
            ];

            function addTest(url, param, result) {
                it("Should strip a param from a query string", function () {
                        var urlReturned = utils.StripParamFromQueryString(url, param);

                        expect(urlReturned).toEqual(result);
                    });
            }

            for (var test in tests) {
                addTest(tests[test].url, tests[test].param, tests[test].result);
            }

        });
}); 