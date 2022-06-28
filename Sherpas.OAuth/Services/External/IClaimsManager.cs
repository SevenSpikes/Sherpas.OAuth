namespace Sherpas.OAuth.Services.External
{
    public interface IClaimsManager
    { 
        Task<Dictionary<string, string>> GetClaimsForClient(string clientId);
    }
}
