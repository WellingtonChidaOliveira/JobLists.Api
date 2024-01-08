using Bogus;
using JobLists.Api.Models;
using JobLists.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("JobLists");
});

using var serviceProvider = builder.Services.BuildServiceProvider();
var context = serviceProvider.GetService<AppDbContext>();
AddFaker(context);


void AddFaker(AppDbContext context)
{
    var faker = new Faker();

    var fakeJobs = new Faker<Job>()
        .CustomInstantiator(f => new Job(
            f.Name.JobTitle(),
            f.Address.City(),
            f.Lorem.Sentence(),
            f.Random.Decimal(3000, 10000)
        ));

    var jobs = fakeJobs.Generate(10);

    context.Jobs.AddRange(jobs);
    context.SaveChanges();
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
