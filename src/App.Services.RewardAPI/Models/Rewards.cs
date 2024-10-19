namespace App.Services.RewardAPI.Models;
public class Rewards
{
    public int Id { get; set; }
    public string userId { get; set; }
    public DateTime dateTime { get; set; }
    public int RewardsActivity { get; set; }
    public int OrderId { get; set; }
}