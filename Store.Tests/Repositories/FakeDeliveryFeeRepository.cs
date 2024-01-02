using Store.Domain.Repositories;

namespace Store.Tests.Repositories;

public class FakeDeliveryFreeRepository : IDeliveryFeeRepository
{
  public decimal Get(string zipCode)
  {
    return 10;
  }
}
