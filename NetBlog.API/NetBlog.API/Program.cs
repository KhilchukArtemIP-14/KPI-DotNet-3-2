using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetBlog.API.Authorization;
using NetBlog.BAL.Services.AuthServices;
using NetBlog.BAL.Services.CommentsService;
using NetBlog.BAL.Services.PostsServices;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.BAL.Services.UserSummaryServices;
using NetBlog.Common.Mappings;
using NetBlog.DAL.Data;
using NetBlog.DAL.Repositories;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NetBlogDbContext>(options =>
{
    options.UseSqlServer(connection, b => b.MigrationsAssembly("NetBlog.API"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "NetBlog Api", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
            });
});
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentsService, CommentService>();
builder.Services.AddScoped<IUserSummaryService, UserSummaryService>();

builder.Services.AddScoped<IAuthorizationRequirement, CanModifyPostRequirement>();
builder.Services.AddScoped<IAuthorizationRequirement, CanDeleteCommentRequirement>();
builder.Services.AddScoped<IAuthorizationRequirement, CanUpdateUserSummaryRequirement>();

builder.Services.AddScoped<IAuthorizationHandler, CanModifyPostHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanDeleteCommentHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CanUpdateUserSummaryHandler>();

builder.Services
    .AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NetBlog")
    .AddEntityFrameworkStores<NetBlogDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        AuthenticationType = "Jwt",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanModifyPostPolicy", policy =>
    {
        policy.Requirements.Add(new CanModifyPostRequirement());
    });
    options.AddPolicy("CanDeleteCommentPolicy", policy =>
    {
        policy.Requirements.Add(new CanDeleteCommentRequirement());
    });
    options.AddPolicy("CanUpdateUserSummary", policy =>
    {
        policy.Requirements.Add(new CanUpdateUserSummaryRequirement());
    });
});

var app = builder.Build();

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
