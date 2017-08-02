describe("Date tests", function () {
    beforeEach(function () {
        affix('input#input1[value="2"] input#input2[value=""]');
    });

    it("sets end date to be start date", function () {
        Modernizr.inputtypes.date = false;

        STK.Events.SetEndDateToStartDate("input2", "input1");
        expect($("#input2").val()).toEqual($("#input1").val());
    });
});

describe("Logo tests", function () {
    beforeEach(function () {
        affix('#header .logo-main-image[data-mobile-image="mobileimage.jpg"][data-desktop-image="desktopimage.jpg"]');
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
            STK.Utils.SwapLogo();
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
            STK.Utils.SwapLogo();
            // assert
            expect($(".logo-main-image").attr("src")).toEqual("desktopimage.jpg");
        });
});

describe("Utils tests",
    function () {

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
            it("Should strip a param from a query string",
                function () {
                    var urlReturned = STK.Utils.StripParamFromQueryString(url, param);

                    expect(urlReturned).toEqual(result);
                });
        }

        for (var test in tests) {
            addTest(tests[test].url, tests[test].param, tests[test].result);
        }

    });


function $sandbox(content) {
    var $box = $('#sandbox');
    if ($box.length == 0) {
        $box = $('<div id="sandbox"></div>').appendTo('body');
    }
    if (content == undefined) {
        return $box;
    }
    return $box.html(content);
}