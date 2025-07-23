using Microsoft.EntityFrameworkCore;
using SurveyBackend.Api.Interfaces;
using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;
using SurveyBackend.Domain.Entities.Dto;
using SurveyBanckend.Infrastructure;

namespace SurveyBackend.Api.Services;

public class VendorService : IVendorService
{
    private readonly DataContext _context;
    private readonly Vendor vendor;
    
    public VendorService(DataContext context)
    {
        _context = context;
    }

    public async Task<Vendor> CreateVendorAsync(VendorCreateDTO dto)
    {
        var vendor = new Vendor
        {
            name = dto.name,
            product_service = dto.product_service
        };

        _context.vendors.Add(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }
    public async Task<object> GetVendorSurveyDetailsAsync(int vendorId)
    {
        var surveys = await _context.surveys
            .Where(s => s.vendor_id == vendorId)
            .Include(s => s.user)
            .ToListAsync();

        var completed = surveys
            .Where(s => s.status == "completed")
            .Select(s => new
            {
                raterName = s.user!.full_name,
                submittedAt = s.submitted_at
            }).ToList();

        var pending = surveys
            .Where(s => s.status == "pending")
            .Select(s => new
            {
                raterName = s.user!.full_name
            }).ToList();

        return new
        {
            completed,
            pending
        };
    }

    public async Task<List<Vendor>> GetVendorsAsync()
    {
        return await _context.vendors.ToListAsync();
    }

    public async Task<Vendor?> GetVendorByIdAsync(int id)
    {
        return await _context.vendors.FindAsync(id);
    }

    public async Task<Vendor?> UpdateVendorAsync(int id, VendorCreateDTO dto)
    {
        var vendor = await _context.vendors.FindAsync(id);
        if (vendor == null) return null;

        // ✅ Check if another vendor already has the same name
        var existingVendor = await _context.vendors
            .Where(v => v.vendor_id != id && v.name.ToLower() == dto.name.ToLower())
            .FirstOrDefaultAsync();

        if (existingVendor != null)
        {
            // Throw an exception or return a special signal
            throw new InvalidOperationException("Another vendor with the same name already exists.");
        }

        // Proceed with update
        vendor.name = dto.name;
        vendor.product_service = dto.product_service;

        await _context.SaveChangesAsync();
        return vendor;
    }

    
    public async Task<bool> DeleteVendorAsync(int id)
    {
        var vendor = await _context.vendors.FindAsync(id);
        if (vendor == null) return false;

        _context.vendors.Remove(vendor);
        await _context.SaveChangesAsync();
        return true;
    }
}