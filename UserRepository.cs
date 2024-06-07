using Microsoft.EntityFrameworkCore;
using ModelsMQTT_Server;
using BCrypt.Net;


namespace My_MQTT_Server;
public class UserRepository
{
    private readonly MqttDbContext _context;

    public UserRepository(MqttDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Email == email) ?? throw new Exception("User not found.");
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}