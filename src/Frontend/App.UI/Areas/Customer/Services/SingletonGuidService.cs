namespace App.UI.Services
{
    public class SingletonGuidService : ISingletonGuidService
    {
        
        private readonly Guid Id;
        public SingletonGuidService()
        {
            Id = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return Id.ToString();
        }
    }
}