using YSManagmentSystem.Domain.Product;

namespace YSManagmentSystem.BLL.Categories
{
    public interface IBrandServices
    {
        int AddBrand(Brand model);
        int DeleteBrand(int id);
        
        Brand GetBrandByID(int Id);
        int UpdateBrand(Brand model);
    }
}