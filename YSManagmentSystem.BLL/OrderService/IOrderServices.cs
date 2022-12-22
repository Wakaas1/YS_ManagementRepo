﻿using YSManagmentSystem.Domain.Order;
using YSManagmentSystem.web.Models.DataTable;

namespace YSManagmentSystem.BLL.OrderService
{
    public interface IOrderServices
    {
       
        int AddOrder(tbl_Order model);
        int UpdateOrder(tbl_Order model);
        tbl_Order GetOrderByID(int id);
        int DeleteOrder(int id);
        List<OrderItemList> GetOrderByItem(int id);
        int AddOrderItem(OrderItem model);
        int CreateNewOrder(int id);
        Task<DataTableResponse<OrderList>> GetAllOrderDT(DTReq request);
    }
}