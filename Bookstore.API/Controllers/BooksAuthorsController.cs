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
    /// Queries supported for the BooksAuthors Table.
    /// </summary>
    /// <remarks>
    /// BooksAuthors Table captures the relationship between Authors (1-to-Multiple objects) and Books (1 object).
    /// </remarks>
    [RoutePrefix("api/booksauthors")]
    public class BooksAuthorsController : ApiController
    {
        //Set the ApplicationDBContext:
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/booksauthors
        /// <summary>
        /// Returns a list of all BooksAuthors records.
        /// </summary>
        /// <returns></returns>
        [Route("get")]
        public IEnumerable<BooksAuthors> Get()
        {
            return db.BooksAuthors;
        }

        // GET: api/booksauthors/5/details
        /// <summary>
        /// Returns details for a specific BooksAuthors object.
        /// </summary>
        /// <param name="bookAuthorsId"></param>
        /// <returns></returns>
        [Route("{bookAuthorsId:int}/details")]
        [ResponseType(typeof(BooksAuthors))]
        public IHttpActionResult Get(int bookAuthorsId)
        {
            BooksAuthors bookAuthor = db.BooksAuthors.Find(bookAuthorsId);

            //Check if record exists, return 404 if false.
            if (bookAuthor == null)
            {
                return NotFound();
            }

            //Return 200 OK with object in body.
            return Ok(bookAuthor);
        }

        // POST: api/BooksAuthors
        /// <summary>
        /// Create a new BooksAuthors object.
        /// </summary>
        /// <remarks>Book object and Author object should already exist before using this API POST call.</remarks>
        /// <param name="booksAuthors">BooksAuthor object contains Id, BookId, and AuthorId. Id can be anything, as the DB will assign the next available value.</param>
        [Route("create")]
        public IHttpActionResult PostBooksAuthors([FromBody] BooksAuthors booksAuthors)
        {
            Authors author = db.Authors.Find(booksAuthors.AuthorId);
            Books book = db.Books.Find(booksAuthors.BookId);

            if (author == null)
            {
                return BadRequest($"The AuthorId of {booksAuthors.AuthorId} could not be found. Please verify your information and resubmit.");
            }

            if (book == null)
            {
                return BadRequest($"The BookId of {booksAuthors.BookId} could not be found. Please verify your information and resubmit.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BooksAuthors.Add(booksAuthors);
            db.SaveChanges();

            return Ok(booksAuthors);

        }//end PostBooksAuthors()

        // PUT: api/booksauthors/edit/5
        /// <summary>
        /// Update an existing BooksAuthors object. NOTE: You CANNOT create a new BooksAuthors object using this method.
        /// </summary>
        /// <param name="booksAuthorsId">Unique identifier for an existing BooksAuthors object.</param>
        /// <param name="booksAuthors">A fully qualified BooksAuthors object.</param>
        /// <returns>Returns 400 BAD REQUEST if a prerequisite object does not exist. Returns a 400 BAD REQUEST if the BooksAuthors object has a bad ModelState. Returns a 400 BAD REQUEST if the booksAuthorsId and BooksAuthors.Id do not match. Returns 200 OK, with booksAuthors object in body, if successful.</returns>
        [Route("edit/{booksAuthorsId:int}")]
        public IHttpActionResult PutBooksAuthors(int booksAuthorsId, [FromBody] BooksAuthors booksAuthors)
        {
            //Check for dependent objects' existence. Creating new BooksAuthors objects using a PUT is not allowed.
            if (!BookExists(booksAuthors.BookId) 
                || !AuthorExists(booksAuthors.AuthorId)
                || !BooksAuthorsExists(booksAuthorsId))
            {
                return BadRequest("One of the following is missing: Book, Author, or BooksAuthors records.");
            }
            //Validate the BooksAuthors object is valid.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Validate the passed booksAuthorsId matches the BooksAuthor object in the body.
            if (booksAuthorsId != booksAuthors.Id)
            {
                return BadRequest($"The BookId provided in the body, {booksAuthors.Id} does not match the id provided in the URI, {booksAuthors}. Please create all necessary objects prior to updating.");
            }

            db.Entry(booksAuthors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksAuthorsExists(booksAuthors.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(booksAuthors);
        }//end PutBooksAuthors()


        // DELETE: api/booksauthors/delete/5
        /// <summary>
        /// Remove a BooksAuthors object from the database.
        /// </summary>
        /// <param name="booksAuthorsId">Unique identifier for the BooksAuthors object.</param>
        /// <returns>Returns 200 OK, with BooksAuthors object in the body, if successful. If the BooksAuthor object does not exist, returns 404 NOT FOUND.</returns>
        [Route("delete/{booksAuthorsId:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteBooksAuthors(int booksAuthorsId)
        {
            BooksAuthors ba = db.BooksAuthors.Find(booksAuthorsId);

            if (BooksAuthorsExists(ba.Id))
            {
                db.BooksAuthors.Remove(ba);
                db.SaveChanges();

                return Ok(ba);
            }

            return NotFound();
        }//end DeleteBooksAuthors()

        //Create some methods to check the existence of dependent objects:

        //Use this to prevent PUT from creating NEW objects and POST from editing existing ones.
        private bool BooksAuthorsExists(int id)
        {
            return (db.BooksAuthors.Count(ba => ba.Id == id) > 0);
        }

        //Books must exist before a relationship can be created.
        private bool BookExists(int id)
        {
            return (db.Books.Count(b => b.BookId == id) > 0);
        }

        //Authors must exist before a relationship can be created.
        private bool AuthorExists(int id)
        {
            return (db.Authors.Count(a => a.AuthorId == id) > 0);
        }



    }//end class



}//end namespace
