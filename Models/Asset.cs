namespace SmartAssetTrackingSystem.Models;

public abstract class Asset
{
    public int Id { get; set; }

    public string AssetType { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePriceUsd { get; set; }

    public string Currency { get; set; } = string.Empty;
    public decimal LocalPrice { get; set; }

    public string OfficeLocation { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;

    public string? EmployeeUsername { get; set; }

    public DateTime WarrantyExpirationDate { get; set; }
}