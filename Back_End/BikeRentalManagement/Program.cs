using BikeRentalManagement.Repositories;
using BikeRentalManagement.Services;

namespace BikeRentalManagement
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

            // Enable CORS
                builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            // Get connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register Repositories
            builder.Services.AddScoped<UserRepository>(_ => new UserRepository(connectionString));
            builder.Services.AddScoped<MotorbikeRepository>(_ => new MotorbikeRepository(connectionString));
            builder.Services.AddScoped<RentalRepository>(_ => new RentalRepository(connectionString));
            builder.Services.AddScoped<RentalRequestRepository>(_ => new RentalRequestRepository(connectionString));
            builder.Services.AddScoped<OrderHistoryRepository>(_ => new OrderHistoryRepository(connectionString));

            // Register Services
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MotorbikeService>();
            builder.Services.AddScoped<RentalService>();
            builder.Services.AddScoped<RentalRequestService>();
            builder.Services.AddScoped<OrderHistoryService>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure Swagger for production
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bike Rental API V1");
                    c.RoutePrefix = string.Empty; // Swagger at the root URL
                });
            }

            // Use CORS policy
            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
