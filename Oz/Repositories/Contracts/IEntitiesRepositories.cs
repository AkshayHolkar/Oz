using Oz.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Repositories.Contracts
{
    public interface IProductRepository : IAsyncRepository<Product>, IIntIdEntitiesMethods<Product>, IGetAllAsyncWithoutParameter<Product> { }
    public interface ICategoryRepository : IAsyncRepository<Category>, IIntIdEntitiesMethods<Category>, IGetAllAsyncWithoutParameter<Category> { }
    public interface ISizeRepository : IAsyncRepository<Size>, IIntIdEntitiesMethods<Size>, IGetAllAsyncWithoutParameter<Size> { }
    public interface IProductSizeRepository : IAsyncRepository<ProductSize>, IIntIdEntitiesMethods<ProductSize>, IGetAllAsyncWithoutParameter<ProductSize>
    {
        Task<List<ProductSize>> GetAllProductSizesByProductIdAsync(int productId);
    }
    public interface IImageRepository : IAsyncRepository<Image>, IIntIdEntitiesMethods<Image>, IGetAllAsyncWithoutParameter<Image>
    {
        Task<List<Image>> GetAllProductImagesAsync(int productId);
        Task<Image> GetMainImageAsync(int productId);
        bool IsMainImageExist(int id);
    }
    public interface IColourRepository : IAsyncRepository<Colour>, IIntIdEntitiesMethods<Colour>, IGetAllAsyncWithoutParameter<Colour>
    {
        Task<List<Colour>> GetAllProductColoursAsync(int productId);
    }
    public interface IAccountRepository : IAsyncRepository<Account>, IStringIdEntitiesMethods<Account>, IGetAllAsyncWithoutParameter<Account>
    {
        Task<Account> GetIndividualAsync(string userId);
    }
    public interface ICartRepository : IAsyncRepository<Cart>, IIntIdEntitiesMethods<Cart>
    {
        Task<List<Cart>> GetAllAsync(string userId);
    }
    public interface IOrderRepository : IAsyncRepository<Order>, IIntIdEntitiesMethods<Order>, IGetAllAsyncWithoutParameter<Order>
    {
        Task<List<Order>> GetAllByCustomerAsync(string customerId);
        Task<List<Order>> GetAllForCustomerAsync(string userId);
    }
    public interface IOrderDetailRepository : IAsyncRepository<OrderDetail>, IIntIdEntitiesMethods<OrderDetail>
    {
        Task<List<OrderDetail>> GetAllAsync(int id);
    }
    public interface IOrderStatusRepository : IAsyncRepository<OrderStatus>, IIntIdEntitiesMethods<OrderStatus>, IGetAllAsyncWithoutParameter<OrderStatus> { }
}
