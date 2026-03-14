using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Secure_Shipment_Tracker.Common.Enums;
using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Helpers;
using Secure_Shipment_Tracker.Models;
using Secure_Shipment_Tracker.Services.Shipments;
using System.Data.Common;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Secure_Shipment_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipService;
        private readonly ILogger<ShipmentController> _logger;
        public ShipmentController(IShipmentService shipService, ILogger<ShipmentController> logger)
        {
            _shipService = shipService;
            _logger = logger;
        }
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("createShipment")]
        public async Task<IActionResult> CreateShipment(CreateShipmentDto shipment)
        {
            //if(!ModelState.IsValid)
            //{
            //    _logger.LogWarning("Invalid model state for CreateShipment");
            //    return BadRequest(ModelState); }
            try
            {
               var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
               if (string.IsNullOrWhiteSpace(userClaim))  return Unauthorized("user Id not found in token");
               int userId = int.Parse(userClaim);
              _logger.LogInformation($"User Id {userId}");
               var resp =  await _shipService.AddShipment(shipment, userId);
                _logger.LogInformation("Shipment created successfully by user {UserId}, ShipmentId: {ShipmentId}", userId, resp.Id);
                return ApiResponseHelper.Success(resp,"Shipment created successfully!");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating shipment");
                return ApiResponseHelper.Error("Database error while creating shipment", StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occured during Creating shipment");
                return ApiResponseHelper.Error("Something went wrong while creating shipment", StatusCodes.Status500InternalServerError);
             }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllShipments")]
        public async Task<IActionResult>  GetAllShipmentsOfAdmin()
        {
            try
            {
                _logger.LogInformation("Admin {AdminId} requested all shipments", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var shipments = await _shipService.GetAllShipmentsOfAdmin();
                _logger.LogInformation("{Count} Shipments found ", shipments.Count());
                return ApiResponseHelper.Success(shipments, "All shipments fetched!");
            }
            catch(SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error while fetching all shipments");
                return ApiResponseHelper.Error("Database error while fetching shipments",StatusCodes.Status503ServiceUnavailable);
            }
            catch(DbException dbEx)
            {
                _logger.LogError(dbEx, "DB exception while fetching all shipments");
                return ApiResponseHelper.Error("Error occured while fetching shipmets");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching all shipments");
                return ApiResponseHelper.Error("Something went wrong while fetching shipments", StatusCodes.Status500InternalServerError);
            }
          }
        [Authorize(Roles = "Staff")]
        [HttpGet("GetAllShipmentsByStaff")]
        public async Task<IActionResult> GetShipmentByShipmentNumber()
        {
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userClaim))
                {
                    _logger.LogWarning("GetAllShipmentsByStaff: User Id not found in token");
                    return Unauthorized("User Id not found in token");
                }
                int userId = int.Parse(userClaim);
                _logger.LogInformation("Staff {UserId} requested their shipments", userId);
                var shipments = await _shipService.GetShipmentsByUser(userId);
                _logger.LogInformation("{Count} shipments found for Staff {UserId}", shipments.Count(), userId);
                return ApiResponseHelper.Success(shipments, "All shipments found of staff");
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error while fetching shipments for Staff");
                return ApiResponseHelper.Error("Database error occured");
            }
            catch (DbException dbEx)
            {
                _logger.LogError(dbEx, "Database error while fetching shipments for Staff");
                return ApiResponseHelper.Error("Error occured while fetching shipmets");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Something went wrong while fetching staff shipments");
                return ApiResponseHelper.Error("Something went wrong while fetching", StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles ="Admin,Staff")]
        [HttpPut("UpdateShipmentStatus")]
        public async Task<IActionResult> UpdateShipmentStatus(UpdateShipmentDto status)
        {
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userClaim)) return Unauthorized("User id not found in token");
                int userId = int.Parse(userClaim);
                var roleClaim = User.FindFirst(ClaimTypes.Role);
                if (roleClaim == null)
                {
                    _logger.LogWarning("UpdateShipmentStatus: User role not found");
                    return Unauthorized("User role not found"); 
                }
                var role = roleClaim.Value.ToString();
                var shipment = await _shipService.UpdateShipmentStatus(status,role,userId);
                _logger.LogInformation("Shipment {ShipmentId} status updated successfully to {NewStatus} by user {UserId}", shipment.Id, shipment.Status, userId);
                return ApiResponseHelper.Success(shipment,"Status updated success!");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized status update attempt by user");
                return ApiResponseHelper.Error(ex.Message, StatusCodes.Status403Forbidden);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Shipment not found for update");
                return ApiResponseHelper.Error(ex.Message, StatusCodes.Status404NotFound);
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, "Unexpected error while updating shipment status");
                return ApiResponseHelper.Error("Something went wrong at the time of update "+ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
