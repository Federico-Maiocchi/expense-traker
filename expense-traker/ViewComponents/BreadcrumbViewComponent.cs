using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Collections.Generic;

namespace expense_traker.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public BreadcrumbViewComponent(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
        }

        public IViewComponentResult Invoke()
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            var routeData = _actionContextAccessor.ActionContext.RouteData;

            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();

            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Name = "Home", Url = urlHelper.Action("Index", "Home") }
            };

            // Logica personalizzata per Dashboard
            if (controllerName == "Dashboard") 
            {
                //INDEX
                breadcrumbs.Add(new BreadcrumbItem { Name = "Dashboard", Url = urlHelper.Action("Index", "Dashboard") });
            }
            // Logica personalizzata per Categories
            else if (controllerName == "Categories") 
            {
                //INDEX
                breadcrumbs.Add(new BreadcrumbItem { Name = "Categorie", Url = urlHelper.Action("Index", "Categories") });

                //CREATE
                if (actionName == "Create")
                {
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Crea Nuova Categoria", Url = urlHelper.Action("Create", "Categories") });
                }

                //EDIT
                // Se l'azione è "Details", "Edit", o altre azioni specifiche, aggiungi breadcrumb corrispondenti
                if (actionName == "Edit")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Modifica Categoria",
                        Url = urlHelper.Action("Edit", "Categories", new { id = routeData.Values["id"] })
                    });
                }

                //DETAILS
                if (actionName == "Details")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Dettaglio Categoria",
                        Url = urlHelper.Action("Details", "Categories", new { id = routeData.Values["id"] })
                    });
                }

                //DELETE
                if (actionName == "Delete")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Elimina Categoria",
                        Url = urlHelper.Action("Delete", "Categories", new { id = routeData.Values["id"] })
                    });
                }

                //CATEGORIES EXPENSE
                if (actionName == "IndexExpense")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Categorie Spese",
                        Url = urlHelper.Action("IndexExpense", "Categories")
                    });
                }

                //CATEGORIES INCOME
                if (actionName == "IndexIncome")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Categorie Reddito",
                        Url = urlHelper.Action("IndexIncome", "Categories")
                    });
                }
            }
            // Logica personalizzata per Transactions
            else if (controllerName == "Transactions")
            {
                breadcrumbs.Add(new BreadcrumbItem { Name = "Transazioni", Url = urlHelper.Action("Index", "Transactions") });

                //CREATE
                if (actionName == "Create")
                {
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Crea Nuova Transazione", Url = urlHelper.Action("Create", "Transactions") });
                }

                //EDIT
                if (actionName == "Edit")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Modifica Transazione",
                        Url = urlHelper.Action("Edit", "Transactions", new { id = routeData.Values["id"] })
                    });
                }
                //DETAILS
                if (actionName == "Details")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Dettagli Transazione",
                        Url = urlHelper.Action("Details", "Transactions", new { id = routeData.Values["id"] })
                    });
                }
                //DELETE
                if (actionName == "Delete")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Elimina Transazione",
                        Url = urlHelper.Action("Delete", "Transactions", new { id = routeData.Values["id"] })
                    });
                }
                //TRANSACTIONS EXPENSE
                if (actionName == "IndexTransactionsExpenseails")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Transizioni SPese",
                        Url = urlHelper.Action("IndexTransactionsExpense", "Transactions")
                    });
                }
                //TRANSACTION INCOME
                if (actionName == "IndexTransactionsIncome")
                {
                    breadcrumbs.Add(new BreadcrumbItem
                    {
                        Name = "Transizioni Reddito",
                        Url = urlHelper.Action("IndexTransactionsIncome", "Transactions")
                    });
                }
            }
            // Logica generica per altre azioni (non Index)
            else if (actionName != "Index")
            {
                breadcrumbs.Add(new BreadcrumbItem { Name = actionName, Url = urlHelper.Action(actionName, controllerName) });
            }

            return View(breadcrumbs);
        }
    }

    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

