namespace JTX_Ecom.Services
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, List<string> roles);
    }
}
