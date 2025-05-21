document.addEventListener('DOMContentLoaded', function() {
    // Initialize Select2 for multi-select dropdowns
    $('.select2').select2({
        placeholder: "Select options",
        allowClear: true,
        tags: true // Allow adding new options
    });

    // Movie Poster Preview
    const moviePosterInput = document.getElementById('moviePoster');
    const posterPreview = document.querySelector('.poster-preview');
    
    if (moviePosterInput) {
        moviePosterInput.addEventListener('change', function() {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    posterPreview.style.backgroundImage = `url(${e.target.result})`;
                    if (posterPreview.querySelector('.placeholder')) {
                        posterPreview.querySelector('.placeholder').style.display = 'none';
                    }
                };
                reader.readAsDataURL(file);
            } else {
                posterPreview.style.backgroundImage = 'none';
                if (posterPreview.querySelector('.placeholder')) {
                    posterPreview.querySelector('.placeholder').style.display = 'flex';
                }
            }
        });
    }

    // Director Modal Functionality
    const addDirectorBtn = document.getElementById('addDirectorBtn');
    const addDirectorModal = document.getElementById('addDirectorModal');
    
    if (addDirectorBtn && addDirectorModal) {
        addDirectorBtn.addEventListener('click', function() {
            addDirectorModal.style.display = 'block';
            document.body.style.overflow = 'hidden';
        });
    }

    // Actor Modal Functionality
    const addActorBtn = document.getElementById('addActorBtn');
    const addActorModal = document.getElementById('addActorModal');
    
    if (addActorBtn && addActorModal) {
        addActorBtn.addEventListener('click', function() {
            addActorModal.style.display = 'block';
            document.body.style.overflow = 'hidden';
        });
    }

    // Close Modals
    const closeButtons = document.querySelectorAll('.close-modal');
    const cancelButtons = document.querySelectorAll('.cancel-btn');
    
    closeButtons.forEach(button => {
        button.addEventListener('click', function() {
            const modal = this.closest('.modal');
            modal.style.display = 'none';
            document.body.style.overflow = 'auto';
        });
    });
    
    cancelButtons.forEach(button => {
        button.addEventListener('click', function() {
            const modal = this.closest('.modal');
            if (modal) {
                modal.style.display = 'none';
                document.body.style.overflow = 'auto';
            }
        });
    });
    
    // Close modal when clicking outside
    window.addEventListener('click', function(event) {
        if (event.target.classList.contains('modal')) {
            event.target.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    });

    // Add New Director Form Submission
    const addDirectorForm = document.getElementById('addDirectorForm');
    
    if (addDirectorForm) {
        addDirectorForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const directorName = document.getElementById('directorName').value;
            
            // Create new option for the directors dropdown
            const newDirectorOption = new Option(directorName, 'new_' + Date.now(), true, true);
            $('#movieDirectors').append(newDirectorOption).trigger('change');
            
            // Close modal and reset form
            addDirectorModal.style.display = 'none';
            document.body.style.overflow = 'auto';
            addDirectorForm.reset();
            
            // Show success message
            showNotification('Director added successfully');
        });
    }

    // Add New Actor Form Submission
    const addActorForm = document.getElementById('addActorForm');
    
    if (addActorForm) {
        addActorForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const actorName = document.getElementById('actorName').value;
            
            // Create new option for the actors dropdown
            const newActorOption = new Option(actorName, 'new_' + Date.now(), true, true);
            $('#movieActors').append(newActorOption).trigger('change');
            
            // Close modal and reset form
            addActorModal.style.display = 'none';
            document.body.style.overflow = 'auto';
            addActorForm.reset();
            
            // Show success message
            showNotification('Actor added successfully');
        });
    }

    // Create Movie Form Submission
    const createMovieForm = document.getElementById('createMovieForm');
    
    if (createMovieForm) {
        createMovieForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Get form data
            const formData = new FormData(this);
            const movieData = {};
            
            // Process form data
            formData.forEach((value, key) => {
                if (key === 'genres' || key === 'directors' || key === 'actors') {
                    if (!movieData[key]) movieData[key] = [];
                    movieData[key].push(value);
                } else {
                    movieData[key] = value;
                }
            });
            
            // Convert checkboxes to boolean
            movieData.hasSubtitles = document.getElementById('movieSubtitles').checked;
            movieData.isFeatured = document.getElementById('movieFeatured').checked;
            
            // Here you would typically send this data to your backend API
            console.log('Movie data to be submitted:', movieData);
            
            // Show success message and redirect
            alert('Movie added successfully!');
            window.location.href = 'dashboard.html';
        });
    }

    // Helper function to show notifications
    function showNotification(message) {
        const notification = document.createElement('div');
        notification.className = 'notification';
        notification.textContent = message;
        
        document.body.appendChild(notification);
        
        setTimeout(() => {
            notification.classList.add('show');
        }, 10);
        
        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    }

    // Add notification styles
    const style = document.createElement('style');
    style.textContent = `
        .notification {
            position: fixed;
            bottom: 20px;
            right: 20px;
            background-color: var(--success-color);
            color: white;
            padding: 12px 20px;
            border-radius: var(--border-radius);
            box-shadow: var(--box-shadow);
            transform: translateY(100px);
            opacity: 0;
            transition: all 0.3s ease;
            z-index: 9999;
        }
        
        .notification.show {
            transform: translateY(0);
            opacity: 1;
        }
    `;
    document.head.appendChild(style);
}); 