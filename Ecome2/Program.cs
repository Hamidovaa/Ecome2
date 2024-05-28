using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecome2.EXtentions;
using Ecome2.Services;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("AuthMessageSenderOptions"));


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<ProgramUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; //R?q?m t?l?b edin
    options.Password.RequireLowercase = false; //Kiçik h?rf t?l?b edin
    options.Password.RequireUppercase = true; //Böyük h?rf t?l?b edin
    options.Password.RequiredLength = 6; //T?l?b olunan uzunluq...
    options.Password.RequireNonAlphanumeric = false; //@ * ! ve.s kimi simvollar olmalidi
    options.Lockout.MaxFailedAccessAttempts = 5; //5 giri?ten sonra bloklanir 
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(5); //bloklamndiqdan 5deq sonra acilir
    options.Lockout.AllowedForNewUsers = true; //yeni qeydiyyat userdirse passwordu unuda biler.bir nece yazdiqda bloklamaya bilersiz
    options.User.RequireUniqueEmail = true; //unique emaail adresleri olsun (1emaille bir qeydiyyat)
    options.SignIn.RequireConfirmedEmail = false; //qeydiyyat etdikden sonra email ile token gönderecek 
    options.SignIn.RequireConfirmedPhoneNumber = false; //telefon do?rulamas?
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<Ecome2.EXtentions.IEmailService, Ecome2.EXtentions.EmailSender>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun ne kadar süre aktif kalaca??n? belirleyin
    options.Cookie.HttpOnly = true; // XSS korumas? için
    options.Cookie.IsEssential = true; // Oturum çerezlerinin temel oldu?undan emin olun
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Call SeedDataAsync after app is built
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ProgramUser>>();
    await SeedDataAsync(roleManager, userManager);
}

app.UseSession();

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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseStatusCodePagesWithReExecute("/Error/NotFound", "?statusCode={0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


async Task SeedDataAsync(RoleManager<IdentityRole> roleManager, UserManager<ProgramUser> userManager)
{
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    var adminEmail = "hemidoffa55@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var programUser = new ProgramUser
        {
            Email = adminEmail,
            UserName = "hemidoffa",
            Name = "hemidoffa",
        };
        var result = await userManager.CreateAsync(programUser, "Ecome7777");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(programUser, "Admin");
        }
        else
        {
            // Kullan?c? zaten mevcutsa, rolünü kontrol et ve ekle
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}