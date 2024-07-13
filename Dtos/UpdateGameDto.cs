namespace GameStore.Api.Dtos;

public record UpdateGameDto(string Title, string Genre, decimal Price, DateOnly ReleaseDate);