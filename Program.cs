using Microsoft.EntityFrameworkCore;
using HatersRating.Data;
using HatersRating.Models;
using FluentValidation;
using System.Security.Cryptography;
using System.Text;
using HatersRating.Helpers;
using HatersRating.Dtos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSwaggerGen(x => x.EnableAnnotations());
builder.Services.AddDbContext<HatersRatingContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();
DbMigrationHelpers.EnsureSeedData(app).Wait();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/current_string_connection", (IConfiguration c) => c.GetConnectionString("DefaultConnection"))
    .WithName("connection_string");

#region Usuario

app.MapGet("/usuario", async (HatersRatingContextDb context) =>
        Results.Ok(await context.Usuario.ToListAsync())
    )
    .WithName("GetAllUsuario")
    .WithTags("Usuario")
    .Produces(StatusCodes.Status200OK, typeof(List<Usuario>));

app.MapGet("/usuario/{id:guid}", async (Guid id, HatersRatingContextDb context) =>
        await context.Usuario.SingleOrDefaultAsync(o => o.Id == id)
            is Usuario usuario
            ? Results.Ok(usuario)
            : Results.NotFound()
    )
    .WithName("GetByIdUsuario")
    .WithTags("Usuario")
    .Produces(StatusCodes.Status200OK, typeof(Usuario))
    .Produces(StatusCodes.Status404NotFound);

app.MapPost("/usuario", async (IValidator<UserDto> validator, HatersRatingContextDb context, UserDto user) =>
        {
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var password = new Password();

            var usuario = new Usuario()
            {
                Email = user.Email,
                Senha = Password.Hash(user.Password),
                Nome = user.Name
            };

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
    .WithTags("Usuario")
    .Produces(StatusCodes.Status201Created, typeof(Usuario))
    .Produces(StatusCodes.Status400BadRequest);

// TODO: Analisar melhor oq alterar do usuario (email, senha) endpoint especificos?
app.MapPut("/usuario/{id:guid}",
        async (IValidator<UserDto> req, Guid id, HatersRatingContextDb context, UserDto user) =>
        {
            var target = await context.Usuario.AsNoTracking<Usuario>().FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) return Results.NotFound();

            var validationResult = await req.ValidateAsync(user);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var password = new Password();
            var confirmedSenha = Password.Verify(user.Password, target.Senha);
            if (!confirmedSenha) return Results.BadRequest();

            var usuario = new Usuario()
            {
                Email = user.Email,
                Senha = target.Senha,
                Nome = user.Name
            };

            context.Usuario.Update(usuario);
            var result = await context.SaveChangesAsync();

            return result > 0
                ? Results.Ok(usuario)
                : Results.BadRequest();
        }
    )
    .WithName("PutUsuario")
    .WithTags("Usuario")
    .Produces(StatusCodes.Status200OK, typeof(Usuario))
    .Produces(StatusCodes.Status400BadRequest);

app.MapDelete("/usuario/{id:guid}", async (Guid id, HatersRatingContextDb context) =>
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
    .WithTags("Usuario")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest);

#endregion

#region Rating

app.MapGet("/rating", async (HatersRatingContextDb context) =>
        await context.Rating.ToListAsync()
    )
    .WithName("GetAllRating")
    .WithTags("Rating");

app.MapGet("/rating/{id:guid}", async (Guid id, HatersRatingContextDb context) =>
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

app.MapPut("/rating/{id:guid}", async (Guid id, HatersRatingContextDb context, Rating rating) =>
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

app.MapDelete("/rating/{id:guid}", async (Guid id, HatersRatingContextDb context) =>
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

#endregion

#region Amizade

app.MapGet("/amizade/list/{id:guid}", async (Guid id, HatersRatingContextDb context) =>
        await context.Amizade.Where(x => x.Activated == true && x.UsuarioId == id).ToListAsync()
    )
    .WithName("GetAllAmizade")
    .WithTags("Amizade");

app.MapPost("/amizade/add", async (HatersRatingContextDb context, AmizadeDto amizade) =>
    {
        if (!await HasUsuario(context, amizade.UsuarioId)) return Results.BadRequest();
        if (!await HasUsuario(context, amizade.AmigoId)) return Results.BadRequest();
        
        var entity = new Amizade()
        {
            UsuarioId = amizade.UsuarioId,
            AmigoId = amizade.AmigoId
        };

        context.Amizade.Add(entity);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.Created($"/amizade/{entity.Id}", entity)
            : Results.BadRequest();
    })
    .WithName("PostAddAmizade")
    .WithTags("Amizade");

app.MapPost("/amizade/remove", async (HatersRatingContextDb context, AmizadeDto amizade) =>
        {
            if (!await HasUsuario(context, amizade.UsuarioId)) return Results.BadRequest();
            if (!await HasUsuario(context, amizade.AmigoId)) return Results.BadRequest();
            
            var target = await context.Amizade.Where(x => x.AmigoId == amizade.AmigoId).FirstOrDefaultAsync();
            if (target == null) return Results.NotFound();

            target.Activated = false;

            context.Amizade.Update(target);
            var result = await context.SaveChangesAsync();

            return result > 0
                ? Results.NoContent()
                : Results.BadRequest();
        }
    )
    .WithName("PostRemoveAmizade")
    .WithTags("Amizade")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest);

#endregion

app.Run();
return;

async Task<bool> HasUsuario(HatersRatingContextDb context,  Guid id) => await context.Usuario.AsNoTracking().AnyAsync(x => x.Id == id);

internal sealed class Password
{
    public static string Hash(string password)
    {
        var md5Hasher = MD5.Create();
        var valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
        var strBuilder = new StringBuilder();
        foreach (var t in valorCriptografado)
        {
            strBuilder.Append(t.ToString("x2"));
        }

        return strBuilder.ToString();
    }

    public static bool Verify(string password, string hashedPassword) => string.Equals(Hash(password), hashedPassword,
        StringComparison.CurrentCultureIgnoreCase);
}