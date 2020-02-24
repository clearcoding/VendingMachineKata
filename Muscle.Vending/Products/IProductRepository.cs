using System.Collections.Generic;

namespace Muscle.Vending.Products
{
    public interface IProductRepository
    {
        IList<Product> Products { get; set; }
    }
}