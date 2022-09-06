using AutoMapper;
using Back_end;
using Back_end.Filters;
using Back_end.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.Utilidades;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddSingleton(provider => new MapperConfiguration(config => {
        var geomeryFactory = provider.GetRequiredService<GeometryFactory>();
        config.AddProfile(new AutoMapperProfile(geomeryFactory));
    }).CreateMapper()
);

builder.Services.AddTransient<IFileSaver, AzureStorageSaver>();

//To use local storage
//builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"),
    sqlServer => sqlServer.UseNetTopologySuite()
));

builder.Services.AddCors(options =>
{
    var url = Configuration.GetValue<string>("frontend_url");
    
    options.AddDefaultPolicy(opt =>
    {
        opt.WithOrigins(url).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "TotalQuantity" });
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(CustomExceptionFilter));
});
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

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
