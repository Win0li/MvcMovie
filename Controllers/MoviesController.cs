
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using System.ComponentModel.DataAnnotations;

public class MoviesController : Controller
{
    private readonly MvcMovieContext _context;

    public MoviesController(MvcMovieContext context)
    {
        _context = context;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index(string movieGenre, string searchString)    
    {
        if(_context.Movie == null)
        {
            return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        IQueryable<string> genreQuery = from m in _context.Movie
                                        orderby m.Genre
                                        select m.Genre;

        var movies = from m in _context.Movie.Include(m => m.Director)
                     select m;

        if (!String.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!String.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            Movies = await movies.ToListAsync()
        };

        return View(movieGenreVM);
    
    }

// GET: MOVIES/Details/5
public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }
        // need to load director and reviews for the movie

        // load the director for the movie
        var director = await _context.Director.FirstOrDefaultAsync(d => d.Id == movie.DirectorId);
        
        // need to implement the logic to retrieve reviews for the movie and pass them to the view
        var reviews = await _context.Review.Where(r => r.MovieId == id).ToListAsync();

        return View(movie);
    }

    // GET: MOVIES/Create

    public async Task<IActionResult> Create()
    {
        var directors = await _context.Director
            .OrderBy(d => d.Name)
            .Select(d => new 
            {   d.Id,
                d.Name 
            })
            .ToListAsync();

        MovieFormViewModel movieFormViewModel = new MovieFormViewModel
        {
            DirectorOptions = new SelectList(directors, "Id", "Name")
        };

        return View(movieFormViewModel);
    }
    
    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]

    // remove bind
    public async Task<IActionResult> Create(MovieFormViewModel model) {
        if (ModelState.IsValid)
        {
            var movie = model.ToMovie();
            _context.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
  
        model.DirectorOptions = await GetDirectorOptionsAsync(model.DirectorId);
        return View(model);
    }

    // GET: MOVIES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null) 
        {
            return NotFound(); 
        }
        var model = movie.ToFormViewModel();
        model.DirectorOptions = await GetDirectorOptionsAsync(movie.DirectorId);
        return View(model);


    }

    // POST: MOVIES/Edit/5
    // make sure route id matches form id
    // validate form
    // find existing Movie entity
    // copy ViewModel values onto Movie
    // save
    // redirect to Index
    // if invalid/error, reload dropdown and return form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, MovieFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.DirectorOptions = await GetDirectorOptionsAsync(model.DirectorId);
            return View(model);
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        try
        {
            movie.UpdateFrom(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(model.Id))
            {
                return NotFound();
            }

            throw;
        }
    }

    // GET: MOVIES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
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

    // helper to get director options for the dropdown in the create and edit views
    private async Task<SelectList> GetDirectorOptionsAsync(int? selectedDirectorId = null)
    {
        var directors = await _context.Director
            .OrderBy(d => d.Name)
            .Select(d => new
            {
                d.Id,
                d.Name
            })
            .ToListAsync();

        return new SelectList(directors, "Id", "Name", selectedDirectorId);
    }
}
