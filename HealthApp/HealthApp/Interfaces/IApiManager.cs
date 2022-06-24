using HealthApp.Common.Model;
using HealthApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Interfaces
{
    public interface IApiManager
    {
        Task<List<RecordModel>> GetCategoryRecordsAsync(int categoryId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<RecordModel>> GetRecordsAsync();

        Task<RecordModel> GetHotRecordAsync();

        Task<List<RecordModel>> GetPopularsRecordsAsync(int skipRecords, int takeRecord);

        Task<List<Bookmark>> GetBookmarksAsync();
    }
}
