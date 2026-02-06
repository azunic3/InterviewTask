using InterviewTask.Data;
using InterviewTask.Options;
using InterviewTask.Services;
using Microsoft.EntityFrameworkCore;

static string ToNpgsql(string url)
{
    var uri = new Uri(url); // postgresql://user:pass@host:port/db

    var userInfo = uri.UserInfo.Split(':', 2);
    var user = Uri.UnescapeDataString(userInfo[0]);
    var pass = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";

    var db = uri.AbsolutePath.TrimStart('/');

    return $"Host={uri.Host};Port={uri.Port};Database={db};Username={user};Password={pass};SSL Mode=Require;Trust Server Certificate=true";
}

var builder = WebApplication.CreateBuilder(args);
var rawUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var conn = !string.IsNullOrWhiteSpace(rawUrl)
    ? ToNpgsql(rawUrl)
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(conn));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        p => p.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.Configure<OpenFDAOptions>(builder.Configuration.GetSection("OpenFda"));
builder.Services.AddScoped<OpenFDAService>();
builder.Services.AddScoped<InventoryService>();

builder.Services.AddScoped<MailgunService>();

builder.Services.AddHttpClient("OpenFda", (sp, client) =>
{
    var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<OpenFDAOptions>>().Value;
    client.BaseAddress = new Uri(opt.BaseUrl.TrimEnd('/'));
});

var app = builder.Build();

//automatska migracija na startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
