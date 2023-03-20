using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions.Data;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Common.Exceptions;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class ShoppingCartsRepository : IShoppingCartsRepository
{
    private readonly WebApplicationDbContext _dbContext;

    public ShoppingCartsRepository(
        WebApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Guid Create(ShoppingCart entity)
    {
        var customer = _dbContext.Customers
            .FirstOrDefault(x => x.Id == entity.CustomerId)
            .ThrowIfNotFound(entity.CustomerId);

        entity.CustomerId = customer.Id;
        entity.Customer = customer;

        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        return entity.Id;
    }

    public ShoppingCart Read(Guid id)
    {
        var shoppingCart = _dbContext.ShoppingCarts
            .FirstOrDefault(x => x.Id == id)
            .ThrowIfNotFound(id);

        return shoppingCart;
    }

    public ShoppingCart ReadByCustomer(Guid customerId)
    {
        var shoppingCart = _dbContext.ShoppingCarts
            .FirstOrDefault(x => x.CustomerId == customerId)
            .ThrowIfNotFound(customerId);

        return shoppingCart;
    }

    public void Update(Guid entityId, ShoppingCart newEntityState)
    {
        RemoveEmptyCartItems(newEntityState);

        var isTransactionCommitted = false;
        _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var shoppingCart = _dbContext.ShoppingCarts
                .FirstOrDefault(x => x.Id == entityId)
                .ThrowIfNotFound(entityId);

            var saleItemIds = newEntityState.CartItems
                .Select(x => x.SaleItemId)
                .ToArray();
            var saleItems = GetSaleItems(saleItemIds);

            shoppingCart.CartItems = new List<ShoppingCartItem>();

            foreach (var cartItem in newEntityState.CartItems)
            {
                var saleItem = saleItems.First(x => x.Id == cartItem.SaleItemId);

                shoppingCart.CartItems.Add(new ShoppingCartItem
                {
                    SaleItemId = saleItem.Id,
                    SaleItem = saleItem,
                    Quantity = cartItem.Quantity,
                    AvailableQuantity = saleItem.Quantity
                });
            }

            _dbContext.SaveChanges();
            _dbContext.Database.CommitTransaction();
            isTransactionCommitted = true;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _dbContext.Database.RollbackTransaction();
            }
        }
    }

    public void UpdateShoppingCartItem(Guid id, Guid saleItemId, int quantity)
    {
        if (quantity == 0)
        {
            RemoveEmptyCartItem(id, saleItemId);
            return;
        }

        var isTransactionCommitted = false;
        _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var shoppingCart = _dbContext.ShoppingCarts
                .FirstOrDefault(x => x.Id == id)
                .ThrowIfNotFound(id);

            var currentSaleItem = shoppingCart.CartItems
                .FirstOrDefault(x => x.SaleItemId == saleItemId);

            if (currentSaleItem is null)
            {
                currentSaleItem = new ShoppingCartItem
                {
                    SaleItemId = saleItemId
                };
                shoppingCart.CartItems.Add(currentSaleItem);
            }

            var saleItem = _dbContext.SaleItems
                .FirstOrDefault(x => x.Id == saleItemId)
                .ThrowIfNotFound(saleItemId);

            currentSaleItem.Quantity = quantity;
            currentSaleItem.AvailableQuantity = saleItem.Quantity;

            _dbContext.SaveChanges();
            _dbContext.Database.CommitTransaction();
            isTransactionCommitted = true;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _dbContext.Database.RollbackTransaction();
            }
        }
    }

    public void Delete(Guid entityId)
    {
        var shoppingCart = _dbContext.ShoppingCarts
            .FirstOrDefault(x => x.Id == entityId)
            .ThrowIfNotFound(entityId);

        _dbContext.ShoppingCarts.Remove(shoppingCart);

        _dbContext.SaveChanges();
    }

    public IEnumerable<ShoppingCart> Read(IEnumerable<Guid> ids)
    {
        var shoppingCarts = _dbContext.ShoppingCarts
            .Where(x => ids.Contains(x.Id))
            .ToArray();

        var notFoundEntitiesIds = ids
            .Except(shoppingCarts.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(ShoppingCart), notFoundEntitiesIds);
        }

        return shoppingCarts.ToArray();
    }

    public PagedModel<ShoppingCart> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var shoppingCarts = _dbContext.ShoppingCarts
            .Where(x => ids.Contains(x.Id));

        var notFoundEntitiesIds = ids
            .Except(shoppingCarts.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(ShoppingCart), notFoundEntitiesIds);
        }

        return PagedModel<ShoppingCart>.Paginate(shoppingCarts, page, pageSize);
    }

    private SaleItem[] GetSaleItems(Guid[] ids)
    {
        var saleItems = _dbContext.SaleItems
            .Where(x => ids.Contains(x.Id))
            .ToArray();

        var notFoundItemIds = ids
            .Except(saleItems.Select(x => x.Id))
            .ToArray();

        if (notFoundItemIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(SaleItem), notFoundItemIds);
        }

        return saleItems;
    }

    private void RemoveEmptyCartItems(ShoppingCart shoppingCart)
    {
        shoppingCart.CartItems = shoppingCart.CartItems
            .Where(x => x.Quantity > 0)
            .ToList();
    }

    private void RemoveEmptyCartItem(Guid shoppingCartId, Guid saleItemId)
    {
        var shoppingCart = _dbContext.ShoppingCarts
            .FirstOrDefault(x => x.Id == shoppingCartId)
            .ThrowIfNotFound(shoppingCartId);

        var cartItem = shoppingCart.CartItems
            .FirstOrDefault(x => x.SaleItemId == saleItemId);

        if (cartItem == null)
            return;

        shoppingCart.CartItems.Remove(cartItem);
        _dbContext.SaveChanges();
    }
}
