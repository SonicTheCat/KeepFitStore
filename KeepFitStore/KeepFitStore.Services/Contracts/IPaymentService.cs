namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IPaymentService 
    {
        Task<bool> MakePaymentAsync(string email, string token, decimal amount); 
    }
}