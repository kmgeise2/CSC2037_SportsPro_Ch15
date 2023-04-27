namespace CSC2037_SportsPro_Ch15.Models
{
    public static class Check
    {
        public static string EmailExists(Repository<Customer> data, string email)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(email))
            {
                var options = new QueryOptions<Customer>
                {
                    Where = c => c.Email!.ToLower() == email.ToLower()
                };
                var customer = data.Get(options);
                if (customer != null)
                    msg = "Email address already in use.";
            }
            return msg;
        }
    }
}