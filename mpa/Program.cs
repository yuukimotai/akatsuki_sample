using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using mpa.Areas.Identity.Data;
using MyApplication.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("mpaIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MemoDbContextConnection' not found.");

builder.Services.AddDbContext<mpaIdentityDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<MemoDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<mpaIdentityDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.Services.AddControllers(config =>
// {
//     var policy = new AuthorizationPolicyBuilder()
//                      .RequireAuthenticatedUser()
//                      .Build();
//     config.Filters.Add(new AuthorizeFilter(policy));
// });
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
