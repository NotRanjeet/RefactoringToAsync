using System.Threading.Tasks;
using BookStoreExample.Models;

namespace BookStoreExample.Repository.Contracts
{
    public interface IStoreRepositoryAsync
    {
        Task<ReferenceData> GetReferenceDataAsync();
    }
}
