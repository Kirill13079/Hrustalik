﻿namespace HealthApp.Common.Model.Helper
{
    public static class ApiRoutes
    {
        public const string BaseUrl = "https://hrustalik.by/";

        public const string MobileAuth = "mobileauth";
        public const string GoogleAuth = "https://www.googleapis.com/oauth2/v3/tokeninfo?access_token=";

        public const string Register = "api/user/register";
        public const string Login = "api/user/login";
        public const string AddBookmark = "api/user/bookmark/add";
        public const string DeleteBookmark = "api/user/bookmark/delete";

        public const string GetBookmarks = "api/user/bookmark";
        public const string GetCustomer = "api/user/customer";

        public const string GetRecords = "api/record/news";
        public const string GetArticleRecords = "api/record/article";
        public const string GetHotRecord = "api/record/hot";
        public const string GetPopularRecords = "api/record/popular";
        public const string GetYoutubeRecords = "api/record/youtube";
        public const string GetCategoryRecords = "api/record/category";
        public const string GetAuthorRecords = "api/record/author";

        public const string GetCategories = "api/category/list";

        public const string GetAuthors = "api/author/list";
    }
}
