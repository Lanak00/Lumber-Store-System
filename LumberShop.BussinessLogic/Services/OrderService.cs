using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class OrderService : IOrderService
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
                Id = new int(),
                ClientId = orderDTO.ClientId,
                Date = orderDTO.Date,
                Status = DataAccess.Model.Enummerations.OrderStatus.active,
                //add list of items here by inserting them 
            };
            await _orderRepository.Add(order);
        }

        public async Task Delete(int id)
        {
            var order = await _orderRepository.GetById(id);
            await _orderRepository.Delete(order);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders = await _orderRepository.GetAll();
            return orders;
        }

        public async Task<IEnumerable<Order>> GetByClientId(int clientId)
        {
            return await _orderRepository.GetByClientId(clientId);
        }

        public async Task<Order> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task Update(OrderDTO orderDTO)
        {
            var order = new Order
            {
                Id = orderDTO.Id,
                ClientId = orderDTO.ClientId,
                Date = orderDTO.Date,
                Status = orderDTO.Status
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
    }
}
