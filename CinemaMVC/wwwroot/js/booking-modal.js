/**
 * Booking Modal JavaScript
 * Handles all functionality for the movie ticket booking process
 */

document.addEventListener('DOMContentLoaded', function() {
    // DOM Elements
    const bookingModal = document.getElementById('bookingModal');
    const closeModalBtn = document.querySelector('.close-modal');
    const backBtn = document.querySelector('.back-btn');
    const dateItems = document.querySelectorAll('.date-item');
    const dateNavPrev = document.querySelector('.date-nav.prev');
    const dateNavNext = document.querySelector('.date-nav.next');
    const timeSlots = document.querySelectorAll('.time-slot:not(.sold-out)');
    const seats = document.querySelectorAll('.seat:not(.taken)');
    const selectedSeatsList = document.getElementById('selectedSeatsList');
    const totalPrice = document.getElementById('totalPrice');
    const completeBookingBtn = document.getElementById('completeBooking');
    const datesContainer = document.querySelector('.dates');
    const trailerBtn = document.querySelector('.trailer-btn');

    // Constants
    const REGULAR_SEAT_PRICE = 12.99;
    const PREMIUM_SEAT_PRICE = 15.99;

    // State
    let selectedSeats = [];
    let currentStep = 2; // Default to seat selection step

    // Initialize
    function init() {
        // Set up event listeners
        setupEventListeners();

        // Disable booking button initially
        if (completeBookingBtn) {
            completeBookingBtn.disabled = true;
            completeBookingBtn.style.opacity = '0.5';
        }

        // Add animation class to modal content
        setTimeout(() => {
            const modalContent = document.querySelector('.booking-modal-content');
            if (modalContent) {
                modalContent.classList.add('animated');
            }
        }, 100);
    }

    // Event Listeners
    function setupEventListeners() {
        // Close button
        if (closeModalBtn) {
            closeModalBtn.addEventListener('click', handleCloseModal);
        }

        // Back button
        if (backBtn) {
            backBtn.addEventListener('click', handleBackButton);
        }

        // Date selection
        dateItems.forEach(item => {
            item.addEventListener('click', handleDateSelection);
        });

        // Date navigation
        if (dateNavPrev) dateNavPrev.addEventListener('click', scrollDatesLeft);
        if (dateNavNext) dateNavNext.addEventListener('click', scrollDatesRight);

        // Time slot selection
        timeSlots.forEach(slot => {
            slot.addEventListener('click', handleTimeSelection);
        });

        // Seat selection
        seats.forEach(seat => {
            seat.addEventListener('click', handleSeatSelection);

            // Hover effects
            seat.addEventListener('mouseenter', function() {
                if (!this.classList.contains('taken')) {
                    this.style.transform = 'scale(1.1)';
                    this.style.boxShadow = '0 0 15px rgba(255, 255, 255, 0.3)';
                }
            });

            seat.addEventListener('mouseleave', function() {
                this.style.transform = '';
                this.style.boxShadow = '';
            });
        });

        // Complete booking button
        if (completeBookingBtn) {
            completeBookingBtn.addEventListener('click', handleCompleteBooking);
        }

        // Trailer button
        if (trailerBtn) {
            trailerBtn.addEventListener('click', handleTrailerClick);
        }
    }

    // Handle trailer button click
    function handleTrailerClick(e) {
        e.preventDefault();

        // Create modal for trailer
        const trailerModal = document.createElement('div');
        trailerModal.className = 'trailer-modal';

        // Get movie title for YouTube search
        const movieTitle = document.getElementById('modalMovieTitle').textContent;

        // Create trailer content - in a real app, you would use the actual trailer URL
        trailerModal.innerHTML = `
            <div class="trailer-modal-content">
                <button class="close-trailer"><i class="fas fa-times"></i></button>
                <div class="trailer-container">
                    <iframe width="100%" height="100%"
                        src="https://www.youtube.com/embed/YoHD9XEInc0?autoplay=1"
                        title="${movieTitle} Trailer"
                        frameborder="0"
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                        allowfullscreen>
                    </iframe>
                </div>
            </div>
        `;

        // Add to DOM
        document.body.appendChild(trailerModal);

        // Prevent body scrolling
        document.body.style.overflow = 'hidden';

        // Add animation
        setTimeout(() => {
            trailerModal.classList.add('active');
        }, 10);

        // Add close event
        const closeTrailerBtn = trailerModal.querySelector('.close-trailer');
        if (closeTrailerBtn) {
            closeTrailerBtn.addEventListener('click', () => {
                trailerModal.classList.remove('active');
                setTimeout(() => {
                    trailerModal.remove();
                    document.body.style.overflow = 'auto';
                }, 300);
            });
        }

        // Add CSS for trailer modal
        addTrailerModalStyles();
    }

    // Add CSS for trailer modal
    function addTrailerModalStyles() {
        // Check if styles already exist
        if (document.getElementById('trailer-modal-styles')) return;

        const style = document.createElement('style');
        style.id = 'trailer-modal-styles';
        style.textContent = `
            .trailer-modal {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0, 0, 0, 0.9);
                z-index: 2000;
                display: flex;
                justify-content: center;
                align-items: center;
                opacity: 0;
                transition: opacity 0.3s ease;
            }

            .trailer-modal.active {
                opacity: 1;
            }

            .trailer-modal-content {
                width: 90%;
                max-width: 900px;
                position: relative;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
                transform: scale(0.9);
                transition: transform 0.3s ease;
            }

            .trailer-modal.active .trailer-modal-content {
                transform: scale(1);
            }

            .close-trailer {
                position: absolute;
                top: -40px;
                right: 0;
                width: 36px;
                height: 36px;
                border-radius: 50%;
                background-color: var(--primary-color);
                border: none;
                color: white;
                display: flex;
                align-items: center;
                justify-content: center;
                cursor: pointer;
                transition: all 0.3s ease;
                z-index: 10;
            }

            .close-trailer:hover {
                background-color: var(--primary-hover);
                transform: rotate(90deg);
            }

            .trailer-container {
                position: relative;
                padding-bottom: 56.25%; /* 16:9 aspect ratio */
                height: 0;
                overflow: hidden;
            }

            .trailer-container iframe {
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                border-radius: 8px;
            }
        `;
        document.head.appendChild(style);
    }



    // Handle date selection
    function handleDateSelection() {
        dateItems.forEach(date => date.classList.remove('active'));
        this.classList.add('active');

        // Add a subtle animation
        this.style.animation = 'pulse 0.5s';
        setTimeout(() => {
            this.style.animation = '';
        }, 500);

        // Update available times based on date (in a real app)
        updateAvailableTimes(this.querySelector('.day').textContent);
    }

    // Handle time selection
    function handleTimeSelection() {
        timeSlots.forEach(time => time.classList.remove('active'));
        this.classList.add('active');

        // Add a subtle animation
        this.style.animation = 'pulse 0.5s';
        setTimeout(() => {
            this.style.animation = '';
        }, 500);

        // Update available seats based on time (in a real app)
        updateAvailableSeats(this.textContent);
    }

    // Handle seat selection
    function handleSeatSelection() {
        const seatId = this.getAttribute('data-seat');

        if (this.classList.contains('selected')) {
            // Deselect seat
            this.classList.remove('selected');
            selectedSeats = selectedSeats.filter(id => id !== seatId);

            // Add deselection animation
            this.animate([
                { transform: 'scale(1.1)' },
                { transform: 'scale(1)' }
            ], {
                duration: 300,
                easing: 'ease-out'
            });
        } else {
            // Select seat
            this.classList.add('selected');
            selectedSeats.push(seatId);

            // Add selection animation
            this.animate([
                { transform: 'scale(0.9)' },
                { transform: 'scale(1.1)' },
                { transform: 'scale(1)' }
            ], {
                duration: 400,
                easing: 'ease-out'
            });
        }

        updateSelectedSeatsDisplay();
    }

    // Update selected seats display
    function updateSelectedSeatsDisplay() {
        if (selectedSeats.length === 0) {
            selectedSeatsList.textContent = 'No seats selected';
            totalPrice.textContent = '$0.00';

            if (completeBookingBtn) {
                completeBookingBtn.disabled = true;
                completeBookingBtn.style.opacity = '0.5';
            }
            return;
        }

        // Sort seats by row and number
        selectedSeats.sort((a, b) => {
            const aRow = a.charAt(0);
            const bRow = b.charAt(0);

            if (aRow === bRow) {
                return parseInt(a.substring(1)) - parseInt(b.substring(1));
            }

            return aRow.localeCompare(bRow);
        });

        // Display selected seats
        selectedSeatsList.textContent = selectedSeats.join(', ');

        // Calculate and display total price
        let total = 0;
        selectedSeats.forEach(seatId => {
            const seatElement = document.querySelector(`.seat[data-seat="${seatId}"]`);
            if (seatElement.classList.contains('premium')) {
                total += PREMIUM_SEAT_PRICE;
            } else {
                total += REGULAR_SEAT_PRICE;
            }
        });

        // Animate price change
        animateCounter(totalPrice, parseFloat(totalPrice.textContent.replace('$', '')), total);

        // Enable continue button
        if (completeBookingBtn) {
            completeBookingBtn.disabled = false;
            completeBookingBtn.style.opacity = '1';
        }
    }

    // Handle complete booking
    function handleCompleteBooking() {
        if (selectedSeats.length === 0) {
            showNotification('Please select at least one seat to continue.', 'warning');
            return;
        }

        // Get selected date and time
        const selectedDate = document.querySelector('.date-item.active');
        const selectedTime = document.querySelector('.time-slot.active');

        // For demo purposes, show a confirmation message
        const dateText = selectedDate ? `${selectedDate.querySelector('.day').textContent} ${selectedDate.querySelector('.weekday').textContent}` : '';
        const timeText = selectedTime ? selectedTime.textContent : '';

        // Create a summary for the alert
        const summary = `Movie: ${document.getElementById('modalMovieTitle').textContent}\nDate: ${dateText}\nTime: ${timeText}\nSeats: ${selectedSeats.join(', ')}\nTotal: ${totalPrice.textContent}`;

        // Show confirmation and proceed to next step
        showNotification('Proceeding to payment...', 'success');

        // In a real app, we would move to the next step
        // For demo, just show an alert
        setTimeout(() => {
            alert(`Proceeding to payment...\n\n${summary}`);
        }, 1000);
    }

    // Scroll dates left
    function scrollDatesLeft() {
        datesContainer.scrollBy({
            left: -200,
            behavior: 'smooth'
        });
    }

    // Scroll dates right
    function scrollDatesRight() {
        datesContainer.scrollBy({
            left: 200,
            behavior: 'smooth'
        });
    }

    // Update available times based on selected date (simulation)
    function updateAvailableTimes(day) {
        // In a real app, this would fetch available times from a server
        // For demo purposes, we'll just simulate different times for different days

        // Reset all time slots
        document.querySelectorAll('.time-slot').forEach(slot => {
            slot.classList.remove('sold-out');
            slot.classList.remove('active');
        });

        // Select the first time slot as active by default
        const firstAvailableSlot = document.querySelector('.time-slot:not(.sold-out)');
        if (firstAvailableSlot) firstAvailableSlot.classList.add('active');

        // Simulate sold-out times based on the day
        if (day === '5') {
            // Thursday is busy
            document.querySelectorAll('.time-slot')[3].classList.add('sold-out');
        } else if (day === '6' || day === '7') {
            // Weekend is very busy
            document.querySelectorAll('.time-slot')[2].classList.add('sold-out');
            document.querySelectorAll('.time-slot')[4].classList.add('sold-out');
        }
    }

    // Update available seats based on selected time (simulation)
    function updateAvailableSeats(time) {
        // In a real app, this would fetch available seats from a server
        // For demo purposes, we'll just simulate different seat availability

        // Reset all seats
        document.querySelectorAll('.seat').forEach(seat => {
            if (seat.classList.contains('selected')) return; // Don't change selected seats

            seat.classList.remove('taken');
            seat.classList.add('available');
        });

        // Simulate taken seats based on time
        if (time === '4:30 PM') {
            // Afternoon show is moderately busy
            const takenSeats = ['A3', 'A7', 'A8', 'B4', 'B5', 'C9', 'C10', 'F5', 'F6'];
            takenSeats.forEach(seatId => {
                const seat = document.querySelector(`.seat[data-seat="${seatId}"]`);
                if (seat) {
                    seat.classList.remove('available');
                    seat.classList.add('taken');
                }
            });
        } else if (time === '7:15 PM') {
            // Evening show is busier
            const takenSeats = ['A2', 'A3', 'A4', 'B3', 'B4', 'B5', 'B6', 'C7', 'C8', 'C9', 'C10', 'D5', 'D6', 'E5', 'E6', 'F5', 'F6'];
            takenSeats.forEach(seatId => {
                const seat = document.querySelector(`.seat[data-seat="${seatId}"]`);
                if (seat) {
                    seat.classList.remove('available');
                    seat.classList.add('taken');
                }
            });
        }

        // Clear any selected seats when changing times
        selectedSeats = [];
        document.querySelectorAll('.seat.selected').forEach(seat => {
            seat.classList.remove('selected');
        });
        updateSelectedSeatsDisplay();
    }

    // Helper: Animate counter for price changes
    function animateCounter(element, start, end) {
        const duration = 500;
        const startTime = performance.now();

        function updateCounter(currentTime) {
            const elapsedTime = currentTime - startTime;

            if (elapsedTime < duration) {
                const progress = elapsedTime / duration;
                const current = start + (end - start) * progress;
                element.textContent = `$${current.toFixed(2)}`;
                requestAnimationFrame(updateCounter);
            } else {
                element.textContent = `$${end.toFixed(2)}`;
            }
        }

        requestAnimationFrame(updateCounter);
    }

    // Helper: Show notification
    function showNotification(message, type = 'info') {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        notification.innerHTML = `
            <div class="notification-icon">
                <i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'warning' ? 'fa-exclamation-triangle' : 'fa-info-circle'}"></i>
            </div>
            <div class="notification-message">${message}</div>
        `;

        // Add to DOM
        document.body.appendChild(notification);

        // Animate in
        setTimeout(() => {
            notification.classList.add('show');
        }, 10);

        // Remove after delay
        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => {
                notification.remove();
            }, 300);
        }, 3000);
    }

    // Add CSS for notifications
    function addNotificationStyles() {
        // Check if styles already exist
        if (document.getElementById('notification-styles')) return;

        const style = document.createElement('style');
        style.id = 'notification-styles';
        style.textContent = `
            @keyframes pulse {
                0% { transform: scale(1); }
                50% { transform: scale(1.05); }
                100% { transform: scale(1); }
            }

            .notification {
                position: fixed;
                top: 20px;
                right: 20px;
                background-color: rgba(0, 0, 0, 0.8);
                color: white;
                padding: 15px 20px;
                border-radius: 8px;
                display: flex;
                align-items: center;
                gap: 12px;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
                z-index: 2000;
                transform: translateX(120%);
                transition: transform 0.3s ease;
                max-width: 350px;
            }

            .notification.show {
                transform: translateX(0);
            }

            .notification-icon {
                font-size: 1.2rem;
            }

            .notification.success .notification-icon {
                color: var(--success-color);
            }

            .notification.warning .notification-icon {
                color: var(--warning-color);
            }

            .notification-message {
                font-size: 0.9rem;
            }
        `;
        document.head.appendChild(style);
    }

    // Initialize
    addNotificationStyles();
    init();
});