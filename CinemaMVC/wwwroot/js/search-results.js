document.addEventListener('DOMContentLoaded', function() {
    // DOM Elements
    const searchInput = document.getElementById('searchInput');
    const searchTerm = document.querySelector('.search-term');
    const resultsCount = document.querySelector('.results-count');
    const sortSelect = document.querySelector('.sort-select');
    const viewToggleBtns = document.querySelectorAll('.toggle-btn');
    const filterTags = document.querySelectorAll('.filter-tag');
    const movieItems = document.querySelectorAll('.movie-item');
    const noResultsDiv = document.querySelector('.no-results');
    const resetSearchBtn = document.querySelector('.reset-search-btn');
    const paginationBtns = document.querySelectorAll('.pagination-btn');
    
    // Pagination variables
    const itemsPerPage = 8;
    let currentPage = 1;
    let filteredMovies = [...movieItems]; // Start with all movies
    
    // Initialize the search results page
    function initSearchPage() {
        // Get search query from URL parameters
        const urlParams = new URLSearchParams(window.location.search);
        const query = urlParams.get('q') || 'Inception'; // Default to "Inception" if no query
        
        // Update search input and title
        if (searchInput) searchInput.value = query;
        if (searchTerm) searchTerm.textContent = query;
        
        // Apply initial filtering based on URL parameters
        applyFilters();
        
        // Set up event listeners
        setupEventListeners();
    }
    
    // Set up all event listeners
    function setupEventListeners() {
        // Search input handling
        if (searchInput) {
            searchInput.addEventListener('keypress', function(e) {
                if (e.key === 'Enter') {
                    window.location.href = `search-results.html?q=${encodeURIComponent(this.value)}`;
                }
            });
        }
        
        // Sort select handling
        if (sortSelect) {
            sortSelect.addEventListener('change', function() {
                sortMovies(this.value);
                updateMovieDisplay();
            });
        }
        
        // View toggle handling
        viewToggleBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                viewToggleBtns.forEach(b => b.classList.remove('active'));
                this.classList.add('active');
                
                // Toggle between grid and list view
                const searchResults = document.querySelector('.search-results');
                if (this.querySelector('.fa-th')) {
                    searchResults.classList.remove('list-view');
                } else {
                    searchResults.classList.add('list-view');
                }
            });
        });
        
        // Filter tags handling
        filterTags.forEach(tag => {
            tag.addEventListener('click', function() {
                // Find all tags in the same filter group
                const filterGroup = this.closest('.filter-group');
                const groupTags = filterGroup.querySelectorAll('.filter-tag');
                
                // Remove active class from all tags in this group
                groupTags.forEach(t => t.classList.remove('active'));
                
                // Add active class to clicked tag
                this.classList.add('active');
                
                // Apply filters and update display
                applyFilters();
            });
        });
        
        // Reset search button
        if (resetSearchBtn) {
            resetSearchBtn.addEventListener('click', function() {
                window.location.href = 'index.html';
            });
        }
        
        // Pagination handling
        paginationBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                if (btn.classList.contains('disabled')) return;
                
                // Handle previous/next buttons
                if (btn.title === 'Previous page') {
                    if (currentPage > 1) currentPage--;
                } else if (btn.title === 'Next page') {
                    const maxPage = Math.ceil(filteredMovies.length / itemsPerPage);
                    if (currentPage < maxPage) currentPage++;
                } else {
                    // Handle number buttons
                    const pageNum = parseInt(btn.textContent);
                    if (!isNaN(pageNum)) {
                        currentPage = pageNum;
                    }
                }
                
                updatePagination();
                updateMovieDisplay();
            });
        });
    }
    
    // Apply all active filters
    function applyFilters() {
        // Get selected genre filter
        const selectedGenre = document.querySelector('.filter-group:nth-child(1) .filter-tag.active').textContent.trim();
        
        // Get selected year filter
        const selectedYear = document.querySelector('.filter-group:nth-child(2) .filter-tag.active').textContent.trim();
        
        // Filter movies based on genre and year
        filteredMovies = [...movieItems].filter(movie => {
            let passesGenreFilter = true;
            let passesYearFilter = true;
            
            // Apply genre filter if not "All"
            if (selectedGenre !== 'All') {
                const movieGenres = Array.from(movie.querySelectorAll('.genre-pill'))
                    .map(pill => pill.textContent.trim());
                passesGenreFilter = movieGenres.includes(selectedGenre);
            }
            
            // Apply year filter if not "All"
            if (selectedYear !== 'All') {
                const movieYear = parseInt(movie.querySelector('.movie-year').textContent.trim());
                
                switch (selectedYear) {
                    case '2020+':
                        passesYearFilter = movieYear >= 2020;
                        break;
                    case '2010-2019':
                        passesYearFilter = movieYear >= 2010 && movieYear <= 2019;
                        break;
                    case '2000-2009':
                        passesYearFilter = movieYear >= 2000 && movieYear <= 2009;
                        break;
                    case 'Before 2000':
                        passesYearFilter = movieYear < 2000;
                        break;
                }
            }
            
            return passesGenreFilter && passesYearFilter;
        });
        
        // Update results count
        if (resultsCount) {
            resultsCount.textContent = `${filteredMovies.length} results found`;
        }
        
        // Show/hide no results message
        if (noResultsDiv) {
            noResultsDiv.style.display = filteredMovies.length === 0 ? 'flex' : 'none';
        }
        
        // Reset to first page
        currentPage = 1;
        
        // Update pagination and display
        updatePagination();
        updateMovieDisplay();
    }
    
    // Sort movies based on selected criterion
    function sortMovies(criterion) {
        filteredMovies.sort((a, b) => {
            switch (criterion) {
                case 'rating':
                    const ratingA = parseFloat(a.querySelector('.movie-rating span').textContent);
                    const ratingB = parseFloat(b.querySelector('.movie-rating span').textContent);
                    return ratingB - ratingA; // Descending
                    
                case 'year':
                    const yearA = parseInt(a.querySelector('.movie-year').textContent);
                    const yearB = parseInt(b.querySelector('.movie-year').textContent);
                    return yearB - yearA; // Descending
                    
                case 'title':
                    const titleA = a.querySelector('.movie-title').textContent;
                    const titleB = b.querySelector('.movie-title').textContent;
                    return titleA.localeCompare(titleB); // Ascending
                    
                default: // relevance - keep original order
                    return 0;
            }
        });
    }
    
    // Update pagination buttons
    function updatePagination() {
        const totalPages = Math.max(1, Math.ceil(filteredMovies.length / itemsPerPage));
        
        // Update number buttons
        const numberBtns = Array.from(paginationBtns).filter(btn => !btn.title);
        
        // Show/hide number buttons based on total pages
        numberBtns.forEach((btn, index) => {
            if (index < totalPages && index < 5) { // Show max 5 number buttons
                btn.style.display = 'flex';
                btn.textContent = index + 1;
                btn.classList.toggle('active', index + 1 === currentPage);
            } else {
                btn.style.display = 'none';
            }
        });
        
        // Show/hide ellipsis
        const ellipsis = document.querySelector('.pagination-ellipsis');
        if (ellipsis) {
            ellipsis.style.display = totalPages > 5 ? 'flex' : 'none';
        }
        
        // Update last page button if we have more than 5 pages
        if (totalPages > 5) {
            const lastPageBtn = numberBtns[numberBtns.length - 1];
            if (lastPageBtn) {
                lastPageBtn.style.display = 'flex';
                lastPageBtn.textContent = totalPages;
                lastPageBtn.classList.toggle('active', totalPages === currentPage);
            }
        }
        
        // Update prev/next buttons
        const prevBtn = document.querySelector('.pagination-btn[title="Previous page"]');
        const nextBtn = document.querySelector('.pagination-btn[title="Next page"]');
        
        if (prevBtn) {
            prevBtn.classList.toggle('disabled', currentPage === 1);
        }
        
        if (nextBtn) {
            nextBtn.classList.toggle('disabled', currentPage === totalPages);
        }
    }
    
    // Update movie display based on current page and filters
    function updateMovieDisplay() {
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        
        // Hide all movies first
        movieItems.forEach(movie => {
            movie.style.display = 'none';
        });
        
        // Show only the movies for the current page
        filteredMovies.slice(startIndex, endIndex).forEach(movie => {
            movie.style.display = 'block';
        });
    }
    
    // Initialize the booking modal functionality
    function initBookingModal() {
        // Check if booking modal functionality exists
        if (typeof setupBookButtons === 'function') {
            setupBookButtons();
        }
    }
    
    // Initialize the search page
    initSearchPage();
    
    // Initialize booking modal
    initBookingModal();
}); 