using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSManagmentSystem.DAL.Data;
using YSManagmentSystem.Domain.Product;

namespace YSManagmentSystem.BLL.Categories
{
    public class BrandServices : IBrandServices
    {
        private readonly IDapperRepo _dapper;
        public BrandServices(IDapperRepo dapper)
        {
            _dapper = dapper;
        }

       
        public int AddBrand(Brand model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@BrandName", model.BrandName);
            param.Add("@Description", model.Description);

            var result = _dapper.CreateUserReturnInt("dbo.AddBrand", param);
            if (result > 0)
            { }
            return result;
        }
        public int UpdateBrand(Brand model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@BrandName", model.BrandName);
            param.Add("@Description", model.Description);

            var result = _dapper.CreateUserReturnInt("dbo.UpdateBrand", param);
            if (result > 0)
            { }
            return result;
        }
        public Brand GetBrandByID(int Id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);
            var bnd = _dapper.ReturnList<Brand>("dbo.GetBrandByID", param).FirstOrDefault();

            return bnd;
        }
        public int DeleteBrand(int id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);

            var bnd = _dapper.CreateUserReturnInt("dbo.DeleteBrand", param);

            return bnd;
        }
    }
}
