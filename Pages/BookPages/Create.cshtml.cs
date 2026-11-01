using Chincisan_Laura_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chincisan_Laura_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Chincisan_Laura_Lab2Context _context;

    public CreateModel(Chincisan_Laura_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        LoadDropdowns();
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private void LoadDropdowns()
    {
        // incarcam editurile
        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(),
            "ID",
            "PublisherName"
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
            "FullName"
        );
    }
}