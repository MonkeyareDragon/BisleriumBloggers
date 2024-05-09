using Application.BisleriumBlog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.BisleriumBlog;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.BisleriumBlog.Model;
using Microsoft.Extensions.FileProviders;
using Application.BisleriumBlog.Utils;
using Infrastructure.BisleriumBlog.Utils;

var builder = WebApplication.CreateBuilder(args);
var apiCorsPolicy = "ApiCorsPolicy";

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Authentication
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

//Add Authorization
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>();
builder.Services.AddControllers();

//Add Service and Interface
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReplyService, ReplyService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

//Activate Identity APIs
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>().AddSignInManager()
    .AddRoles<IdentityRole>();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });

// Add SwaggerGen
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bislerium BLoggers API", Version = "v1" });

    // Configure JWT Bearer authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

//CORS Service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:5181")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                          //.WithMethods("OPTIONS", "GET");
                      });
});

var app = builder.Build();

// Seed roles into the database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define roles
    string[] roles = { "Admin", "Blogger" };

    // Seed roles if they don't exist
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

//Map Identity routes
//app.MapIdentityApi<AppUser>();

//CORS Enable For Web
app.UseCors(apiCorsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/image"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
