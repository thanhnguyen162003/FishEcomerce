// using System.ComponentModel.DataAnnotations;
// using System.Net;
// using Application.Common.Models.ProductModels;
// using Application.Common.Utils;
// using Application.Images.Commands;
// using Application.Products.Commands.CreateTankProduct;
// using Application.Products.Commands.UpdateTankProduct;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Web.Controller;
//
// [ApiController]
// [Route("api/v1/[controller]")]
// public class ProductController : ControllerBase
// {
//     private readonly ISender _sender;
//     private readonly ValidationHelper<TankProductCreateModel> _validationTankCreate;
//     private readonly ValidationHelper<TankProductUpdateModel> _validationTankUpdate;
//
//     public ProductController(ISender sender, ValidationHelper<TankProductCreateModel> validationTankCreate, ValidationHelper<TankProductUpdateModel> validationTankUpdate)
//     {
//         _sender = sender;
//         _validationTankCreate = validationTankCreate;
//         _validationTankUpdate = validationTankUpdate;
//     }
//
//     [HttpPost]
//     public async Task<IResult> CreateTankProduct([FromBody, Required] TankProductCreateModel tankProduct)
//     {
//         var (isValid, response) = await _validationTankCreate.ValidateAsync(tankProduct);
//         if (!isValid)
//         {
//             return Results.BadRequest(response);
//         }
//         
//         var result = await _sender.Send(new CreateTankProductCommand{TankProductCreateModel = tankProduct});
//         return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
//     }
//     
//     [HttpPatch("{productId}")]
//     private async Task<IResult> UpdateTankProduct(ISender sender,[FromBody, Required] TankProductUpdateModel tankProduct, [Required] Guid productId ,ValidationHelper<TankProductUpdateModel> validationHelper)
//     {
//         var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
//         if (!isValid)
//         {
//             return Results.BadRequest(response);
//         }
//         
//         var result = await sender.Send(new UpdateTankProductCommand{ProductId = productId, TankProductUpdateModel = tankProduct});
//         
//         if (result.Status == HttpStatusCode.OK)
//         {
//             if (tankProduct.DeleteImages.Any() || tankProduct.UpdateImages.Any())
//             {
//                 var updateImages = await sender.Send(new UpdateImageCommand
//                 {
//                     ProductId = productId, DeleteImages = tankProduct.DeleteImages,
//                     UpdateImages = tankProduct.UpdateImages
//                 });
//                 
//                 return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
//             }
//         }
//         return Results.BadRequest(result);
//     }
//     
// }