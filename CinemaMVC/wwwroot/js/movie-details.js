//document.addEventListener('DOMContentLoaded', function() {
//    // Get movie ID from URL
//    const urlParams = new URLSearchParams(window.location.search);
//    const movieTitle = urlParams.get('title');

//    // Movie data (in a real app, this would come from a database)
//    const movies = {
//        'La La Land': {
//            title: 'La La Land',
//            rating: '4.5',
//            duration: '2h 8min',
//            year: '2016',
//            genres: ['Musical', 'Romance', 'Drama'],
//            description: 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future.',
//            backdrop: 'images/La La Land.jpg',
//            poster: 'images/La La Land.jpg',
//            director: 'Damien Chazelle',
//            language: 'English',
//            cast: [
//                { name: 'Ryan Gosling', photo: 'images/Crow/Ryan_Gosling.jpg' },
//                { name: 'Emma Stone', photo: 'images/Crow/Emma_Stone.jpg' }
//            ],
//            showtimes: ['14:30', '17:00', '19:30', '22:00']
//        },
//        'Inception': {
//            title: 'Inception',
//            rating: '4.8',
//            duration: '2h 28min',
//            year: '2010',
//            genres: ['Action', 'Sci-Fi', 'Thriller'],
//            description: 'A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.',
//            backdrop: 'images/Inception.jpg',
//            poster: 'images/Inception.jpg',
//            director: 'Christopher Nolan',
//            language: 'English',
//            cast: [
//                { name: 'Leonardo DiCaprio', photo: 'images/Crow/Leonardo_DiCaprio.jpg' },
//                { name: 'Joseph Gordon-Levitt', photo: 'images/Crow/Joseph_Gordon_Levitt.jpg' }
//            ],
//            showtimes: ['13:00', '16:00', '19:00', '22:00']
//        },
//        'Interstellar': {
//            title: 'Interstellar',
//            rating: '4.7',
//            duration: '2h 49min',
//            year: '2014',
//            genres: ['Adventure', 'Drama', 'Sci-Fi'],
//            description: 'A team of explorers travel through a wormhole in space in an attempt to ensure humanity\'s survival.',
//            backdrop: 'images/Interstellar.jpg',
//            poster: 'images/Interstellar.jpg',
//            director: 'Christopher Nolan',
//            language: 'English',
//            cast: [
//                { name: 'Matthew McConaughey', photo: 'images/Crow/Matthew_McConaughey.jpg' },
//                { name: 'Anne Hathaway', photo: 'images/Crow/Anne_Hathaway.jpg' }
//            ],
//            showtimes: ['14:00', '17:30', '21:00']
//        },
//        'Pulp Fiction': {
//            title: 'Pulp Fiction',
//            rating: '4.8',
//            duration: '2h 34min',
//            year: '1994',
//            genres: ['Crime', 'Drama'],
//            description: 'The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.',
//            backdrop: 'images/Pulp Fiction.jpg',
//            poster: 'images/Pulp Fiction.jpg',
//            director: 'Quentin Tarantino',
//            language: 'English',
//            cast: [
//                { name: 'John Travolta', photo: 'images/Crow/John_Travolta.jpg' },
//                { name: 'Samuel L. Jackson', photo: 'images/Crow/Samuel_L_Jackson.jpg' }
//            ],
//            showtimes: ['15:00', '18:30', '22:00']
//        }
//    };

//    // Get movie data
//    const movie = movies[movieTitle];

//    if (movie) {
//        // Update page content
//        document.getElementById('movie-backdrop-img').src = movie.backdrop;
//        document.getElementById('movie-poster-img').src = movie.poster;
//        document.getElementById('movie-title').textContent = movie.title;
//        document.getElementById('movie-rating').textContent = movie.rating;
//        document.getElementById('movie-duration').textContent = movie.duration;
//        document.getElementById('movie-year').textContent = movie.year;
//        document.getElementById('movie-description').textContent = movie.description;
//        document.getElementById('movie-director').textContent = movie.director;
//        document.getElementById('movie-language').textContent = movie.language;

//        // Update genres
//        const genresContainer = document.getElementById('movie-genres');
//        movie.genres.forEach(genre => {
//            const genreTag = document.createElement('span');
//            genreTag.className = 'genre-tag';
//            genreTag.textContent = genre;
//            genresContainer.appendChild(genreTag);
//        });

//        // Update showtimes
//        const showtimesContainer = document.getElementById('movie-showtimes');
//        movie.showtimes.forEach(time => {
//            const timeSlot = document.createElement('div');
//            timeSlot.className = 'showtime-slot';
//            timeSlot.textContent = time;
//            showtimesContainer.appendChild(timeSlot);
//        });

//        // Update cast
//        const castContainer = document.getElementById('movie-cast');
//        movie.cast.forEach(actor => {
//            const castItem = document.createElement('div');
//            castItem.className = 'cast-item';
//            castItem.innerHTML = `
//                <div class="cast-photo">
//                    <img src="${actor.photo}" alt="${actor.name}">
//                </div>
//                <div class="cast-name">${actor.name}</div>
//            `;
//            castContainer.appendChild(castItem);
//        });
//    } else {
//        // Handle movie not found
//        document.querySelector('.movie-details-container').innerHTML = `
//            <div style="text-align: center; padding: 50px;">
//                <h2>Movie not found</h2>
//                <a href="index.html" style="color: var(--accent-color); text-decoration: none;">
//                    <i class="fas fa-arrow-left"></i> Back to Home
//                </a>
//            </div>
//        `;
//    }
//});