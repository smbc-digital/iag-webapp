define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicatorHero = document.querySelector(".carousel-indicators__hero");
                let currentIndex = 0;
                const totalSlides = carouselItems.length;

                // Helper: Generate indicators
                function generateIndicators() {
                    indicatorHero.innerHTML = "";
                
                    for (let i = 0; i < totalSlides; i++) {
                        let size = "hidden";
                
                        if (currentIndex <= 1 && i < 3) {
                            size = "big";
                        } else if (currentIndex === 2) {
                            if ( i === 0 ) size = "small"
                            if (i === 1 || i === 2 || i === 3) size = "big";
                            if (i === 4) size = "small"; // Show index 4 as "small"
                        } else if (currentIndex === 3) {
                            if (i === 2 || i === 3 || i === 4) size = "big"; // Show 3 as "big"
                            if (i === 1 || i === 5) size = "small"; // Show 1 and 4 as "small"
                        } else if (currentIndex === 4) {
                            if (i === 3 || i === 4) size = "big"; // Show 2, 3, 4 as "big"
                            if (i === 2) size = "small"; // Show index 1 as "small"
                        }
                
                        if (size !== "hidden") {
                            const li = createIndicator(size, i);
                            indicatorHero.appendChild(li);
                        }
                    }
                }
                
                // Helper: Create an indicator
                function createIndicator(size, index) {
                    const li = document.createElement("li");
                    const span = document.createElement("span");
                    span.classList.add("carousel-indicators__item");
                    if (size === "small") {
                        span.classList.add("small");
                    }

                    // Add current class if this is the current index
                    if (index === currentIndex) {
                        span.classList.add("current");
                    }

                    li.appendChild(span);
                    return li;
                }

                // Update carousel and indicators
                function updateCarousel() {
                    generateIndicators();

                    // Transform slides for the carousel
                    document.querySelector(".carousel-items").style.transform = `translateX(-${currentIndex * 100}%)`;

                    // Aria attributes for accessibility
                    carouselItems.forEach((slide, index) => {
                        slide.setAttribute("aria-hidden", index !== currentIndex ? "true" : "false");
                    });
                }

                // Event listeners
                document.querySelector(".next").addEventListener("click", function () {
                    currentIndex = (currentIndex + 1) % totalSlides;
                    updateCarousel();
                });

                document.querySelector(".prev").addEventListener("click", function () {
                    currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
                    updateCarousel();
                });

                // Initialize carousel
                updateCarousel();
            };
        }
    };
});
