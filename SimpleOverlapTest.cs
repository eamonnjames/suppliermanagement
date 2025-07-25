using System;
using System.Collections.Generic;
using System.Linq;

// Simple test to verify the overlap logic works correctly
// This uses the same logic as implemented in SupplierService
public class SupplierRate
{
    public int SupplierRateId { get; set; }
    public int SupplierId { get; set; }
    public decimal Rate { get; set; }
    public DateTime RateStartDate { get; set; }
    public DateTime? RateEndDate { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
}

class TestOverlapLogic
{
    static void Main(string[] args)
    {
        Console.WriteLine("Testing Overlap Logic for Supplier Management");
        Console.WriteLine("============================================");

        // Test the same data structure as in our seeded data
        TestSupplierOverlaps();
    }

    static void TestSupplierOverlaps()
    {
        Console.WriteLine("\n=== Testing Supplier 3 (Should have overlaps) ===");
        var supplier3Rates = new List<SupplierRate>
        {
            new SupplierRate { SupplierRateId = 6, SupplierId = 3, Rate = 30, RateStartDate = new DateTime(2016, 12, 1), RateEndDate = new DateTime(2017, 1, 1) },
            new SupplierRate { SupplierRateId = 7, SupplierId = 3, Rate = 30, RateStartDate = new DateTime(2017, 1, 2), RateEndDate = null },
            new SupplierRate { SupplierRateId = 8, SupplierId = 3, Rate = 35, RateStartDate = new DateTime(2016, 12, 15), RateEndDate = new DateTime(2017, 1, 15) }
        };

        var overlapping3 = FindOverlappingRates(supplier3Rates);
        Console.WriteLine($"Supplier 3 - Found {overlapping3.Count} overlapping rates:");
        foreach (var rate in overlapping3)
        {
            Console.WriteLine($"  Rate ID: {rate.SupplierRateId}, Rate: {rate.Rate:C}, Start: {rate.RateStartDate:d}, End: {rate.RateEndDate?.ToString("d") ?? "Open-ended"}");
        }

        Console.WriteLine("\n=== Testing Supplier 4 (Should have overlaps) ===");
        var supplier4Rates = new List<SupplierRate>
        {
            new SupplierRate { SupplierRateId = 9, SupplierId = 4, Rate = 50, RateStartDate = new DateTime(2020, 1, 1), RateEndDate = new DateTime(2020, 6, 30) },
            new SupplierRate { SupplierRateId = 10, SupplierId = 4, Rate = 60, RateStartDate = new DateTime(2020, 3, 1), RateEndDate = new DateTime(2020, 9, 30) },
            new SupplierRate { SupplierRateId = 11, SupplierId = 4, Rate = 55, RateStartDate = new DateTime(2020, 8, 1), RateEndDate = null }
        };

        var overlapping4 = FindOverlappingRates(supplier4Rates);
        Console.WriteLine($"Supplier 4 - Found {overlapping4.Count} overlapping rates:");
        foreach (var rate in overlapping4)
        {
            Console.WriteLine($"  Rate ID: {rate.SupplierRateId}, Rate: {rate.Rate:C}, Start: {rate.RateStartDate:d}, End: {rate.RateEndDate?.ToString("d") ?? "Open-ended"}");
        }

        Console.WriteLine("\n=== Testing Supplier 1 (Should have NO overlaps) ===");
        var supplier1Rates = new List<SupplierRate>
        {
            new SupplierRate { SupplierRateId = 1, SupplierId = 1, Rate = 10, RateStartDate = new DateTime(2015, 1, 1), RateEndDate = new DateTime(2015, 3, 31) },
            new SupplierRate { SupplierRateId = 2, SupplierId = 1, Rate = 20, RateStartDate = new DateTime(2015, 4, 1), RateEndDate = new DateTime(2015, 5, 1) },
            new SupplierRate { SupplierRateId = 3, SupplierId = 1, Rate = 10, RateStartDate = new DateTime(2015, 5, 30), RateEndDate = new DateTime(2015, 7, 25) },
            new SupplierRate { SupplierRateId = 4, SupplierId = 1, Rate = 25, RateStartDate = new DateTime(2015, 10, 1), RateEndDate = null }
        };

        var overlapping1 = FindOverlappingRates(supplier1Rates);
        Console.WriteLine($"Supplier 1 - Found {overlapping1.Count} overlapping rates:");
        foreach (var rate in overlapping1)
        {
            Console.WriteLine($"  Rate ID: {rate.SupplierRateId}, Rate: {rate.Rate:C}, Start: {rate.RateStartDate:d}, End: {rate.RateEndDate?.ToString("d") ?? "Open-ended"}");
        }

        Console.WriteLine("\n=== Analysis ===");
        Console.WriteLine("Expected results:");
        Console.WriteLine("- Supplier 3: Rates 6 and 8 should overlap (Dec 1-Jan 1 overlaps with Dec 15-Jan 15)");
        Console.WriteLine("- Supplier 4: Multiple overlaps expected (Jan-Jun overlaps with Mar-Sep, Mar-Sep overlaps with Aug-open)");
        Console.WriteLine("- Supplier 1: No overlaps expected (all rates have gaps or are contiguous)");
    }

    private static List<SupplierRate> FindOverlappingRates(List<SupplierRate> rates)
    {
        var overlappingRates = new List<SupplierRate>();

        for (int i = 0; i < rates.Count; i++)
        {
            for (int j = i + 1; j < rates.Count; j++)
            {
                var rate1 = rates[i];
                var rate2 = rates[j];

                if (DoRatesOverlap(rate1, rate2))
                {
                    if (!overlappingRates.Contains(rate1))
                        overlappingRates.Add(rate1);

                    if (!overlappingRates.Contains(rate2))
                        overlappingRates.Add(rate2);
                }
            }
        }

        return overlappingRates;
    }

    private static bool DoRatesOverlap(SupplierRate rate1, SupplierRate rate2)
    {
        // Get effective end dates (null means open-ended, use MaxValue)
        var rate1EndDate = rate1.RateEndDate ?? DateTime.MaxValue;
        var rate2EndDate = rate2.RateEndDate ?? DateTime.MaxValue;

        // Two date ranges overlap if:
        // rate1 starts before rate2 ends AND rate2 starts before rate1 ends
        return rate1.RateStartDate <= rate2EndDate && rate2.RateStartDate <= rate1EndDate;
    }
}
