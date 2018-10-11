using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    //These classes represent the data structure of the database

    /// <summary>
    /// Book Model.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Unique identifier for the Book.
        /// </summary>
        /// <example>5</example>
        [Key]
        public int BookId { get; set; }
        /// <summary>
        /// Title of the Book.
        /// </summary>
        /// <example>Hire Chad: Making Great Business Decisions</example>
        [MaxLength(80, ErrorMessage ="* Max length is 80 characters.")]
        [DisplayFormat(ConvertEmptyStringToNull =false, HtmlEncode =false)]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Description of the Book content.
        /// </summary>
        [MaxLength(500,ErrorMessage ="*Maximum length is 500 characters.")]
        [DisplayFormat(HtmlEncode =false,ConvertEmptyStringToNull =false,NullDisplayText ="Description not available at this time.")]
        public string Description { get; set; }
        /// <summary>
        /// Publication date for the Book.
        /// </summary>
        [Display(Name ="Publication Date")]
        [DisplayFormat(ApplyFormatInEditMode =true,ConvertEmptyStringToNull =false,DataFormatString = "{0:MM/dd/yyyy}",HtmlEncode =false)]
        [Required]
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// URL for the cover image.
        /// </summary>
        [Display(Name ="Cover Image URL")]
        [DisplayFormat(ConvertEmptyStringToNull =false, HtmlEncode =false,NullDisplayText ="Cover image not available at this time.")]
        [MaxLength(50,ErrorMessage ="*Max length is 50 characters.")]
        public string CoverImageUrl { get; set; }
        /// <summary>
        /// Genre/Category for the Book, from the Categories Model.
        /// </summary>
        public int CategoryId { get; set; }
    }//end Book

    //Categories (Genre) Model
    /// <summary>
    /// Categories Model.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique Identifier for the Category.
        /// </summary>
        [Key]
        public int CategoryId { get; set; }
        /// <summary>
        /// Name of the Category.
        /// </summary>
        [Required]
        [MaxLength(100,ErrorMessage ="*Max length is 100 characters.")]
        [Display(Name ="Category Name")]
        [DisplayFormat(ConvertEmptyStringToNull =false, HtmlEncode =false, NullDisplayText ="Category name not available at this time.")]
        public string Name { get; set; }
    }//end Categories

    /// <summary>
    /// Author Modal
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Unique Identifier for the Author.
        /// </summary>
        [Key]
        public int AuthorId { get; set; }
        /// <summary>
        /// First name of the Author.
        /// </summary>
        [Required]
        [Display(Name ="First Name")]
        [DisplayFormat(ConvertEmptyStringToNull =false, HtmlEncode =false,NullDisplayText ="Author first name not available.")]
        [MaxLength(80,ErrorMessage ="*Max length is 80 characters.")]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the Author.
        /// </summary>
        [Required]
        [Display(Name ="Last Name")]
        [DisplayFormat(ConvertEmptyStringToNull =false,HtmlEncode =false,NullDisplayText ="Author last name not available.")]
        [MaxLength(80,ErrorMessage ="*Max length is 80 characters.")]
        public string LastName { get; set; }
        /// <summary>
        /// Url for the Head Shot Image of the Author.
        /// </summary>
        [Required]
        [Display(Name ="Headshot Image URL")]
        [DisplayFormat(ConvertEmptyStringToNull =false, HtmlEncode =false,NullDisplayText ="Author headshot image URL is not available.")]
        [MaxLength(100,ErrorMessage ="*Max length is 100 characters.")]
        public string HeadshotImageUrl { get; set; }
    }//end Authors

    /// <summary>
    /// Book Reviews Model
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Unique identifier for the Book Review.
        /// </summary>
        [Key]
        public int ReviewId { get; set; }
        /// <summary>
        /// Text content of the Review.
        /// </summary>
        [Display(Name ="Review Text")]
        [DisplayFormat(ConvertEmptyStringToNull =false,HtmlEncode =false,NullDisplayText ="Review text not available.")]
        [StringLength(10000,ErrorMessage ="*Review text must be at least 50 charcaters and not more than 10,000 characters.",MinimumLength =50)]
        public string ReviewText { get; set; }
        /// <summary>
        /// Review rating of the Book.
        /// </summary>
        [Required]
        [Range(0.0, 5.0,ErrorMessage ="*Ratings must be between 0.0 and 5.0")]
        [DisplayFormat(DataFormatString =("{0:n2}"))]
        public decimal Rating { get; set; }
        /// <summary>
        /// Publication date of the Review.
        /// </summary>
        [Required]
        [Display(Name ="Publication Date")]
        [DisplayFormat(ApplyFormatInEditMode =true,ConvertEmptyStringToNull =false,DataFormatString = "{0:0:MM/dd/yyyy}",HtmlEncode =false,NullDisplayText ="Date is required.")]
        public DateTime PublishDate { get; set; }
        
    }//end Reviews

    //This model is derived from multiple tables
    /// <summary>
    /// Author-Rating Model. Derives Average Rating and Total Reviews.
    /// </summary>
    public class AuthorRating
    {
        //From Author Table
        /// <summary>
        /// Unique identifier of the Author from the Author Model.
        /// </summary>
        [Key]
        public int AuthorId { get; set; }
        /// <summary>
        /// First name of the Author from the Author Model.
        /// </summary>
        [Display(Name ="Author First Name")]
        [DisplayFormat(ConvertEmptyStringToNull =false,HtmlEncode =false,NullDisplayText ="Author first name not available.")]
        [MaxLength(80,ErrorMessage ="*Max length is 80 characters.")]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the Author from the Author Model.
        /// </summary>
        [Display(Name ="Author Last Name")]
        [DisplayFormat(ConvertEmptyStringToNull =false,HtmlEncode =false,NullDisplayText ="Author last name not available.")]
        [MaxLength(80,ErrorMessage ="*Max length is 80 characters.")]
        public string LastName { get; set; }
        //Using Reviews, provide Count as Total Reviews
        /// <summary>
        /// Total number of Reviews from the Review Model.
        /// </summary>
        [Range(0,1000000)]
        [Display(Name ="Total Reviews")]
        public int TotalReviews { get; set; }
        //Using Reviews, provide Average as Average Rating
        /// <summary>
        /// Average Review rating from the Review Model.
        /// </summary>
        [Range(0.0,5.0)]
        [Display(Name ="Average Rating")]
        public decimal AvgRating { get; set; }
    }//end AuthorRating

    /// <summary>
    /// Reference Table for Book and Authors (Many Authors-to-One Book)
    /// </summary>
    public class BookAuthor
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Book Unique Identifier
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Author Unique Identifier
        /// </summary>
        public int AuthorId { get; set; }
    }//end BookAuthors

    /// <summary>
    /// Reference Table for Book and Reviews (Many Reviews-to-One Book)
    /// </summary>
    public class BookReview
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Book Unique Identifier
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Review Unique Identifier
        /// </summary>
        public int ReviewId { get; set; }
        /// <summary>
        /// Author Unique Identifier
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Related Book information.
        /// </summary>
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        /// <summary>
        /// Related Author information.
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
        /// <summary>
        /// Related BookReview information.
        /// </summary>
        public virtual Review Reviews { get; set; }
    }//end BookReviews

    /// <summary>
    /// Author Rankings based on Book Reviews. Minimum of 4 reviews is required.
    /// </summary>
    [Table("dbo.AuthorRankings")]
    public class AuthorRanking
    {
        /// <summary>
        /// Unique identifier for Author, reflects the AuthorId in Authors
        /// </summary>
        [Key]
        [Display(Name ="Author ID")]
        public int AuthorId { get; set; }
        /// <summary>
        /// Author's first name.
        /// </summary>
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        /// <summary>
        /// Author's last name.
        /// </summary>
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        /// <summary>
        /// Average rating for Author's Book Reviews. Minimum of 4 reviews is required.
        /// </summary>
        [Display(Name ="Average Rating",Description ="With a minimum of 4 reviews, represents the average to 2 decimal places.")]
        public decimal AverageRating { get; set; }
    }

    /// <summary>
    /// Book and Category Rankings based on Book Reviews. Minimum of 4 reviews required.
    /// </summary>
    public class BookCategoryRanking
    {
        /// <summary>
        /// Unique Identifier for Book
        /// </summary>
        [Key]
        public int BookId { get; set; }
        /// <summary>
        /// Book title from Book.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Book publication data from Book.
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// Average Rating of Book from Reviews. Minimum of 4 required.
        /// </summary>
        public decimal AverageRating { get; set; }
        /// <summary>
        /// Category from Book.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Author's first name from Authors.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Author's last name from Authors.
        /// </summary>
        public string LastName { get; set; }

    }

}//end namespace