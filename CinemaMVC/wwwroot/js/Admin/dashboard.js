document.addEventListener('DOMContentLoaded', function() {
    // Tab Navigation
    const tabItems = document.querySelectorAll('.sidebar-nav li');

    tabItems.forEach(item => {
        item.addEventListener('click', function() {
            // Remove active class from all tabs
            tabItems.forEach(tab => tab.classList.remove('active'));
            // Add active class to clicked tab
            this.classList.add('active');
        });
    });

    document.querySelectorAll('.sidebar-nav li').forEach(item => {
        item.addEventListener('click', function () {
            const link = this.querySelector('a');
            if (link && link.href) {
                window.location.href = link.href;
            }
        });
    });

    // Action buttons for edit/delete in tables
    const editButtons = document.querySelectorAll('.action-icon.edit:not([href])');
    const deleteButtons = document.querySelectorAll('.action-icon.delete:not([type="submit"])');

    editButtons.forEach(button => {
        button.addEventListener('click', handleEditClick);
    });

    deleteButtons.forEach(button => {
        button.addEventListener('click', handleDeleteClick);
    });

    function handleEditClick() {
        const row = this.closest('tr');
        const id = row.dataset.id;

        // Here you would typically fetch the item details and open the edit modal
        console.log(`Edit item with ID: ${id}`);

        // For demonstration purposes
        showNotification('Edit mode activated');
    }

    function handleDeleteClick() {
        const row = this.closest('tr');
        const id = row.dataset.id;

        // Confirm deletion
        if (confirm('Are you sure you want to delete this item?')) {
            // Here you would typically send a delete request to your backend API
            console.log(`Delete item with ID: ${id}`);

            // For demonstration, remove the row with animation
            row.style.transition = 'opacity 0.3s ease';
            row.style.opacity = '0';

            setTimeout(() => {
                row.remove();
                showNotification('Item deleted successfully');
            }, 300);
        }
    }

    // Pagination functionality
    const paginationButtons = document.querySelectorAll('.pagination .page-btn');

    if (paginationButtons.length > 0) {
        paginationButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                
                // Remove active class from all pagination buttons
                paginationButtons.forEach(btn => btn.classList.remove('active'));

                // Add active class to clicked button (except for next button)
                if (!this.classList.contains('next')) {
                    this.classList.add('active');
                }

                // Here you would typically fetch data for the selected page
                // For demonstration, we'll just show a notification
                if (!this.classList.contains('next')) {
                    showNotification(`Showing page ${this.textContent}`);
                } else {
                    // Find the currently active button and click the next one
                    const activeButton = document.querySelector('.pagination .page-btn.active');
                    const activeIndex = Array.from(paginationButtons).indexOf(activeButton);

                    if (activeIndex < paginationButtons.length - 2) { // -2 because we exclude the "next" button
                        paginationButtons[activeIndex + 1].click();
                    }
                }
            });
        });
    }

    // Filter functionality
    const filterSelects = document.querySelectorAll('.filter-bar select');

    if (filterSelects.length > 0) {
        filterSelects.forEach(select => {
            select.addEventListener('change', function(e) {
                e.preventDefault();
                
                const filterType = this.previousElementSibling.textContent.includes('Genre') ? 'genre' : 'sort';
                const value = this.value;

                console.log(`Filtering by ${filterType}: ${value}`);
                
                // Client-side filtering for demonstration
                // In a real application, you would make an AJAX request or use a more sophisticated filtering system
                if (filterType === 'genre') {
                    filterTableByGenre(value);
                } else if (filterType === 'sort') {
                    sortTable(value);
                }

                showNotification(`Applied ${filterType} filter: ${value}`);
            });
        });
    }
    
    // Client-side filtering function
    function filterTableByGenre(genre) {
        const tableRows = document.querySelectorAll('tbody tr');
        
        if (genre === 'all') {
            tableRows.forEach(row => {
                row.style.display = '';
            });
            return;
        }
        
        // This is a simplified example - in a real app, you would need to know which genres each movie has
        // For now, we'll just simulate filtering by checking if the title contains the genre name
        tableRows.forEach(row => {
            const title = row.querySelector('td:first-child').textContent.toLowerCase();
            if (title.includes(genre.toLowerCase())) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    }
    
    // Client-side sorting function
    function sortTable(sortBy) {
        const tableBody = document.querySelector('tbody');
        const tableRows = Array.from(tableBody.querySelectorAll('tr'));
        
        // Sort the rows based on the selected criteria
        tableRows.sort((a, b) => {
            if (sortBy === 'title') {
                const titleA = a.querySelector('td:first-child').textContent.toLowerCase();
                const titleB = b.querySelector('td:first-child').textContent.toLowerCase();
                return titleA.localeCompare(titleB);
            } else if (sortBy === 'rating') {
                const ratingA = parseFloat(a.querySelector('.rating span').textContent);
                const ratingB = parseFloat(b.querySelector('.rating span').textContent);
                return ratingB - ratingA; // Descending order for ratings
            }
            // Default to newest first (we don't have date info in the table, so we'll use the ID as a proxy)
            return parseInt(b.dataset.id) - parseInt(a.dataset.id);
        });
        
        // Re-append the sorted rows to the table body
        tableRows.forEach(row => {
            tableBody.appendChild(row);
        });
    }

    // Show notification function
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
    const notificationStyle = document.createElement('style');
    notificationStyle.textContent = `
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
    document.head.appendChild(notificationStyle);
});