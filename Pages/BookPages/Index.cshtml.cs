using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chincisan_Laura_Lab2.Models;

namespace Chincisan_Laura_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Chincisan_Laura_Lab2Context _context;

    public IndexModel(Chincisan_Laura_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;

    public SelectList AuthorsList { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int? SelectedAuthorID { get; set; }

    public async Task OnGetAsync()
    {
        // luam toti autorii din baza de date
        var authors = await _context.Authors
            .OrderBy(a => a.LastName)
            .ToListAsync();

        // pregatim lista pentru dropdown
        AuthorsList = new SelectList(
            authors.Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            }),
            "ID",
            "FullName"
        );

        // luam cartile impreuna cu autorii si editurile
        var books = _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .AsQueryable();

        // filtram cartile dupa autorul selectat
        if (SelectedAuthorID.HasValue)
        {
            books = books.Where(b => b.AuthorsID == SelectedAuthorID.Value);
        }

        Book = await books.ToListAsync();
    }
}