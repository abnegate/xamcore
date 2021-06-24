using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamCore.Models;
using NETAPI.Models;
using NETAPI.Services;

namespace XamCore.Services
{
    public abstract class DataService<TModel, TCollection, TResponse>
        : IDataService<TModel, TCollection, TResponse> where TResponse : new()
    {
        protected ApiEndpoint _endpoint;

        protected readonly IAnalyticsService _analyticsService;
        protected readonly IDatabaseService _databaseService;
        protected readonly IApiService<ApiEnvironment, ApiEndpoint> _apiService;
        protected readonly IDialogService _dialogService;

        public DataService(
            ApiEndpoint endpoint,
            IAnalyticsService analyticsService,
            IApiService<ApiEnvironment, ApiEndpoint> apiService,
            IDatabaseService databaseService,
            IDialogService dialogService

            )
        {
            _endpoint = endpoint;
            _analyticsService = analyticsService;
            _dialogService = dialogService;
            _databaseService = databaseService;
            _apiService = apiService;
        }

        public virtual async Task<TCollection> GetDataAsync()
        {
            if (_apiService.IsNetworkAvailable) {
                var items = await OnNetworkFetch();
                if (items?.Data is not null) {
                    items.Data = await OnDbPreInsertMap(items.Data);
                    await _databaseService.DeleteAllAsync<TResponse>();
                    await OnDbInsert(items.Data);
                    return OnMapOutput(items.Data);
                }
            }
            if (await _databaseService.DoesTableExistAsync<TModel>()) {
                var items = await OnDbFetch();
                if (items is not null) {
                    return OnMapOutput(items);
                }
            }
            return OnMapOutput(Enumerable.Empty<TResponse>());
        }

        public virtual async Task<ResponseBase<IEnumerable<TResponse>?>?> OnNetworkFetch()
        {
            var response = await _apiService
                .TryGet<IEnumerable<TResponse>>(_endpoint);

            if (response?.Exception is not null
                && response.Exception is TimeoutException) {
                await _dialogService.ShowTimeoutDialog();
            }
            return response;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<IEnumerable<TResponse>> OnDbPreInsertMap(IEnumerable<TResponse> data) => data;
#pragma warning restore CS1998

        public virtual Task OnDbInsert(IEnumerable<TResponse> data) =>
            _databaseService.InsertAllAsync(data);

        public virtual Task<IEnumerable<TResponse>?> OnDbFetch() =>
            _databaseService.GetItemsAsync<TResponse>();

        public abstract TCollection OnMapOutput(IEnumerable<TResponse> data);
    }

    public abstract class DataService<TModel>
        : DataService<TModel, ObservableCollection<TModel>, TModel>, IDataService<TModel> where TModel : new()
    {
        public DataService(
            ApiEndpoint endpoint,
            IAnalyticsService analyticsService,
            IApiService<ApiEnvironment, ApiEndpoint> apiService,
            IDatabaseService databaseService,
            IDialogService dialogService) : base(
                endpoint,
                analyticsService,
                apiService,
                databaseService,
                dialogService)
        {
        }
    }

    public abstract class GroupedDataService<TModel>
        : DataService<TModel, ObservableCollection<StringKeyGroup<TModel>>, TModel> where TModel : new()
    {
        public GroupedDataService(
            ApiEndpoint endpoint,
            IAnalyticsService analyticsService,
            IApiService<ApiEnvironment, ApiEndpoint> apiService,
            IDatabaseService databaseService,
            IDialogService dialogService) : base(
                endpoint,
                analyticsService,
                apiService,
                databaseService,
                dialogService)
        {

        }
    }
}
