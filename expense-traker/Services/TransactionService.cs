using expense_traker.Data;
using expense_traker.Models;
using Microsoft.EntityFrameworkCore;

namespace expense_traker.Services
{
    public class TransactionService
    {

        //Costruttore
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Metodi Transaction (Asincroni)

        //tutte le transazioni
        public async Task<List<Transaction>> GettAllTransactionsAsync()
        {
            var getAllTransaction = await _context.Transactions.ToListAsync();

            return getAllTransaction;
        }

        //recuperare la transazione tramite ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            var getTransactionById = await _context.Transactions.FindAsync(id);

            return getTransactionById;
        }

        //aggiungere Nuova transazione
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);

            await _context.SaveChangesAsync();
        }

        //Modificare transazione 
        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);

            await _context.SaveChangesAsync();
        }

        //cancellare transazione
        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await GetTransactionByIdAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);

                await _context.SaveChangesAsync();
            }
        }

        //tuttle le transazioni con assegnazioni alle categorie
        public async Task<List<Transaction>> GetAllTransactionsWithCategoryAsync()
        {
            var getAllTransactionWithCategory = await _context.Transactions
                .Include(t => t.Category)
                .ToListAsync();

            return getAllTransactionWithCategory;       
        }

        //trovare la transazione compresa con la categoria di appartenenza
        public async Task<Transaction?> GetTransactionByIdWithCategoryAsync(int id)
        {
            var getTransactionById = await _context.Transactions
                .Include(t => t.Category) // Include la categoria associata
                .FirstOrDefaultAsync(t => t.TransactionId == id); // Usa FirstOrDefaultAsync

            return getTransactionById;
        }

        //ottenere Transazioni in base al tipo di categoria di appartenenza
        public async Task<List<Transaction>> GetTransactionsByCategoryTypeAsync(string type)
        {
            var GetTransactionsByCategoryType = await _context.Transactions
                .Include(c => c.Category)
                .Where(t => t.Category.Type == type)
                .ToListAsync();

            return GetTransactionsByCategoryType;
        }
        
    }
}
