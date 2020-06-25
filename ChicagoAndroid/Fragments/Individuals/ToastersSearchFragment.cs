using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class ToastersSearchFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static ToastersSearchFragment instance;
        public SearchParameters param = new SearchParameters();

        #endregion

        #region Properties

        /// Gets or sets the search view
        /// </summary>
        public Android.Support.V7.Widget.SearchView SearchView { get; set; }

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager ListLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        public RecyclerView ToastersSearchRecycler { get; set; }

        private ICollection<ToastersSearchItem> Toasters { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private ToastersSearchAdapter ToastersSearchAdapter { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        public int TotalResultCount { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchFragment(Activities.Individuals.IndividualHomeActivity context)
        {
            this.HomeContext = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create instance of this fragment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static V4Fragment NewInstance(Activities.Individuals.IndividualHomeActivity context)
        {
            instance = new ToastersSearchFragment(context);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ToastersSearch, container, false);
            this.ToastersSearchRecycler = view.FindViewById<RecyclerView>(Resource.Id.toastersSearchRecycler);
            this.HomeContext.SupportActionBar.Title = "Search Toasters";

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //try
            //{
            inflater.Inflate(Resource.Menu.my_search_menu, menu);
            var searchItem = menu.FindItem(Resource.Id.action_search);

            this.SearchView = new Android.Support.V7.Widget.SearchView(this.HomeContext);
            searchItem.SetActionView(this.SearchView);
            SearchView.SetIconifiedByDefault(false);
            SearchView.Iconified = false;
            //SearchView.SetFocusable(ViewFocusability.FocusableAuto);
            SearchManager searchManager = (SearchManager)this.HomeContext.GetSystemService(Context.SearchService);
            this.SearchView.SetSearchableInfo(searchManager.GetSearchableInfo(this.HomeContext.ComponentName));
            this.SearchView.QueryHint = AppText.searchToastPlaceHolder;

            this.SearchView.QueryTextSubmit += async (sender, args) =>
            {
                await Search(args.NewText);
            };

            //}
            //catch (Exception) { }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <summary>
        /// Initialize recycler view
        /// </summary>
        private void InitRecyclerView(List<ToastersSearchItem> data)
        {
            this.ToastersSearchAdapter = new ToastersSearchAdapter(this, data, this.ImageViewImages);
            this.ListLayoutManager = new LinearLayoutManager(this.HomeContext);
            this.ToastersSearchRecycler.SetItemAnimator(new DefaultItemAnimator());
            this.ToastersSearchRecycler.HasFixedSize = true;
            this.ToastersSearchRecycler.SetLayoutManager(this.ListLayoutManager);
            this.ToastersSearchRecycler.AddOnScrollListener(new Listeners.Individuals.ToastersSearchScrollListener(this.HomeContext, this.ToastersSearchAdapter, Listeners.Individuals.ToastersSearchScrollListener.Caller.ToasterSearch));
            this.ToastersSearchRecycler.SetAdapter(this.ToastersSearchAdapter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 15;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
        }

        public async Task GetPicUris()
        {
            try
            {
                foreach (var b in this.Toasters)
                {
                    ImageViewImage itemLogo = new ImageViewImage();
                    itemLogo.Id = b.UserId;

                    var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        itemLogo.ImageUrl = imageUri;
                        this.ImageViewImages.Add(itemLogo);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task Search(string searchTerm)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    //this.HomeContext.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NoInternet, "OK");
                    Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }
                else
                {
                    var imm = (InputMethodManager)this.HomeContext.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(SearchView.WindowToken, HideSoftInputFlags.NotAlways);
                    InitSearchParameters(searchTerm);
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Searching);
                    Toasters = await App.IndividualFactory.ToasterSearch(param);

                    if(Toasters != null && Toasters.Count > 0)
                    {
                        var toasters = Toasters;
                        Toasters = new List<ToastersSearchItem>();
                        var item = toasters.Where(x => x.UserId == this.HomeContext.CurrentUser.UserId).FirstOrDefault();
                        if (item != null)
                        {
                            toasters.Remove(item);
                        }
                        Toasters = toasters;
                        await GetPicUris();
                        if (ToastersSearchAdapter == null)
                        {
                            InitRecyclerView(Toasters.ToList());
                        }
                        else
                        {
                            this.HomeContext.RunOnUiThread(() =>
                            {
                                this.ToastersSearchAdapter.Rows = Toasters.ToList();
                                this.ToastersSearchAdapter.ImageViewImages = this.ImageViewImages;
                                this.ToastersSearchAdapter.NotifyDataSetChanged();
                            });
                        }
                        this.ToastersSearchAdapter.LoadMore = true;
                    }
                    else
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.NullResult, ToastLength.Short).Show();
                        //this.HomeContext.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NullResult, "OK");
                    }
                }
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
            }
            catch (Exception)
            {
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
                Toast.MakeText(this.HomeContext, ToastMessage.ServerError, ToastLength.Short).Show();
                //this.HomeContext.ShowSnack(this.ToastersSearchRecycler, ToastMessage.ServerError, "OK");
            }
        }

        #endregion

    }
}