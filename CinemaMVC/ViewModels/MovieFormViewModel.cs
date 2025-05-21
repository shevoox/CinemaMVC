using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaMVC.Models.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute")]
        public int DurationMinutes { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ReleaseYear { get; set; }
        
        [Range(0, 5)]
        public decimal Rating { get; set; }
        
        [StringLength(50)]
        public string Format { get; set; } // IMAX, 3D, Standard, 4DX, etc.
        
        public bool HasSubtitles { get; set; }
        
        public bool IsFeatured { get; set; }
        
        [StringLength(200)]
        public string PosterImage { get; set; }
        
        [StringLength(200)]
        [Display(Name = "Trailer URL")]
        public string TrailerUrl { get; set; }
        
        // For file upload
        [Display(Name = "Upload Poster")]
        public IFormFile PosterFile { get; set; }
        
        // Price
        [Required]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        // Language
        [StringLength(50)]
        public string Language { get; set; }
        
        // Director
        public int? DirectorId { get; set; }
        public List<Director> Directors { get; set; }
        
        // Theaters
        public List<int> SelectedTheaterIds { get; set; }
        public List<Theater> Theaters { get; set; }
        
        // Cast
        public List<int> SelectedCastIds { get; set; }
        public List<Cast> Casts { get; set; }
        
        // For genre selection
        public List<Genre> Genres { get; set; }
        public List<int> SelectedGenreIds { get; set; }
        
        public MovieFormViewModel()
        {
            Genres = new List<Genre>();
            SelectedGenreIds = new List<int>();
            Directors = new List<Director>();
            Theaters = new List<Theater>();
            Casts = new List<Cast>();
            SelectedCastIds = new List<int>();
            SelectedTheaterIds = new List<int>();
        }
    }
} 