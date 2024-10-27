namespace App.Frontend.Services.IServices
{
    public interface IGlobalSession
    {
        void SetSessionData(string key, string value);
        string GetSessionData(string key);
    }
}