using Chincisan_Laura_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chincisan_Laura_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Chincisan_Laura_Lab2Context _context;

    public EditModel(Chincisan_Laura_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book is null)
        {
            return NotFound();
        }

        Book = book;

        LoadDropdowns();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private void LoadDropdowns()
    {
        // incarcam editurile
        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(),
            "ID",
            "PublisherName",
            Book.PublisherID
        );

        // incarcam autorii cu numele complet
        var authors = _context.Authors
            .OrderBy(a => a.LastName)
            .ToList()
            .Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            });

        ViewData["AuthorsID"] = new SelectList(
            authors,
            "ID",
            "FullName",
            Book.AuthorsID
        );
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}