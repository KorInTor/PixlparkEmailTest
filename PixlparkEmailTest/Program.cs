using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using PixlparkEmailTest.Services;
using System.Globalization;

namespace PixlparkEmailTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
			builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));

			builder.Services.AddSingleton<ICodeSenderService>(sp =>
			{
				var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>();
				return RabbitMqCodeSenderService.CreateAsync(options).GetAwaiter().GetResult();
			});

			builder.Services.AddSingleton<ICodeManager, RegistrationCodeManager>();
			builder.Services.AddSingleton<ICodeGenerator, BasicCodeGenerator>();

			builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Registration}/{action=Index}");

            app.Run();
        }
    }
}
