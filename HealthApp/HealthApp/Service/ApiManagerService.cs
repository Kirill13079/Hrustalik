using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Interfaces;
using HealthApp.Models;
using HealthApp.ViewModels.Data;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthApp.Service
{
    public class ApiManagerService : IApiManager
    {
        public ApiManagerService()
        {
            Barrel.ApplicationId = "CachingData";
        }

        public async Task<Customer> GetCustomerAsync()
        {
            try
            {
                string token = Settings.GetSetting(prefrence: Settings.AppPrefrences.token);
                
                if (!string.IsNullOrWhiteSpace(token))
                {
                    string url = ApiRoutes.BaseUrl + ApiRoutes.GetCustomer;

                    string response = await ApiCallerService.Get(url);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        Customer customer = JsonConvert.DeserializeObject<Customer>(response);

                        return customer;
                    }
                    else
                    {
                        Settings.RemoveSetting(prefrence: Settings.AppPrefrences.token);
                    }
                }
            }
            catch
            {
            
            }

            return null;
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

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<Bookmark> bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(result);

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

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(result);

                    Barrel.Current.Add(key: url, data: categories, expireIn: TimeSpan.FromDays(1));

                    return categories;
                }
            }
            catch
            {

            }

            return null;
        }

        public async Task<List<RecordViewModel>> GetCategoryRecordsAsync(int categoryId)
        {
            try
            {
                string url = $"{ApiRoutes.BaseUrl}{ApiRoutes.GetCategoryRecords}?id={categoryId}";

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordViewModel>>(key: url);
                }

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<RecordViewModel> records = JsonConvert.DeserializeObject<List<RecordViewModel>>(result);

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

        public async Task<RecordViewModel> GetHotRecordAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetHotRecord;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<RecordViewModel>(key: url);
                }

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    RecordViewModel record = JsonConvert.DeserializeObject<RecordViewModel>(result);

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

        public async Task<List<RecordViewModel>> GetPopularsRecordsAsync(int skipRecords, int takeRecord)
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetPopularRecords;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordViewModel>>(key: url);
                }

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<RecordViewModel> records = JsonConvert.DeserializeObject<List<RecordViewModel>>(result);

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

        public async Task<List<RecordViewModel>> GetRecordsAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetRecords;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<RecordViewModel>>(key: url);
                }

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<RecordViewModel> records = JsonConvert.DeserializeObject<List<RecordViewModel>>(result);

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

        public async Task<List<Author>> GetAuthorsAsync()
        {
            try
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetAuthors;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet
                    && !Barrel.Current.IsExpired(key: url))
                {
                    return Barrel.Current.Get<List<Author>>(key: url);
                }

                string result = await ApiCallerService.Get(url);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(result);

                    authors.ForEach((author) =>
                    {
                        author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{author.Logo}";
                    });

                    Barrel.Current.Add(key: url, data: authors, expireIn: TimeSpan.FromDays(1));

                    return authors;
                }
            }
            catch
            {

            }

            return null;
        }

        public async Task<GoogleResponseModel> GetInfoGoogleUserAsync(string authToken)
        {
            string response = await ApiCallerService.GetTest(ApiRoutes.GoogleAuth + authToken);

            if (!string.IsNullOrWhiteSpace(response))
            {
                GoogleResponseModel googleUserJson = JsonConvert.DeserializeObject<GoogleResponseModel>(response);

                return googleUserJson;
            }

            return null;
        }
    }
}
