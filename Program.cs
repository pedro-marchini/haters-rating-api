using Microsoft.EntityFrameworkCore;
using HatersRating.Data;
using HatersRating.Models;
using FluentValidation;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSwaggerGen(x => x.EnableAnnotations());
builder.Services.AddDbContext<HatersRatingContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

#region Usuario
app.MapGet("/usuario", async (HatersRatingContextDb context) =>
    await context.Usuario.Where(o => o.Ativo == true).ToListAsync()
)
.WithName("GetAllUsuario")
.WithTags("Usuario");

app.MapGet("/usuario/{id}", async (Guid id, HatersRatingContextDb context) =>
    await context.Usuario.SingleOrDefaultAsync(o => o.Ativo == true && o.Id == id)
        is Usuario usuario
            ? Results.Ok(usuario)
            : Results.NotFound()
)
.WithName("GetByIdUsuario")
.WithTags("Usuario");

app.MapPost("/usuario", async (IValidator<Usuario> validator, HatersRatingContextDb context, Usuario usuario) =>
{
    var validationResult = await validator.ValidateAsync(usuario);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var password = new Password();
    usuario.Senha = password.Hash(usuario.Senha);

    context.Usuario.Add(usuario);
    var result = await context.SaveChangesAsync();

    // var token = JwtHelper.GenerateToken(
    //     patient.IdPaciente,
    //     patient.Email,
    //     Configurations.AuthenticationConfig.Key,
    //     "Issuer",
    //     "audience",
    //     DateTime.Now.AddHours(2)
    // );

    // return Ok(new AuthResultDto(
    //     patient.Nome,
    //     patient.IdPaciente,
    //     "Bearer",
    //     token,
    //     "not yet implemented",
    //     DateTime.Now.AddHours(2)));

    return result > 0
        ? Results.Created($"/usuario/{usuario.Id}", usuario)
        : Results.BadRequest();
}
)
.WithName("PostUsuario")
.WithTags("Usuario");

// TODO: Analisar melhor oq alterar do usuario (email, senha) endpoint especificos?
app.MapPut("/usuario/{id}", async (IValidator<Usuario> req, Guid id, HatersRatingContextDb context, Usuario usuario) =>
{
    var target = await context.Usuario.AsNoTracking<Usuario>().FirstOrDefaultAsync(t => t.Id == id);
    if (target == null) return Results.NotFound();

    var validationResult = await req.ValidateAsync(usuario);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    // var password = new Password();
    // var confirmedSenha = password.Verify(usuario.Senha, target.Senha);

    context.Usuario.Update(usuario);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.Created($"/usuario/{usuario.Id}", usuario)
        : Results.BadRequest();
}
)
.WithName("PutUsuario")
.WithTags("Usuario");

app.MapDelete("/usuario/{id}", async (Guid id, HatersRatingContextDb context) =>
{
    var target = await context.Usuario.FindAsync(id);
    if (target == null) return Results.NotFound();

    context.Usuario.Remove(target);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest();
}
)
.WithName("DeleteUsuario")
.WithTags("Usuario");
#endregion

#region Rating
app.MapGet("/rating", async (HatersRatingContextDb context) =>
    await context.Rating.ToListAsync()
)
.WithName("GetAllRating")
.WithTags("Rating");

app.MapGet("/rating/{id}", async (Guid id, HatersRatingContextDb context) =>
    await context.Rating.FindAsync(id)
        is Rating rating
            ? Results.Ok(rating)
            : Results.NotFound()
)
.WithName("GetByIdRating")
.WithTags("Rating");

app.MapPost("/rating", async (HatersRatingContextDb context, Rating rating) =>
{
    // Fazer validações do rating
    context.Rating.Add(rating);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.Created($"/rating/{rating.Id}", rating)
        : Results.BadRequest();

})
.WithName("PostRating")
.WithTags("Rating");

app.MapPut("/rating/{id}", async (Guid id, HatersRatingContextDb context, Rating rating) =>
{
    var target = await context.Rating.AsNoTracking<Rating>().FirstOrDefaultAsync(t => t.Id == id);
    if (target == null) return Results.NotFound();

    // Fazer validações do rating
    context.Rating.Update(rating);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest();
}
)
.WithName("PutRating")
.WithTags("Rating");

app.MapDelete("/rating/{id}", async (Guid id, HatersRatingContextDb context) =>
{
    var target = await context.Rating.FindAsync(id);
    if (target == null) return Results.NotFound();

    // Fazer validações do rating
    context.Rating.Remove(target);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest();
}
)
.WithName("DeleteRating")
.WithTags("Rating");

app.Run();
#endregion

internal class Password
{

    public string Hash(string password)
    {
        MD5 md5Hasher = MD5.Create();
        byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < valorCriptografado.Length; i++)
        {
            strBuilder.Append(valorCriptografado[i].ToString("x2"));
        }
        return strBuilder.ToString();
    }
    public bool Verify(string password, string hashedPassword)
    {
        return Hash(password).ToUpper() == hashedPassword.ToUpper();
    }

}
