using System;
using System.Collections.Generic;
using System.Linq;
using SupplierManagement.Core.Entities;

// Simple test to verify the overlap logic works correctly
class TestOverlapLogic
{
    static void Main(string[] args)
    {
        Console.WriteLine("Testing Overlap Logic");
        Console.WriteLine("====================");

        // Test the overlap logic with sample data
        var testRates = new List<SupplierRate>
        {
            // Test case 1: Clear overlap
            new SupplierRate { SupplierRateId = 1, SupplierId = 1, Rate = 10, RateStartDate = new DateTime(2020, 1, 1), RateEndDate = new DateTime(2020, 6, 30) },
            new SupplierRate { SupplierRateId = 2, SupplierId = 1, Rate = 20, RateStartDate = new DateTime(2020, 3, 1), RateEndDate = new DateTime(2020, 9, 30) },

            // Test case 2: No overlap (gap between)
            new SupplierRate { SupplierRateId = 3, SupplierId = 1, Rate = 30, RateStartDate = new DateTime(2021, 1, 1), RateEndDate = new DateTime(2021, 3, 31) },
            new SupplierRate { SupplierRateId = 4, SupplierId = 1, Rate = 40, RateStartDate = new DateTime(2021, 5, 1), RateEndDate = new DateTime(2021, 7, 31) },

            // Test case 3: Open-ended overlap
            new SupplierRate { SupplierRateId = 5, SupplierId = 1, Rate = 50, RateStartDate = new DateTime(2022, 1, 1), RateEndDate = null },
            new SupplierRate { SupplierRateId = 6, SupplierId = 1, Rate = 60, RateStartDate = new DateTime(2022, 6, 1), RateEndDate = new DateTime(2022, 12, 31) }
        };

        var overlapping = FindOverlappingRates(testRates);

        Console.WriteLine($"Found {overlapping.Count} overlapping rates:");
        foreach (var rate in overlapping)
        {
            Console.WriteLine($"Rate ID: {rate.SupplierRateId}, Rate: {rate.Rate:C}, Start: {rate.RateStartDate:d}, End: {rate.RateEndDate?.ToString("d") ?? "Open-ended"}");
        }

        Console.WriteLine();
        Console.WriteLine("Expected overlaps:");
        Console.WriteLine("- Rates 1 and 2 (Jan-Jun 2020 overlaps with Mar-Sep 2020)");
        Console.WriteLine("- Rates 5 and 6 (Open-ended from Jan 2022 overlaps with Jun-Dec 2022)");
        Console.WriteLine("- Rates 3 and 4 should NOT overlap (gap between Mar and May 2021)");
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
