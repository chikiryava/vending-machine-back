using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Core.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<Brand> AddAsync(Brand brand);
        Task<bool> UpdateAsync(Brand brand);
        Task<bool> DeleteAsync(int id);
    }
}
