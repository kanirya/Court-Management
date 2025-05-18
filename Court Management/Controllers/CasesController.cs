using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Court_Management.Data;
using Court_Management.Models;
using Court_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Court_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Court_Management.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CasesController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager=roleManager;
        }

        // GET: Cases
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.Cases.Include(c => c.AssignedTo).Include(c => c.SubmittedBy).Where(c=>c.SubmittedBy.Id==user.Id||c.AssignedTo.Id==user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases
                .Include(a => a.AssignedTo)
                .Include(a => a.SubmittedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // GET: Cases/Create
        public async Task<IActionResult> Create()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Advocate");
            ViewData["AssignedToId"] = new SelectList(usersInRole, "Id", "Name");
            ViewData["SubmittedById"] = new SelectList(_context.Users, "Id", "Id");
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseDTO ca)
        {
          
            if (ModelState.IsValid)
            {
                var user= await _userManager.GetUserAsync(User);
               var  A = new Case
                {
                    Title = ca.Title,
                    Description = ca.Description,
                    AssignedToId = ca.AssignedToId,
                   SubmittedById=user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = CaseStatus.Submitted,
                    };
                _context.Add(A);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Name");
            return View(ca);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Name", @case.AssignedToId);
            ViewData["SubmittedById"] = new SelectList(_context.Users, "Id", "Name", @case.SubmittedById);
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedAt,UpdatedAt,Status,SubmittedById,AssignedToId")] Case @case)
        {
            if (id != @case.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.Id))
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

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases
                .Include(a => a.AssignedTo)
                .Include(a => a.SubmittedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            if (@case != null)
            {
                _context.Cases.Remove(@case);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }
    }
}
