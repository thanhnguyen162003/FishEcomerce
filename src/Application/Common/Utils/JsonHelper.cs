using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Common.Utils;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions _snakeCaseOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };

    public static IResult Json(object data)
    {
        return Results.Json(data, _snakeCaseOptions, statusCode:200);
    }
}