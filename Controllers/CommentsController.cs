﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.CreatedBy).Include(c => c.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> TicketsComments(int Id)
        {
            var comments = await _context.Comments.Where(x => x.TicketId == Id)
                .Include(c => c.CreatedBy)
                .Include(c => c.Ticket).ToListAsync();
            return View(comments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.CreatedBy)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            // Logged in User
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.CreatedOn = DateTime.Now;
            comment.CreatedById = userId;
            _context.Add(comment);
            await _context.SaveChangesAsync();

            //Log the Audit Trail 
            var activity = new AuditTrail
            {
                Action = "Create",
                TimeStamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserId = userId,
                Module = "Comments",
                AffectedTable = "Comments"
            };

            _context.Add(activity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName", comment.CreatedById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Title", comment.TicketId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName", comment.CreatedById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Title", comment.TicketId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FullName", comment.CreatedById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Title", comment.TicketId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.CreatedBy)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
