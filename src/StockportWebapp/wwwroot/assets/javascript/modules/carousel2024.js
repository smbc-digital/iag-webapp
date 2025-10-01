define(function () {
    return {
        Init: function () {
            window.onload = function () {
                const carouselItems = document.querySelectorAll(".carousel-item");
                const indicatorHero = document.querySelector(".carousel-indicators");
                let currentIndex = 0;
                const totalSlides = carouselItems.length;
                
                function setUniformCarouselHeight() {
                    const items = document.querySelectorAll(".carousel-item img");
                    let maxHeight = 0;

                    items.forEach(item => {
                        item.style.height = "auto";
                    });

                    items.forEach(item => {
                        const itemHeight = item.offsetHeight;
                        if (itemHeight > maxHeight) {
                            maxHeight = itemHeight;
                        }
                    });

                    items.forEach(item => {
                        item.style.height = maxHeight + "px";
                    });
                }

                // Set lazy loading for background images
                carouselItems.forEach((item, index) => {
                    if (index !== 0) {
                        const bgImage = item.style.backgroundImage;
                        item.dataset.bgImage = bgImage;
                        item.style.backgroundImage = "";
                    }
                });

                function generateIndicators() {
                    indicatorHero.innerHTML = "";
                
                    for (let i = 0; i < totalSlides; i++) {
                        let size = "hidden";
                
                        // Define size based on currentIndex and position
                        if (currentIndex <= 1 && i < 3) {
                            size = "big";
                        } else {
                            const distance = Math.abs(i - currentIndex);
                            if (distance <= 1) {
                                size = "big";
                            } else if (distance === 2) {
                                size = "small";
                            }
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

                    setUniformCarouselHeight();
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

                window.addEventListener("resize", setUniformCarouselHeight);
            };
        }
    };
});