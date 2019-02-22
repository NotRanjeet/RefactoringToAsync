using BookStoreExample.Models;

namespace BookStoreExample.Repository.Contracts
{
    public interface IStoreRepository
    {
        ReferenceData GetReferenceData();
    }
}
