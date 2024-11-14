define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicators = document.querySelectorAll(".carousel-indicators button");
                const status = document.getElementById("carousel-status");
                let currentIndex = 0;

                function updateCarousel() {
                    document.querySelector(".carousel-items").style.transform = `translateX(-${currentIndex * 100}%)`;

                    indicators.forEach((indicator, index) => {
                        indicator.classList.toggle("active", index === currentIndex);
                    });

                    status.textContent = `Slide ${currentIndex + 1} of ${carouselItems.length}`;
                }

                document.querySelector(".next").addEventListener("click", function () {
                    currentIndex = (currentIndex + 1) % carouselItems.length;
                    updateCarousel();
                });

                document.querySelector(".prev").addEventListener("click", function () {
                    currentIndex = (currentIndex - 1 + carouselItems.length) % carouselItems.length;
                    updateCarousel();
                });

                document.querySelector(".mob-next").addEventListener("click", function () {
                    currentIndex = (currentIndex + 1) % carouselItems.length;
                    updateCarousel();
                });

                document.querySelector(".mob-prev").addEventListener("click", function () {
                    currentIndex = (currentIndex - 1 + carouselItems.length) % carouselItems.length;
                    updateCarousel();
                });

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