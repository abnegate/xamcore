using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XamCore.Services
{
    public interface IDataService<TModel, TCollection, TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task OnDbInsert(IEnumerable<TResponse> data);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TResponse>?> OnDbFetch();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TCollection> GetDataAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TCollection OnMapOutput(IEnumerable<TResponse> data);
    }

    public interface IDataService<TModel> : IDataService<TModel, ObservableCollection<TModel>, TModel>
    {
    }
}
