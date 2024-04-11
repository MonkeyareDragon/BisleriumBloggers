using Application.BisleriumBlog;
using Domain.BisleriumBlog;
using Infrastructure.BisleriumBlog;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

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

//Add Controller and Interface
builder.Services.AddScoped<IPostService, PostService>();

//Activate Identity APIs
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddApiEndpoints();

var app = builder.Build();

//Map Identity routes
app.MapIdentityApi<AppUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
