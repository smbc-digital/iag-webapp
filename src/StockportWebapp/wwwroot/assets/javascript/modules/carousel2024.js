define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicatorHero = document.querySelector(".carousel-indicators");
                let currentIndex = 0;
                const totalSlides = carouselItems.length;
                
                // Set lazy loading for background images
                carouselItems.forEach((item, index) => {
                    if (index !== 0) {
                        const bgImage = item.style.backgroundImage;
                        item.dataset.bgImage = bgImage; // Store the background image in a data attribute
                        item.style.backgroundImage = ""; // Remove the initial background image
                    }
                });

                function generateIndicators() {
                    indicatorHero.innerHTML = "";
                
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
                
                        if (size !== "hidden") {
                            const div = document.createElement("div");
                            const span = document.createElement("span");
                
                            span.className = `carousel-indicators__item ${size}`;
                            if (i === currentIndex) {
                                span.classList.add("current", "active");
                            }
                
                            span.dataset.slide = i;
                
                            div.appendChild(span);
                            indicatorHero.appendChild(div);
                        }
                    }
                }
                
                function updateCarousel() {
                    generateIndicators();

                    // Lazy-load the background image for the current item
                    const currentItem = carouselItems[currentIndex];
                    if (currentItem.style.backgroundImage === "" && currentItem.dataset.bgImage) {
                        currentItem.style.backgroundImage = currentItem.dataset.bgImage;
                    }
                    
                    document.querySelector(".carousel-items").style.transform = `translateX(-${currentIndex * 100}%)`;

                    carouselItems.forEach((slide, index) => {
                        slide.setAttribute("aria-hidden", index !== currentIndex ? "true" : "false");
                        document.getElementsByClassName("carousel-item__link")[index]?.setAttribute("tabindex", index !== currentIndex ? "-1" : "0")
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