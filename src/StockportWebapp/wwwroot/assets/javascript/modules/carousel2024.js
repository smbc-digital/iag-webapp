define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicatorHero = document.querySelector(".carousel-indicators");
                let currentIndex = 0;
                const totalSlides = carouselItems.length;

                function generateIndicators() {
                    for (let i = 0; i < totalSlides; i++) {
                        let size = "hidden";
                
                        if (currentIndex <= 1 && i < 3) {
                            size = "big";
                        } else {
                            const bigStart = Math.max(0, currentIndex - 1);
                            const bigEnd = Math.min(totalSlides - 1, currentIndex + 1);
                            const smallStart = Math.max(0, bigStart - 1);
                            const smallEnd = Math.min(totalSlides - 1, bigEnd + 1);
                
                            if (i >= bigStart && i <= bigEnd) {
                                size = "big";
                            } else if ((i === smallStart && i < bigStart) || (i === smallEnd && i > bigEnd)) {
                                size = "small";
                            }
                        }
                
                        const existingIndicator = document.querySelector(`.carousel-indicators li:nth-child(${i + 1})`);
                
                        if (size === "hidden" && existingIndicator) {
                            // Fade-out animation before removal
                            existingIndicator.firstChild.style.opacity = "0";
                            existingIndicator.firstChild.style.transform = "scale(0)";
                            setTimeout(() => existingIndicator.remove(), 1000); // Matches transition duration
                        } else if (size !== "hidden") {
                            if (existingIndicator) {
                                // Update existing indicator class
                                const span = existingIndicator.firstChild;
                                span.className = `carousel-indicators__item ${size}`;

                                if (i === currentIndex) {
                                    span.classList.add("current");
                                }
                            } else {
                                // Create and append new indicator
                                const li = createIndicator(size, i);
                                indicatorHero.appendChild(li);
                            }
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