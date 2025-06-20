using BusinessObject.DTO;
using BusinessObject.Models;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPI : ControllerBase
    {
        private readonly IOrderRepository repository;

        public OrderAPI()
        {
            repository = new OrderRepository();
        }

        // GET: api/OrderAPI
        [HttpGet]
        public ActionResult<IEnumerable<OrderDTO>> GetOrders()
        {
            var orders = repository.GetOrders();

            var result = orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                MemberId = o.MemberId,
                OrderDate = o.OrderDate,
                RequiredDate = o.RequiredDate,
                ShippedDate = o.ShippedDate,
                Freight = o.Freight,
                OrderDetails = o.OrderDetails.Select(d => new OrderDetailDTO
                {
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/OrderAPI/5
        [HttpGet("{id}")]
        public ActionResult<OrderDTO> GetOrderById(int id)
        {
            var o = repository.GetOrderById(id);

            if (o == null)
                return NotFound();

            var order = new OrderDTO
            {
                OrderId = o.OrderId,
                MemberId = o.MemberId,
                OrderDate = o.OrderDate,
                RequiredDate = o.RequiredDate,
                ShippedDate = o.ShippedDate,
                Freight = o.Freight,
                OrderDetails = o.OrderDetails.Select(d => new OrderDetailDTO
                {
                    OrderId = d.OrderId,
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                }).ToList()
            };

            return Ok(order);
        }
    }
}
