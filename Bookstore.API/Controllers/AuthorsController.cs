using Bookstore.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bookstore.API.Controllers
{
    /// <summary>
    /// Author Details
    /// </summary>
    [RoutePrefix("api/authors")]
    public class AuthorsController : ApiController
    {
        //TODO: Add Authors Custom Routing!
        //TODO: Trim Author Names before response!

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Author
        /// <summary>
        /// Get a list of all Authors objects in the database.
        /// </summary>
        /// <returns>List of all Authors in the database.</returns>
        [Route("get")]
        public IEnumerable<Authors> Get()
        {
            List<Authors> authors = db.Authors.ToList();

            foreach (Authors author in authors)
            {
                author.FirstName = author.FirstName.Trim();
                author.LastName = author.LastName.Trim();
                author.HeadshotImageUrl = author.HeadshotImageUrl.Trim();
            }

            return db.Authors;
        }//end Get() Authors

        // GET: api/authors/details/10
        /// <summary>
        /// Get the details for a specific Author by unique identifier.
        /// </summary>
        /// <param name="authorId">Unique identifier for the Author record.</param>
        /// <returns>Returns a single Author object. NOTE: If the {id} provided does not exist in the database, returns a 404 Not Found response.</returns>
        [Route("details/{authorId:int}")]
        [ResponseType(typeof(Authors))]
        public IHttpActionResult Get(int authorId)
        {
            if (!AuthorExists(authorId))
            {
                return NotFound();
            }

            Authors author = db.Authors.Find(authorId);

            author.FirstName = author.FirstName.Trim();
            author.LastName = author.LastName.Trim();
            author.HeadshotImageUrl = author.HeadshotImageUrl.Trim();
            
            return Ok(author);
        }//end Get(int id)

        //GET: api/authors/name?fName=cha&lName=smi
        /// <summary>
        /// GET: Find Authors by name search.
        /// </summary>
        /// <param name="fName">String contained in Author First Name.</param>
        /// <param name="lName">String contained in Author Last Name.</param>
        /// <returns>Returns a list of Authors whose names contain the search query. Both names must match. Returns 404 NOT FOUND if no Author names match the query.</returns>
        [Route("name")]
        [HttpGet]
        public IHttpActionResult AuthorByName(string fName, string lName)
        {
            List<Authors> authors = db.Authors.Where(a => a.FirstName.Contains(fName) && a.LastName.Contains(lName)).ToList();

            if (authors.Count > 0)
            {
                return Ok(authors);
            }

            return NotFound();
        }

        public void BooksByName(string fName, string lName)
        {
            IEnumerable<Authors> authors = AuthorByName(fName, lName);

            List<Books> books = new List<Books>();

            foreach (var author in authors)
            {

            }


        }



        // POST: api/Authors
        /// <summary>
        /// Create a new Author by providing a valid Author object
        /// </summary>
        /// <param name="author">A complete Authors object.</param>
        /// <returns>Returns 400 BAD REQUEST if the Authors object is not valid. Returns a 201 CREATED with specifics of the object if successful.</returns>
        [ResponseType(typeof(Authors))]
        [Route("create")]
        public IHttpActionResult PostAuthor([FromBody]Authors author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.AuthorId }, author);

        }

        // PUT: api/Authors/5
        /// <summary>
        /// Update an Author using authorId and an Author object. NOTE: You cannot create a new Author object with this method.
        /// </summary>
        /// <param name="authorId">Unique identifier of the Author in the database.</param>
        /// <param name="author">A complete Author object.</param>
        /// <returns>Returns 404 NOT FOUND for invalid authorId parameters. Returns a 400 BAD REQUEST for invalid Author objects. Returns a 202 ACCEPTED response if successfull.</returns>
        [Route("edit/{authorId:int}")]
        public IHttpActionResult PutAuthor(int authorId, [FromBody]Authors author)
        {
            //Check if authorId already exists: If not, return NotFound()
            if (!AuthorExists(authorId))
            {
                return NotFound();
            }
            //Check ModelState of author:
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(author).State = EntityState.Modified;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.Accepted);
        }

        // DELETE: api/Authors/5
        /// <summary>
        /// Removes a single Authors object from the database.
        /// </summary>
        /// <param name="removeId">Unique identifier of the Author object to be removed.</param>
        /// <returns>Returns a 404 NOT FOUND response if the removeId parameter does not match an existing Authors object. Returns a 200 OK response with the details of the removed author object if successful.</returns>
        [ResponseType(typeof(Authors))]
        [Route("delete/{removeId:int}")]
        public IHttpActionResult DeleteAuthor(int removeId)
        {
            if (!AuthorExists(removeId))
            {
                return NotFound();
            }

            Authors removeAuthor = db.Authors.Find(removeId);

            db.Authors.Remove(removeAuthor);
            db.SaveChanges();

            return Ok(removeAuthor);

        }

        /***********************************************************
         Customized API query: Get Author ratings by ID
        ***********************************************************/

        //GET: api/AuthorsStats?authorId=17
        /// <summary>
        /// Get an Authors Book Review Stats. NOTE: This covers all books by an Author, not just one book.
        /// </summary>
        /// <param name="authorId">Unique identifier for the Author.</param>
        /// <returns>If the Author is not in the database, returns a 404 NOT FOUND. Returns a 200 OK with an AuthorRating object if successful.</returns>
        [ResponseType(typeof(AuthorRatings))]
        [Route("{authorId:int}/Stats/")]
        public IHttpActionResult GetAuthorRating(int authorId)
        {
            if (!AuthorExists(authorId))
            {
                return NotFound();
            }

            AuthorRatings stats = new AuthorRatings();
            stats.AuthorId = authorId;
            stats.FirstName = db.Authors.Find(authorId).FirstName.Trim();
            stats.LastName = db.Authors.Find(authorId).LastName.Trim();

            //Calculate Review Stats for this Author:
            var reviewedBooks = from br in db.BooksReviews
                                         join r in db.Reviews on br.ReviewId equals r.ReviewId
                                         where br.AuthorId == authorId && br.ReviewId == r.ReviewId
                                         select new { r.Rating };
            
            //Calculate Average Rating:
            stats.AvgRating = reviewedBooks.Average(rb => rb.Rating);
            //Get the Total Number of Reviews:
            stats.TotalReviews = reviewedBooks.Count();

            return Ok(stats);
        }//end GetAuthorRating()

        //GET api/AuthorsStats/
        /// <summary>
        /// Get the Authors ranked by their Reviews. (Minimum of 4 reviews are required.)
        /// </summary>
        /// <example>api/AuthorsRankings/</example>
        /// <returns>Returns 200 OK with list of Authors ordered by Ranking in descending order. List is then ordered by Author's Last Name, and First Name alphabetically.</returns>
        [Route("rankings")]
        public IHttpActionResult AuthorRanking()
        {
            List<AuthorRankings> rankedAuthors = db.AuthorRankings.OrderByDescending(a => a.AverageRating).ToList();

            return Ok(rankedAuthors.Take(10));
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Check if Author exists in the DB:
        private bool AuthorExists(int id)
        {
            return (db.Authors.Count(a => a.AuthorId == id) > 0);
        }
    }
}
