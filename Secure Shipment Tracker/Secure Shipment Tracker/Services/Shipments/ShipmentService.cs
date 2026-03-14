using Secure_Shipment_Tracker.Common.Enums;
using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Models;
using Secure_Shipment_Tracker.Repositories.Shipments;

namespace Secure_Shipment_Tracker.Services.Shipments
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepo _shipRepo;
        public ShipmentService(IShipmentRepo shipRepo)
        {
            _shipRepo = shipRepo;
        }
        public async Task<ShipmentResponseDto> AddShipment(CreateShipmentDto shipmentDto,int userId)
        {
            var shipment = new Shipment
            {
               
                SenderName = shipmentDto.SenderName,
                ReceiverName = shipmentDto.ReceiverName,
                Origin = shipmentDto.Origin,
                Destination = shipmentDto.Destination,
                Weight= shipmentDto.Weight,
                CreatedAt= DateTime.UtcNow,
                CreatedBy= userId
            };
            await _shipRepo.AddShipment(shipment);
            
            var user = await _shipRepo.GetUserById(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found."); ;
            var response = new ShipmentResponseDto
            {
                Id = shipment.Id,
                ShipmentNumber= $"SHIP-{1000 + shipment.ShipmentNumber}",
                SenderName = shipment.SenderName,
                ReceiverName = shipment.ReceiverName,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                Weight= shipment.Weight,
                Status= shipment.Status.ToString(),
                CreateAt= shipment.CreatedAt,
                CreatedBy=user.UserName
            };
            return response;
        }
        public async Task<IEnumerable<ShipmentResponseDto>> GetAllShipmentsOfAdmin()
        {
            var shipments = await _shipRepo.GetAllShipments() ?? Enumerable.Empty<Shipment>();
            var response = shipments.Select(s => new ShipmentResponseDto
            {
                Id = s.Id,
                ShipmentNumber= $"SHIP-{1000 + s.ShipmentNumber}",
                SenderName= s.SenderName,
                ReceiverName= s.ReceiverName,
                Origin = s.Origin,
                Destination = s.Destination,
                Weight= s.Weight,
                Status= s.Status.ToString(),
                CreateAt= s.CreatedAt,
                CreatedBy= s.User?.UserName
            });
            return response;
        }

        public async Task<IEnumerable<ShipmentResponseDto>> GetShipmentsByUser(int userId)
        {
            var shipments =  await _shipRepo.GetShipmentsByUserId(userId) ?? Enumerable.Empty<Shipment>();

            var response = shipments.Select(s => new ShipmentResponseDto
            {
                Id= s.Id,
                ShipmentNumber= $"SHIP-{1000 + s.ShipmentNumber}",
                SenderName= s.SenderName,
                ReceiverName= s.ReceiverName,
                Origin= s.Origin,
                Destination = s.Destination,
                Weight= s.Weight,
                Status= s.Status.ToString(),
                CreateAt= s.CreatedAt,
                CreatedBy=s.User?.UserName
            });
            return response;
        }

        public async Task<Shipment> UpdateShipmentStatus(UpdateShipmentDto dto, string role, int userId)
        {
            var shipme = await _shipRepo.GetShipmentByShipId(dto.Id);
            if(role=="Staff" && shipme.CreatedBy!= userId) { throw new UnauthorizedAccessException("You can only update your own shipments"); }
           if(role=="Staff" && dto.NewStatus == ShipmentStatus.Delivered)
            {   throw new UnauthorizedAccessException("Staff cannot mark shipment as Delivered.");
            }
            if (shipme.Status == dto.NewStatus) throw new InvalidOperationException("Shipment is already in this status");
            //IsValidTransition(shipme.Status,dto.NewStatus);
            if (!CanTransitionTo(shipme.Status,dto.NewStatus))
            {
                throw new InvalidOperationException(
             $"Cannot transition from {shipme.Status} to {dto.NewStatus}");
            }
            shipme.Status = dto.NewStatus;
            await _shipRepo.SaveChangesAsync();
            return shipme;
        }
        private static bool CanTransitionTo( ShipmentStatus current, ShipmentStatus next)
        {
            return current switch
            {
                ShipmentStatus.Created => next == ShipmentStatus.Dispatched,
                ShipmentStatus.Dispatched => next == ShipmentStatus.InTransit,
                ShipmentStatus.InTransit=> next== ShipmentStatus.Delivered,
                ShipmentStatus.Delivered => false,
                _ => false
            };
        }
        //private void IsValidTransition(ShipmentStatus current, ShipmentStatus next)
        //{
        //    if (current == next) throw new InvalidOperationException("Shipment is already in this status");
        //    else if (current == ShipmentStatus.Created)
        //    {
        //        if (next != ShipmentStatus.Dispatched)
        //        {
        //            throw new InvalidOperationException("Create can only move to Dispatched");
        //        }
        //    }
        //    else if (current == ShipmentStatus.Dispatched)
        //    {
        //        if (next != ShipmentStatus.InTransit)
        //        {
        //            throw new InvalidOperationException("Dispatched can only move to In-Transit");
        //        }
        //    }
        //    else if (current == ShipmentStatus.InTransit)
        //    {
        //        if (next != ShipmentStatus.Delivered)
        //        {
        //            throw new InvalidOperationException("In-Transit can only move to Deliverd");
        //        }
        //    }
        //    else if (current == ShipmentStatus.Delivered)
        //    {
        //        throw new InvalidOperationException("Delivered shipment can't be update");
        //    }      
        //}
    }
}
