using expense_traker.Data;
using expense_traker.Models;
using Microsoft.EntityFrameworkCore;

namespace expense_traker.Service
{
    public class CategoryService
    {
        //costruttore
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Tutti i metodi saranno asincroni 

        //tutte le categorie
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var getAllCategories = await _context.Categories.ToListAsync();

            return getAllCategories;
        }

        //recuparare una categoria tramite il suo id
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var getCategoryById = await _context.Categories.FindAsync(id);

            return getCategoryById;
        }

        //aggiungere una nuova categoria
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            
            await _context.SaveChangesAsync();
        }

        //Modifica categoria esistente
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();
        }

        //Elimina categoria
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        //Metodo per ottenere le categorie in base al tipo
        public async Task<List<Category>> GetCategoriesByTypeAsync(string type)
        {
            return await _context.Categories
                .Where(c => c.Type == type)
                .ToListAsync();
        }
    }
}
