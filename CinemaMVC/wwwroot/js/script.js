document.addEventListener('DOMContentLoaded', function () {
    // Search functionality
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter' && this.value.trim() !== '') {
                window.location.href = `search-results.html?q=${encodeURIComponent(this.value.trim())}`;
            }
        });

        // Also handle click on the search icon
        const searchIcon = searchInput.previousElementSibling;
        if (searchIcon) {
            searchIcon.addEventListener('click', function () {
                if (searchInput.value.trim() !== '') {
                    window.location.href = `search-results.html?q=${encodeURIComponent(searchInput.value.trim())}`;
                }
            });
        }
    }

    // تفعيل أزرار القائمة
    const menuButtons = document.querySelectorAll('.menu-btn');
    menuButtons.forEach(button => {
        button.addEventListener('click', () => {
            menuButtons.forEach(btn => btn.classList.remove('active'));
            button.classList.add('active');
        });
    });

    // تفعيل روابط التنقل
    const navLinksTop = document.querySelectorAll('.nav-links a');
    navLinksTop.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            navLinksTop.forEach(l => l.classList.remove('active'));
            link.classList.add('active');
        });
    });

    // إضافة تأثيرات حركية للبطاقات
    const movieCards = document.querySelectorAll('.movie-card');
    movieCards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            card.style.transform = 'scale(1.05)';
        });
        card.addEventListener('mouseleave', () => {
            card.style.transform = 'scale(1)';
        });
    });

    // Initialize Font Awesome
    const fontAwesomeScript = document.createElement('script');
    fontAwesomeScript.src = 'https://kit.fontawesome.com/a076d05399.js';
    document.head.appendChild(fontAwesomeScript);

    // Initialize all carousels
    initializeAllCarousels();

    // Genre Filter Functionality
    const genreTags = document.querySelectorAll('.genre-tag');
    const movieItems = document.querySelectorAll('.movie-item');

    genreTags.forEach(tag => {
        tag.addEventListener('click', () => {
            // Remove active class from all tags
            genreTags.forEach(t => t.classList.remove('active'));
            // Add active class to clicked tag
            tag.classList.add('active');

            const selectedGenre = tag.textContent.toLowerCase();

            // If 'All' is selected, show all movies
            if (selectedGenre === 'all') {
                movieItems.forEach(movie => {
                    movie.style.display = 'block';
                });
                return;
            }

            // Filter movies based on genre
            movieItems.forEach(movie => {
                const movieGenres = movie.dataset.genres?.toLowerCase().split(',') || [];
                if (movieGenres.includes(selectedGenre)) {
                    movie.style.display = 'block';
                } else {
                    movie.style.display = 'none';
                }
            });
        });
    });

    // View Toggle Functionality
    const viewToggleBtns = document.querySelectorAll('.toggle-btn');
    const moviesContainer = document.querySelector('.movies-container');

    viewToggleBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            viewToggleBtns.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');

            // Toggle between grid and list view
            if (btn.querySelector('.fa-th')) {
                moviesContainer.classList.remove('list-view');
            } else {
                moviesContainer.classList.add('list-view');
            }
        });
    });

    // Pagination Functionality
    const paginationBtns = document.querySelectorAll('.pagination-btn');
    const itemsPerPage = 8; // Number of movies per page
    let currentPage = 1;

    function updatePagination() {
        const totalPages = Math.ceil(movieItems.length / itemsPerPage);

        // Update pagination buttons visibility
        paginationBtns.forEach(btn => {
            const pageNum = parseInt(btn.textContent);
            if (!isNaN(pageNum)) {
                btn.style.display = pageNum <= totalPages ? 'flex' : 'none';
            }
        });

        // Update movies visibility
        movieItems.forEach((movie, index) => {
            const startIndex = (currentPage - 1) * itemsPerPage;
            const endIndex = startIndex + itemsPerPage;

            if (index >= startIndex && index < endIndex) {
                movie.style.display = 'block';
            } else {
                movie.style.display = 'none';
            }
        });
    }

    paginationBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            if (btn.classList.contains('disabled')) return;

            // Handle previous/next buttons
            if (btn.title === 'Previous page') {
                if (currentPage > 1) currentPage--;
            } else if (btn.title === 'Next page') {
                if (currentPage < Math.ceil(movieItems.length / itemsPerPage)) currentPage++;
            } else {
                // Handle number buttons
                currentPage = parseInt(btn.textContent);
            }

            // Update active state
            paginationBtns.forEach(b => b.classList.remove('active'));
            paginationBtns[currentPage].classList.add('active');

            updatePagination();
        });
    });

    // Initialize pagination
    updatePagination();

    // Add smooth scroll behavior when clicking on pagination
    document.querySelector('.pagination').addEventListener('click', () => {
        document.querySelector('.all-movies-section').scrollIntoView({ behavior: 'smooth' });
    });

    // Initialize booking functionality
    if (typeof setupBookButtons === 'function') {
        setupBookButtons();
    }

    // Hero banner enhancements
    const heroSection = document.querySelector('.hero-banner');
    const bookBtn = document.querySelector('.hero-btn.book-btn');
    const timeChips = document.querySelectorAll('.time-chip');

    if (heroSection) {
        // Add parallax effect to hero image
        window.addEventListener('scroll', function () {
            const scrollPosition = window.pageYOffset;
            const heroImage = heroSection.querySelector('.hero-image img');
            if (heroImage) {
                heroImage.style.transform = `translateY(${scrollPosition * 0.15}px)`;
            }
        });

        // Make time chips selectable
        if (timeChips.length) {
            timeChips.forEach(chip => {
                chip.addEventListener('click', function () {
                    timeChips.forEach(c => c.classList.remove('selected'));
                    this.classList.add('selected');

                    // Update book button text to include the selected time
                    if (bookBtn) {
                        bookBtn.innerHTML = `<i class="fas fa-ticket-alt"></i> Book for ${this.textContent}`;

                        // Add extra highlight effect when a time is selected
                        bookBtn.classList.add('time-selected');

                        // Scroll to booking button if not in view
                        if (!isElementInViewport(bookBtn)) {
                            bookBtn.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        }
                    }
                });
            });
        }

        // Highlight booking button on hover with moving gradient
        if (bookBtn) {
            bookBtn.addEventListener('mousemove', function (e) {
                const rect = this.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;

                // Calculate position as percentage
                const posX = Math.round((x / rect.width) * 100);
                const posY = Math.round((y / rect.height) * 100);

                // Apply gradient at mouse position
                this.style.background = `radial-gradient(circle at ${posX}% ${posY}%, #ff6b81 0%, var(--accent-color) 50%)`;
            });

            bookBtn.addEventListener('mouseleave', function () {
                this.style.background = 'var(--accent-color)';
            });

            // Add click functionality
            bookBtn.addEventListener('click', function () {
                const selectedTime = document.querySelector('.time-chip.selected');
                const showtime = selectedTime ? selectedTime.textContent : 'next available';

                // Show a confirmation tooltip
                const tooltip = document.createElement('div');
                tooltip.className = 'booking-tooltip';
                tooltip.innerHTML = `<i class="fas fa-check-circle"></i> Booking for ${showtime}`;
                document.body.appendChild(tooltip);

                // Position tooltip near button
                const rect = this.getBoundingClientRect();
                tooltip.style.top = `${rect.top - 50}px`;
                tooltip.style.left = `${rect.left + rect.width / 2 - tooltip.offsetWidth / 2}px`;

                // Remove tooltip after animation
                setTimeout(() => {
                    tooltip.classList.add('show');
                    setTimeout(() => {
                        tooltip.classList.remove('show');
                        setTimeout(() => {
                            document.body.removeChild(tooltip);
                        }, 300);
                    }, 2000);
                }, 10);

                // Here you would normally redirect to booking page
                // window.location.href = 'booking.html?time=' + encodeURIComponent(showtime);
            });
        }
    }

    // Helper function to check if element is in viewport
    function isElementInViewport(el) {
        const rect = el.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    }

    // Initialize availability tags for movie cards
    const availabilityTags = document.querySelectorAll('.availability-tag');
    if (availabilityTags.length) {
        availabilityTags.forEach(tag => {
            // Add hover effect
            tag.addEventListener('mouseenter', function () {
                this.style.transform = 'scale(1.05)';
                this.style.boxShadow = '0 4px 15px rgba(0, 0, 0, 0.3)';

                // Change parent card z-index to ensure tag is fully visible
                const parentCard = this.closest('.carousel-item');
                if (parentCard) {
                    parentCard.style.zIndex = '10';
                }
            });

            tag.addEventListener('mouseleave', function () {
                this.style.transform = 'scale(1)';
                this.style.boxShadow = '';

                // Reset parent card z-index
                const parentCard = this.closest('.carousel-item');
                if (parentCard) {
                    parentCard.style.zIndex = '';
                }
            });

            // For sold out items, make the booking button unclickable
            if (tag.classList.contains('unavailable')) {
                const parentCard = tag.closest('.carousel-item');
                if (parentCard) {
                    const bookBtn = parentCard.querySelector('.book-btn');
                    if (bookBtn) {
                        bookBtn.disabled = true;
                        bookBtn.innerHTML = '<i class="fas fa-ticket-alt"></i> Sold Out';
                    }
                }
            }
        });
    }

    // Initialize movie cards with staggered animation
    const allMoviesItems = document.querySelectorAll('.all-movies-section .movie-item');
    if (allMoviesItems.length) {
        allMoviesItems.forEach((item, index) => {
            // Add a staggered entrance animation
            item.style.opacity = '0';
            item.style.transform = 'translateY(20px)';
            item.style.transition = 'opacity 0.5s ease, transform 0.5s ease';

            setTimeout(() => {
                item.style.opacity = '1';
                item.style.transform = 'translateY(0)';
            }, 100 + (index * 50)); // Stagger the animations

            // Make time slots interactive
            const timeSlots = item.querySelectorAll('.time-slot');
            if (timeSlots.length) {
                timeSlots.forEach(slot => {
                    slot.addEventListener('click', function () {
                        // Remove selection from all slots in this card
                        timeSlots.forEach(s => s.classList.remove('selected'));
                        // Add selected class to clicked slot
                        this.classList.add('selected');

                        // Update the booking button
                        const bookBtn = item.querySelector('.book-btn');
                        if (bookBtn && !bookBtn.disabled) {
                            bookBtn.innerHTML = `<i class="fas fa-ticket-alt"></i> Book for ${this.textContent}`;
                            bookBtn.classList.add('time-selected');
                        }
                    });
                });
            }

            // Add shimmer effect to booking buttons on hover
            const bookBtn = item.querySelector('.book-btn');
            if (bookBtn && !bookBtn.disabled) {
                bookBtn.addEventListener('mouseenter', function () {
                    this.style.backgroundPosition = '100%';
                });

                bookBtn.addEventListener('mouseleave', function () {
                    this.style.backgroundPosition = '0%';
                });
            }
        });
    }

    // Toggle view functionality (Grid/List)
    const viewButtons = document.querySelectorAll('.toggle-btn');
    const moviesGrid = document.querySelector('.movies-container');

    if (viewButtons.length && moviesGrid) {
        viewButtons.forEach(btn => {
            btn.addEventListener('click', function () {
                viewButtons.forEach(b => b.classList.remove('active'));
                this.classList.add('active');

                const isListView = this.querySelector('i').classList.contains('fa-list');

                if (isListView) {
                    moviesGrid.classList.add('list-view');
                } else {
                    moviesGrid.classList.remove('list-view');
                }

                // Re-animate the movie items for a smooth transition
                allMoviesItems.forEach((item, index) => {
                    item.style.opacity = '0';
                    item.style.transform = 'translateY(20px)';

                    setTimeout(() => {
                        item.style.opacity = '1';
                        item.style.transform = 'translateY(0)';
                    }, 50 + (index * 30));
                });
            });
        });
    }

    // Add special effects to top rated movies section
    const topRatedSection = document.querySelector('.top-rated-section');
    if (topRatedSection) {
        // Add a subtle parallax effect to the section
        window.addEventListener('scroll', function () {
            const rect = topRatedSection.getBoundingClientRect();
            // Only apply effect when section is in viewport
            if (rect.top < window.innerHeight && rect.bottom > 0) {
                const scrollPosition = window.scrollY;
                const offset = scrollPosition * 0.05;
                topRatedSection.style.backgroundPosition = `center ${offset}px`;
            }
        });

        // Add shine effect to gold stars on hover
        const starIcons = topRatedSection.querySelectorAll('.fa-star');
        starIcons.forEach(star => {
            star.addEventListener('mouseenter', function () {
                this.style.color = '#FFD700';
                this.style.transform = 'scale(1.3)';
                this.style.filter = 'drop-shadow(0 0 5px rgba(255, 215, 0, 0.7))';
            });

            star.addEventListener('mouseleave', function () {
                this.style.color = '';
                this.style.transform = '';
                this.style.filter = '';
            });
        });

        // Add special 3D tilt effect to top rated movie cards
        const topRatedCards = topRatedSection.querySelectorAll('.carousel-item');
        topRatedCards.forEach(card => {
            card.addEventListener('mousemove', function (e) {
                const rect = this.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;

                // Calculate tilt values
                const tiltX = (y / rect.height - 0.5) * 10;
                const tiltY = (x / rect.width - 0.5) * -10;

                // Apply the 3D tilt effect
                this.style.transform = `perspective(1000px) rotateX(${tiltX}deg) rotateY(${tiltY}deg) scale3d(1.05, 1.05, 1.05)`;

                // Add a moving highlight effect
                const highlight = this.querySelector('.carousel-thumb');
                if (highlight) {
                    const percentX = Math.round((x / rect.width) * 100);
                    const percentY = Math.round((y / rect.height) * 100);
                    highlight.style.background = `radial-gradient(circle at ${percentX}% ${percentY}%, rgba(255, 215, 0, 0.4), transparent 60%)`;
                }
            });

            card.addEventListener('mouseleave', function () {
                this.style.transform = '';
                const highlight = this.querySelector('.carousel-thumb');
                if (highlight) {
                    highlight.style.background = '';
                }
            });
        });
    }

    // Full Width All Movies Section functionality
    initializeAllMoviesSection();

    // Mobile Menu Settings
    // Initialize movie carousels
    initializeAllCarousels();

    // Set active states for main navigation links
    const mainNavLinks = document.querySelectorAll('.nav-menu a');
    mainNavLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            // Check if it's not a link to another page (no href or #)
            if (!this.getAttribute('href') || this.getAttribute('href') === '#') {
                e.preventDefault();
            }

            // Set active state
            mainNavLinks.forEach(navLink => {
                navLink.classList.remove('active');
            });
            this.classList.add('active');
        });
    });

    // Initialize all movies section
    initializeAllMoviesSection();

    // Enhanced Popular Movies section
    enhancePopularMoviesSection();
});

// Function to initialize all carousels on the page
function initializeAllCarousels() {
    const carousels = document.querySelectorAll('.movie-carousel');

    carousels.forEach(function (carousel) {
        const container = carousel.closest('.carousel-container');
        if (!container) return;

        const prevBtn = container.querySelector('.carousel-large-arrow.left');
        const nextBtn = container.querySelector('.carousel-large-arrow.right');

        if (!prevBtn || !nextBtn) return;

        const itemWidth = carousel.querySelector('.carousel-item')?.offsetWidth + 25 || 325; // Item width + gap
        const visibleItems = Math.floor(carousel.offsetWidth / itemWidth);

        let currentPosition = 0;

        // Next button click
        nextBtn.addEventListener('click', function () {
            if (window.innerWidth <= 768) {
                // On mobile, scroll one item at a time
                currentPosition += 1;
            } else {
                // On desktop, scroll multiple items at a time
                currentPosition += visibleItems;
            }

            // Prevent scrolling too far
            const maxPosition = carousel.children.length - visibleItems;
            if (currentPosition > maxPosition) {
                currentPosition = maxPosition;
            }

            // Apply scroll transform
            carousel.style.transform = `translateX(-${currentPosition * itemWidth}px)`;
        });

        // Previous button click
        prevBtn.addEventListener('click', function () {
            if (window.innerWidth <= 768) {
                // On mobile, scroll one item at a time
                currentPosition -= 1;
            } else {
                // On desktop, scroll multiple items at a time
                currentPosition -= visibleItems;
            }

            // Prevent scrolling too far
            if (currentPosition < 0) {
                currentPosition = 0;
            }

            // Apply scroll transform
            carousel.style.transform = `translateX(-${currentPosition * itemWidth}px)`;
        });

        // Reset on window resize
        window.addEventListener('resize', function () {
            currentPosition = 0;
            carousel.style.transform = 'translateX(0)';
        });
    });
}

// Genre filter functionality
function initializeGenreFilters() {
    const genreButtons = document.querySelectorAll('.genre-btn');
    const movieItems = document.querySelectorAll('.movie-item');

    genreButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Add animation effect when clicking
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = '';
            }, 150);

            // Remove active class from all buttons
            genreButtons.forEach(btn => btn.classList.remove('active'));

            // Add active class to clicked button
            this.classList.add('active');

            // Get selected genre - extract just the text without the icon
            const buttonText = this.textContent.trim();
            const selectedGenre = buttonText.split(' ')[0].toLowerCase();

            // Visual feedback for selection
            this.style.animation = 'none';
            void this.offsetWidth; // Trigger reflow to restart animation
            this.style.animation = 'pulse 1s ease';

            console.log(`Filtering by: ${selectedGenre}`);

            // Filter movies based on genre
            if (selectedGenre === 'all') {
                // Show all movies with a fade-in effect
                movieItems.forEach((item, index) => {
                    setTimeout(() => {
                        item.style.display = 'block';
                        item.style.opacity = '0';
                        item.style.transform = 'translateY(20px)';

                        // Trigger reflow
                        void item.offsetWidth;

                        item.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                        setTimeout(() => {
                            item.style.opacity = '1';
                            item.style.transform = 'translateY(0)';
                        }, 50);
                    }, index * 50);
                });
            } else {
                // Filter movies and apply staggered animation
                movieItems.forEach((item) => {
                    // First hide all items
                    item.style.opacity = '0';
                    item.style.transform = 'translateY(20px)';

                    setTimeout(() => {
                        // Get data-genres attribute or use class names as fallback
                        const genres = item.dataset.genres ||
                            Array.from(item.classList)
                                .filter(cls => cls !== 'movie-item')
                                .join(',');

                        // Check if this movie matches the selected genre
                        const hasGenre = genres.toLowerCase().includes(selectedGenre);

                        if (hasGenre || selectedGenre === 'all') {
                            item.style.display = 'block';
                            setTimeout(() => {
                                item.style.opacity = '1';
                                item.style.transform = 'translateY(0)';
                            }, 50);
                        } else {
                            setTimeout(() => {
                                item.style.display = 'none';
                            }, 300); // Wait for fade-out to complete
                        }
                    }, 100);
                });
            }
        });
    });

    // Add hover shimmer effect to genre buttons
    genreButtons.forEach(button => {
        button.addEventListener('mouseenter', function () {
            if (!this.classList.contains('active')) {
                this.style.boxShadow = '0 8px 20px rgba(0, 0, 0, 0.2)';
            }
        });

        button.addEventListener('mouseleave', function () {
            if (!this.classList.contains('active')) {
                this.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.1)';
            }
        });
    });
}

// Movie card interactions
function initializeMovieCards() {
    const movieItems = document.querySelectorAll('.movie-item');

    movieItems.forEach(item => {
        // Time slot selection
        const timeSlots = item.querySelectorAll('.time-slot');
        timeSlots.forEach(slot => {
            slot.addEventListener('click', function (e) {
                e.stopPropagation();

                // Remove active class from all slots in this card
                timeSlots.forEach(s => s.classList.remove('selected'));

                // Add active class to clicked slot
                this.classList.add('selected');
            });
        });

        // Movie overlay actions
        const actionButtons = item.querySelectorAll('.movie-action-btn');
        actionButtons.forEach(button => {
            button.addEventListener('click', function (e) {
                e.stopPropagation();

                const action = this.querySelector('i').className;

                if (action.includes('play')) {
                    // Play trailer
                    alert('Playing trailer...');
                } else if (action.includes('heart')) {
                    // Add to favorites
                    this.classList.toggle('active');
                    const isFavorite = this.classList.contains('active');
                    console.log(`${isFavorite ? 'Added to' : 'Removed from'} favorites`);
                } else if (action.includes('info')) {
                    // Show movie details
                    console.log('Showing movie details...');
                    // Here you would typically navigate to the movie details page
                }
            });
        });
    });
}

// Toggle view (grid/list)
const toggleButtons = document.querySelectorAll('.toggle-btn');
toggleButtons.forEach(button => {
    button.addEventListener('click', function () {
        toggleButtons.forEach(btn => btn.classList.remove('active'));
        this.classList.add('active');

        const isGridView = this.querySelector('i').classList.contains('fa-th');
        const moviesContainer = document.querySelector('.movies-container');

        if (isGridView) {
            moviesContainer.classList.remove('list-view');
        } else {
            moviesContainer.classList.add('list-view');
        }
    });
});

// Function to initialize the All Movies section
function initializeAllMoviesSection() {
    const fullWidthSection = document.querySelector('.full-width-section');
    if (!fullWidthSection) return;

    // Genre filtering functionality
    const genreButtons = fullWidthSection.querySelectorAll('.genre-btn');
    const movieItems = fullWidthSection.querySelectorAll('.movie-item');
    let filteredMovies = Array.from(movieItems); // Keep track of currently filtered movies

    genreButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Remove active class from all buttons
            genreButtons.forEach(btn => btn.classList.remove('active'));

            // Add active class to clicked button
            this.classList.add('active');

            const selectedGenre = this.textContent.trim().split(' ')[0].toLowerCase();

            // Filter movies based on genre
            if (selectedGenre === 'all') {
                filteredMovies = Array.from(movieItems);
                movieItems.forEach(item => {
                    item.classList.remove('filtered-out');
                });
            } else {
                filteredMovies = Array.from(movieItems).filter(item => {
                    const genres = item.getAttribute('data-genres');
                    const hasGenre = genres && genres.includes(selectedGenre);

                    // Mark items as filtered out or not
                    if (hasGenre) {
                        item.classList.remove('filtered-out');
                        return true;
                    } else {
                        item.classList.add('filtered-out');
                        return false;
                    }
                });
            }

            // Reset to page 1 when filtering
            const firstPageBtn = fullWidthSection.querySelector('.pagination-btn:not([title])');
            if (firstPageBtn) {
                // Remove active class from all pagination buttons
                paginationButtons.forEach(btn => {
                    if (!btn.hasAttribute('title')) {
                        btn.classList.remove('active');
                    }
                });

                // Set first page as active
                firstPageBtn.classList.add('active');
            }

            // Update pagination based on filtered movies
            updatePagination();

            // Show first page of filtered movies
            updateMoviesVisibility(1);
        });
    });

    // Function to update pagination based on filtered movies
    function updatePagination() {
        const totalPages = Math.ceil(filteredMovies.length / itemsPerPage);

        // Update page number buttons
        paginationButtons.forEach(btn => {
            if (!btn.hasAttribute('title') && !btn.classList.contains('pagination-ellipsis')) {
                const pageNum = parseInt(btn.textContent);
                if (!isNaN(pageNum)) {
                    // Show/hide page numbers based on total pages
                    if (pageNum <= totalPages) {
                        btn.style.display = 'flex';
                    } else {
                        btn.style.display = 'none';
                    }
                }
            }
        });
    }

    // Function to update movie visibility based on current page and filtering
    function updateMoviesVisibility(pageNumber) {
        const startIndex = (pageNumber - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;

        // Hide all movies first
        movieItems.forEach(item => {
            item.style.display = 'none';
            item.style.opacity = '0';
        });

        // Show only filtered movies for current page with animation
        filteredMovies.slice(startIndex, endIndex).forEach((item, index) => {
            item.style.display = 'block';
            setTimeout(() => {
                item.style.transition = 'all 0.4s cubic-bezier(0.23, 1, 0.32, 1)';
                item.style.opacity = '1';
                item.style.transform = 'translateY(0)';
            }, index * 50);
        });
    }

    // View toggle functionality (grid/list view)
    const toggleButtons = fullWidthSection.querySelectorAll('.toggle-btn');
    const moviesContainer = fullWidthSection.querySelector('.movies-container');

    toggleButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Remove active class from all toggle buttons
            toggleButtons.forEach(btn => btn.classList.remove('active'));

            // Add active class to clicked button
            this.classList.add('active');

            // Toggle between grid and list view
            if (this.querySelector('i').classList.contains('fa-list')) {
                moviesContainer.classList.add('list-view');
                moviesContainer.classList.remove('grid-view');
            } else {
                moviesContainer.classList.remove('list-view');
                moviesContainer.classList.add('grid-view');
            }

            // Animate the items in the new view
            const visibleItems = Array.from(movieItems).filter(item => item.style.display !== 'none');
            visibleItems.forEach((item, index) => {
                setTimeout(() => {
                    item.style.opacity = '0';
                    item.style.transform = 'translateY(20px)';
                    requestAnimationFrame(() => {
                        item.style.transition = 'all 0.4s cubic-bezier(0.23, 1, 0.32, 1)';
                        item.style.opacity = '1';
                        item.style.transform = 'translateY(0)';
                    });
                }, index * 50);
            });
        });
    });

    // Pagination functionality
    const paginationButtons = fullWidthSection.querySelectorAll('.pagination-btn');
    const itemsPerPage = 8; // Number of movies per page

    // Function to update movie visibility based on current page
    function updateMoviesVisibility(pageNumber) {
        const startIndex = (pageNumber - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;

        // Hide all movies first
        movieItems.forEach(item => {
            item.style.display = 'none';
            item.style.opacity = '0';
        });

        // Show only movies for current page with animation
        Array.from(movieItems)
            .slice(startIndex, endIndex)
            .forEach((item, index) => {
                item.style.display = 'block';
                setTimeout(() => {
                    item.style.transition = 'all 0.4s cubic-bezier(0.23, 1, 0.32, 1)';
                    item.style.opacity = '1';
                    item.style.transform = 'translateY(0)';
                }, index * 50);
            });
    }

    // Initialize pagination on page load
    updateMoviesVisibility(1);

    paginationButtons.forEach(button => {
        if (!button.hasAttribute('title')) { // Skip prev/next buttons
            button.addEventListener('click', function () {
                // Remove active class from all pagination buttons
                paginationButtons.forEach(btn => {
                    if (!btn.hasAttribute('title')) {
                        btn.classList.remove('active');
                    }
                });

                // Add active class to clicked button
                this.classList.add('active');

                // Get the page number
                const pageNumber = parseInt(this.textContent);

                // Update movies visibility for this page
                updateMoviesVisibility(pageNumber);

                // Scroll to the top of the section
                fullWidthSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
            });
        } else {
            // Prev/Next button functionality
            button.addEventListener('click', function () {
                const activePage = fullWidthSection.querySelector('.pagination-btn.active:not([title])');
                let nextPage;

                if (this.getAttribute('title') === 'Previous page') {
                    nextPage = activePage.previousElementSibling;
                    while (nextPage && nextPage.hasAttribute('title')) {
                        nextPage = nextPage.previousElementSibling;
                    }
                } else {
                    nextPage = activePage.nextElementSibling;
                    while (nextPage && nextPage.hasAttribute('title')) {
                        nextPage = nextPage.nextElementSibling;
                    }
                }

                if (nextPage && !nextPage.classList.contains('pagination-ellipsis')) {
                    nextPage.click();
                }
            });
        }
    });

    // Add scroll reveal animations for the section
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('in-view');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.2 });

    observer.observe(fullWidthSection);
}

// Function to enhance the Popular Movies section with additional effects
function enhancePopularMoviesSection() {
    const popularMoviesSection = document.querySelector('.movie-categories:not(.top-rated-section)');
    if (!popularMoviesSection) return;

    // Add a subtle parallax effect to the section when scrolling
    window.addEventListener('scroll', function () {
        const rect = popularMoviesSection.getBoundingClientRect();
        if (rect.top < window.innerHeight && rect.bottom > 0) {
            const scrollProgress = (window.innerHeight - rect.top) / (window.innerHeight + rect.height);
            const moveY = scrollProgress * 20 - 10; // Move between -10px and +10px
            popularMoviesSection.style.transform = `translateY(${moveY}px)`;

            // Also apply a subtle background shift
            popularMoviesSection.style.backgroundPosition = `center ${scrollProgress * 15}px`;
        }
    });

    // Enhanced hover effects for carousel items
    const carouselItems = popularMoviesSection.querySelectorAll('.carousel-item');
    carouselItems.forEach(item => {
        // Add 3D tilt effect on mouse move
        item.addEventListener('mousemove', function (e) {
            if (window.innerWidth < 768) return; // Skip on mobile

            const rect = this.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            // Calculate tilt values (less extreme than the top rated section)
            const tiltX = (y / rect.height - 0.5) * 5;
            const tiltY = (x / rect.width - 0.5) * -5;

            // Apply the 3D tilt effect
            this.style.transform = `perspective(1000px) rotateX(${tiltX}deg) rotateY(${tiltY}deg) scale3d(1.03, 1.03, 1.03)`;

            // Add dynamic highlight based on mouse position
            const thumb = this.querySelector('.carousel-thumb');
            if (thumb) {
                const percentX = Math.round((x / rect.width) * 100);
                const percentY = Math.round((y / rect.height) * 100);
                thumb.style.background = `radial-gradient(circle at ${percentX}% ${percentY}%, rgba(255, 255, 255, 0.2), transparent 50%)`;
            }
        });

        // Reset transforms on mouse leave
        item.addEventListener('mouseleave', function () {
            this.style.transform = '';
            const thumb = this.querySelector('.carousel-thumb');
            if (thumb) {
                thumb.style.background = '';
            }
        });

        // Add special effect for availability tags
        const availTag = item.querySelector('.availability-tag');
        if (availTag) {
            availTag.addEventListener('mouseenter', function (e) {
                e.stopPropagation(); // Prevent triggering the card's events
                this.style.transform = 'scale(1.1) translateY(-5px)';
                this.style.boxShadow = '0 10px 25px rgba(0, 0, 0, 0.4)';
            });

            availTag.addEventListener('mouseleave', function () {
                this.style.transform = '';
                this.style.boxShadow = '';
            });
        }
    });

    // Add staggered entrance animation when scrolling into view
    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                // Section is now visible
                const items = entry.target.querySelectorAll('.carousel-item');
                items.forEach((item, index) => {
                    setTimeout(() => {
                        item.classList.add('animate-in');
                    }, index * 100); // Stagger the animations
                });

                // Add class to the section for additional animations
                entry.target.classList.add('in-view');

                // Stop observing after animation
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.2 });

    // Start observing the section
    observer.observe(popularMoviesSection);

    // Add a class to enable CSS animations
    carouselItems.forEach(item => {
        item.classList.add('with-hover-effects');
    });

    // Initialize enhanced carousel navigation
    initializeEnhancedCarouselNav(popularMoviesSection);
}

// Function to enhance carousel navigation in the Popular Movies section
function initializeEnhancedCarouselNav(section) {
    const carousel = section.querySelector('.movie-carousel');
    const leftArrow = section.querySelector('.carousel-large-arrow.left');
    const rightArrow = section.querySelector('.carousel-large-arrow.right');

    if (!carousel || !leftArrow || !rightArrow) return;

    // Add pulse effect to arrows when section is in view
    leftArrow.classList.add('pulse-on-view');
    rightArrow.classList.add('pulse-on-view');

    // Add ripple effect on click
    [leftArrow, rightArrow].forEach(arrow => {
        arrow.addEventListener('click', function (e) {
            // Create ripple element
            const ripple = document.createElement('span');
            ripple.classList.add('nav-ripple');
            this.appendChild(ripple);

            // Set position
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            ripple.style.width = ripple.style.height = `${size * 2}px`;
            ripple.style.left = `${e.clientX - rect.left - size}px`;
            ripple.style.top = `${e.clientY - rect.top - size}px`;

            // Clean up
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });
}

// Make movie items responsive with touch events for mobile
document.addEventListener('DOMContentLoaded', function () {
    // Add touch capability to carousels for mobile
    const carousels = document.querySelectorAll('.movie-carousel');

    carousels.forEach(function (carousel) {
        let startX, endX;
        let isDown = false;

        carousel.addEventListener('touchstart', function (e) {
            startX = e.touches[0].clientX;
            isDown = true;
        });

        carousel.addEventListener('touchmove', function (e) {
            if (!isDown) return;
            e.preventDefault();
            endX = e.touches[0].clientX;
        });

        carousel.addEventListener('touchend', function () {
            isDown = false;
            if (!startX || !endX) return;

            const container = carousel.closest('.carousel-container');
            const prevBtn = container.querySelector('.carousel-large-arrow.left');
            const nextBtn = container.querySelector('.carousel-large-arrow.right');

            const diff = startX - endX;

            if (diff > 50) {
                // Swipe left, go next
                nextBtn.click();
            } else if (diff < -50) {
                // Swipe right, go previous
                prevBtn.click();
            }

            startX = null;
            endX = null;
        });
    });
});



        // Mobile Navigation Toggle
    document.addEventListener('DOMContentLoaded', function() {
            const mobileToggle = document.querySelector('.mobile-toggle');
    const mainNav = document.querySelector('.main-nav');

    if (mobileToggle && mainNav) {
        mobileToggle.addEventListener('click', function () {
            mainNav.classList.toggle('menu-active');

            // Change icon between bars and times
            const icon = mobileToggle.querySelector('i');
            if (icon.classList.contains('fa-bars')) {
                icon.classList.remove('fa-bars');
                icon.classList.add('fa-times');
            } else {
                icon.classList.remove('fa-times');
                icon.classList.add('fa-bars');
            }
        });
            }

    // Close menu when clicking outside
    document.addEventListener('click', function(event) {
                if (!event.target.closest('.main-nav') && mainNav.classList.contains('menu-active')) {
        mainNav.classList.remove('menu-active');

    const icon = mobileToggle.querySelector('i');
    icon.classList.remove('fa-times');
    icon.classList.add('fa-bars');
                }
            });

    // Handle window resize
    window.addEventListener('resize', function() {
                if (window.innerWidth > 992 && mainNav.classList.contains('menu-active')) {
        mainNav.classList.remove('menu-active');

    const icon = mobileToggle.querySelector('i');
    icon.classList.remove('fa-times');
    icon.classList.add('fa-bars');
                }
            });
        });