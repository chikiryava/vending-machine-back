# VendingMachineApp
## Использованные технологии
- C# для реализации серверной логики.
- EPPlus для чтения данных из Excel файлов.
- Entity Framework Core (EF Core) для работы с базой данных.
- SignalR
## Инструкция по запуску
- Проверьте и настройте строку подключения к базе данных в файле appsettings.json.
- Выполните миграцию
	add-migration initial -context InventoryContext -outputdir Migrations/Inventory -project VendingMachineApp.Infrastructure
	add-migration initial -context OrderContext -outputdir Migrations/Order -project VendingMachineApp.Infrastructure
	update-database -context InventoryContext
	update-database -context OrderContext
   - Запустите серверную часть проекта.
