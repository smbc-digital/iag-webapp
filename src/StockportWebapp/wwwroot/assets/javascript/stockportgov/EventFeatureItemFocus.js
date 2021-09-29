var topic = document.getElementsByClassName('featured-topic-list');

window.onload = function () {
    $("#see-more-services").click(function () {
        $(topic).focus();
    });
};

