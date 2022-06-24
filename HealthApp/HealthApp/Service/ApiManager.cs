using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Interfaces;
using HealthApp.Models;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthApp.Service
{
    public class ApiManager : IApiManager
    {
        public ApiManager()
        {
            Barrel.ApplicationId = "CachingData";
        }

        public async Task<List<Bookmark>> GetBookmarksAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetBookmarks;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet 
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<Bookmark>>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(result);

                    bookmarks.ForEach((bookmark) =>
                    {
                        bookmark.Record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{bookmark.Record.Image}";
                        bookmark.Record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{bookmark.Record.Author.Logo}";
                    });

                    Barrel.Current.Add(key: url, data: bookmarks, expireIn: TimeSpan.FromDays(1));

                    return bookmarks;
                }
            }
            catch 
            {
            
            }

            return null;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetCategories;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<Category>>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var categories = JsonConvert.DeserializeObject<List<Category>>(result);

                    Barrel.Current.Add(key: url, data: categories, expireIn: TimeSpan.FromDays(1));

                    return categories;
                }
            }
            catch 
            {

            }

            return null;
        }

        public async Task<List<RecordModel>> GetCategoryRecordsAsync(int categoryId)
        {
            try
            {
                string url = $"{ApiRoutes.BaseUrl}{ApiRoutes.GetCategoryRecords}?id={categoryId}";

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordModel>>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var records = JsonConvert.DeserializeObject<List<RecordModel>>(result);

                    records.ForEach((record) =>
                    {
                        record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                        record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                    });

                    Barrel.Current.Add(key: url, data: records, expireIn: TimeSpan.FromDays(1));

                    return records;
                }
            }
            catch 
            {
            
            }

            return null;
        }

        public async Task<RecordModel> GetHotRecordAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetHotRecord;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<RecordModel>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var record = JsonConvert.DeserializeObject<RecordModel>(result);

                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";

                    Barrel.Current.Add(key: url, data: record, expireIn: TimeSpan.FromDays(1));

                    return record;
                }
            }
            catch 
            {
            
            }

            return null;
        }

        public async Task<List<RecordModel>> GetPopularsRecordsAsync(int skipRecords, int takeRecord)
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetPopularRecords;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordModel>>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var records = JsonConvert.DeserializeObject<List<RecordModel>>(result);

                    records.ForEach((record) =>
                    {
                        record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                        record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                    });

                    records = records.Skip(skipRecords).Take(takeRecord).ToList();

                    Barrel.Current.Add(key: url, data: records, expireIn: TimeSpan.FromDays(1));

                    return records;
                }
            }
            catch 
            {
            
            }

            return null;
        }

        public async Task<List<RecordModel>> GetRecordsAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetRecords;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordModel>>(key: url);
                }

                var result = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var records = JsonConvert.DeserializeObject<List<RecordModel>>(result);

                    records.ForEach((record) =>
                    {
                        record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                        record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                    });

                    Barrel.Current.Add(key: url, data: records, expireIn: TimeSpan.FromDays(1));

                    return records.ToList();
                }
            }
            catch 
            {
            
            }

            return null;
        }
    }
}
