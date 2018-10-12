using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bookstore.API.Models;
using System.Web.Http.Description;

namespace Bookstore.API.Controllers
{
    /// <summary>
    /// Supported queries for the Categories of Books within the AMC Bookstore.
    /// </summary>
    [RoutePrefix("api/category")]
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Categories
        /// <summary>
        /// GET: api/Categories
        /// </summary>
        /// <returns>List of all Categories</returns>
        [Route("get")]
        public IEnumerable<Categories> Get()
        {
            return db.Categories;
        }


        // GET: api/Category/details/5
        /// <summary>
        /// Get Category information by unique identifier.
        /// </summary>
        /// <param name="categoryID">Unique identifier for each category.</param>
        /// <returns>Returns a single Category object. NOTE: If the Category does not exist in the database, you will receive a 404 NOT FOUND response.</returns>
        [Route("details/{categoryId:int}")]
        [ResponseType(typeof(Categories))]
        public IHttpActionResult Get(int categoryID)
        {
            Categories category = db.Categories.Find(categoryID);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Select all books within a Category
        /// </summary>
        /// <param name="bookInCategory">Unique identifier for the Category.</param>
        /// <returns>Returns a list of Books based on the Category queried. NOTE: You will receive a 404 NOT FOUND response if the Category does not exist.</returns>
        [Route("{bookInCategory:int}/books")]
        [ResponseType(typeof(IEnumerable<Books>))]
        public IHttpActionResult GetBooksByCategory(int bookInCategory)
        {
            Categories cat = db.Categories.Find(bookInCategory);

            if (cat == null)
            {
                return NotFound();
            }

            IEnumerable<Books> books = db.Books.Where(b => b.CategoryId == cat.CategoryId);
            foreach (Books book in books)
            {
                book.Title = book.Title.Trim();
                book.CoverImageUrl = book.CoverImageUrl.Trim();
                book.Description = book.Description.Trim();
            }
            
            return Ok(books);
        }//end GetBooksByCategory()

        /// <summary>
        /// Get the count of all books within a Category.
        /// </summary>
        /// <param name="countCategory">Unique identifier of the Category.</param>
        /// <returns>Returns an integer that represents the count of all books within the Category.</returns>
        [Route("{countCategory:int}/count/")]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetBookCount(int countCategory)
        {
            Categories cat = db.Categories.Find(countCategory);
            if (cat == null)
            {
                return NotFound();
            }

            int count = db.Books.Where(b => b.CategoryId == cat.CategoryId).Count();

            return Ok(count);
        }//end GetBookCount()

        // POST: api/Categories
        /// <summary>
        /// Create a new Category by providing a valid Category object.
        /// </summary>
        /// <param name="category">A valid Category object.</param>
        /// <returns>Returns a 201 CREATED response if successful. Returns a 400 BAD REQUEST response if Category object is not valid. Returns a 400 BAD REQUEST if the Category object already exists.</returns>
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Categories category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Category object is not valid. Please verify you have included the required properties and retry.");
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]string value)
        {
            //TODO: complete PUT Categories!!
        }

        // DELETE: api/Category/5
        public void Delete(int id)
        {
            //TODO: complete DELETE Categories!
        }
    }
}
