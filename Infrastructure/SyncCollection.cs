using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.Call.Infrastructure
{
    /// <summary>
    /// An observable collection which automatically syncs to the underlying models collection
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public class SyncCollection<TViewModel, TModel> : ObservableCollection<TViewModel>
    {
        IList<TModel> modelCollection;
        Func<TViewModel, TModel> modelExtractorFunc;

        /// <summary>
        /// Creates a new instance of SyncCollection
        /// </summary>
        /// <param name="modelCollection">The list of Models to sync to</param>
        /// <param name="viewModelCreatorFunc">Creates a new ViewModel instance for the given Model</param>
        /// <param name="modelExtractorFunc">Returns the model which is wrapped by the given ViewModel</param>
        public SyncCollection(IList<TModel> modelCollection, Func<TModel, TViewModel> viewModelCreatorFunc, Func<TViewModel, TModel> modelExtractorFunc)
        {
            if (modelCollection == null)
                throw new ArgumentNullException("modelCollection");
            if (viewModelCreatorFunc == null)
                throw new ArgumentNullException("vmCreatorFunc");
            if (modelExtractorFunc == null)
                throw new ArgumentNullException("modelExtractorFunc");

            this.modelCollection = modelCollection;
            this.modelExtractorFunc = modelExtractorFunc;

            // create ViewModels for all Model items in the modelCollection
            foreach (var model in modelCollection)
                Add(viewModelCreatorFunc(model));

            CollectionChanged += SyncCollection_CollectionChanged;
        }

        private void SyncCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // update the modelCollection accordingly

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                        modelCollection.Insert(i + e.NewStartingIndex, modelExtractorFunc((TViewModel)e.NewItems[i]));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // NOTE: currently this ignores the index (works when there are no duplicates in the list)
                    foreach (var vm in e.OldItems.OfType<TViewModel>())
                        modelCollection.Remove(modelExtractorFunc(vm));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Reset:
                    modelCollection.Clear();
                    foreach (var viewModel in this)
                        modelCollection.Add(modelExtractorFunc(viewModel));
                    break;
            }
        }
    }
}
