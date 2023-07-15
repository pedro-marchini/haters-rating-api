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

app.MapGet("/ratings", async (HatersRatingContextDb context) =>
    await context.Ratings.ToListAsync()
)
.WithName("GetAllRatings")
.WithTags("Rating");

app.MapGet("/ratings/{id}", async (Guid id, HatersRatingContextDb context) =>
    await context.Ratings.FindAsync(id)
        is Rating Rating
            ? Results.Ok(Rating)
            : Results.NotFound()
)
.WithName("GetRatingById")
.WithTags("Rating");

app.MapPost("/ratings", async (HatersRatingContextDb context, Rating rating) =>
{
    // Fazer validações do rating
    context.Ratings.Add(rating);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.Created($"/ratings/{rating.Id}", rating)
        : Results.BadRequest();

})
.WithName("PostRating")
.WithTags("Rating");

app.MapPut("/ratings/{id}", async (Guid id, HatersRatingContextDb context, Rating rating) =>
{
    var target = await context.Ratings.AsNoTracking<Rating>().FirstOrDefaultAsync(t => t.Id == id);
    if (target == null) return Results.NotFound();

    // Fazer validações do rating
    context.Ratings.Update(rating);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest();
}
)
.WithName("PutRating")
.WithTags("Rating");

app.MapDelete("/ratings/{id}", async (Guid id, HatersRatingContextDb context) =>
{
    var target = await context.Ratings.FindAsync(id);
    if (target == null) return Results.NotFound();

    // Fazer validações do rating
    context.Ratings.Remove(target);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest();
}
)
.WithName("DeleteRating")
.WithTags("Rating");

app.Run();