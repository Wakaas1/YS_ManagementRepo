﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSManagmentSystem.BLL.Categories;
using YSManagmentSystem.DAL.Data;
using YSManagmentSystem.Domain.Order;
using YSManagmentSystem.Domain.Product;
using YSManagmentSystem.web.Models.DataTable;

namespace YSManagmentSystem.BLL.OrderService
{
    public class OrderServices : IOrderServices
    {
        private readonly IDapperRepo _dapper;
        public OrderServices(IDapperRepo dapper)
        {
            _dapper = dapper;
        }

      
        public int AddOrder(tbl_Order model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@OrderNumber", model.OrderNumber);
            param.Add("@CustomerId", model.CustomerId);
            param.Add("@OrderDetail", model.OrderDetail);
            param.Add("@Status", model.Status);
            param.Add("@OrderDate", model.OrderDate);
            param.Add("@CompletedDate", model.CompletedDate);
            param.Add("@TotalCost", model.TotalCost);
            param.Add("@Tax", model.Tax);
            param.Add("@Discount", model.Discount);
            param.Add("@DeliveryCharges", model.DeliveryCharges);
            param.Add("@Total", model.Total);


            var result = _dapper.CreateUserReturnInt("dbo.AddOrder", param);
            if (result > 0)
            { }
            return result;
        }
        public int UpdateOrder(tbl_Order model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", model.Id);
            param.Add("@OrderNumber", model.OrderNumber);
            param.Add("@CustomerId", model.CustomerId);
            param.Add("@OrderDetail", model.OrderDetail);
            param.Add("@Status", model.Status);
            param.Add("@OrderDate", model.OrderDate);
            param.Add("@CompletedDate", model.CompletedDate);
            param.Add("@TotalCost", model.TotalCost);
            param.Add("@Tax", model.Tax);
            param.Add("@Discount", model.Discount);
            param.Add("@DeliveryCharges", model.DeliveryCharges);
            param.Add("@Total", model.Total);


            var result = _dapper.CreateUserReturnInt("dbo.UpdateOrder", param);
            if (result > 0)
            { }
            return result;
        }
        public tbl_Order GetOrderByID(int id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);
            var ord = _dapper.ReturnList<tbl_Order>("dbo.GetOrderByID", param).FirstOrDefault();

            return ord;
        }
        public int DeleteOrder(int id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);

            var ord = _dapper.CreateUserReturnInt("dbo.DeleteOrder", param);

            return ord;
        }

        // DataTable, paging Sorting Searching
        public async Task<DataTableResponse<OrderList>> GetAllOrderDT(DTReq request)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("SearchText", request.SearchText, DbType.String);
            param.Add("SortExpression", request.SortExpression, DbType.String);
            param.Add("StartRowIndex", request.StartRowIndex, DbType.Int32);
            param.Add("PageSize", request.PageSize, DbType.Int32);

            var ord = _dapper.ReturnOrderListMultiple("GetAllOrderDT", param);
            var Response = new DataTableResponse<OrderList>()
            {
                draw = request.draw,
                data = ord.Result.Rec,
                recordsFiltered = ord.Result.TotalRecord,
                recordsTotal = ord.Result.TotalRecord,

            };
            return Response;
        }
    }
}