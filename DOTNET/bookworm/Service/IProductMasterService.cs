using Bookworm.Dto;
using Bookworm.Models;

namespace Bookworm.Service
{
    public interface IProductMasterService
    {
        IEnumerable<ProductMaster> GetAll();
        ProductMaster GetProduct(int id);
        ProductMaster AddProduct(ProductMaster product);
        ProductMaster UpdateProduct(ProductMaster product);
        ProductMaster DeleteProduct(int id);
        IEnumerable<ProductMaster> GetProducts(ProductSearch ps);


    }
}
