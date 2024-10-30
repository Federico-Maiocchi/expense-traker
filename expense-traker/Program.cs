using expense_traker.Data;
using expense_traker.Service;
using expense_traker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace expense_traker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            //Ragistrazione dei servizi
            //Servizio Category
            builder.Services.AddScoped<CategoryService>();
            //Servizion Transaction
            builder.Services.AddScoped<TransactionService>();

            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173") // URL della tua app React
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // Registra IActionContextAccessor
            // IActionContextAccessor viene registrato come un servizio singleton nel contenitore di iniezione delle dipendenze.
            // Significa che un'unica istanza di ActionContextAccessor sarà creata e condivisa tra tutte le richieste.
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Servizi di localizzazione / Add localization services
            // Questa riga aggiunge il supporto per la localizzazione. ResourcesPath è la cartella in cui puoi memorizzare le risorse di traduzione (file .resx)
            // se desideri localizzare i tuoi contenuti.
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Configurazione delle opzioni di localizzazione / Configure globalization options
            // Questo imposta la cultura predefinita del sito su it-IT (Italiano) e definisce le culture supportate.
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("it-IT") // Italian culture
                };

                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("it-IT");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Usa CORS
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            // Middleware di localizzazione / Configure localization middleware
            // Questa parte del codice abilita il middleware di localizzazione, che applica le impostazioni di cultura e interfaccia utente.
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
