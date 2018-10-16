using Bookstore.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bookstore.API.Controllers
{
    /// <summary>
    /// Supported queries for BooksReviews within the AMC Bookstore. These queries show the relationship between a specific Reviews object and the corresponding Books and Authors objects.
    /// </summary>
    [RoutePrefix("api/bookreview")]
    public class BooksReviewsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/bookreview/
        /// <summary>
        /// GET List of all BookReview objects. These are the relationships between Books, Reviews, and Authors. Links are established through unique identifiers for each object.
        /// </summary>
        /// <returns>Returns list of all BookReview objects and associated Reviews, Books, and Authors objects.</returns>
        [Route("get")]
        public IEnumerable<BooksReviews> GetBooksReviews()
        {
            return db.BooksReviews;
        }

        // GET: api/bookreview/details/5
        /// <summary>
        /// GET: Single BookReview object by Id.
        /// </summary>
        /// <param name="bookreviewId">Unique identifier for the BookReview object.</param>
        /// <returns>Returns 200 OK with full Authors, Books, Reviews, and BooksReviews object. Returns 404 NOT FOUND if brId does not exist in the database.</returns>
        [Route("details/{bookreviewId}")]
        public IHttpActionResult GetBooksReviews(int bookreviewId)
        {
            BooksReviews br = db.BooksReviews.Find(bookreviewId);
            if (br == null)
            {
                return NotFound();
            }
            return Ok(br);
        }

        // POST: api/bookreview/create
        /// <summary>
        /// Create a new BooksReview object. Request must include valid Books, Authors, and Reviews objects.
        /// </summary>
        /// <param name="bookReview">Full qualified BooksReviews object. Includes Books, Reviews, and Authors objects.</param>
        /// <returns>If ModelState is not valid, returns 400 BAD REQUEST. Returns 200 OK, with BooksReviews object, if successful.</returns>
        [Route("create")]
        public IHttpActionResult CreateBooksReviews([FromBody] BooksReviews bookReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BooksReviews.Add(bookReview);
            db.SaveChanges();

            return Ok(bookReview);
        }

        // PUT: api/bookreview/edit/5
        /// <summary>
        /// Update a Review using the ReviewId (brId) and a full BooksReviews object.
        /// </summary>
        /// <param name="bookreviewId">Unique identifier for the BooksReviews relationships.</param>
        /// <param name="booksReview">Fully qualified BooksReviews object.</param>
        /// <returns>Returns 400 BAD REQUEST for invalid BooksReviews objects. Returns 400 BAD REQUEST if the brId and booksReview.Id do not match. Returns a 404 NOT FOUND if the brId does not match an existing BooksReview. Returns 204 NO CONTENT if PUT is successful.</returns>
        [Route("edit/{bookreviewId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooksReview(int bookreviewId, [FromBody] BooksReviews booksReview)
        {
            //Model requires inclusion of Author, Book, & Review objects;
            Authors author = db.Authors.Find(booksReview.AuthorId);
            Reviews review = db.Reviews.Find(booksReview.ReviewId);
            Books book = db.Books.Find(booksReview.BookId);
            //Add existing Author, Book, and Review to booksReview object;
            booksReview.Author = author;
            booksReview.Reviews = review;
            booksReview.Book = book;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookreviewId != booksReview.Id)
            {
                return BadRequest();
            }

            db.Entry(booksReview).State = EntityState.Modified;

            

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksReviewExists(bookreviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(booksReview);
        }//end PutBooksReview()

        // DELETE: api/bookreview/delete/5
        /// <summary>
        /// DELETE: Remove a BookReviw object from the database.
        /// </summary>
        /// <param name="bookreviewId">Unique identifier for the BookReview object.</param>
        /// <return></return>
        [Route("delete/{bookreviewId}")]
        public IHttpActionResult Delete(int bookreviewId)
        {
            BooksReviews br = db.BooksReviews.Find(bookreviewId);

            if (br != null)
            {
                db.BooksReviews.Remove(br);
                db.SaveChanges();

                return Ok(br);
            }
            else
            {
                return BadRequest($"No BookReview object with bookReviewId of {bookreviewId} was found.");
            }
        }//end Delete()

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BooksReviewExists(int id)
        {
            return (db.BooksReviews.Count(br => br.Id == id) > 0);
        }
    }
}
