using GameStore.Api.Dtos;

namespace GameStore.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        const string getGameEndpointName = "Get Game";

        List<GameDto> gameList = [
            new GameDto(1, "Warframe", "Action", 29.99m, new DateOnly(2013, 3, 25)),
            new GameDto(2, "The Legend of Zelda: Tears of the Kingdom", "Action RPG", 59.99m, new DateOnly(2023, 5, 12)),
            new GameDto(3, "Elden Ring", "Action RPG", 49.99m, new DateOnly(2022, 2, 25)),
            new GameDto(4, "Call of Duty: Black Ops 6", "First-Person Shooter", 59.99m, new DateOnly(2021, 11, 5)),
            new GameDto(5, "Helldivers II", "Top-Down Shooter", 39.99m, new DateOnly(2024, 2, 8))
        ];

        // GET: /
        app.MapGet("/", () => "Welcome to the Game Store API");
        
        // GET: /games
        app.MapGet("games", () => gameList);
        
        // GET: /games/{id}
        app.MapGet("games/{id}", (int id) => gameList.Find(game => game.Id == id))
            .WithName(getGameEndpointName);
        
        // POST: /games
        app.MapPost("games", (CreateGameDto newGame) =>
        {

            GameDto? conflictingGame = gameList.Find(game => game.Title == newGame.Title);

            if (conflictingGame is null)
            {
                GameDto game = new(
                    gameList.Count + 1,
                    newGame.Title,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );
            
                gameList.Add(game);

                return Results.CreatedAtRoute(getGameEndpointName, new {id = game.Id}, game);
            }

            return Results.NotFound();

        });
        
        // PUT: /games
        app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = gameList.FindIndex(game => game.Id == id);

            gameList[index] = new GameDto(
                id,
                updatedGame.Title,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
        });
        
        // DELETE: games/{id}
        app.MapDelete("games/{id}", (int id) =>
        {
            gameList.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        app.Run();
    }
}