<h1>Bookstore.API</h1>

<p>API for fictional AMC Bookstore. Each database object is self-contained in it's own folder. Functions are grouped
    according to HTTP Call-Types.</p>

<p>The deployed version of this API has most of the POST, PUT, and DELETE functions disabled in the controllers. Until
    I add Authentication, I did not want unscrupulous people to sabotage the DB with garbage or malicious content.</p>

<p>As a general rule, PUT requests are only allowed for existing objects. Attempts to edit/update an object that does
    not exist will return a 400 BAD REQUEST response. Similarly, POST request are only allowed to create new objects.
    Attempts to use POST to edit an object will return a 400 BAD REQUEST response.</p>

<p>Where possible, POST, PUT, and DELETE calls will return the object in the Body of the 200 OK response.</p>

<p>API calls that use a string query to find objects (api/books/title/{title} or api/books/authors/name) treat the
    string query as a wildcard against the search field. For example, searching for "Cha" and "Smi" against the Authors
    objects, will return "Chad Smith" and "Charles Smithers".</p>

<p>String queries are NOT case-sensitive. "Cha," "CHA," and "cha" are treated the same and produce the same results.</p>

<h2>Disclaimer</h2>
<p>This is my first solo attempt at creating an API. I probably did things differently that expected or maybe just
    differently than you would like. If there's something that really flies in the face of convention, please let me
    know. I've attempted to be consistent throughout the API code-base. So if it's wrong, it's consistently wrong (I
    hope). The bright side of my flaws in this API is their consistency, which I think will makes correction and
    refactoring easier.</p>
<p>You can reach me at <a href="mailto:chad@codingbychad.com">chad@codingbychad.com</a>.</p>

<p>Thanks for checking out my API.</p>

<h1>Books</h1>
<p>Books objects represent Books in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
    <ul>
        <li>List of all Books.</li>
        <li>Single Book by Id.</li>
        <li>List of Books matching a Title.</li>
        <li>List of Books in a Category.</li>
        <li>List of Books by an Author, via AuthorId.</li>
        <li>List of Books by Publication Date (yyyy-mm-dd or yyyy/mm/dd).</li>
        <li>Book's average Rating from Reviews, by BookId.</li>
        <li>Edit a single Book, by BookId.</li>
        <li>Create a new Book.</li>
        <li>Delete a Book, by BookId.</li>
    </ul>
</p>

<h1>Reviews</h1>
<p>Reviews objects represent Reviews of books in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
    <ul>
        <li>List of all Reviews.</li>
        <li>Single Review by Id.</li>
        <li>List of Reviews by Publication Date (yyyy-mm-dd or yyyy/mm/dd).</li>
        <li>Edit a single Review, by Id.</li>
        <li>Create a new Review.</li>
        <li>Delete a Review, by Id.</li>
        <li>List of Reviews not associated with a Book. (Orphaned records.)</li>
    </ul>
</p>

<h3>Features In Development:</h3>
<ul>
    <li>List of Reviews by Category, sorted by Highest to Lowest Rating.</li>
    <li>List of Reviews for an Author (using AuthorId) grouped by Book, sorted Highest to Lowest Rating.</li>
</ul>

<h1>Categories</h1>

<p>Categories objects represent Categories of books in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
    <ul>
        <li>List of all Categories.</li>
        <li>Single Category by Id.</li>
        <li>Edit a single Category, by Id.</li>
        <li>Create a new Category.</li>
        <li>Delete a Category, by Id.</li>
        <li>List of Books within a Category (using CategoryId).</li>
        <li>Count of Books within a Category (using CategoryId).</li>
        <li>List of the Top Rated Books (with more than 4 Reviews) in the Category (using CategoryId).</li>
    </ul>
</p>

<h1>Authors</h1>

<p>Authors objects represent Authors of books in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
    <ul>
        <li>List of all Authors.</li>
        <li>Single Author by Id.</li>
        <li>Author's average Rating based on ALL Book Reviews for that Author.</li>
        <li>Edit a single Author, by Id.</li>
        <li>Create a new Author.</li>
        <li>Search for an Author by querying FirstName and LastName (wildcard and NOT case sensitive). Returns a List of matching Authors.</li>
        <li>List of top ranted Authors, based on Reviews (uses ALL books by ALL Authors.</li>
        <li>Delete a Author, by Id.</li>
    </ul>
</p>

<h1>BooksReviews</h1>

<p>BooksReviews objects represent the relationship between Reviews, Books, and Authors in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
    <ul>
        <li>List of all BooksReviews objects.</li>
        <li>Single BooksReviews by Id.</li>
        <li>Edit a single BooksReviews, by Id.</li>
        <li>Create a new BooksReviews.</li>
        <li>Delete a BooksReviews, by Id.</li>
    </ul>
 <p><strong>NOTE:</strong> BooksReviews are validated for POST and PUT calls. There MUST be an existing Authors, Books, and Reviews object prior to using the POST and PUT calls.</p>

<h1>BooksAuthors</h1>

<p>BooksAuthors objects represent the relationship between Authors and Books in this fictional Bookstore.</p>

<p>With this API, you can retrieve the following:
<ul>
<li>List of all BooksAuthors.</li>
<li>Single BooksAuthors by Id.</li>
<li>Edit a single BooksAuthors, by Id.</li>
<li>Create a new BooksAuthors.</li>
<li>Delete a BooksAuthors, by Id.</li>
</ul>
</p>

<p><strong>NOTE:</strong> BooksAuthors objects are validated during POST and PUT calls. There must be an existing Books and Authors object prior to a POST and PUT call.</p>
