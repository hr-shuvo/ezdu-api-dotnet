namespace Core.App.Services.Interfaces;

public interface ISeeder
{
    public Task SeedRolesAsync();
    public Task SeedDefaultUsersAsync();
}