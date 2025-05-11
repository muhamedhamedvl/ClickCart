using eCommerce.Data;
using eCommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class OrdersService
    {
        #region Define as Singleton
        private static OrdersService _Instance;

        public static OrdersService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new OrdersService();
                }

                return (_Instance);
            }
        }

        private OrdersService()
        {
        }
        #endregion

        public bool SaveOrder(Order order)
        {
            var context = DataContextHelper.GetNewContext();

            context.Orders.Add(order);

            return context.SaveChanges() > 0;
        }

        public Order GetOrderByID(int ID)
        {
            var context = DataContextHelper.GetNewContext();

            return context.Orders.Include("OrderItems.Product.ProductRecords").FirstOrDefault(x=>x.ID == ID);
        }

        public List<Order> SearchOrders(string userID, int? orderID, int? orderStatus, int? pageNo, int recordSize, out int count)
        {
            var context = DataContextHelper.GetNewContext();

            var orders = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(userID))
            {
                orders = orders.Where(x => x.CustomerID.Equals(userID));
            }

            if (orderID.HasValue && orderID.Value > 0)
            {
                orders = orders.Where(x => x.ID == orderID.Value);
            }

            if (orderStatus.HasValue && orderStatus.Value > 0)
            {
                orders = orders.Where(x => x.OrderHistory.OrderByDescending(y => y.ModifiedOn).FirstOrDefault().OrderStatus == orderStatus);
            }

            count = orders.Count();

            pageNo = pageNo ?? 1;
            var skipCount = (pageNo.Value - 1) * recordSize;

            return orders.OrderByDescending(x => x.PlacedOn).Skip(skipCount).Take(recordSize).ToList();
        }

        public bool AddOrderHistory(OrderHistory orderHistory)
        {
            var context = DataContextHelper.GetNewContext();

            context.OrderHistories.Add(orderHistory);

            return context.SaveChanges() > 0;
        }
    }
}
