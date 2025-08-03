using iworfShop_backend_light.Common;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace iworfShop_backend_light.Services;

public interface IProductService
{
    Task<QueryResult> DeactivateProduct(int brandId, List<string> skus);
    Task<QueryResult> ActivateProduct(int brandId, List<string> skus);
    Task<QueryResult> DeleteProduct(int brandId, List<string> skus);
    Task<QueryResult> GetProducts(ProductSearchModel search);
    Task<QueryResult> ImportProducts(ProductImportModel import);

}

public class ProductService : IProductService
{
    private readonly SqlLiteClient _sqlLiteClient;

    public ProductService(SqlLiteClient sqlLiteClient)
    {
        _sqlLiteClient = sqlLiteClient;
    }

    public async Task<QueryResult> DeactivateProduct(int brandId, List<string> skus)
    {
        try
        {
            var products = await _sqlLiteClient.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => x.Brand.Id == brandId && skus.Contains(x.Sku)).ToListAsync();

            foreach (var p in products)
            {
                if (p.IsActive)
                {
                    p.IsActive = false;
                    _sqlLiteClient.Update(p);
                }
            }

            await _sqlLiteClient.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new QueryResult { Code = IworfResultCode.Error, IsSuccess = false, Message = e.Message, Data = null };
        }
        
        return new QueryResult { Code = IworfResultCode.Success, IsSuccess = true, Message = "Successfully update", Data = null };
    }

    public async Task<QueryResult> ActivateProduct(int brandId, List<string> skus)
    {
        try
        {
            var products = await _sqlLiteClient.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => x.Brand.Id == brandId && skus.Contains(x.Sku)).ToListAsync();

            foreach (var p in products)
            {
                if (!p.IsActive)
                {
                    p.IsActive = true;
                    _sqlLiteClient.Update(p);
                }
            }

            await _sqlLiteClient.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new QueryResult { Code = IworfResultCode.Error, IsSuccess = false, Message = e.Message, Data = null };
        }
        
        return new QueryResult
            { Code = IworfResultCode.Success, IsSuccess = true, Message = "Successfully update", Data = null };
    }

    public async Task<QueryResult> DeleteProduct(int brandId, List<string> skus)
    {
        try
        {
            var products = await _sqlLiteClient.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => x.Brand.Id == brandId && skus.Contains(x.Sku)).ToListAsync();
            
            _sqlLiteClient.RemoveRange(products);
            await _sqlLiteClient.SaveChangesAsync();
            
            return new QueryResult
                { Code = IworfResultCode.Success, IsSuccess = true, Message = "Successfully delete", Data = null };
        }
        catch (Exception e)
        {
            return new QueryResult { Code = IworfResultCode.Error, IsSuccess = false, Message = e.Message, Data = null };
        }
    }

    public Task<QueryResult> GetProducts(ProductSearchModel search)
    {
        throw new NotImplementedException();
    }

    public Task<QueryResult> ImportProducts(ProductImportModel import)
    {
        throw new NotImplementedException();
    }
}