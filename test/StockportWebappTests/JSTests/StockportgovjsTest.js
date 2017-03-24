describe("setEndDateToStartDate", function (){
    beforeEach(function (){
        affix('input#input1[value="2"] input#input2[value=""]')
    });
    
    it("sets end date to be start date", function (){
        Modernizr.inputtypes.date = false;
        
        setEndDateToStartDate("input2", "input1");               
        expect($("#input2").val()).toEqual($("#input1").val());
    });

});

describe("hide global alert", function () {
    beforeEach(function() {
        affix("div.global-alert div.global-alert-close-container a");
    });
    it("should close global-alert-container", function () {
        expect($(".global-alert").css("display")).toEqual("block");
        $link = $('.global-alert-close-container a');
        var spyButtonClick = spyOn($link,"click")        
        $link.click();       
        expect($(".global-alert").css("display")).toEqual("none");
    });    
});