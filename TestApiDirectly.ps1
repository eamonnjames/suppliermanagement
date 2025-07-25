# PowerShell script to test the API directly
$apiBase = "http://localhost:5204"

Write-Host "Testing Supplier Management API..." -ForegroundColor Green

# Step 1: Get authentication token
Write-Host "1. Getting authentication token..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "password123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiBase/api/auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "Successfully obtained token" -ForegroundColor Green
    Write-Host "Token: $($token.Substring(0,50))..." -ForegroundColor Gray
} catch {
    Write-Host "Failed to get token: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 2: Test all suppliers with rates
Write-Host "`n2. Testing 'Get All Suppliers with Rates'..." -ForegroundColor Yellow
try {
    $headers = @{ Authorization = "Bearer $token" }
    $allSuppliers = Invoke-RestMethod -Uri "$apiBase/api/suppliers/all-with-rates" -Method Get -Headers $headers
    Write-Host "Successfully retrieved $($allSuppliers.Count) suppliers" -ForegroundColor Green

    foreach ($supplier in $allSuppliers) {
        Write-Host "  - Supplier $($supplier.supplierId): $($supplier.name) ($($supplier.rates.Count) rates)" -ForegroundColor Gray
    }
} catch {
    Write-Host "Failed to get suppliers: $($_.Exception.Message)" -ForegroundColor Red
}

# Step 3: Test overlapping rates (all suppliers)
Write-Host "`n3. Testing 'Get Overlapping Rates' (all suppliers)..." -ForegroundColor Yellow
try {
    $overlappingAll = Invoke-RestMethod -Uri "$apiBase/api/suppliers/overlapping-rates" -Method Get -Headers $headers
    Write-Host "Successfully retrieved overlapping rates" -ForegroundColor Green
    Write-Host "  Count: $($overlappingAll.Count)" -ForegroundColor Gray

    if ($overlappingAll.Count -eq 0) {
        Write-Host "  WARNING: No overlapping rates found (this might be the issue!)" -ForegroundColor Yellow
    } else {
        foreach ($overlap in $overlappingAll) {
            Write-Host "  - Supplier $($overlap.supplierId): $($overlap.supplierName) ($($overlap.overlappingRates.Count) overlapping rates)" -ForegroundColor Gray
        }
    }
} catch {
    Write-Host "Failed to get overlapping rates: $($_.Exception.Message)" -ForegroundColor Red
}

# Step 4: Test overlapping rates (supplier 3 specifically)
Write-Host "`n4. Testing 'Get Overlapping Rates' (supplier 3)..." -ForegroundColor Yellow
try {
    $overlappingSupplier3 = Invoke-RestMethod -Uri "$apiBase/api/suppliers/overlapping-rates?supplierId=3" -Method Get -Headers $headers
    Write-Host "Successfully retrieved overlapping rates for supplier 3" -ForegroundColor Green
    Write-Host "  Count: $($overlappingSupplier3.Count)" -ForegroundColor Gray

    if ($overlappingSupplier3.Count -eq 0) {
        Write-Host "  WARNING: No overlapping rates found for supplier 3 (this is definitely an issue!)" -ForegroundColor Yellow
    } else {
        foreach ($overlap in $overlappingSupplier3) {
            Write-Host "  - Supplier $($overlap.supplierId): $($overlap.supplierName)" -ForegroundColor Gray
            Write-Host "    Reason: $($overlap.overlapReason)" -ForegroundColor Gray
            Write-Host "    Overlapping rates:" -ForegroundColor Gray
            foreach ($rate in $overlap.overlappingRates) {
                $endDate = if ($rate.rateEndDate) { $rate.rateEndDate } else { "Open-ended" }
                Write-Host "      Rate ID $($rate.supplierRateId): $($rate.rate) from $($rate.rateStartDate) to $endDate" -ForegroundColor Gray
            }
        }
    }
} catch {
    Write-Host "Failed to get overlapping rates for supplier 3: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nTest completed!" -ForegroundColor Green
