using System;
using System.Collections.Generic;

namespace BusinessObject.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? MemberId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public DateOnly? RequiredDate { get; set; }
        public DateOnly? ShippedDate { get; set; }
        public decimal? Freight { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    }
}
