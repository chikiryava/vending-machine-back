using Microsoft.AspNetCore.SignalR;

namespace VendingMachineApp.Hubs
{
    public class VendingMachineHub : Hub
    {
        private static bool _isBusy = false;
        private static string? _lockedByConnectionId;

        // Метод для попытки заблокировать машину
        public async Task<bool> TryLock()
        {
            var currentId = Context.ConnectionId;

            // Если машина уже заблокирована другим пользователем
            if (_isBusy && _lockedByConnectionId != currentId)
            {
                await Clients.Caller.SendAsync("MachineBusy");
                return false;
            }

            // Машина доступна для блокировки
            _isBusy = true;
            _lockedByConnectionId = currentId;
            await Clients.Others.SendAsync("MachineLocked"); // Уведомление другим пользователям
            return true;
        }

        // Метод для разблокировки машины
        public async Task Unlock()
        {
            if (_lockedByConnectionId == Context.ConnectionId)
            {
                _isBusy = false;
                _lockedByConnectionId = null;
                await Clients.All.SendAsync("MachineUnlocked"); // Уведомление всем пользователям о разблокировке
            }
        }

        // Обработка отключения пользователя
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_lockedByConnectionId == Context.ConnectionId)
            {
                _isBusy = false;
                _lockedByConnectionId = null;
                await Clients.All.SendAsync("MachineUnlocked"); // Разблокировка при отключении
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
