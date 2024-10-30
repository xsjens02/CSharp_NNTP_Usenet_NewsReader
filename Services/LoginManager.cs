namespace UsenetProgram.Services
{
    public abstract class LoginManager
    {
        public static async Task<bool> Authenticate(string login, string password)
        {
            await NetworkManager.Instance.WriteToStreamAsync($"AUTHINFO USER {login}");
            string loginResponse = await NetworkManager.Instance.ReadFromStreamAsync(false);
            if (!loginResponse.StartsWith("381"))
                return false;

            await NetworkManager.Instance.WriteToStreamAsync($"AUTHINFO PASS {password}");
            string passwordResponse = await NetworkManager.Instance.ReadFromStreamAsync(false);
            if (!passwordResponse.StartsWith("281"))
                return false;

            return true;
        }
    }
}
