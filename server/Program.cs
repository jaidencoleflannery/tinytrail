using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Data.LinkContext;
using Services.LinkService;
using Middleware.ExceptionHandler;
using Models.UserModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

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
builder.Services
  .AddIdentity<User, IdentityRole>(opts =>
  {
    opts.SignIn.RequireConfirmedAccount = false;
  })
  .AddEntityFrameworkStores<LinkContext>()
  .AddDefaultTokenProviders();

// setup authentication for user login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

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
