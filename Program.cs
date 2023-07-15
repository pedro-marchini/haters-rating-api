using Microsoft.EntityFrameworkCore;
using HatersRating.Data;
using HatersRating.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.EnableAnnotations());

builder.Services.AddDbContext<HatersRatingContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/Ratings", async (HatersRatingContextDb context) =>
{
    await context.Ratings.ToListAsync();
}
)
.WithName("GetAllRatings")
.WithTags("Rating");


app.Run();