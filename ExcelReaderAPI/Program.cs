using ExcelReaderAPI.Services;

namespace ExcelReaderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /*builder.Configuration.AddJsonFile("appsettings.json");*/
            builder.Services.AddSingleton(provider => new DatabaseHelper(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<FileUploadService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
  );


            app.MapControllers();

            app.Run();
        }
    }
}