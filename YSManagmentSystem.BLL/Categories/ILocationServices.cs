using YSManagmentSystem.Domain.Product;

namespace YSManagmentSystem.BLL.Categories
{
    public interface ILocationServices
    {
        int AddLocation(Locations model);
        int DeleteLocation(int id);
        
        Locations GetBrandByID(int Id);
        int UpdateBrand(Locations model);
    }
}