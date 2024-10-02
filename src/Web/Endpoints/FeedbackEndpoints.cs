using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models.FeedbackModels;
using Application.Common.Utils;
using Application.Feedbacks.Commands.CreateFeedback;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class FeedbackEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/feedback");
        group.MapPost("", CreateFeedback).WithName(nameof(CreateFeedback));
    }

    public async Task<IResult> CreateFeedback(ISender sender, [FromBody, Required] FeedbackCreateModel feedbackModel, ValidationHelper<FeedbackCreateModel> validationHelper )
    {
        var (isValid, response) = await validationHelper.ValidateAsync(feedbackModel);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateFeedbackCommand(){FeedbackCreateModel = feedbackModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}