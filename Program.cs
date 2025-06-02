using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MoviesAPI.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MoviesDatabaseSettings>(builder.Configuration.GetSection("MoviesDatabaseSettings"));
builder.Services.AddSingleton<MoviesService>();
var app = builder.Build();

IDatabase redis = RedisConnectorHelper.Connection.GetDatabase();

app.MapGet("/", async (MoviesService moviesService) => {
    return Results.Ok();
});

app.MapGet("/api/movies", async (MoviesService moviesService) => {
    string value = redis.StringGet("movies");
    if (string.IsNullOrEmpty(value))
    {
        var movies = await moviesService.Get();

        redis.StringSet("movies", JsonConvert.SerializeObject(movies));

        return movies.Count > 0 ? Results.Ok(movies) : Results.NoContent();
    }

    var cachedMovies = JsonConvert.DeserializeObject<List<Movie>>(value);
    if (cachedMovies != null && cachedMovies.Count > 0)
    {
        return Results.Ok(cachedMovies);
    }

    return Results.NoContent();
});


app.MapPost("/api/movies", async (MoviesService moviesService, Movie movie) =>

{
    await moviesService.Create(movie);
    return Results.Ok();
});

app.Run();
