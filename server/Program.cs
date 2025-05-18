using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using Data.LinkContext;
using Services.LinkService;
using Middleware.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args); // < THIS COMES WITH LOGGING!

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILinkService, LinkService>();

// scoped lifetime for controllers
builder.Services.AddDbContext<LinkContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'LinkContext' not found."))
);

// manually controlled (factory) lifetime for holding in containers with differing lifetimes
/*
builder.Services.AddDbContextFactory<LinkContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'LinkContext' not found."))
);
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandler>();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    app.UseExceptionHandler("/error"); // this redirects to /error, which I have not implemented yet
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
