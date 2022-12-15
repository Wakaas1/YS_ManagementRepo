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
    public class LocationServices : ILocationServices
    {
        private readonly IDapperRepo _dapper;
        public LocationServices(IDapperRepo dapper)
        {
            _dapper = dapper;
        }

      
        public int AddLocation(Locations model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@Location", model.Location);

            var result = _dapper.CreateUserReturnInt("dbo.AddLocation", param);
            if (result > 0)
            { }
            return result;
        }
        public int UpdateBrand(Locations model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@Location", model.Location);

            var result = _dapper.CreateUserReturnInt("dbo.UpdateLocation", param);
            if (result > 0)
            { }
            return result;
        }
        public Locations GetBrandByID(int Id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);
            var loc = _dapper.ReturnList<Locations>("dbo.GetLocationByID", param).FirstOrDefault();

            return loc;
        }
        public int DeleteLocation(int id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);

            var loc = _dapper.CreateUserReturnInt("dbo.DeleteLocation", param);

            return loc;
        }
    }
}
