define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicators = document.querySelectorAll(".carousel-indicators button");
                const status = document.getElementById("carousel-status");
                let currentIndex = 0;
                    
                status.setAttribute("aria-live", "off");

                function updateCarousel() {
                    document.querySelector(".carousel-items").style.transform = `translateX(-${currentIndex * 100}%)`;
                    
                    carouselItems.forEach((slide, index) => {
                        slide.setAttribute("aria-hidden", index !== currentIndex ? "true" : "false");
                        document.getElementsByClassName("carousel-item__link")[index]?.setAttribute("tabindex", index !== currentIndex ? "-1" : "0")

                        if(index === currentIndex)
                            slide.focus()
                    });

                    if (indicators.length) {
                        indicators.forEach((indicator, index) => {
                            indicator.classList.toggle("current", index === currentIndex);
                            if (index === currentIndex) {
                                document.getElementById(index).innerHTML = "Current slide"
                            }
                            else {
                                document.getElementById(index).innerHTML = ""
                            }
                        });
                    }

                    const currentSlide = carouselItems[currentIndex];
                    const slideImage = currentSlide.querySelector("img");
                    const slideTitle = currentSlide.querySelector(".carousel-item__title")?.textContent.trim() || "";
                    const slideDate = currentSlide.querySelector("p")?.textContent.trim() || "";
                    
                    const slideDetails = slideImage
                        ? `Image: ${slideImage.alt}`
                        : `${slideTitle}. ${slideDate}.`.trim();
                    
                    if (status.getAttribute("aria-live") === "polite") {
                        status.textContent = `Slide ${currentIndex + 1} of ${carouselItems.length}: ${slideDetails}`;
                    }
                }

                setTimeout(() => {
                    status.setAttribute("aria-live", "polite");
                }, 500);

                document.querySelector(".next").addEventListener("click", function () {
                    currentIndex = (currentIndex + 1) % carouselItems.length;
                    updateCarousel();
                });

                document.querySelector(".prev").addEventListener("click", function () {
                    currentIndex = (currentIndex - 1 + carouselItems.length) % carouselItems.length;
                    updateCarousel();
                });

                if (document.querySelector(".mob-next") != null) {
                    document.querySelector(".mob-next").addEventListener("click", function () {
                        currentIndex = (currentIndex + 1) % carouselItems.length;
                        updateCarousel();
                    });

                    document.querySelector(".mob-prev").addEventListener("click", function () {
                        currentIndex = (currentIndex - 1 + carouselItems.length) % carouselItems.length;
                        updateCarousel();
                    });
                }

                indicators.forEach((indicator, index) => {
                    indicator.addEventListener("click", function () {
                        currentIndex = index;
                        updateCarousel();
                    });
                });

                updateCarousel();
            }
        }
    }
})