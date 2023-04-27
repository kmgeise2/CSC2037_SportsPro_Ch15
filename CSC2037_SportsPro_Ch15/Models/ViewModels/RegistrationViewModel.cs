namespace CSC2037_SportsPro_Ch15.Models
{
    public class RegistrationViewModel
    {
        public IEnumerable<Customer> Customers { get; set; } = null!;
        public IEnumerable<Product> Products { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Customer Customer { get; set; } = null!;

        public bool HasProduct => Product?.ProductID > 0;
        public bool HasCustomer => Customer?.CustomerID > 0;
    }
}
