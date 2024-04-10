using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Property_Mangement.Data;
using Property_Mangement.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Property_Mangement.Controllers.Customer
{
    [Area("Customer")]
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchOwnerName, string searchPropertyType, DateTime? searchRegistrationDate)
        {
            ViewBag.CurrentFilterOwnerName = searchOwnerName;
            ViewBag.CurrentFilterPropertyType = searchPropertyType;
            ViewBag.CurrentFilterRegistrationDate = searchRegistrationDate;

            var filteredProperties = await _context.Properties
                .Where(p =>
                    (string.IsNullOrEmpty(searchOwnerName) || p.OwnerName.Contains(searchOwnerName)) &&
                    (string.IsNullOrEmpty(searchPropertyType) || p.PropertyType.Contains(searchPropertyType)) &&
                    (!searchRegistrationDate.HasValue || p.RegistrationDate.Date == searchRegistrationDate.Value.Date)
                )
                .ToListAsync();

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
        public async Task<IActionResult> CreateOrEdit(int id, [Bind("PropertyId,OwnerName,PropertyType,Address,RegistrationDate,Price,ApplicationUserId,ImageUrl")] Property property, IFormFile imageFile)
        {
            if (id != property.PropertyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (property.Price > 1000000000) // Check if price exceeds the limit
                {
                    ModelState.AddModelError("Price", "Price must be under 1,000,000,000.");
                    return View(property);
                }

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
}
