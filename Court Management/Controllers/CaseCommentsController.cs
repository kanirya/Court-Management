using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Court_Management.Data;
using Court_Management.Models;

namespace Court_Management.Controllers
{
    public class CaseCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaseCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CaseComments
        public async Task<IActionResult> Index(int? Id)
        {
            
            if (Id!=null)
            {
                ViewBag.Id=Id;
                var applicationDbContext = _context.CaseComments.Include(c => c.Author).Include(c => c.Case).Include(c=>c.Case.AssignedTo).Where(c=>c.CaseId==Id);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.CaseComments.Include(c => c.Author).Include(c => c.Case);
                return View(await applicationDbContext.ToListAsync());
            }
               
        }

        // GET: CaseComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComments
                .Include(c => c.Author)
                .Include(c => c.Case)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseComment == null)
            {
                return NotFound();
            }

            return View(caseComment);
        }

        // GET: CaseComments/Create
        public IActionResult Create(int? Id)
        {
            if (Id != null)
            {
                var selectedCase = _context.Cases.FirstOrDefault(c => c.Id == Id);
                if (selectedCase != null)
                {
                    ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Id", selectedCase.Id);
                }
            }
            else
            {
                ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "Id");
            }

            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: CaseComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,PostedAt,CaseId,AuthorId")] CaseComment caseComment)
        {
           
                _context.Add(caseComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
        }

        // GET: CaseComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComments.FindAsync(id);
            if (caseComment == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", caseComment.AuthorId);
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "AssignedToId", caseComment.CaseId);
            return View(caseComment);
        }

        // POST: CaseComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,PostedAt,CaseId,AuthorId")] CaseComment caseComment)
        {
            if (id != caseComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseCommentExists(caseComment.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", caseComment.AuthorId);
            ViewData["CaseId"] = new SelectList(_context.Cases, "Id", "AssignedToId", caseComment.CaseId);
            return View(caseComment);
        }

        // GET: CaseComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseComment = await _context.CaseComments
                .Include(c => c.Author)
                .Include(c => c.Case)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseComment == null)
            {
                return NotFound();
            }

            return View(caseComment);
        }

        // POST: CaseComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseComment = await _context.CaseComments.FindAsync(id);
            if (caseComment != null)
            {
                _context.CaseComments.Remove(caseComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseCommentExists(int id)
        {
            return _context.CaseComments.Any(e => e.Id == id);
        }
    }
}
