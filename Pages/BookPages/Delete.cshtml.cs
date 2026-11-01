using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Chincisan_Laura_Lab2.Models;

namespace Chincisan_Laura_Lab2.Pages.BookPages;

public class DeleteModel : PageModel
{
    private readonly Chincisan_Laura_Lab2Context _context;

    public DeleteModel(Chincisan_Laura_Lab2Context context)
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
    .Include(b => b.Publisher)
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        else
        {
            Book = book;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book.FindAsync(id);
        if (book != null)
        {
            Book = book;
            _context.Book.Remove(Book);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
