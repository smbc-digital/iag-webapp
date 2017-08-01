describe("Date tests", function (){
    beforeEach(function () {
        affix('input#input1[value="2"] input#input2[value=""]');
    });
    
    it("sets end date to be start date", function (){
        Modernizr.inputtypes.date = false;
        
        setEndDateToStartDate("input2", "input1");               
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
            SwapLogo();
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
            SwapLogo();
            // assert
            expect($(".logo-main-image").attr("src")).toEqual("desktopimage.jpg");
        });
});

describe("Utils tests",
    function () {

<<<<<<< HEAD
describe("Util Tests", function() {
    beforeEach(function() {
    });

    it("should strip param from url", function () {

        var result = STK.Utils.StripParamFromQueryString('https://www.here.com?val1=this&val2=that', 'val1')

        expect(Location.Longitude).toEqual('https://www.here.com?val2=that');
=======
        var tests = [
            {
                url:"http://www.test.com?param1=test",
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

>>>>>>> c02f161bee6e692178dc0388a3a12c78a2d9329c
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