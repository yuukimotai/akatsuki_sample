using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mpa.Models;

namespace mpa.Controllers
{
    [Authorize]
    public class MemoController : Controller
    {
        private readonly MemoDbContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        private IHttpContextAccessor _httpContextAccessor;

        public MemoController(MemoDbContext context, UserManager<IdentityUser> usermanager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usermanager = usermanager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Memo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Memo.ToListAsync());
        }

        // GET: Memo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contextUser = _httpContextAccessor.HttpContext.User;
            var userId = _usermanager.GetUserId(contextUser);
            var memo = await _context.Memo
                .FirstOrDefaultAsync(m => m.MemoID == id);
            if (memo == null)
            {
                return NotFound();
            }

            return View(memo);
        }

        // GET: Memo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Memo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemoID,Title,Content")] Memo memo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memo);
        }

        // GET: Memo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memo = await _context.Memo.FindAsync(id);
            if (memo == null)
            {
                return NotFound();
            }
            return View(memo);
        }

        // POST: Memo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemoID,Title,Content")] Memo memo)
        {
            if (id != memo.MemoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemoExists(memo.MemoID))
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
            return View(memo);
        }

        // GET: Memo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memo = await _context.Memo
                .FirstOrDefaultAsync(m => m.MemoID == id);
            if (memo == null)
            {
                return NotFound();
            }

            return View(memo);
        }

        // POST: Memo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memo = await _context.Memo.FindAsync(id);
            if (memo != null)
            {
                _context.Memo.Remove(memo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemoExists(int id)
        {
            return _context.Memo.Any(e => e.MemoID == id);
        }
    }
}
