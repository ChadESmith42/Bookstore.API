﻿using Bookstore.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bookstore.API.Controllers
{
    /// <summary>
    /// Supported queries for the Reviews objects within the AMC Bookstore.
    /// </summary>
    [RoutePrefix("api/reviews")]
    public class ReviewsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //TODO: Trim ReviewText before response!

        // GET: api/Reviews/get
        /// <summary>
        /// GET All Book Reviews from database.
        /// </summary>
        /// <returns>Returns a list of Book Reviews.</returns>
        [Route("get")]
        public IEnumerable<Reviews> GetReviews()
        {
            return db.Reviews;
        }//end GetReviews()

        // GET: api/Reviews/details/5
        /// <summary>
        /// Get a single Review by ReviewId
        /// </summary>
        /// <param name="reviewId">Unique identifier for Review objects.</param>
        /// <returns>Returns a 200 OK response with the requested Review. Returns a 404 NOT FOUND response if the Review does not exist.</returns>
        [Route("details/{reviewId:int}")]
        [ResponseType(typeof(Reviews))]
        public IHttpActionResult GetReviewById(int reviewId)
        {
            if (ReviewExists(reviewId))
            {
                return Ok(db.Reviews.Find(reviewId));
            }

            return NotFound();
        }//end GetReviewById

        /// <summary>
        /// Query Reviews object by Publication Date.
        /// </summary>
        /// <param name="pubDate">Publication date of the Reviews object. Valid date formats are yyyy-mm-dd or yyyy/mm/dd.</param>
        /// <returns>Returns list of Reviews objects published on the query data. Returns 404 NOT FOUND if no Reviews objects were published on that date. Returns 400 BAD REQUEST if the reviewDate value is not properly formatted.</returns>
        [Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<Reviews>))]
        public IHttpActionResult GetReviewsByDate(DateTime pubDate)
        {

            List<Reviews> reviews = db.Reviews.Where(r => DbFunctions.TruncateTime(r.PublishDate) == DbFunctions.TruncateTime(pubDate)).ToList();

            if (reviews.Count == 0)
            {
                return NotFound();
            }

            return Ok(reviews);
        }//end GetReviewsByDate()

        //GET: api/reviews/orphans
        /// <summary>
        /// Get a list of Reviews objects that are not associated with any BooksReviews records.
        /// </summary>
        /// <returns>Returns 200 OK with list of ReviewIds for orphaned records. If all Reviews are linked with BooksReviews, returns 204 NO CONTENT.</returns>
        [Route("orphans")]
        [HttpGet]
        public IHttpActionResult GetOrphanedReviews()
        {
            IEnumerable<int> reviews = db.Reviews.Select(r => r.ReviewId).ToList();
            IEnumerable<int> booksReviews = db.BooksReviews.Select(br => br.ReviewId).ToList();

            reviews = reviews.Except(booksReviews);

            if (reviews.Count() > 0)
            {
                return Ok(reviews.ToList());
            }

            return StatusCode(HttpStatusCode.NoContent);
        }//end OrphanedReviews();


        //// POST: api/Reviews/{review}
        /// <summary>
        /// Create a new Review by providing a valid Review object
        /// </summary>
        /// <param name="review">A complete Review object.</param>
        /// <returns>Returns 400 BAD REQUEST if the Reviews object is not valid. Returns a 201 CREATED with specifics of the object if successful.</returns>
        [Authorize]
        [ResponseType(typeof(Reviews))]
        [Route("create")]
        public IHttpActionResult PostReview([FromBody]Reviews review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return Ok(review);

        }//end PostReview()

        //// PUT: api/Reviews/?reviewId=5
        /// <summary>
        /// Update a Review using ReviewId and a Review object. NOTE: You cannot create a new Review object with this method.
        /// </summary>
        /// <param name="reviewId">Unique identifier of the Review in the database.</param>
        /// <param name="review">A complete Review object.</param>
        /// <returns>Returns 404 NOT FOUND for invalid reviewId parameters. Returns a 400 BAD REQUEST for invalid Review objects. Returns a 202 ACCEPTED response if successfull.</returns>
        [Authorize]
        [Route("edit/{reviewId:int}")]
        public IHttpActionResult PutReview(int reviewId, [FromBody] Reviews review)
        {
            //Check if reviewId already exists: If not, return NotFound()
            if (!ReviewExists(reviewId))
            {
                return NotFound();
            }
            //Check ModelState of author:
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(review).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(review);
        }//end PutReview()

        // DELETE: api/Reviews/remove/5
        /// <summary>
        /// Removes a single Reviews object from the database.
        /// </summary>
        /// <param name="removeId">Unique identifier of the Reviews object to be removed.</param>
        /// <returns>Returns a 404 NOT FOUND response if the removeId parameter does not match an existing Reviewss object. Returns a 200 OK response with the details of the removed Reviews object if successful.</returns>
        [Authorize]
        [Route("delete/{removeId:int}")]
        [ResponseType(typeof(Reviews))]
        public IHttpActionResult DeleteReview(int removeId)
        {
            if (!ReviewExists(removeId))
            {
                return NotFound();
            }

            Reviews removeReview = db.Reviews.Find(removeId);

            db.Reviews.Remove(removeReview);
            db.SaveChanges();

            return Ok(removeReview);

        }//end DeleteReview()

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return (db.Reviews.Count(r => r.ReviewId == id) > 0);
        }
    }
}
