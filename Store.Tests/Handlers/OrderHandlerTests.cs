using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers;

[TestClass]
public class OrderHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeRepository _deliveryFeeRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderHandlerTests()
    {
        _customerRepository = new FakeCustomerRepository();
        _deliveryFeeRepository = new FakeDeliveryFreeRepository();
        _discountRepository = new FakeDiscountRepository();
        _orderRepository = new FakeOrderRepository();
        _productRepository = new FakeProductRepository();
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void DadoUmClienteInexistenteOPedidoNaoDeveSerGerado()
    {
        var command = new CreateOrderCommand();
        command.Customer = "";
        command.ZipCode = "13411080";
        command.PromoCode = "12345678";
        command.Validate();

        Assert.AreEqual(command.Valid, false);
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void CpeInvalido()
    {
        var command = new CreateOrderCommand();
        command.Customer = "1234568";
        command.ZipCode = "";
        command.PromoCode = "12345678";
        command.Validate();

        Assert.AreEqual(command.Valid, false);
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void PromoCodeInesistenteGerarPedidoNormalmente()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "13411080";
        command.PromoCode = "";
        command.Validate();

        Assert.AreEqual(command.Valid, true);
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void PedidoSemItem()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678911";
        command.ZipCode = "13411080";
        command.PromoCode = "";
        var items = new CreateOrderItemCommand();
        items.Validate();

        Assert.AreEqual(items.Valid, false);
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void ComandoInvalido()
    {
        var command = new CreateOrderCommand();
        command.Customer = "";
        command.ZipCode = "13411080";
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Validate();
        Assert.AreEqual(command.Valid, false);
    }

    [TestMethod]
    [TestCategory("Handler")]
    public void ComandoValido()
    {
        var command = new CreateOrderCommand();
        command.Customer = "12345678";
        command.ZipCode = "13411080";
        command.PromoCode = "12345678";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

        var handler = new OrderHandler(_customerRepository, _deliveryFeeRepository, _discountRepository,
            _productRepository, _orderRepository);
        handler.Handle(command);

        Assert.AreEqual(handler.Valid, true);
    }
}