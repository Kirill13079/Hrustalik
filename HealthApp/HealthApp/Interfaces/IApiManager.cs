using HealthApp.Common.Model;
using HealthApp.Models;
using HealthApp.ViewModels.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Interfaces
{
    public interface IApiManager
    {
        Task<List<RecordViewModel>> GetCategoryRecordsAsync(int categoryId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<RecordViewModel>> GetRecordsAsync();

        Task<RecordViewModel> GetHotRecordAsync();

        Task<List<RecordViewModel>> GetPopularsRecordsAsync(int skipRecords, int takeRecord);

        Task<List<Bookmark>> GetBookmarksAsync();

        Task<Customer> GetCustomerAsync();

        Task<List<Author>> GetAuthorsAsync();

        Task<GoogleResponseModel> GetInfoGoogleUserAsync(string authToken);
    }
}
