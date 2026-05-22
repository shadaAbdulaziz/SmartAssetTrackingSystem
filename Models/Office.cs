namespace SmartAssetTrackingSystem.Models;

public class Office
{
    public int Id { get; set; }

    public string OfficeName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string CurrencySymbol { get; set; } = string.Empty;

    public decimal ExchangeRateFromUsd { get; set; }
}