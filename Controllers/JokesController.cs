
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security;

public class JokesController : Controller
{
    private readonly ApplicationDbContext _context;

    public JokesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: JOKES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Joke.ToListAsync());
    }

    // GET: JOKES/SEARCHFORM
    public async Task<IActionResult> ShowSearchForm()
    {
        return View();
    }

    // POST: Jokes/ShowSearchResults
    [HttpPost]
    public async Task<IActionResult> ShowSearchResults(string searchPhrase)
    {
        return View("Index", await _context.Joke
            .Where(j => j.JokeQuestion.Contains(searchPhrase))
            .ToListAsync());
    }

    // GET: JOKES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke
            .FirstOrDefaultAsync(m => m.Id == id);
        if (joke == null)
        {
            return NotFound();
        }

        return View(joke);
    }

    // GET: JOKES/Create

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: JOKES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
    {
        // Receive email/name current user
        joke.AuthorEmail = User.Identity?.Name;
        if (ModelState.IsValid)
        {
            _context.Add(joke);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(joke);
    }

    // GET: JOKES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke.FindAsync(id);
        if (joke == null)
        {
            return NotFound();
        }
        return View(joke);

        // Checks if joke was created by user
        if (joke.AuthorEmail != User.Identity.Name)
        {
            return Forbid(); // Returns 403 Forbidden
        }
    }

    // POST: JOKES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
    {
        if (id != joke.Id)
        {
            return NotFound();
        }

        // Checking the author of the original post
        var existingJoke = await _context.Joke.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
        if (existingJoke.AuthorEmail != User.Identity?.Name)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            try
            {
                joke.AuthorEmail = existingJoke.AuthorEmail;
                _context.Update(joke);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JokeExists(joke.Id))
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
        return View(joke);
    }

    // GET: JOKES/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke
            .FirstOrDefaultAsync(m => m.Id == id);
        if (joke == null)
        {
            return NotFound();
        }

        // Permission check before opening the deletion confirmation page
        if (joke.AuthorEmail != User.Identity?.Name)
        {
            return Forbid();
        }
        return View(joke);
    }

    // POST: JOKES/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var joke = await _context.Joke.FindAsync(id);
        if (joke != null)
        {
            // Permission check before deletion from DB
            if (joke.AuthorEmail != User.Identity?.Name)
            {
                return Forbid();
            }
            _context.Joke.Remove(joke);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JokeExists(int? id)
    {
        return _context.Joke.Any(e => e.Id == id);
    }
}
