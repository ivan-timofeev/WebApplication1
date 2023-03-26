using System.Data;
using AutoMapper;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Models;
using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Exceptions;
using WebApplication1.Common.Extensions;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SearchEngine;

namespace WebApplication1.Services.OrdersManagementService;

public class OrdersManagementService : IOrdersManagementService
{
    #region Private Fields

    private readonly IMapper _mapper;
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly ISalePointsRepository _salePointsRepository;
    private readonly ISaleItemsRepository _saleItemsRepository;
    private readonly IShoppingCartsManagementService _shoppingCartsManagementService;
    private readonly IUpdateOrderStateStrategyResolver _updateOrderStateStrategyResolver;
    private readonly IDatabaseTransactionsManagementService _databaseTransactionsManagementService;

    #endregion

    #region CTOR

    public OrdersManagementService(
        IMapper mapper,
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        ISalePointsRepository salePointsRepository,
        ISaleItemsRepository saleItemsRepository,
        IShoppingCartsManagementService shoppingCartsManagementService,
        IUpdateOrderStateStrategyResolver updateOrderStateStrategyResolver,
        IDatabaseTransactionsManagementService databaseTransactionsManagementService)
    {
        _mapper = mapper;
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
        _salePointsRepository = salePointsRepository;
        _saleItemsRepository = saleItemsRepository;
        _shoppingCartsManagementService = shoppingCartsManagementService;
        _updateOrderStateStrategyResolver = updateOrderStateStrategyResolver;
        _databaseTransactionsManagementService = databaseTransactionsManagementService;
    }

    #endregion


    #region CreateOrderFromCustomerShoppingCart

    public Guid CreateOrderFromCustomerShoppingCart(Guid customerId)
    {
        var shoppingCart = _shoppingCartsManagementService.GetShoppingCartByCustomer(customerId);
        var customer = _customersRepository.Read(shoppingCart.CustomerId);

        var createdOrderId = Guid.Empty;

        _databaseTransactionsManagementService.ExecuteInTransaction(IsolationLevel.Serializable, () =>
        {
            var order = CreateOrder(customer);

            var saleItemIds = shoppingCart.CartItems
                .Select(x => x.SaleItemId)
                .ToArray();
            var saleItems = _saleItemsRepository
                .Read(saleItemIds)
                .ToArray();

            ReserveItems(order, shoppingCart, saleItems);
            createdOrderId = _ordersRepository.Create(order);
        });

        return createdOrderId;
    }

    private void ReserveItems(Order order, ShoppingCartVm shoppingCart, SaleItem[] saleItems)
    {
        foreach (var cartItem in shoppingCart.CartItems)
        {
            var saleItem = saleItems.First(x => x.Id == cartItem.SaleItemId);

            if (saleItem.Quantity < cartItem.Quantity)
            {
                var errorVm = new ErrorVmBuilder()
                    .WithGlobalError("There is not enough product in stock.")
                    .WithInfo("SalePointId", saleItem.SalePointId)
                    .WithInfo("SaleItemId", saleItem.Id)
                    .WithInfo("RequiredQuantity", cartItem.Quantity)
                    .WithInfo("AvailableQuantity", saleItem.Quantity)
                    .Build();

                throw new BusinessErrorException("There is not enough product in stock.",
                    StatusCodes.Status409Conflict, errorVm);
            }

            saleItem.Quantity -= cartItem.Quantity;
            _saleItemsRepository.Update(saleItem.Id, saleItem);

            var orderItem = new OrderItem
            {
                Quantity = cartItem.Quantity,
                SaleItemId = cartItem.SaleItemId,
                Price = saleItem.SellingPrice
            };
            order.OrderedItems.Add(orderItem);
        }

        OrderStateUtils.AddOrderState(order, OrderStateEnum.AwaitingPayment,
            enterDescription: "Order successfully reserved.");
    }

    private Order CreateOrder(Customer customer)
    {
        return new Order
        {
            OrderStateHierarchical = new List<OrderStateHierarchicalItem>()
            {
                new ()
                {
                    SerialNumber = 1,
                    EnteredDateTimeUtc = DateTime.UtcNow,
                    State = OrderStateEnum.Created,
                    EnterDescription = "Order created."
                }
            },
            CustomerId = customer.Id,
            Customer = customer
        };
    }

    #endregion

    #region GetOrder

    public OrderVm GetOrder(Guid orderId)
    {
        var order = _ordersRepository.Read(orderId);
        var orderVm = _mapper.Map<OrderVm>(order);

        return orderVm;
    }

    #endregion

    #region UpdateOrder

    public void UpdateOrder(Guid orderId, UpdateOrderStateVm model)
    {
        var order = _ordersRepository.Read(orderId);
        _updateOrderStateStrategyResolver
            .ResolveStrategy(order.ActualOrderState, model.NewOrderState)
            .UpdateOrder(order);
        _ordersRepository.Update(order.Id, order);
    }

    #endregion

    #region DeleteOrder

    public void DeleteOrder(Guid orderId)
    {
        _ordersRepository.Delete(orderId);
    }

    #endregion

    #region SearchOrder

    public PagedModel<OrderVm> SearchWithPagination(SearchEngineFilterVm? filterVm, int page, int pageSize)
    {
        var filter = _mapper.Map<SearchEngineFilter>(filterVm);
        var pagedModel = _ordersRepository.SearchWithPagination(filter, page, pageSize);
        var mappedPagedModel = pagedModel.MapTo<OrderVm>();

        return mappedPagedModel;
    }

    #endregion
}


public static class OrderStateUtils
{
    public static void AddOrderState(Order order, OrderStateEnum newState, string enterDescription)
    {
        var previousStateSerialNumber = order.OrderStateHierarchical.Max(x => x.SerialNumber);

        order.OrderStateHierarchical.Add(new OrderStateHierarchicalItem
        {
            SerialNumber = previousStateSerialNumber + 1,
            State = newState,
            EnterDescription = enterDescription,
            EnteredDateTimeUtc = DateTime.UtcNow
        });
    }
}
