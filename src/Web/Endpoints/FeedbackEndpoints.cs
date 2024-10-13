using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.FeedbackModels;
using Application.Common.Utils;
using Application.Feedbacks.Commands.CreateFeedback;
using Application.Feedbacks.Commands.DeleteFeedback;
using Application.Feedbacks.Queries;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class FeedbackEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/feedback");
        group.MapPost("", CreateFeedback).WithName(nameof(CreateFeedback));
        group.MapGet("", GetFeedbacks).WithName(nameof(GetFeedbacks));
        group.MapDelete("{feedbackId}", DeleteFeedback).WithName(nameof(DeleteFeedback));
    }

    private async Task<IResult> CreateFeedback(ISender sender, [FromBody, Required] FeedbackCreateModel feedbackModel, ValidationHelper<FeedbackCreateModel> validationHelper )
    {
        var (isValid, response) = await validationHelper.ValidateAsync(feedbackModel);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateFeedbackCommand(){FeedbackCreateModel = feedbackModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    public async Task<IResult> GetFeedbacks(ISender sender, [AsParameters] FeedBackQueryFilter filter, HttpContext httpContext)
    {
        filter.ApplyDefaults();
        var result = await sender.Send(new GetFeedbackWithPaginationQuery(){QueryFilter = filter});
        
        var metadata = new Metadata
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages
        };
        
        httpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        return JsonHelper.Json(result);
    }

    private async Task<IResult> DeleteFeedback(ISender sender, Guid feedbackId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteFeedbackCommand(){feedbackId = feedbackId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.Ok(result);
    }
}