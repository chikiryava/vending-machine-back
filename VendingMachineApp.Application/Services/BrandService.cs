using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;

namespace VendingMachineApp.Application.Services
{
    public class BrandService
    {
        private readonly IBrandRepository _brandRepository;
        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            return await _brandRepository.AddAsync(brand);
        }

        public async Task<bool> UpdateAsync(Brand brand)
        {
            return await _brandRepository.UpdateAsync(brand);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _brandRepository.DeleteAsync(id);
        }
    }
}
