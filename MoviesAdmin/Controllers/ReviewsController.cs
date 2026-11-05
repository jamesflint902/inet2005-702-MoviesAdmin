
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Data;
using MoviesAdmin.Models;

[Authorize(Roles = "Critic")]
public class ReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    //the logged in user
    private string? _userId;


    //constructor
    public ReviewsController(MoviesAdminContext context)
    {
        _context = context;
      //  _userId = User.GetUserId();
    }

    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        _userId = User.GetUserId();
    }


    // GET: REVIEWS
    public async Task<IActionResult> Index()
    {
        //    //get reviews for the loggerd in user
        var reviews = await _context.Reviews
            .Where(m => m.CreatedBy == _userId)
            .Include(m => m.Movie)
            .ToListAsync();

   
        return View(reviews);
    }

    // GET: REVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        //critics can only view their own reviews
        if (review.CreatedBy != _userId)
        {
            return Forbid();
        }

        return View(review);
    }

    // GET: REVIEWS/Create
    public IActionResult Create(int movieId)
    {

        ViewBag.MovieId = movieId;

        return View();
    }

    // POST: REVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Description,Rating,IsPublished")] Review review) // removed movies!! do the same on genre
    {

        // initilize some fields 
        review.CreatedBy = _userId!;
        review.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

        //One review per Critic and validation error if critic already reviewed movie
        bool alreadyReviewed = await _context.Reviews.AnyAsync(r =>
            r.MovieId == review.MovieId &&
            r.CreatedBy == _userId);

        if (alreadyReviewed)
        {
            ModelState.AddModelError("", "You have already reviewed this movie.");
        }


        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(review);
    }

    // GET: REVIEWS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        //Can only edit Own review
        if (review.CreatedBy != _userId)
        {
            return Forbid();
        }

        //published reviews cannot be modified
        if (review.IsPublished)
        {
            return Forbid();
        }


        return View(review);
    }

    // POST: REVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
        }

        //verify the logged-in critic owns this review before updating
        var existingReview = await _context.Reviews
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == review.Id);

        if (existingReview == null)
        {
            return NotFound();
        }

        // critics can only update reviews they created
        if (existingReview.CreatedBy != _userId)
        {
            return Forbid();
        }

        //published reviews cannot be modified
        if (existingReview.IsPublished)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(review.Id))
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
        return View(review);
    }

    // GET: REVIEWS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        // critics can only delete reviews they created
        if (review.CreatedBy != _userId)
        {
            return Forbid();
        }

        //published reviews cannot be deleted
        if (review.IsPublished)
        {
            return Forbid();
        }

        return View(review);
    }

    // POST: REVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        //verify ownership before deleting the review
        if (review.CreatedBy != _userId)
        {
            return Forbid();
        }


        //published reviews cannot be deleted
        if (review.IsPublished)
        {
            return Forbid();
        }


        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReviewExists(int? id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}
