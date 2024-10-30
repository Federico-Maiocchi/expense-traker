//using expense_traker.Data;
//using Microsoft.AspNetCore.Mvc;

//namespace expense_traker.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DataController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public DataController(ApplicationDbContext context)
//        {
//            _context = context;
//        }


//        public IActionResult Index()
//        {
//            //tutte le categorie
//            var categories = _context.Categories.ToList();

//            //tutte le transazioni con la categoria associata
//            var transactions = _context.Transactions.ToList();



//            return Ok(new
//            {
//                categorie = categories,
//                transazioni = transactions
//            });
//        }
//    }
//}
