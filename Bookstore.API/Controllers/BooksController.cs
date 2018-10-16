using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Description;
using Bookstore.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Bookstore.API.Controllers
{
    /// <summary>
    /// Supported queries for Books within the AMC Bookstore. Don't see what you need? Contact us!
    /// </summary>
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        //Set the ApplicationDBContext:
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: api/books/get
        /// <summary>
        /// Get a list of all books in the database.
        /// </summary>
        /// <returns>Returns all Books in the database.</returns>
        [Route("get")]
        [HttpGet]
        public IEnumerable<Books> GetBooks()
        {
            List<Books> books = db.Books.ToList();

            //Remove extra spaces from values:
            foreach (Books book in books)
            {
                book.Title = book.Title.Trim();
                book.Description = book.Description.Trim();
                book.CoverImageUrl = book.CoverImageUrl.Trim();
            }

            return books;
        }//end GetBooks()

        //GET: api/books/details/5
        /// <summary>
        /// Get a single book based on the unique identifier for that book.
        /// </summary>
        /// <param name="bookId">Id of the Book in the database.</param>
        /// <returns>Returns a single Book object filtered by the Id in the query. Returns a 404 NOT FOUND if the Book object does not exist in the database.</returns>
        [Route("details/{bookId:int}")]
        [ResponseType(typeof(Books))]
        [HttpGet]
        public IHttpActionResult GetBook(int bookId)
        {
            Books book = db.Books.Find(bookId);
            if (!BookExists(bookId))
            {
                return NotFound();
            }
            //Remove extra spaces from values:
            book.Title = book.Title.Trim();
            book.Description = book.Description.Trim();
            book.CoverImageUrl = book.CoverImageUrl.Trim();

            return Ok(book);
        }//end GetBook()

        //GET: api/books/title/twilight
        /// <summary>
        /// Get a list of books that contain the search term in the title.
        /// </summary>
        /// <param name="title">Title to query against all Book titles.</param>
        /// <returns>Returns a list of Book objects filtered by the value provided. The title variable acts as a wildcard. List is alphabetized by title.</returns>
        [Route("title/{title}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Books>))]
        public IHttpActionResult GetBookByTitle(string title)
        {
            //Test query string for NULL values:
            if (title == null)
            {
                return NotFound();
            }

            //Change query string to LOWER case to avoid casing errors:
            string queryString = title.ToLower();

            //Create IEnumberable<Book> to capture results:            
            IEnumerable<Books> books = db.Books.Where(b => b.Title.ToLower().Contains(queryString)).OrderBy(b => b.Title);

            foreach (Books book in books)
            {
                book.Title = book.Title.Trim();
                book.CoverImageUrl = book.CoverImageUrl.Trim();
                book.Description = book.Description.Trim();
            }

            return Ok(books);

        }//end GetBookByTitle()

        //GET: api/books/category/childrens
        /// <summary>
        /// Get a list of books based on category name.
        /// </summary>
        /// <param name="category">Category name to query against all Book categories.</param>
        /// <returns>Returns a list of Book objects filtered by the String in the query. List is alphabetized by book title.</returns>
        [Route("category/{category}")]
        [ResponseType(typeof(IEnumerable<Books>))]
        [HttpGet]
        public IHttpActionResult GetBookByCategory(string category)
        {
            string queryString = category.ToLower();
            Categories cat = db.Categories.Where(c => c.Name.ToLower().Trim() == queryString.Trim() || c.Name.ToLower().Trim().Contains(queryString)).FirstOrDefault();

            if (cat == null)
            {
                return BadRequest($"No books found for Category {category}.");
            }
            IEnumerable<Books> books = db.Books.Where(b => b.CategoryId == cat.CategoryId);

            foreach (Books book in books)
            {
                book.Title = book.Title.Trim();
                book.CoverImageUrl = book.CoverImageUrl.Trim();
                book.Description = book.Description.Trim();
            }

            return Ok(books);
        }//end GetBookByCategory()

        /// <summary>
        /// GET List of books by AuthorId
        /// </summary>
        /// <param name="authorId">Unique identifier of the Author object.</param>
        /// <returns>Returns a 200 OK with a list of Books objects. If no books are found, returns a 404 NOT FOUND.</returns>
        [Route("author/{authorId}/details")]
        [ResponseType(typeof(IEnumerable<Books>))]
        public IHttpActionResult GetBookByAuthorId(int authorId)
        {
            if (!AuthorExists(authorId))
            {
                return NotFound();
            }
            IEnumerable<BooksAuthorsView> bookViews = db.BooksAuthorsView.Where(b => b.AuthorId == authorId).ToList();

            List<Books> books = new List<Books>();

            foreach (var item in bookViews)
            {
                Books book = db.Books.Find(item.BookId);
                books.Add(book);
            }

            if (books.Count() > 0)
            {
                return Ok(books.ToList());
            }
            
            return NotFound();
        }//end GetBookByAuthorId

        
        //GET api/books/date/2018-10-12
        /// <summary>
        /// Query Books object by Publication Date.
        /// </summary>
        /// <param name="pubDate">Publication date of the Books object. Valid date formats are yyyy-mm-dd or yyyy/mm/dd.</param>
        /// <returns>Returns list of Books objects published on the query data. Returns 404 NOT FOUND if no Books objects were published on that date. Returns 400 BAD REQUEST if the pubDate value is not properly formatted.</returns>
        [Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [HttpGet]
        [ResponseType(typeof(List<Books>))]
        public IHttpActionResult GetBooksByDate(DateTime pubDate)
        {

            List<Books> books = db.Books.Where(b => DbFunctions.TruncateTime(b.PublishDate) == DbFunctions.TruncateTime(pubDate)).ToList();

            if (books.Count == 0)
            {
                return NotFound();
            }

            foreach (Books book in books)
            {
                book.Title = book.Title.Trim();
                book.CoverImageUrl = book.CoverImageUrl.Trim();
                book.Description = book.Description.Trim();
            }

            return Ok(books);
        }


        //GET api/books/stats/14
        /// <summary>
        /// Get Book Rating using bookId
        /// </summary>
        /// <param name="bookId">Unique Identifier for Book object.</param>
        /// <returns>Returns a 404 NOT FOUND if bookId is not valid. Returns a 204 NO CONTENT if there are no reviews of the Book. Returns a 200 OK with an AuthorStats object if successful.</returns>
        [ResponseType(typeof(AuthorRatings))]
        [HttpGet]
        [Route("stats/{bookId}")]
        public IHttpActionResult GetBookRating(int bookId)
        {
            var reviewCount = db.ReviewCountRating.Where(rc => rc.BookId == bookId).FirstOrDefault();

            if (reviewCount.CountReviews < 4)
            {
                return BadRequest("Book does not have sufficient reviews to calculate ranking.");
            }

            BooksAuthorsView input = db.BooksAuthorsView.Where(i => i.BookId == bookId).FirstOrDefault();
            if (input == null)
            {
                return NotFound();
            }

            if (!BookExists(bookId))
            {
                return NotFound();
            }
            
            AuthorRatings stats = new AuthorRatings();
            stats.AuthorId = input.AuthorId;
            stats.FirstName = input.FirstName;
            stats.LastName = input.LastName;
            stats.TotalReviews = reviewCount.CountReviews;
            stats.AvgRating = reviewCount.AvgRating;
            
            return Ok(stats);
        }

        //PUT: api/books/edit/12
        /// <summary>
        /// Edit an existing Book object. NOTE: You cannot create a new Book object using this method.
        /// </summary>
        /// <param name="bookId">Id of the Book in the database.</param>
        /// <param name="book">Name of the Book in the database.</param>
        /// <returns>Returns a 400 if the model is invalid. Returns a 400 Bad Request if the id is invalid. Returns a 204 No Content if successful. Returns a 404 Not Found if the id does not already exist in the database.</returns>
        [ResponseType(typeof(Books))]
        [HttpPut]
        [Route("edit/{bookId:int}")]
        public IHttpActionResult PutBooks(int bookId, [FromBody] Books book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookId != book.BookId)
            {
                return BadRequest($"The BookId provided in the body, {book.BookId} does not match the id provided in the URI, {bookId}.");
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(book);
        }//end PutBooks()


        // POST: api/books/create
        /// <summary>
        /// Creates a new Book object in the database.
        /// </summary>
        /// <param name="book">A complete Book object.</param>
        /// <returns>Returns complete Book object created by POST request.</returns>
        [ResponseType(typeof(Books))]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult PostBooks(Books book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return Ok(book);
        }//end PostBooks()


        // DELETE: api/books/delete/5
        /// <summary>
        /// Delete a Book by unique identifier.
        /// </summary>
        /// <param name="id">Unique identifier for the book, as an int.</param>
        [Route("delete/{id:int}")]
        [ResponseType(typeof(Books))]
        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            Books book = db.Books.Find(id);
            if (!BookExists(id))
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookId == id) > 0;
        }//end BookExists()

        private bool AuthorExists(int id)
        {
            return (db.Authors.Count(a => a.AuthorId == id) > 0);
        }//end AuthorExists()

    }//end BooksController

}//end Namespace
