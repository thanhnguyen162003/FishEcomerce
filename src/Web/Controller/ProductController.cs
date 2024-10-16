using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models.ProductModels;
using Application.Common.Utils;
using Application.Images.Commands;
using Application.Products.Commands.CreateTankProduct;
using Application.Products.Commands.UpdateTankProduct;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;

namespace Web.Controller;

[ApiController]
[Route("api/v1/test")]
public class ProductController : ControllerBase
{
    private readonly PayOS _payOs;

    public ProductController()
    {
        _payOs = new PayOS("e3f616ae-df58-404e-bd39-8f48ad7d414b","c9c26a69-dfc5-474d-b426-ac606dc92e87","afa7ba252d807b06386a5e507c46c6ac7f9382dafb046de2662803d112cfbf9e");
    }

    [HttpGet("success/{orderid}")]
    public async Task<IResult> Confirm(long orderid)
    {
        var result = await _payOs.getPaymentLinkInformation(orderid);
        await _payOs.confirmWebhook("https://localhost:7158/api/v1/payment");
        
        return Results.Ok("success");
    }
    
    [HttpPost("success")]
    public async Task<IResult> Success()
    {
        return Results.Ok("success");
    }
    
    [HttpPost("fail")]
    public async Task<IResult> Failed()
    {
        return Results.Ok("Failed");
    }
}