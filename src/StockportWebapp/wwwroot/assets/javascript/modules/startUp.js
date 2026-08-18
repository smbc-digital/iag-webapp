define(["jquery", "utils"], function ($, utils) {

    var documentReady = function () {

        utils.SwapLogo();

        /*Duplicating this to work with newer header - this will need removing as needed - this is also used in HS so need to be aware*/
        $(".show-search-button").click(function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        });

        /*This is the behaviour for the new header search - potentially for the "mege menu" */
        $(".site-header_mobile-search-button").click(function () {
            $("#siteHeaderMobileSearchInput").slideToggle(220);
            
            var isNotDesktop = window.matchMedia("(max-width: 1024px)").matches;
            if (isNotDesktop) {
                var expanded = $(this).attr("aria-expanded") === "true" || false;

                $(this).attr("aria-expanded", !expanded);
                $(this).attr("aria-label", expanded ? "Open search" : "Close search");
            }
        });

        var mobileMenuToggle = document.getElementById('mobileMenuToggle');
        
        if (mobileMenuToggle) {
            mobileMenuToggle.addEventListener('click', function (e) {
                e.preventDefault();
                var menuContent = document.getElementById('siteHeaderMobileMenuContent');
                var heroWrapper = document.querySelector('.homepageHero_wrapper--mobile');
                var expanded = this.getAttribute('aria-expanded') === 'true';

                if (expanded) {
                    if (menuContent) { menuContent.setAttribute('hidden', ''); menuContent.style.display = 'none'; }
                    this.querySelector('.fa').className = 'fa fa-bars';
                    this.lastChild.textContent = 'Menu';
                    if (heroWrapper) heroWrapper.style.display = '';
                } else {
                    if (menuContent) { menuContent.removeAttribute('hidden'); menuContent.style.display = 'block'; }
                    this.querySelector('.fa').className = 'fa fa-times';
                    this.lastChild.textContent = 'Close';
                    if (heroWrapper) heroWrapper.style.display = 'none';
                }

                this.setAttribute('aria-expanded', !expanded);
                this.setAttribute('aria-label', expanded ? 'Menu' : 'Close menu');
            });
        }

        if (isIE()) {
            $("#browser-check").removeClass("hidden");

            var element = document.getElementById("browser-check");
            element.className = element.className.replace(/\bhidden\b/g, "");
        }
    };

    var documentResize = function () {

        utils.SwapLogo();

        if ($(window).width() > utils.TabletWidth) {
            $("#mobileSearchInput").hide();
            $(".show-search-button").removeClass("arrow");
            $('#displayRefineBy').css('display', 'block');
        }
    };

    var cookieNoticeAttributeRemoval = function () {
        var cookieNotice = $("#freeprivacypolicy-com---nb")
        if (cookieNotice != null) {
            cookieNotice.removeAttr("role");
            cookieNotice.removeAttr("aria-modal");
        }
    }

    var isIE = function (userAgent) {
        userAgent = userAgent || navigator.userAgent;
        return userAgent.indexOf("MSIE ") > -1 || userAgent.indexOf("Trident/") > -1 || userAgent.indexOf("Edge/") > -1;
    }

    return {
        Init: function () {

            if (isIE()) {
                $("html").addClass("ie");
            }

            $(document).ready(function () {
                documentReady();
                cookieNoticeAttributeRemoval();
            });

            $(window).resize(function () {
                documentResize();
            });
        }
    }
});

