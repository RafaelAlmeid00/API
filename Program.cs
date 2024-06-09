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
using Stripe;
using StripeWebApiExample.Services;

DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: true));

var builder = WebApplication.CreateBuilder(args);
var _envVariables = DotEnv.Read();

builder.Services.AddDbContext<EasyPassContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(_envVariables["connectionString"])));

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

builder.Services.AddDistributedMemoryCache();
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
builder.Services.AddScoped<IBussinesDTO, Bussines>();
builder.Services.AddScoped<IBussDTO, Buss>();
builder.Services.AddScoped<IBussStopDTO, BusStop>();
builder.Services.AddScoped<ICardDTO, Card>();
builder.Services.AddScoped<IBussinesLoginDTO, BussinesLogin>();
builder.Services.AddScoped<IRepositoryAdmin, AdminRepository>();
builder.Services.AddScoped<IRepositoryUser, UserRepository>();
builder.Services.AddScoped<IRepositoryBussines, BussinesRepository>();
builder.Services.AddScoped<IRepositoryCard, CardRepository>();
builder.Services.AddScoped<IRepositoryListCpf, ListCpfRepository>();
builder.Services.AddScoped<IServiceAdmin, ServiceAdmin>();
builder.Services.AddScoped<IServiceUser, ServiceUser>();
builder.Services.AddScoped<IServiceBussines, ServiceBussines>();
builder.Services.AddScoped<IServiceCard, ServiceCard>();
builder.Services.AddScoped<IServiceListCpf, ServiceListCpf>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<ICrypto, Crypto>();
builder.Services.AddScoped<IListCPFValidator, ListCPFValidator>();
builder.Services.AddScoped<ICpfValidator, CpfValidator>();
builder.Services.AddScoped<ICnpjValidator, CnpjValidator>();

// Stripe injections

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ChargeService>();
builder.Services.AddScoped<IStripeService, StripeService>();


StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeOptions:SecretKey");

builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSession();
app.Use(async (ctx, next) =>
{
    string tokensCSRF = ctx.Request.Cookies["CSRF-TOKEN"];
    if (!string.IsNullOrEmpty(tokensCSRF))
    {
        ctx.Request.Headers["X-CSRF-TOKEN"] = tokensCSRF;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationMiddleware>();

app.MapDefaultControllerRoute();

app.Run();
