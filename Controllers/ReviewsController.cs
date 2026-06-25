
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

public class ReviewsController : Controller
{
    private readonly MvcMovieContext _context;

    public ReviewsController(MvcMovieContext context)
    {
        _context = context;
    }

    // GET: REVIEWS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Review.ToListAsync());
    }

    // GET: REVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Review
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // GET: REVIEWS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: REVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,RatingScore,Comment,CreatedAt")] Review review)
    {
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

        var review = await _context.Review.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }
        return View(review);
    }

    // POST: REVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,RatingScore,Comment,CreatedAt")] Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
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

        var review = await _context.Review
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // POST: REVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var review = await _context.Review.FindAsync(id);
        if (review != null)
        {
            _context.Review.Remove(review);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReviewExists(int? id)
    {
        return _context.Review.Any(e => e.Id == id);
    }
}
