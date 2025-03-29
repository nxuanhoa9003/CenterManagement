using Application.Extensions;
using Application.Mapper;
using Domain.Entities;
using Infrastructures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Net.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// cấu hình automapper
builder.Services.AddAutoMapper(typeof(Mapper));

// bind cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));


// Binds `JwtSettings` từ `appsettings.json` vào DI container
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);


// Đăng ký VNPayConfig từ appsettings.json
builder.Services.Configure<VNPaySettings>(builder.Configuration.GetSection("VNPay"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<VNPaySettings>>().Value);



// Binds `SmtpSettings` từ `appsettings.json` vào DI container
var mailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddFluentEmail(mailSettings.Mail, mailSettings.DisplayName)
.AddRazorRenderer()
.AddSmtpSender(new SmtpClient(mailSettings.Host)
{
    Port = mailSettings.Port,
    Credentials = new System.Net.NetworkCredential(mailSettings.Mail,mailSettings.Password),
    EnableSsl = mailSettings.UseSSL,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    UseDefaultCredentials = false
});


// Đăng ký dịch vụ bằng cách gọi extension method
builder.Services.AddApplicationServices();

// add authorizationPolicies
builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicies.AddCustomPolicies(options);
});

// Đăng ký Custom Authorization Handler
builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();



// Cấu hình Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity và password
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// cấu hình JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
