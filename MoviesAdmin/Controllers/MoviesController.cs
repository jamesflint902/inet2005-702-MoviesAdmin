using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

namespace MoviesAdmin.Data;

[Authorize]
public class MoviesController : Controller
{
    private readonly MoviesAdminContext _context;
    private readonly AzureStorageService _blobService;

    public MoviesController(MoviesAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index()    // added to order by release date
    {
        var user = User.GetUserId();
        if (user != null)
        {
            Console.WriteLine("Logged in user: " + user);
        }
        
         List<Movie> movies = await _context.Movie
                .OrderBy(m => m.ReleaseDate)
                .Include(m => m.Genre)
                .Include(m => m.AudienceRating)
                .ToListAsync();

        return View(movies);

    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
                .Include(m => m.Genre)
                .Include(m => m.AudienceRating)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {

        var genres = _context.Genres.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        var ratings = _context.AudienceRatings.ToList();
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Rating");

        return View();
    }

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Title,GenreId,AudienceRatingId,Description,Runtime,ReleaseDate")] Movie movie, IFormFile? ImageFile) // collect inputs 
    {
        // if igmage is null!
        if (ImageFile == null)
        {
            //return BadRequest();
            // capture here
            ModelState.AddModelError("ImageFile", "Your image is required!");
        }


        //
        // Removed ImageUrl from validation its set after upload
        // ImageUrl is required in the model but not provided by the user
        ModelState.Remove("ImageUrl");


        if (ModelState.IsValid) // can add break point to check validation
        {

            //
            // we need unique file names so they don't get overwritten. Use a GUID.jpg
            //
            //string filename = ImageFile!.FileName;

            var extension = Path.GetExtension(ImageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";





            // Save file in Azure Storage
            string imageUrl = await _blobService.UploadFileAsync(ImageFile, uniqueFileName); // use the original filename
            // Save image Url in Movie record
            movie.ImageUrl = imageUrl; // // use the original filename


            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //
        // repopulate dropdowns if validation fails
        ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Title", movie.GenreId);
        ViewBag.AudienceRatingId = new SelectList(_context.AudienceRatings, "Id", "Rating", movie.AudienceRatingId);


        return View(movie);
    }


    // GET: MOVIES/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        //add drop down for edit
        var genres = _context.Genres.ToList();
        var ratings = _context.AudienceRatings.ToList();

        ViewBag.GenreId = new SelectList(genres, "Id", "Title", movie.GenreId);
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Rating", movie.AudienceRatingId);

        return View(movie);

    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,GenreId,AudienceRatingId,Description,Runtime,ReleaseDate,ImageUrl")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }


        // repopulate dropdowns if validation fails
        var genres = _context.Genres.ToList();
        var ratings = _context.AudienceRatings.ToList();

        ViewBag.GenreId = new SelectList(genres, "Id", "Title", movie.GenreId);
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Rating", movie.AudienceRatingId);

        return View(movie);
    }

    // GET: MOVIES/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .Include(m => m.AudienceRating)
            .FirstOrDefaultAsync(m => m.Id == id);

        ViewBag.ReviewCount = await _context.Reviews
            .CountAsync(r => r.MovieId == id && r.IsPublished);


        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }


    public IActionResult DebugClaims()
    {
        var claims = string.Join("<br>",
            User.Claims.Select(c => $"{c.Type}: {c.Value}"));

        return Content(claims, "text/html");
    }

    public IActionResult TestRole()
    {
        return Content(User.IsInRole("Admin").ToString());
    }
}