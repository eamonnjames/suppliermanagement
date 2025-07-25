
# Supplier Overlap Detection Testing

## Testing the Overlap Detection

### Expected Overlapping Results

Based on the seed data, the overlap detection should identify:

**Supplier 3 (Premium Ltd)** has overlapping rates:

- Rate 1: 2016-12-01 to 2017-01-01 (ends on Jan 1st)
- Rate 2: 2017-01-02 to null (starts on Jan 2nd)

These rates are considered overlapping because:

1. They are consecutive/contiguous (only 1 day gap)
2. Rate 2 has no end date (continues indefinitely)

**Supplier 1 (BestValue)** should NOT show overlaps because:

- Rate 1: 2015-01-01 to 2015-03-31
- Rate 2: 2015-04-01 to 2015-05-01
- Rate 3: 2015-05-30 to 2015-07-25
- Rate 4: 2015-10-01 to null

These have gaps between them, so no overlaps.

**Supplier 2 (Quality Corp)** has only one rate, so no overlaps possible.

### Quick Manual Test

1. Start the API
2. Get a JWT token from `/api/auth/login`
3. Call `/api/suppliers/overlapping-rates`
4. Verify only Supplier 3 appears in results
5. Call `/api/suppliers/overlapping-rates?supplierId=3`
6. Verify same result for Supplier 3 only
7. Call `/api/suppliers/overlapping-rates?supplierId=1`
8. Verify empty array (no overlaps for Supplier 1)

### Test with cURL

```bash
# 1. Get JWT Token
curl -X POST "https://localhost:7xxx/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password123"}'

# 2. Test overlapping rates (replace YOUR_JWT_TOKEN)
curl -X GET "https://localhost:7xxx/api/suppliers/overlapping-rates" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# 3. Test specific supplier
curl -X GET "https://localhost:7xxx/api/suppliers/overlapping-rates?supplierId=3" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```
