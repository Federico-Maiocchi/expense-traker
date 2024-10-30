using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using expense_traker.Data;
using expense_traker.Models;
using expense_traker.Services;
using expense_traker.Service;

namespace expense_traker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;

        public TransactionsController(ApplicationDbContext context, TransactionService transactionService, CategoryService categoryService)
        {
            _context = context;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            /*var applicationDbContext = _context.Transactions.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());*/
            var transactionsWithCategory = await _transactionService.GetAllTransactionsWithCategoryAsync(); 
            return View(transactionsWithCategory);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);*/


            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetTransactionByIdWithCategoryAsync(id.Value);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public async Task<IActionResult> Create()
        {
            /*ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "TitleWithIcon");            
            return View();*/

            //richiamo le mie categorie dal servizio CategoryService per aggiunger alla ViewData
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "TitleWithIcon");

            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            /*if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);
            return View(transaction);*/

            if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transaction);

                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);

            return View(transaction);

        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);
            return View(transaction);*/

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetTransactionByIdWithCategoryAsync(id.Value);
            if (transaction == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            /*if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);
            return View(transaction);*/

            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _transactionService.UpdateTransactionAsync(transaction);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TransactionExists(transaction.TransactionId))
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

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "TitleWithIcon", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);*/

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetTransactionByIdWithCategoryAsync(id.Value);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/

           await _transactionService.DeleteTransactionAsync(id);

            return RedirectToAction(nameof(Index));
        }

        //Verfica esistenza della transazione
        /*private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }*/

        private async Task<bool> TransactionExists(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            return transaction != null;
        }

        // GET: Transactions - Income
        public async Task<IActionResult> IndexTransactionsIncome()
        {
            /*var transactionsIncome = await _context.Transactions
                .Include(t => t.Category)
                .Where(c => c.Category.Type == "Income" )
                .ToListAsync();
            return View(transactionsIncome);*/

            var transactionsIncome = await _transactionService.GetTransactionsByCategoryTypeAsync("Income");

            return View(transactionsIncome);
        }

        // GET: Transactions - Expense
        public async Task<IActionResult> IndexTransactionsExpense()
        {
            /*var transactionsExpense = await _context.Transactions
                .Include(t => t.Category)
                .Where(c => c.Category.Type == "Expense")
                .ToListAsync();
            return View(transactionsExpense);*/

            var transactionsIncome = await _transactionService.GetTransactionsByCategoryTypeAsync("Expense");

            return View(transactionsIncome);
        }
    }
}
