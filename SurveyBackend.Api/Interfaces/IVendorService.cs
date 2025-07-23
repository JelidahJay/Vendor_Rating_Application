using SurveyBackend.Domain.Entities;
using SurveyBackend.Domain.Entities.CreateDto;

namespace SurveyBackend.Api.Interfaces;

public interface IVendorService
{
    Task<Vendor> CreateVendorAsync(VendorCreateDTO dto);
    Task<List<Vendor>> GetVendorsAsync();
    Task<Vendor?> GetVendorByIdAsync(int id);
    Task<Vendor?> UpdateVendorAsync(int id, VendorCreateDTO dto);
    Task<bool> DeleteVendorAsync(int id);
    
    Task<object> GetVendorSurveyDetailsAsync(int vendorId);

}