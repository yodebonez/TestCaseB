using Microsoft.EntityFrameworkCore;
using TestCaseB.Models;
using TestCaseB.Services;
using TestCaseB.Services.Interfaces;

namespace TestCaseB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUser,UserService>();
            builder.Services.AddScoped<IShipping, ShippingService>();


            //db
            builder.Services.AddDbContext<DataContext>(options =>
                          options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));


            //Autommaper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}