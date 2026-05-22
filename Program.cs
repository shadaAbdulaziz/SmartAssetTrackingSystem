using SmartAssetTrackingSystem.Data;
using SmartAssetTrackingSystem.Models;
using (AppDbContext db = new AppDbContext())
{
    SeedData(db);
}

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Smart Asset Tracking System =====");
    Console.WriteLine("1. Add Asset");
    Console.WriteLine("2. Show All Assets");
    Console.WriteLine("3. Update Asset");
    Console.WriteLine("4. Delete Asset");
    Console.WriteLine("5. Reports");
    Console.WriteLine("0. Exit");
    Console.Write("Choose option: ");

    string? choice = Console.ReadLine();

    if (choice == "1")
    {
        AddAsset();
    }
    else if (choice == "2")
    {
        ShowAllAssets();
    }
    else if (choice == "3")
    {
        UpdateAsset();
    }
    else if (choice == "4")
    {
        DeleteAsset();
    }
    else if (choice == "5")
    {
        ShowReports();
    }
    else if (choice == "0")
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}

static void AddAsset()
{
    using AppDbContext db = new AppDbContext();

    Console.WriteLine("Choose asset category:");
    Console.WriteLine("1. Computer Asset");
    Console.WriteLine("2. Mobile Asset");
    Console.Write("Choice: ");

    int choice = ReadValidInt();

    if (choice == 1)
    {
        ComputerAsset asset = new ComputerAsset();

        Console.Write("Enter Asset Type Laptop/Desktop: ");
        asset.AssetType = ReadRequiredText();

        FillCommonAssetData(asset);

        db.ComputerAssets.Add(asset);
        db.SaveChanges();

        Console.WriteLine($"Computer asset added successfully. Asset ID: {asset.Id}");
    }
    else if (choice == 2)
    {
        MobileAsset asset = new MobileAsset();

        Console.Write("Enter Asset Type iPhone/Samsung/Nokia/Tablet: ");
        asset.AssetType = ReadRequiredText();

        FillCommonAssetData(asset);

        db.MobileAssets.Add(asset);
        db.SaveChanges();

        Console.WriteLine($"Mobile asset added successfully. Asset ID: {asset.Id}");
    }
    else
    {
        Console.WriteLine("Invalid category.");
    }
}

static void ShowAllAssets()
{
    using AppDbContext db = new AppDbContext();

    var computerAssets = db.ComputerAssets
    .OrderBy(a => a.Id)
    .ToList();

    var mobileAssets = db.MobileAssets
        .OrderBy(a => a.Id)
        .ToList();

    if (!computerAssets.Any() && !mobileAssets.Any())
    {
        Console.WriteLine("No assets found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("============================== COMPANY ASSETS ==============================");
    Console.WriteLine();

    Console.WriteLine("COMPUTERS");
    PrintReportHeader();

    foreach (var asset in computerAssets)
    {
        PrintReportRow(asset);
    }

    Console.WriteLine(new string('-', 125));
    Console.WriteLine();

    Console.WriteLine("MOBILE DEVICES");
    PrintReportHeader();

    foreach (var asset in mobileAssets)
    {
        PrintReportRow(asset);
    }

    Console.WriteLine(new string('-', 125));
}
static void UpdateAsset()
{
    using AppDbContext db = new AppDbContext();

    Console.WriteLine("Choose asset category to update:");
    Console.WriteLine("1. Computer Asset");
    Console.WriteLine("2. Mobile Asset");
    Console.Write("Choice: ");

    int categoryChoice = ReadValidInt();

    Console.Write("Enter Asset Id to update: ");
    int id = ReadValidInt();

    if (categoryChoice == 1)
    {
        var asset = db.ComputerAssets.FirstOrDefault(a => a.Id == id);

        if (asset == null)
        {
            Console.WriteLine("Computer asset not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Current asset data:");
        PrintSingleAsset(asset);

        Console.WriteLine();
        Console.WriteLine("Enter new values. Press Enter to keep current value.");

        Console.Write($"Asset Type ({asset.AssetType}): ");
        asset.AssetType = ReadOptionalText(asset.AssetType);

        UpdateCommonAssetData(asset);

        db.SaveChanges();

        Console.WriteLine("Computer asset updated successfully.");
    }
    else if (categoryChoice == 2)
    {
        var asset = db.MobileAssets.FirstOrDefault(a => a.Id == id);

        if (asset == null)
        {
            Console.WriteLine("Mobile asset not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Current asset data:");
        PrintSingleAsset(asset);

        Console.WriteLine();
        Console.WriteLine("Enter new values. Press Enter to keep current value.");

        Console.Write($"Asset Type ({asset.AssetType}): ");
        asset.AssetType = ReadOptionalText(asset.AssetType);

        UpdateCommonAssetData(asset);

        db.SaveChanges();

        Console.WriteLine("Mobile asset updated successfully.");
    }
    else
    {
        Console.WriteLine("Invalid category.");
    }
}
static void DeleteAsset()
{
    using AppDbContext db = new AppDbContext();

    Console.WriteLine("Choose asset category to delete:");
    Console.WriteLine("1. Computer Asset");
    Console.WriteLine("2. Mobile Asset");
    Console.Write("Choice: ");

    int categoryChoice = ReadValidInt();

    Console.Write("Enter Asset Id to delete: ");
    int id = ReadValidInt();

    if (categoryChoice == 1)
    {
        var asset = db.ComputerAssets.FirstOrDefault(a => a.Id == id);

        if (asset == null)
        {
            Console.WriteLine("Computer asset not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Asset to delete:");
        PrintSingleAsset(asset);

        Console.Write("Are you sure you want to delete this computer asset? y/n: ");
        string? confirm = Console.ReadLine();

        if (confirm?.ToLower() == "y")
        {
            db.ComputerAssets.Remove(asset);
            db.SaveChanges();
            Console.WriteLine("Computer asset deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }
    else if (categoryChoice == 2)
    {
        var asset = db.MobileAssets.FirstOrDefault(a => a.Id == id);

        if (asset == null)
        {
            Console.WriteLine("Mobile asset not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Asset to delete:");
        PrintSingleAsset(asset);

        Console.Write("Are you sure you want to delete this mobile asset? y/n: ");
        string? confirm = Console.ReadLine();

        if (confirm?.ToLower() == "y")
        {
            db.MobileAssets.Remove(asset);
            db.SaveChanges();
            Console.WriteLine("Mobile asset deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }
    else
    {
        Console.WriteLine("Invalid category.");
    }
}
static Asset? FindAssetById(AppDbContext db, int id)
{
    var computerAsset = db.ComputerAssets.FirstOrDefault(a => a.Id == id);

    if (computerAsset != null)
    {
        return computerAsset;
    }

    var mobileAsset = db.MobileAssets.FirstOrDefault(a => a.Id == id);

    return mobileAsset;
}

static void FillCommonAssetData(Asset asset)
{
    Console.Write("Enter Brand: ");
    asset.Brand = ReadRequiredText();

    Console.Write("Enter Model Name: ");
    asset.ModelName = ReadRequiredText();

    Office office = ChooseOffice();

    asset.OfficeLocation = office.OfficeName;

    Console.Write("Enter Purchase Date yyyy-mm-dd: ");
    asset.PurchaseDate = ReadValidDate();

    Console.Write("Enter Purchase Price USD: ");
    asset.PurchasePriceUsd = ReadPositiveDecimal();

    asset.Currency = office.Currency;
    asset.LocalPrice = asset.PurchasePriceUsd * office.ExchangeRateFromUsd;

    Console.Write("Enter Serial Number: ");
    asset.SerialNumber = ReadRequiredText();

    Console.Write("Enter Employee Username optional, press Enter to skip: ");
    asset.EmployeeUsername = Console.ReadLine();

    Console.Write("Enter Warranty Expiration Date yyyy-mm-dd: ");
    asset.WarrantyExpirationDate = ReadValidDate();
}
static void UpdateCommonAssetData(Asset asset)
{
    Console.Write($"Brand ({asset.Brand}): ");
    asset.Brand = ReadOptionalText(asset.Brand);

    Console.Write($"Model Name ({asset.ModelName}): ");
    asset.ModelName = ReadOptionalText(asset.ModelName);

    Console.Write($"Office Location ({asset.OfficeLocation}): ");
    asset.OfficeLocation = ReadOptionalText(asset.OfficeLocation);

    Console.Write($"Purchase Date ({asset.PurchaseDate:yyyy-MM-dd}): ");
    asset.PurchaseDate = ReadOptionalDate(asset.PurchaseDate);

    Console.Write($"Purchase Price USD ({asset.PurchasePriceUsd}): ");
    asset.PurchasePriceUsd = ReadOptionalPositiveDecimal(asset.PurchasePriceUsd);

    Console.Write($"Currency ({asset.Currency}): ");
    asset.Currency = ReadOptionalCurrency(asset.Currency);

    asset.LocalPrice = ConvertUsdToLocalPrice(asset.PurchasePriceUsd, asset.Currency);

    Console.Write($"Serial Number ({asset.SerialNumber}): ");
    asset.SerialNumber = ReadOptionalText(asset.SerialNumber);

    Console.Write($"Employee Username ({asset.EmployeeUsername ?? "Not assigned"}): ");
    string? employeeInput = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(employeeInput))
    {
        asset.EmployeeUsername = employeeInput.Trim();
    }

    Console.Write($"Warranty Expiration Date ({asset.WarrantyExpirationDate:yyyy-MM-dd}): ");
    asset.WarrantyExpirationDate = ReadOptionalDate(asset.WarrantyExpirationDate);
}

static void PrintReportHeader()
{
    Console.WriteLine(new string('-', 125));

    Console.WriteLine(
        $"{Pad("ID", 5)}" +
        $"{Pad("Type", 12)}" +
        $"{Pad("Brand", 15)}" +
        $"{Pad("Model", 18)}" +
        $"{Pad("Office", 15)}" +
        $"{Pad("Purchase Date", 16)}" +
        $"{Pad("USD", 12)}" +
        $"{Pad("Currency", 12)}" +
        $"{Pad("Local Price", 15)}" +
        $"{Pad("Status", 10)}"
    );

    Console.WriteLine(new string('-', 125));
}

static void PrintReportRow(Asset asset)
{
    Console.WriteLine(
        $"{Pad(asset.Id.ToString(), 5)}" +
        $"{Pad(asset.AssetType, 12)}" +
        $"{Pad(asset.Brand, 15)}" +
        $"{Pad(asset.ModelName, 18)}" +
        $"{Pad(asset.OfficeLocation, 15)}" +
        $"{Pad(asset.PurchaseDate.ToString("yyyy-MM-dd"), 16)}" +
        $"{Pad(asset.PurchasePriceUsd.ToString("0.00"), 12)}" +
        $"{Pad(asset.Currency, 12)}" +
        $"{Pad(asset.LocalPrice.ToString("0.00"), 15)}" +
        $"{Pad(GetAssetStatus(asset), 10)}"
    );
}

static void PrintSingleAsset(Asset asset)
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"{"ID",-20}: {asset.Id}");
    Console.WriteLine($"{"Type",-20}: {asset.AssetType}");
    Console.WriteLine($"{"Brand",-20}: {asset.Brand}");
    Console.WriteLine($"{"Model",-20}: {asset.ModelName}");
    Console.WriteLine($"{"Office",-20}: {asset.OfficeLocation}");
    Console.WriteLine($"{"Purchase Date",-20}: {asset.PurchaseDate:yyyy-MM-dd}");
    Console.WriteLine($"{"Price USD",-20}: {asset.PurchasePriceUsd:0.00}");
    Console.WriteLine($"{"Currency",-20}: {asset.Currency}");
    Console.WriteLine($"{"Local Price",-20}: {asset.LocalPrice:0.00}");
    Console.WriteLine($"{"Serial Number",-20}: {asset.SerialNumber}");
    Console.WriteLine($"{"Employee",-20}: {asset.EmployeeUsername ?? "Not assigned"}");
    Console.WriteLine($"{"Warranty Expiration",-20}: {asset.WarrantyExpirationDate:yyyy-MM-dd}");
    Console.WriteLine($"{"Status",-20}: {GetAssetStatus(asset)}");
    Console.WriteLine("--------------------------------------------------");
}

static string Pad(string text, int width)
{
    if (string.IsNullOrWhiteSpace(text))
    {
        text = "";
    }

    if (text.Length > width - 1)
    {
        text = text.Substring(0, width - 4) + "...";
    }

    return text.PadRight(width);
}

static decimal ConvertUsdToLocalPrice(decimal usdPrice, string currency)
{
    currency = currency.ToUpper();

    if (currency == "USD")
    {
        return usdPrice;
    }
    else if (currency == "EUR")
    {
        return usdPrice * 0.92m;
    }
    else if (currency == "SEK")
    {
        return usdPrice * 10.50m;
    }
    else
    {
        return usdPrice;
    }
}

static string GetAssetStatus(Asset asset)
{
    DateTime endOfLifeDate = asset.PurchaseDate.AddYears(3);
    DateTime today = DateTime.Today;

    int remainingMonths = ((endOfLifeDate.Year - today.Year) * 12)
                          + endOfLifeDate.Month
                          - today.Month;

    if (remainingMonths < 0)
    {
        return "RED";
    }
    else if (remainingMonths < 3)
    {
        return "YELLOW";
    }
    else if (remainingMonths < 6)
    {
        return "RED";
    }
    else
    {
        return "NORMAL";
    }
}

static string ReadRequiredText()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            return input.Trim();
        }

        Console.Write("Input cannot be empty. Try again: ");
    }
}

static DateTime ReadValidDate()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (DateTime.TryParse(input, out DateTime date))
        {
            return date;
        }

        Console.Write("Invalid date. Use format yyyy-mm-dd: ");
    }
}

static decimal ReadPositiveDecimal()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (decimal.TryParse(input, out decimal value) && value > 0)
        {
            return value;
        }

        Console.Write("Invalid price. Enter a positive number: ");
    }
}

static int ReadValidInt()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.Write("Invalid number. Try again: ");
    }
}

static string ReadCurrency()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.Write("Currency cannot be empty. Enter USD, EUR, or SEK: ");
            continue;
        }

        input = input.Trim().ToUpper();

        if (input == "USD" || input == "EUR" || input == "SEK")
        {
            return input;
        }

        Console.Write("Invalid currency. Enter USD, EUR, or SEK: ");
    }
}

static string ReadOptionalText(string currentValue)
{
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        return currentValue;
    }

    return input.Trim();
}

static DateTime ReadOptionalDate(DateTime currentValue)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (DateTime.TryParse(input, out DateTime date))
        {
            return date;
        }

        Console.Write("Invalid date. Use format yyyy-mm-dd or press Enter to keep current: ");
    }
}

static decimal ReadOptionalPositiveDecimal(decimal currentValue)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (decimal.TryParse(input, out decimal value) && value > 0)
        {
            return value;
        }

        Console.Write("Invalid price. Enter positive number or press Enter to keep current: ");
    }
}

static string ReadOptionalCurrency(string currentCurrency)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentCurrency;
        }

        input = input.Trim().ToUpper();

        if (input == "USD" || input == "EUR" || input == "SEK")
        {
            return input;
        }

        Console.Write("Invalid currency. Enter USD, EUR, SEK or press Enter to keep current: ");
    }
}
static void SeedData(AppDbContext db)
{
    if (!db.Offices.Any())
    {
        db.Offices.AddRange(
            new Office
            {
                OfficeName = "Sweden Office",
                Country = "Sweden",
                Currency = "SEK",
                CurrencySymbol = "kr",
                ExchangeRateFromUsd = 10.50m
            },
            new Office
            {
                OfficeName = "USA Office",
                Country = "USA",
                Currency = "USD",
                CurrencySymbol = "$",
                ExchangeRateFromUsd = 1.00m
            },
            new Office
            {
                OfficeName = "Germany Office",
                Country = "Germany",
                Currency = "EUR",
                CurrencySymbol = "€",
                ExchangeRateFromUsd = 0.92m
            },
            new Office
            {
                OfficeName = "Turkey Office",
                Country = "Turkey",
                Currency = "TRY",
                CurrencySymbol = "₺",
                ExchangeRateFromUsd = 32.00m
            }
        );

        db.SaveChanges();
    }

    if (db.ComputerAssets.Any() || db.MobileAssets.Any())
    {
        return;
    }

    var swedenOffice = db.Offices.First(o => o.OfficeName == "Sweden Office");
    var usaOffice = db.Offices.First(o => o.OfficeName == "USA Office");
    var germanyOffice = db.Offices.First(o => o.OfficeName == "Germany Office");
    var turkeyOffice = db.Offices.First(o => o.OfficeName == "Turkey Office");

    db.ComputerAssets.AddRange(
    new ComputerAsset
    {
        AssetType = "Laptop",
        Brand = "Dell",
        ModelName = "XPS 13",
        OfficeLocation = swedenOffice.OfficeName,
        PurchaseDate = new DateTime(2024, 2, 7),
        PurchasePriceUsd = 1200,
        Currency = swedenOffice.Currency,
        LocalPrice = 1200 * swedenOffice.ExchangeRateFromUsd,
        SerialNumber = "DELL-SE-001",
        EmployeeUsername = "sara",
        WarrantyExpirationDate = new DateTime(2027, 2, 7)
    },
    new ComputerAsset
    {
        AssetType = "Desktop",
        Brand = "HP",
        ModelName = "EliteDesk",
        OfficeLocation = germanyOffice.OfficeName,
        PurchaseDate = new DateTime(2023, 6, 15),
        PurchasePriceUsd = 900,
        Currency = germanyOffice.Currency,
        LocalPrice = 900 * germanyOffice.ExchangeRateFromUsd,
        SerialNumber = "HP-DE-002",
        EmployeeUsername = "max",
        WarrantyExpirationDate = new DateTime(2026, 6, 15)
    },
    new ComputerAsset
    {
        AssetType = "Laptop",
        Brand = "Lenovo",
        ModelName = "ThinkPad X1",
        OfficeLocation = usaOffice.OfficeName,
        PurchaseDate = new DateTime(2025, 1, 12),
        PurchasePriceUsd = 1450,
        Currency = usaOffice.Currency,
        LocalPrice = 1450 * usaOffice.ExchangeRateFromUsd,
        SerialNumber = "LEN-US-003",
        EmployeeUsername = "john",
        WarrantyExpirationDate = new DateTime(2028, 1, 12)
    },
    new ComputerAsset
    {
        AssetType = "Desktop",
        Brand = "Apple",
        ModelName = "iMac 24",
        OfficeLocation = turkeyOffice.OfficeName,
        PurchaseDate = new DateTime(2022, 9, 25),
        PurchasePriceUsd = 1600,
        Currency = turkeyOffice.Currency,
        LocalPrice = 1600 * turkeyOffice.ExchangeRateFromUsd,
        SerialNumber = "IMAC-TR-004",
        EmployeeUsername = "ayse",
        WarrantyExpirationDate = new DateTime(2025, 9, 25)
    },
    new ComputerAsset
    {
        AssetType = "Laptop",
        Brand = "Asus",
        ModelName = "ZenBook 14",
        OfficeLocation = swedenOffice.OfficeName,
        PurchaseDate = new DateTime(2021, 12, 1),
        PurchasePriceUsd = 1100,
        Currency = swedenOffice.Currency,
        LocalPrice = 1100 * swedenOffice.ExchangeRateFromUsd,
        SerialNumber = "ASUS-SE-005",
        EmployeeUsername = "lina",
        WarrantyExpirationDate = new DateTime(2024, 12, 1)
    }
);

    db.MobileAssets.AddRange(
        new MobileAsset
        {
            AssetType = "iPhone",
            Brand = "Apple",
            ModelName = "iPhone 15",
            OfficeLocation = usaOffice.OfficeName,
            PurchaseDate = new DateTime(2025, 1, 10),
            PurchasePriceUsd = 999,
            Currency = usaOffice.Currency,
            LocalPrice = 999 * usaOffice.ExchangeRateFromUsd,
            SerialNumber = "IPH-US-001",
            EmployeeUsername = "ali",
            WarrantyExpirationDate = new DateTime(2027, 1, 10)
        },
        new MobileAsset
        {
            AssetType = "Samsung",
            Brand = "Samsung",
            ModelName = "Galaxy S24",
            OfficeLocation = germanyOffice.OfficeName,
            PurchaseDate = new DateTime(2024, 5, 20),
            PurchasePriceUsd = 899,
            Currency = germanyOffice.Currency,
            LocalPrice = 899 * germanyOffice.ExchangeRateFromUsd,
            SerialNumber = "SAM-DE-002",
            EmployeeUsername = "emma",
            WarrantyExpirationDate = new DateTime(2026, 5, 20)
        },
        new MobileAsset
        {
            AssetType = "Tablet",
            Brand = "Samsung",
            ModelName = "Galaxy Tab S9",
            OfficeLocation = turkeyOffice.OfficeName,
            PurchaseDate = new DateTime(2022, 11, 20),
            PurchasePriceUsd = 750,
            Currency = turkeyOffice.Currency,
            LocalPrice = 750 * turkeyOffice.ExchangeRateFromUsd,
            SerialNumber = "TAB-TR-003",
            EmployeeUsername = "mehmet",
            WarrantyExpirationDate = new DateTime(2025, 11, 20)
        },
        new MobileAsset
        {
            AssetType = "Nokia",
            Brand = "Nokia",
            ModelName = "XR21",
            OfficeLocation = swedenOffice.OfficeName,
            PurchaseDate = new DateTime(2021, 8, 14),
            PurchasePriceUsd = 550,
            Currency = swedenOffice.Currency,
            LocalPrice = 550 * swedenOffice.ExchangeRateFromUsd,
            SerialNumber = "NOK-SE-004",
            EmployeeUsername = "erik",
            WarrantyExpirationDate = new DateTime(2024, 8, 14)
        },
        new MobileAsset
        {
            AssetType = "iPhone",
            Brand = "Apple",
            ModelName = "iPhone 14",
            OfficeLocation = usaOffice.OfficeName,
            PurchaseDate = new DateTime(2023, 3, 5),
            PurchasePriceUsd = 799,
            Currency = usaOffice.Currency,
            LocalPrice = 799 * usaOffice.ExchangeRateFromUsd,
            SerialNumber = "IPH-US-005",
            EmployeeUsername = "maria",
            WarrantyExpirationDate = new DateTime(2025, 3, 5)
        }
    );

    db.SaveChanges();
}
static Office ChooseOffice()
{
    using AppDbContext db = new AppDbContext();

    var offices = db.Offices.ToList();

    Console.WriteLine("Choose Office:");

    foreach (var office in offices)
    {
        Console.WriteLine($"{office.Id}. {office.OfficeName} - {office.Country} - {office.Currency}");
    }

    while (true)
    {
        Console.Write("Office Id: ");
        int officeId = ReadValidInt();

        var selectedOffice = offices.FirstOrDefault(o => o.Id == officeId);

        if (selectedOffice != null)
        {
            return selectedOffice;
        }

        Console.WriteLine("Invalid office id. Try again.");
    }
}
static void ShowReports()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("===== Reports =====");
        Console.WriteLine("1. Total asset value per office");
        Console.WriteLine("2. Asset count per office");
        Console.WriteLine("3. Assets close to expiration");
        Console.WriteLine("4. Most expensive asset");
        Console.WriteLine("0. Back to main menu");
        Console.Write("Choose report: ");

        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            ReportTotalValuePerOffice();
        }
        else if (choice == "2")
        {
            ReportAssetCountPerOffice();
        }
        else if (choice == "3")
        {
            ReportAssetsCloseToExpiration();
        }
        else if (choice == "4")
        {
            ReportMostExpensiveAsset();
        }
        else if (choice == "0")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
static void ReportTotalValuePerOffice()
{
    using AppDbContext db = new AppDbContext();

    var allAssets = GetAllAssets(db);

    if (!allAssets.Any())
    {
        Console.WriteLine("No assets found.");
        return;
    }

    var report = allAssets
        .GroupBy(a => a.OfficeLocation)
        .Select(g => new
        {
            Office = g.Key,
            TotalUsd = g.Sum(a => a.PurchasePriceUsd),
            TotalLocal = g.Sum(a => a.LocalPrice),
            Currency = g.First().Currency
        })
        .OrderBy(r => r.Office)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("===== Total Asset Value Per Office =====");
    Console.WriteLine(new string('-', 70));
    Console.WriteLine($"{Pad("Office", 20)}{Pad("Total USD", 15)}{Pad("Currency", 12)}{Pad("Total Local", 15)}");
    Console.WriteLine(new string('-', 70));

    foreach (var item in report)
    {
        Console.WriteLine(
            $"{Pad(item.Office, 20)}" +
            $"{Pad(item.TotalUsd.ToString("0.00"), 15)}" +
            $"{Pad(item.Currency, 12)}" +
            $"{Pad(item.TotalLocal.ToString("0.00"), 15)}"
        );
    }

    Console.WriteLine(new string('-', 70));
}
static void ReportAssetCountPerOffice()
{
    using AppDbContext db = new AppDbContext();

    var allAssets = GetAllAssets(db);

    if (!allAssets.Any())
    {
        Console.WriteLine("No assets found.");
        return;
    }

    var report = allAssets
        .GroupBy(a => a.OfficeLocation)
        .Select(g => new
        {
            Office = g.Key,
            Count = g.Count()
        })
        .OrderBy(r => r.Office)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("===== Asset Count Per Office =====");
    Console.WriteLine(new string('-', 45));
    Console.WriteLine($"{Pad("Office", 25)}{Pad("Asset Count", 15)}");
    Console.WriteLine(new string('-', 45));

    foreach (var item in report)
    {
        Console.WriteLine($"{Pad(item.Office, 25)}{Pad(item.Count.ToString(), 15)}");
    }

    Console.WriteLine(new string('-', 45));
}
static void ReportAssetsCloseToExpiration()
{
    using AppDbContext db = new AppDbContext();

    var allAssets = GetAllAssets(db);

    var closeAssets = allAssets
        .Where(a => GetAssetStatus(a) != "NORMAL")
        .OrderBy(a => a.PurchaseDate.AddYears(3))
        .ToList();

    if (!closeAssets.Any())
    {
        Console.WriteLine("No assets close to expiration.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Assets Close To Expiration =====");
    Console.WriteLine(new string('-', 105));
    Console.WriteLine(
        $"{Pad("ID", 5)}" +
        $"{Pad("Type", 12)}" +
        $"{Pad("Brand", 15)}" +
        $"{Pad("Model", 18)}" +
        $"{Pad("Office", 15)}" +
        $"{Pad("End Date", 15)}" +
        $"{Pad("Status", 10)}"
    );
    Console.WriteLine(new string('-', 105));

    foreach (var asset in closeAssets)
    {
        Console.WriteLine(
            $"{Pad(asset.Id.ToString(), 5)}" +
            $"{Pad(asset.AssetType, 12)}" +
            $"{Pad(asset.Brand, 15)}" +
            $"{Pad(asset.ModelName, 18)}" +
            $"{Pad(asset.OfficeLocation, 15)}" +
            $"{Pad(asset.PurchaseDate.AddYears(3).ToString("yyyy-MM-dd"), 15)}" +
            $"{Pad(GetAssetStatus(asset), 10)}"
        );
    }

    Console.WriteLine(new string('-', 105));
}
static void ReportMostExpensiveAsset()
{
    using AppDbContext db = new AppDbContext();

    var allAssets = GetAllAssets(db);

    if (!allAssets.Any())
    {
        Console.WriteLine("No assets found.");
        return;
    }

    var asset = allAssets
        .OrderByDescending(a => a.PurchasePriceUsd)
        .First();

    Console.WriteLine();
    Console.WriteLine("===== Most Expensive Asset =====");
    PrintSingleAsset(asset);
}
static List<Asset> GetAllAssets(AppDbContext db)
{
    List<Asset> allAssets = new List<Asset>();

    allAssets.AddRange(db.ComputerAssets.ToList());
    allAssets.AddRange(db.MobileAssets.ToList());

    return allAssets;
}
