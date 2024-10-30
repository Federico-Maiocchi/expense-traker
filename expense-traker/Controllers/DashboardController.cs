using expense_traker.Data;
using expense_traker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace expense_traker.Controllers
{
    public class DashboardController : Controller
    {
        //costruttore
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            //Ultimi 7 giorni 
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate =DateTime.Today;
            
            //Lista transizioni
            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category) //includo le categorie
                .Where(y => y.Date >= StartDate && y.Date <= EndDate) // Dove la Data è >= a StartDate e <= EndDate
                .ToListAsync();

            // Debug: Controlla il totale calcolato
            Console.WriteLine($"Totale Income calcolato: {SelectedTransactions}");

            //Totale Income
            decimal TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(t => t.Amount);
            ViewBag.TotalIncome = TotalIncome;

            // Debug: Controlla il totale calcolato
            Console.WriteLine($"Totale Income calcolato: {TotalIncome}");

            //Totale Expense
            decimal TotalExpense = SelectedTransactions
                .Where(e => e.Category.Type == "Expense")
                .Sum(t => t.Amount);
            ViewBag.TotalExpense = TotalExpense;

            // Debug: Controlla il totale calcolato
            Console.WriteLine($"Totale Income calcolato: {TotalExpense}");

            //Totale 
            decimal Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance;

            // Debug: Controlla il totale calcolato
            Console.WriteLine($"Totale Income calcolato: {Balance}");


            //GRAFICI A CINMBELLA
            // Raggruppa raggruppare tutte le categorie con TYPE == "Income"
            var incomeByCategory = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(t => t.Category.TitleWithIcon)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                }).ToList();

            // Prepara i dati per il grafico Income
            ViewBag.IncomeCategoryNames = incomeByCategory.Select(i => i.CategoryName).ToList();
            ViewBag.IncomeTotalAmounts = incomeByCategory.Select(i => i.TotalAmount).ToList();

            // // Raggruppa raggruppare tutte le categorie con TYPE == "Expense"
            var expenseByCategory = SelectedTransactions
                .Where(e => e.Category.Type == "Expense")
                .GroupBy(t => t.Category.TitleWithIcon)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                }).ToList();

            // Prepara i dati per il grafico Expense
            ViewBag.ExpenseCategoryNames = expenseByCategory.Select(e => e.CategoryName).ToList();
            ViewBag.ExpenseTotalAmounts = expenseByCategory.Select(e => e.TotalAmount).ToList();


            // Raggruppamento DI TUTTE LE CATEGORIE
            var allCategories = SelectedTransactions
                .GroupBy(t => t.Category.TitleWithIcon)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                }).ToList();


            // Prepara i dati per il grafico
            ViewBag.AllCategoryNames = allCategories.Select(a => a.CategoryName).ToList();
            ViewBag.AllTotalAmounts = allCategories.Select(a => a.TotalAmount).ToList();

            return View();
        }
    }
}
