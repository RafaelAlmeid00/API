
using System.Text;
using Api.Infrastructure;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SnapObjects.Data.AspNetCore;

//DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));

var builder = WebApplication.CreateBuilder(args);
var _envVariables = DotEnv.Read();

builder.Services.AddDbContext<EasyPassContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(_envVariables["connectionString"])));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "session";
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _envVariables["issuer"],
        ValidAudience = _envVariables["audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_envVariables["key"]))
    };
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Audience = _envVariables["audience"];
});



builder.Services.AddControllers(m =>
{
    m.UseCoreIntegrated();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddScoped<IAdministradorDTO, Administrador>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<ICrypto, Crypto>();*/

builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<AuthorizationMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
