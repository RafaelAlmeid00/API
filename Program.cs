using System.Text;
using Api.Adapters_Repository;
using Api.Domain;
using Api.Infrastructure;
using Api.Interface;
using Api.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SnapObjects.Data.AspNetCore;

DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));

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
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
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

builder.Services.AddScoped<IAdminDTO, Admin>();
builder.Services.AddScoped<IUserDTO, User>();
builder.Services.AddScoped<IUserLoginDTO, UserLogin>();
builder.Services.AddScoped<IAdminLoginDTO, AdminLogin>();
builder.Services.AddScoped<IRepositoryAdmin<Admin>, AdminRepository>();
builder.Services.AddScoped<IRepositoryUser<User>, UserRepository>();
builder.Services.AddScoped<IServiceAdmin<Admin>, ServiceAdmin>();
builder.Services.AddScoped<IServiceUser<User>, ServiceUser>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<ICrypto, Crypto>();

builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSession();
app.Use(async (context, next) =>
{
    context.Session.SetString("AuthToken", "seu_token_jwt");
    await next();
});

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
