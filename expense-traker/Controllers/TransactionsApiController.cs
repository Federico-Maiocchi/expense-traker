using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using expense_traker.Data;
using expense_traker.Models;
using expense_traker.Services;

namespace expense_traker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionsApiController : ControllerBase
    {
        //costruttore
        private readonly ApplicationDbContext _context;
        private readonly TransactionService _transactionService;

        public TransactionsApiController(ApplicationDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        // GET: api/Transactions1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            /*List<Transaction> allTransactionWIthCategory = await _context.Transactions
                .Include(x => x.Category)
                .ToListAsync();

            return allTransactionWIthCategory;*/

            //return await _context.Transactions.ToListAsync();

            var allTransactionsWithCategory = await _transactionService.GetAllTransactionsWithCategoryAsync();
            return Ok(allTransactionsWithCategory);
        }

        // GET: api/Transactions1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            /* var transaction = await _context.Transactions
                 .FindAsync(id);

             if (transaction == null)
             {
                 return NotFound();
             }

             return transaction;*/

            var transaction = await _transactionService.GetTransactionByIdWithCategoryAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            /*if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();*/

            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            try
            {
                await _transactionService.UpdateTransactionAsync(transaction);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            /*_context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);*/

            await _transactionService.AddTransactionAsync(transaction);
            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transactions1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            /*var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();*/

            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            await _transactionService.DeleteTransactionAsync(id);

            return NoContent();
        }

        //Metodo di controllo per vedere se esiste la strasazione
        /*private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }*/

        private async Task<bool> TransactionExists(int id)
        {
            return await _transactionService.GetTransactionByIdAsync(id) != null;
        }
    }
}
