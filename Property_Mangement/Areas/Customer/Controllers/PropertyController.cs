using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Mangement.Data;
using Property_Mangement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;


[Area("Customer")]
public class PropertyController : Controller
{
    private readonly ApplicationDbContext _context;

    public PropertyController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()
    {
        var properties = await _context.Properties.ToListAsync();
        return View(properties);
    }

   
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties
            .Include(p => p.ApplicationUser)
            .FirstOrDefaultAsync(m => m.PropertyId == id);
        if (property == null)
        {
            return NotFound();
        }

        return View(property);
    }

    // GET: Property/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Property/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PropertyId,OwnerName,PropertyType,Address,RegistrationDate,Price,ImageUrl,ApplicationUserId")] Property property)
    {
        if (ModelState.IsValid)
        {
            _context.Add(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(property);
    }

    // GET: Property/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }
        return View(property);
    }

    // POST: Property/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PropertyId,OwnerName,PropertyType,Address,RegistrationDate,Price,ImageUrl,ApplicationUserId")] Property property)
    {
        if (id != property.PropertyId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(property);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(property.PropertyId))
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
        return View(property);
    }

    // GET: Property/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var property = await _context.Properties
            .FirstOrDefaultAsync(m => m.PropertyId == id);
        if (property == null)
        {
            return NotFound();
        }

        return View(property);
    }

    // POST: Property/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var property = await _context.Properties.FindAsync(id);
        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PropertyExists(int id)
    {
        return _context.Properties.Any(e => e.PropertyId == id);
    }
}
