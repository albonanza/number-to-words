using Number_to_Words.web.Services;

namespace Number_to_Words.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Currency settings and number to words service
            builder.Services.AddSingleton<ICurrencySettings, DollarCurrencySettings>();

            // Language configurations and converters
            builder.Services.AddScoped<ILanguageConfiguration, EnglishLanguageConfiguration>();
            builder.Services.AddScoped<INumberToWordsConverter, EnglishNumberToWordsConverter>();

            builder.Services.AddTransient<INumberToWordsService, NumberToWordsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
