//using library.intrastructure.library.Infrastructure.Notifications;
using libaraey.APPLICATION.library.Application;
using libaraey.APPLICATION.library.Application.services;
using library.DOMAIN.library.domain;
using library.intrastructure.library.Infrastructure.rabbit;
using library.intrastructure.library.Infrastructure.Repos;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_mangment.library.domain.@interface;
using library_mangment.library.Infrastructure.appdbcontext;
using library_mangment.library.Infrastructure.@interface;
using library_mangment.library.Infrastructure.Repos;
using library_task.library.Application.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:58830")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});



builder.Services.AddControllers().AddJsonOptions(o=>o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
);


builder.Services.AddDbContext<Dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<IauthService, authService>();
builder.Services.AddScoped<IBookRepo, BookRepo>();
builder.Services.AddScoped<IBorrowRecordRepo, BorrowRecordRepo>();
builder.Services.AddScoped<IMemberRepo, MemberRepo>();
builder.Services.AddScoped<IrequestRepo, requestRepo>();
builder.Services.AddScoped<IrateRepo, rateRepo>();
builder.Services.AddScoped<IBookServies, BookService>();
builder.Services.AddScoped<IBorrowReturnService, BorrowReturnService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IrequestService, requestService>();
builder.Services.AddScoped<IrateService, rateService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, System.Array.Empty<string>() }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            Console.WriteLine($"JWT Received: {context.Request.Headers["Authorization"]}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var claims = context.Principal?.Claims.Select(c => $"{c.Type}:{c.Value}");
            Console.WriteLine($" Token validated successfully! Claims: {string.Join(", ", claims)}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($" JWT Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };


    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Dbcontext>();

    if (!context.members.Any(m => m.role == "Admin"))
    {
        context.members.Add(new member
        {
            Name = "Admin",
            Email = "admin@library",
            passwordHas = "123", 
            role = "Admin"
        });
        context.SaveChanges();
        Console.WriteLine("Admin user created!");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
//    RequestPath = "/uploads"
//});

app.MapHub<NotificationHub>("/notifications");
app.MapControllers();

app.Run();