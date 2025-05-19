using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Data.LinkContext;
using Services.LinkService;
using Middleware.ExceptionHandler;
using Models.UserModel;

var builder = WebApplication.CreateBuilder(args); // < THIS COMES WITH LOGGING!

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDataProtection();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILinkService, LinkService>();

// scoped lifetime database context for controllers
builder.Services.AddDbContext<LinkContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'LinkContext' not found."))
);

// setup identity
builder.Services.AddIdentityCore<User>(options => 
    options.SignIn.RequireConfirmedAccount = false)   
    .AddEntityFrameworkStores<LinkContext>()
    .AddDefaultTokenProviders();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
