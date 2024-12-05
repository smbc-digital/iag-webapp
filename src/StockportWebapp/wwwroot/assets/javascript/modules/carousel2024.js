define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicatorHero = document.querySelector(".carousel-indicators");
                let currentIndex = 0;
                const totalSlides = carouselItems.length;

                function generateIndicators() {
                    indicatorHero.innerHTML = ""; // Clear existing indicators
                
                    for (let i = 0; i < totalSlides; i++) {
                        let size = "hidden";
                
                        // Define size based on currentIndex and position
                        if (currentIndex <= 1 && i < 3) {
                            size = "big";
                        } else if (currentIndex === 2) {
                            if (i === 0) size = "small";
                            if (i === 1 || i === 2 || i === 3) size = "big";
                            if (i === 4) size = "small";
                        } else if (currentIndex === 3) {
                            if (i === 2 || i === 3 || i === 4) size = "big";
                            if (i === 1 || i === 5) size = "small";
                        } else if (currentIndex === 4) {
                            if (i === 3 || i === 4 || i === 5) size = "big";
                            if (i === 2 || i === 6) size = "small";
                        } else if (currentIndex === 5) {
                            if (i === 4 || i === 5 || i === 6) size = "big";
                            if (i === 3 || i === 7) size = "small";
                        } else if (currentIndex === 6) {
                            if (i === 5 || i === 6 || i === 7) size = "big";
                            if (i === 4 || i === 8) size = "small";
                        } else if (currentIndex === 7) {
                            if (i === 6 || i === 7 || i === 8) size = "big";
                            if (i === 5 || i === 9) size = "small";
                        } else if (currentIndex === 5) {
                            if (i === 7 || i === 8 || i === 9) size = "big";
                            if (i === 6 || i === 10) size = "small";
                        }
                
                        // Only create indicators for visible items
                        if (size !== "hidden") {
                            const li = document.createElement("li");
                            const span = document.createElement("span");
                
                            // Add classes based on size and state
                            span.className = `carousel-indicators__item ${size}`;
                            if (i === currentIndex) {
                                span.classList.add("current", "active");
                            }
                
                            // Set data attribute for slide tracking
                            span.dataset.slide = i;
                
                            li.appendChild(span);
                            indicatorHero.appendChild(li);
                        }
                    }
                }
                
                
                function createIndicator(size, index) {
                    const li = document.createElement("li");
                    const span = document.createElement("span");
                    span.classList.add("carousel-indicators__item");
                    
                    if (size === "small") {
                        span.classList.add("small");
                    } else if (size === "big") {
                        span.classList.add("big");
                    }
                
                    // Add the current class for the active indicator
                    if (index === currentIndex) {
                        span.classList.add("current");
                    }
                
                    // Set initial style for animation (optional, based on your CSS)
                    span.style.opacity = "0";
                    span.style.transform = "scale(0.8)";
                
                    // Ensure animation runs after appending
                    setTimeout(() => {
                        span.style.opacity = ""; // Defaults back to CSS
                        span.style.transform = ""; // Defaults back to CSS
                    }, 0);
                
                    li.appendChild(span);
                    return li;
                }                

                function updateCarousel() {
                    generateIndicators();

                    document.querySelector(".carousel-items").style.transform = `translateX(-${currentIndex * 100}%)`;

                    carouselItems.forEach((slide, index) => {
                        slide.setAttribute("aria-hidden", index !== currentIndex ? "true" : "false");
                    });
                }

                document.querySelector(".next").addEventListener("click", function () {
                    currentIndex = (currentIndex + 1) % totalSlides;
                    updateCarousel();
                });

                document.querySelector(".prev").addEventListener("click", function () {
                    currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
                    updateCarousel();
                });

                updateCarousel();
            };
        }
    };
});