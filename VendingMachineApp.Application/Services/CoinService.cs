using VendingMachineApp.Core.Entities;
using VendingMachineApp.Core.Interfaces;

namespace VendingMachineApp.Application.Services
{
    public class CoinService
    {
        private readonly ICoinRepository _coinRepository;
        public CoinService(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<List<Coin>> GetAllAsync()
        {
            return await _coinRepository.GetAllAsync();
        }

        public async Task<Coin?> GetByNominalAsync(int nominal)
        {
            return await _coinRepository.GetByNominalAsync(nominal);
        }

        public async Task<Coin?> GetByIdAsync(int id)
        {
            return await _coinRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateQuantityAsync(int id, int quantity)
        {
            return await _coinRepository.UpdateQuantityAsync(id, quantity);
        }
        public async Task<bool> UpdateRangeQuantitiesAsync(IEnumerable<Coin> coinsToUpdate)
        {
            return await _coinRepository.UpdateRangeQuantitiesAsync(coinsToUpdate);
        }
    }
}
