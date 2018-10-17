using Bookstore.API.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        /// Get a list of all Categories objects.
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
        [Route("details/{categoryId:int}", Name ="GetCategoryById")]
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
        /// <param name="inCategoryId">Unique identifier for the Category.</param>
        /// <returns>Returns a list of Books based on the Category queried. NOTE: You will receive a 404 NOT FOUND response if the Category does not exist.</returns>
        [Route("{inCategoryId:int}/books")]
        [ResponseType(typeof(IEnumerable<Books>))]
        public IHttpActionResult GetBooksByCategory(int inCategoryId)
        {
            Categories cat = db.Categories.Find(inCategoryId);

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

        //GET: api/category/5/top
        /// <summary>
        /// Get a list of the top rated books in a Category by categoryId.
        /// </summary>
        /// <param name="categoryId">Unique identifier for Category object.</param>
        /// <returns>If the categoryId does not exist, returns a 400 BAD REQUEST. If there are no rated books in the Category, returns 404 NOT FOUND. Returns 200 OK, with list of Books objects.</returns>
        [Route("{categoryId:int}/top")]
        [HttpGet]
        public IHttpActionResult TopRatedByCategory(int categoryId)
        {
            //Check if Category exists:
            if (!CategoryExists(categoryId))
            {
                return BadRequest($"No Category with {categoryId} exists in the database.");
            }
            //Placeholder List for return:
            List<Books> bookList = new List<Books>();

            //Get ranked book list for Category from DB:
            IEnumerable<BookCategoryRankings> topBooks = db.BookCategoryRankings.Where(b => b.CategoryId == categoryId).ToList();
            //Loop through the list and add to bookList:
            foreach (var topBook in topBooks)
            {
                Books book = db.Books.Find(topBook.BookId);

                book.CoverImageUrl.TrimEnd();
                book.Description.TrimEnd();
                book.Title.TrimEnd();

                bookList.Add(book);
            }
            if (topBooks.Count() < 0)
            {
                return NotFound();
            }
            
            return Ok(bookList);
        }//end TopRatedByCategory()

        // POST: api/Categories
        /// <summary>
        /// Create a new Category by providing a valid Category object.
        /// </summary>
        /// <param name="category">A valid Category object.</param>
        /// <returns>Returns a 201 CREATED response if successful. Returns a 400 BAD REQUEST response if Category object is not valid. Returns a 400 BAD REQUEST if the Category object already exists.</returns>
        [Authorize]
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Categories category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Categories.Add(category);
            db.SaveChanges();
            
            return Request.CreateResponse(HttpStatusCode.Created);

            //return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // PUT: api/category/edit/5
        /// <summary>
        /// Edit a Category object by unique identifier.
        /// </summary>
        /// <note>You cannot CREATE a new Categories Object with this method.</note>
        /// <param name="categoryId">Unique identifier for the Categories Object.</param>
        /// <param name="category">Fully qualified Categories object.</param>
        /// <returns>Returns 200 OK with updated Categories object in the body. If the categoryId and category.CategoryId do not match, returns a 400 BAD REQUEST. If the Categories Object is not fully qualified, returns a 400 BAD REQUEST. If the categoryId does not exist in the database, returns a 400 BAD REQUEST.</returns>
        [Authorize]
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("edit/{categoryId:int}")]
        public IHttpActionResult PutCategory(int categoryId, [FromBody] Categories category)
        {
            if (!CategoryExists(categoryId))
            {
                return BadRequest($"No record with CategoryId of {categoryId} exists in the database.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoryId != category.CategoryId)
            {
                return BadRequest($"The CategoryId provided in the body, {category.CategoryId} does not match the id provided in the URI, {categoryId}.");
            }

            db.Entry(category).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(category);
        }//end EditCategory()


        // DELETE: api/category/delete/5
        /// <summary>
        /// Delete Category Record by CategoryId.
        /// </summary>
        /// <param name="id">Unique identifier for the Category Object.</param>
        /// <returns>Returns 200 OK and removed Category object if successful. If id does not exist in the database, returns a 404 NOT FOUND.</returns>
        [Authorize]
        [Route("delete/{categoryId:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(int categoryId)
        {
            Categories category = db.Categories.Find(categoryId);
            if (!CategoryExists(categoryId))
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }//end BookExists()
    }
}
