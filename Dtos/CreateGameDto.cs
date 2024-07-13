namespace GameStore.Api.Dtos;

public record CreateGameDto(string Title, string Genre, decimal Price, DateOnly ReleaseDate);