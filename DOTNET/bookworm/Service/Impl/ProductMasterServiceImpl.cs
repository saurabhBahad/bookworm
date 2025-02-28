using Bookworm.Dto;
using Bookworm.Models;
using Bookworm.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Service.Impl
{
    public class ProductMasterServiceImpl : IProductMasterService
    {
        private readonly AppDbContext _dbContext;

        public ProductMasterServiceImpl(AppDbContext ap)
        {
            _dbContext = ap;
        }

        public ProductMaster AddProduct(ProductMaster product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            ProductMaster temp = _dbContext.ProductMasters.Add(product).Entity;
            _dbContext.SaveChanges();
            return product;
        }

        public ProductMaster DeleteProduct(int id)
        {
            if (id == 0)
            {
                throw new InvalidDataException(nameof(id));
            }
            var product = _dbContext.ProductMasters.Find(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            _dbContext.ProductMasters.Remove(product);
            _dbContext.SaveChanges();
            return product;
        }

        public IEnumerable<ProductMaster> GetAll()
        {
            return _dbContext.ProductMasters.ToList();
        }

        public ProductMaster GetProduct(int id)
        {
            return _dbContext.ProductMasters.AsQueryable().FirstOrDefault(x => x.ProductId == id);
        }

        public IEnumerable<ProductMaster> GetProducts(ProductSearch ps)
        {
            var query = _dbContext.ProductMasters.AsQueryable();

            if (ps.LangId > 0)
            {
                query = query.Where(p => p.LanguageId == ps.LangId);
            }

            if (ps.TypeId > 0)
            {
                query = query.Where(p => p.TypeId == ps.TypeId);
            }

            if (ps.GenreId > 0)
            {
                query = query.Where(p => p.GenreId == ps.GenreId);
            }

            IEnumerable<ProductMaster> Products = query.Include(p => p.Author).ToList();

            if (!string.IsNullOrEmpty(ps.ProductName))
            {
                Products = Products.Where(p => findSubString(p.ProductEnglishName?.ToLower(), ps.ProductName?.ToLower()));
            }

            if (!string.IsNullOrEmpty(ps.AuthorName))
            {
                Products = Products.Where(p => findSubString(p.Author.AuthorName?.ToLower(), ps.AuthorName));
            }
            return Products;
        }

        public ProductMaster UpdateProduct(ProductMaster product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            ProductMaster temp = _dbContext.Update(product).Entity;
            _dbContext.SaveChanges();
            return temp;
        }

        private bool findSubString(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return false;
            }
            char[] ch1 = str1.ToCharArray();
            char[] ch2 = str2.ToCharArray();
            for (int i = 0; i <= ch1.Length - ch2.Length; i++)
            {
                bool isSubString = true;
                for (int j = 0; j < ch2.Length; j++)
                {
                    if (ch1[i + j] != ch2[j])
                    {
                        isSubString = false;
                        break;
                    }
                }
                if (isSubString) return true;
            }
            return false;
        }
    }
}