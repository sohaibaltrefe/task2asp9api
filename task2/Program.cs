
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using task2.Data;
using task2.Dto;
using task2.Errors;

namespace task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
            builder.Services.AddControllers();
            builder.Services.AddScoped<IValidator<CreateProductDto>,CreateProductDtoValidation>();
            builder.Services.AddScoped<IValidator<EditProductDto>, UpdateProductDtoValidation>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler(opt=> { });

            app.Run();
        }
    }
}
