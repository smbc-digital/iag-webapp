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


describe("Location search tests", function() {
    it("should set location properties", function () {
        Location.SetLocation("name", 1.1, 2.2);

        expect(Location.Name).toEqual("name");
        expect(Location.Latitude).toEqual(1.1);
        expect(Location.Longitude).toEqual(2.2);
    });
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