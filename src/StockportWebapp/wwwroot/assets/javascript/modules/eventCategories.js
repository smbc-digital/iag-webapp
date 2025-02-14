define(["jquery"], function ($) { 
    var init = function () {
        $(document).ready(function () {
            var toggleButton = document.getElementsByClassName("event-homepage__filter-toggle")[0];
            var filterList = document.getElementById("eventFilters");
            var categoryWrapper = document.querySelector(".event-homepage__category-wrapper");
            var categoryForm = document.querySelector(".event-homepage__categories-form");

            var maxWidth = categoryForm ? 1024 : 767;

            toggleButton.addEventListener("click", function () {
                if (window.innerWidth <= maxWidth) { 
                    filterList.classList.toggle("show");
                    filterList.classList.toggle("event-homepage__categories--no-margin-top");
                    toggleButton.classList.toggle("btn--margin-bottom");

                    if (filterList.classList.contains("show")) {
                        categoryWrapper.classList.remove("event-homepage__category-wrapper--no-padding-bottom--non-desktop");

                        if (categoryForm) {
                            categoryForm.classList.add("event-homepage__categories-form--no-margin-bottom");
                        }

                        toggleButton.textContent = "Hide filters";
                    } else {
                        categoryWrapper.classList.add("event-homepage__category-wrapper--no-padding-bottom--non-desktop");

                        if (categoryForm) {
                            categoryForm.classList.remove("event-homepage__categories-form--no-margin-bottom");
                        }

                        toggleButton.textContent = "Show filters";
                    }
                }
            });

            window.addEventListener("resize", function () {
                if (window.innerWidth > maxWidth) { 
                    filterList.classList.remove("event-homepage__categories--no-margin-top");
                    filterList.classList.remove("show");
                    categoryWrapper.classList.add("event-homepage__category-wrapper--no-padding-bottom--non-desktop");

                    if (categoryForm) {
                        categoryForm.classList.remove("event-homepage__categories-form--no-margin-bottom");
                    }

                    toggleButton.textContent = "Show filters";
                } else {
                    if (!filterList.classList.contains("show")) {
                        toggleButton.classList.remove("btn--margin-bottom");
                        toggleButton.textContent = "Show filters";
                        categoryWrapper.classList.add("event-homepage__category-wrapper--no-padding-bottom--non-desktop");
                    }
                }
            });
        });
    };

    return {
        Init: init
    };
});