using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class OrderService :   IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task Add(NewOrderDTO orderDTO)
        {
            var order = new Order
            {
                ClientId = orderDTO.ClientId,
                Date = orderDTO.Date,
                Status = DataAccess.Model.Enummerations.OrderStatus.active,
                Items = orderDTO.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Amount = item.Amount,
                }).ToList()
            };

            // Add cutting lists to the order
            var cuttingLists = orderDTO.CuttingLists.Select(cl => new CuttingList
            {
                ProductId = cl.ProductId,
                Price = cl.Price,
                cuttingListItems = cl.Items.Select(cli => new CuttingListItem
                {
                    Length = cli.Length,
                    Width = cli.Width,
                    Amount = cli.Amount,
                }).ToList()
            }).ToList();

            order.CuttingLists = cuttingLists;

            await _orderRepository.Add(order);
        }

        public async Task Delete(int id)
        {
            var order = await _orderRepository.GetById(id);
            await _orderRepository.Delete(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetAll()
        {
            var orders = await _orderRepository.GetAll();
            return orders.Select(MapOrderToDTO).ToList();
        }

        public async Task<IEnumerable<OrderDTO>> GetByClientId(int clientId)
        {
            var orders = await _orderRepository.GetByClientId(clientId);
            return orders.Select(MapOrderToDTO).ToList();
        }

        public async Task<OrderDTO> GetById(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
                return null;

            return MapOrderToDTO(order);
        }

        public async Task Update(OrderDTO orderDTO)
        {
            var order = new Order
            {
                Id = orderDTO.Id,
                Date = orderDTO.Date,
                Status = (DataAccess.Model.Enummerations.OrderStatus)orderDTO.Status,
                ClientId = orderDTO.ClientId,
            };
            await _orderRepository.Update(order);
        }

        public ReturnOrderDTO MapToDto(Order order)
        {
            return new ReturnOrderDTO
            {
                Id = order.Id,
                Date = order.Date,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItem
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    ProductId = i.ProductId,
                }).ToList()
            };
        }

        private OrderDTO MapOrderToDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                Date = order.Date,
                Status = (int)order.Status,
                ClientId = order.ClientId,

                Items = order.Items.Select(item => new ItemDto
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Amount,
                    Price = item.Product.Price,
                    ProductImage = item.Product.Image
                }).ToList(),

                CuttingLists = order.CuttingLists.Select(cl => new CuttingListDTO
                {
                    Id = cl.Id,
                    ProductName = cl.Product.Name,
                    Price = cl.Price,
                    Image = cl.Product.Image,
                    CuttingListItems = cl.cuttingListItems.Select(cli => new CuttingListItemDTO
                    {
                        Id = cli.Id,
                        Length = cli.Length,
                        Width = cli.Width,
                        Amount = cli.Amount,
                        CuttingListId = cli.CuttingListId,
                    }).ToList()
                }).ToList(),

                TotalPrice = order.Items.Sum(item => item.Product.Price * item.Amount) +
                             order.CuttingLists.Sum(cl => cl.Price)
            };
        }
    }
}
