using KnowledgeBase.IS.IS4.Data;
using Microsoft.EntityFrameworkCore;
using KnowledgeBase.IS.IS4.Models.Auth;
using Microsoft.AspNetCore.Identity;
using KnowledgeBase.IS.IS4.Models.Options;
using KnowledgeBase.IS.IS4.Services.IServices;
using KnowledgeBase.IS.IS4.Services;

var builder = WebApplication.CreateBuilder(args);

# region Add DbContext
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(defaultConnectionString));
#endregion

#region JWT Tokens
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
#endregion

#region Add Identity to services
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}