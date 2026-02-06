using InterviewTask.Data;
using InterviewTask.Options;
using InterviewTask.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var envConn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
Console.WriteLine("ENV_CONN=" + (envConn ?? "<null>"));

var cfgConn = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("CFG_CONN=" + (cfgConn ?? "<null>"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
