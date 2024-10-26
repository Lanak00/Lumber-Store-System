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
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IProductRepository productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public async Task Add(NewOrderItemDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                Id = new int(),
                Amount = orderItemDTO.Amount,
                ProductId = orderItemDTO.ProductId,
            };
            await _orderItemRepository.Add(orderItem);
        }

        public async Task Delete(int id)
        {
            var orderItem = await _orderItemRepository.GetById(id);
            await _orderItemRepository.Delete(orderItem);
        }

        public async Task Update(OrderItemDTO orderItemDTO)
        {
            var orderItem = new OrderItem
            {
                Id = orderItemDTO.Id,
                Amount = orderItemDTO.Amount,
                OrderId = orderItemDTO.OrderId,
                ProductId = orderItemDTO.ProductId
            };
            await _orderItemRepository.Update(orderItem);
        }

        public async Task<OrderItemDTO> GetById(int id)
        {
            var item = await _orderItemRepository.GetById(id);
            var product = await _productRepository.GetById(item.ProductId);
            var orderItem = new OrderItemDTO
            {
                Id = item.Id,
                Amount = item.Amount,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Price = item.Amount * product.Price,
            };
            return orderItem;
        }

       public async Task<IEnumerable<OrderItemDTO>> GetByOrderId(int orderId)
        {
            var items = await _orderItemRepository.GetByOrderId(orderId);
            var orderItemDTOs = new List<OrderItemDTO>();

            foreach (var item in items)
            {
                var product = await _productRepository.GetById(item.ProductId);

                var orderItemDTO = new OrderItemDTO
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Price = item.Amount * product.Price
                };

                orderItemDTOs.Add(orderItemDTO);
            }
            return orderItemDTOs;
        }
    }
}
