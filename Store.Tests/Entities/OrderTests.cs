namespace Store.Tests.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Domain.Entities;
using Store.Domain.Enums;

[TestClass]
public class OrderTests
{
    private readonly Customer _customer;
    private readonly Discount _discount;
    private readonly Product _product;
//            private readonly Order _order = new Order(customer, 10.50m, discount);

    public OrderTests()
    {
        _customer = new Customer("Bruce Wayne", "batman@dc.com");
        _discount = new Discount(5.50m, DateTime.Now.AddDays(5));
        _product = new Product("Protudo 1", 10.50m, true);
    }


    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmNovoPedidoValidarEleDeveGerarUmNumeroCom8Caracteres()
    {
        var order = new Order(_customer, 10.50m, _discount);
        Assert.AreEqual(8, order.Number.Length);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmNovoPedidoSeuStatusDeveSerAguardandoPagamento()
    {
        var order = new Order(_customer, 10.50m, _discount);
        Assert.AreEqual(EOrderStatus.WaitingPayment, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmPagamentoDoPedidoSeuStatusDeveSerAguardandoEntrega()
    {
        var order = new Order(_customer, 10.50m, _discount);
        order.AddItem(_product, 5);
        // 5 * 10.50 (produto) + 10.50 (frete) - 5.50 (desconto) = 57.50
        order.Pay(57.50m); // Totoal do pedido com frete e desconto.
        Assert.AreEqual(EOrderStatus.WaititngDelivery, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmPedidoCanseladoSeuStatusDeveSerCancelado()
    {
        var order = new Order(_customer, 0, null);
        order.Cancel();
        Assert.AreEqual(EOrderStatus.Canceled, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmNovoItemSemProdutoOMesmoNaoDeveSerAdicionado()
    {
        var order = new Order(_customer, 0, null);
        order.AddItem(null, 0);
        Assert.AreEqual(order.Items.Count, 0);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmNovoItemComQunatidadeZeroOuMenorOMesmoNaoDeveSerAdicionado()
    {
        var order = new Order(_customer, 0, null);
        order.AddItem(_product, 0);
        Assert.AreEqual(order.Items.Count, 0);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmNovoPedidoValidoSeuTotalDeveSer50()
    {
        var order = new Order(_customer, 0, null);
        var product = new Product("Produto teste", 50, true);
        order.AddItem(product, 1);
        order.Pay(50m);
        Assert.AreEqual(order.Total(), 50);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ValidarDescontoExpiradoOValorDeveSer60()
    {
        var diconto = new Discount(5, DateTime.Now.AddDays(-2));
        var order = new Order(_customer, 10, diconto);
        var product = new Product("Produto teste", 50, true);
        order.AddItem(product, 1);
        order.Pay(60);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void TestarDescontoInvalidoDeveRetornoar60()
    {
        var order = new Order(_customer, 10, null);
        var product = new Product("Protudo 1", 10, true);
        order.AddItem(product, 5);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DeveDar10DeDescontoEFicar50()
    {
        var disconto = new Discount(10, DateTime.Now.AddDays(5));
        var order = new Order(_customer, 10, disconto);
        var product = new Product("Protudo 1", 10, true);
        order.AddItem(product, 5);
        Assert.AreEqual(order.Total(), 50);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DeveAdicionar10DeTaxaEntrega()
    {
        var order = new Order(_customer, 10, null);
        var product = new Product("Protudo 1", 10, true);
        order.AddItem(product, 5);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void DadoUmPedidoSemClienteOMesmoDeveSerInvalido()
    {
        var order = new Order(null, 10, _discount);
        Assert.AreEqual(order.Valid, false);
    }
}