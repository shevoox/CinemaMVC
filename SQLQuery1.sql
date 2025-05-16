-- 1. Genres
INSERT INTO Genres (Name, IconClass) VALUES
( 'Action', 'fa-bolt'),
( 'Drama', 'fa-theater-masks'),
( 'Sci-Fi', 'fa-robot'),
( 'Romance', 'fa-heart'),
( 'Animation', 'fa-film');

-- 2. Movies
INSERT INTO Movies ( Title, Description, PosterImage, Rating, DurationMinutes, Format, HasSubtitles, ReleaseYear, IsFeatured)
VALUES
('Inception', 'A thief enters peoples dreams.', 'inception.jpg', 4.8, 148, 'IMAX', 1, '2010', 1),
('The Matrix', 'A computer hacker discovers reality.', 'matrix.jpg', 4.7, 136, 'Standard', 1, '1999', 1),
('Interstellar', 'A team travels through a wormhole.', 'interstellar.jpg', 4.6, 169, 'IMAX', 1, '2014', 1),
('Titanic', 'Love story on the ill-fated ship.', 'titanic.jpg', 4.5, 195, 'Standard', 1, '1997', 1),
('Avatar', 'Humans invade alien world Pandora.', 'avatar.jpg', 4.3, 162, '3D', 1, '2009', 1),
('John Wick', 'An ex-hitman seeks revenge.', 'johnwick.jpg', 4.4, 101, 'Standard', 1, '2014', 1),
('The Dark Knight', 'Batman battles Joker.', 'darkknight.jpg', 4.9, 152, 'IMAX', 1, '2008', 1),
('Forrest Gump', 'Man witnesses history.', 'forrestgump.jpg', 4.7, 142, 'Standard', 1, '1994', 1),
('The Godfather', 'Mafia family saga.', 'godfather.jpg', 4.9, 175, 'Standard', 1, '1972', 1),
( 'Pulp Fiction', 'Stories of LA criminals.', 'pulpfiction.jpg', 4.8, 154, 'Standard', 1, '1994', 1),
( 'Fight Club', 'Underground fight scene.', 'fightclub.jpg', 4.7, 139, 'Standard', 1, '1999', 1),
( 'Joker', 'Man descends into madness.', 'joker.jpg', 4.5, 122, 'IMAX', 1, '2019', 1),
( 'The Avengers', 'Superheroes unite.', 'avengers.jpg', 4.4, 143, '3D', 1, '2012', 1),
( 'Frozen', 'Sisters face magic.', 'frozen.jpg', 4.2, 102, 'Standard', 1, '2013', 1),
( 'Shrek', 'Ogre goes on quest.', 'shrek.jpg', 4.3, 90, 'Standard', 1, '2001', 1),
( 'Gladiator', 'Roman general seeks revenge.', 'gladiator.jpg', 4.6, 155, 'Standard', 1, '2000', 1),
( 'The Lion King', 'Lion cub becomes king.', 'lionking.jpg', 4.4, 88, 'Animation', 1, '1994', 1),
( 'Black Panther', 'Wakanda’s king defends nation.', 'blackpanther.jpg', 4.5, 134, 'IMAX', 1, '2018', 1),
( 'The Notebook', 'Love story through time.', 'notebook.jpg', 4.1, 123, 'Standard', 1, '2004', 1),
( 'La La Land', 'Love and ambition clash.', 'lalaland.jpg', 4.3, 128, 'Standard', 1, '2016', 1);

-- 3. MovieGenres
INSERT INTO MovieGenres (MovieId, GenreId) VALUES
(1, 1), (1, 3),
(2, 1), (2, 3),
(3, 3), (3, 2),
(4, 2), (4, 4),
(5, 1), (5, 3),
(6, 1),
(7, 1),
(8, 2),
(9, 2),
(10, 2),
(11, 1), (11, 2),
(12, 2),
(13, 1),
(14, 5),
(15, 5),
(16, 1),
(17, 5),
(18, 1),
(19, 4),
(20, 2), (20, 4);

-- 4. Theaters
INSERT INTO Theaters ( Name, Capacity, TheaterType) VALUES
( 'Cinema Hall A', 100, 'IMAX'),
( 'Cinema Hall B', 80, '3D'),
( 'Cinema Hall C', 60, 'Standard');

-- 5. Showtimes (create 1 per movie, for simplicity)
INSERT INTO Showtimes ( MovieId, TheaterId, StartTime, EndTime, TotalSeats, AvailableSeats, Price) VALUES
(1, 1, '2025-05-14 18:00', '2025-05-14 20:30', 100, 75, 120),
(2, 2, '2025-05-14 21:00', '2025-05-14 23:15', 80, 60, 100),
(3, 3, '2025-05-15 15:00', '2025-05-15 17:45', 60, 40, 90),
(4, 1, '2025-05-15 18:00', '2025-05-15 21:15', 100, 98, 110),
(5, 2, '2025-05-15 20:00', '2025-05-15 22:30', 80, 80, 130),
(6, 3, '2025-05-16 13:00', '2025-05-16 15:00', 60, 59, 100),
(7, 1, '2025-05-16 16:00', '2025-05-16 18:30', 100, 80, 120),
(8, 2, '2025-05-16 19:00', '2025-05-16 21:30', 80, 70, 100),
(9, 3, '2025-05-17 17:00', '2025-05-17 19:55', 60, 50, 100),
( 10, 1, '2025-05-17 20:00', '2025-05-17 22:30', 100, 95, 110),
( 11, 2, '2025-05-18 15:00', '2025-05-18 17:20', 80, 60, 95),
( 12, 3, '2025-05-18 18:00', '2025-05-18 20:00', 60, 58, 120),
( 13, 1, '2025-05-18 21:00', '2025-05-18 23:30', 100, 98, 130),
( 14, 2, '2025-05-19 10:00', '2025-05-19 11:45', 80, 75, 80),
( 15, 3, '2025-05-19 12:00', '2025-05-19 13:45', 60, 50, 85),
( 16, 1, '2025-05-19 14:00', '2025-05-19 16:30', 100, 100, 115),
( 17, 2, '2025-05-19 17:00', '2025-05-19 18:30', 80, 60, 90),
( 18, 3, '2025-05-19 19:00', '2025-05-19 21:15', 60, 55, 125),
( 19, 1, '2025-05-20 16:00', '2025-05-20 18:00', 100, 95, 100),
( 20, 2, '2025-05-20 18:30', '2025-05-20 20:30', 80, 70, 105);

-- لاحقًا يمكننا نكمل INSERT لبيانات المقاعد Seat، والحجوزات Booking لو تحب
