using Assignment_12._3._2.Data;
using Assignment_12._3._2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Assignment_12._3._2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddDbContext<PetContext>(options =>

            options.UseSqlite("Data Source=pet.db")
                );

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var app=builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PetContext>();
                context.Database.EnsureCreated();
                if (!context.PetList.Any())
                {
                    context.PetList.AddRange
                    (
                        new Pet { Name = "Mocha", Type="ChiPooMix"},
                        new Pet { Name = "Cookie", Type = "Toy Poodle"}
                    );
                }
                context.SaveChanges();
            }
            return app;
            //return builder.Build();
        }
    }
}
