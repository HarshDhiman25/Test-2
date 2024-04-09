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


    public async Task<IActionResult> Index(string searchOwnerName)
    {
        
        ViewBag.CurrentFilterOwnerName = searchOwnerName;

        
        var properties = _context.Properties.AsQueryable();

     
        if (!string.IsNullOrEmpty(searchOwnerName))
        {
            properties = properties.Where(p => p.OwnerName.Contains(searchOwnerName));
        }

       
        var filteredProperties = await properties.ToListAsync();

        return View(filteredProperties);
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

   
    public IActionResult CreateOrEdit(int? id)
    {
        if (id == null)
        {
            return View(new Property());
        }
        else
        {
            var property = _context.Properties.Find(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(int id, [Bind("PropertyId,OwnerName,PropertyType,Address,RegistrationDate,Price,ApplicationUserId")] Property property, IFormFile imageFile)
    {
        if (id != property.PropertyId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
               
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                
                property.ImageUrl = "/images/" + fileName;
            }

            if (id == 0) 
            {
                _context.Add(property);
            }
            else 
            {
                _context.Update(property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(property);
    }




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



    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [HttpDelete]
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
