using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Blocked Toasters", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BlockedToastersActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView blockedToastersList;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private ICollection<Toasters> Toasters { get; set; }

        private BlockedToastersAdapter BlockedToastersAdapter { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.BlockedToasters);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                blockedToastersList = FindViewById<ListView>(Resource.Id.blockedToastersList);
                await RetriveToasters();
            }
            catch (Exception)
            {
            }
        }

        private void InitListview(List<Toasters> data)
        {
            try
            {
                BlockedToastersAdapter = new BlockedToastersAdapter(this, data);
                blockedToastersList.Adapter = BlockedToastersAdapter;
                blockedToastersList.DividerHeight = 2;
            }catch(Exception ex)
            {
                var a = ex;
            }
        }

        public async void UnblockUser(Toasters item)
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(blockedToastersList, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (item == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.UnBlockToaster(item.ToastersId);
                    ToastersActivity.RequiresRefresh = true;
                    this.Finish();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <Param name="item"></Param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RetriveToasters()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.blockedToastersList, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    //InitSearchParameters("");
                    Toasters = await App.ToastersFactory.GetBlockedToasters(this.CurrentUser.UserId);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        if (BlockedToastersAdapter == null)
                        {
                            InitListview(Toasters.ToList());
                        }
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                this.BlockedToastersAdapter.Rows = Toasters.ToList();
                                this.BlockedToastersAdapter.NotifyDataSetChanged();
                            });
                        }
                        //this.BlockedToastersAdapter.LoadMore = true;
                    }
                }
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Loading);
                this.ShowSnack(this.blockedToastersList, ToastMessage.ServerError, "OK");
            }
        }

        #endregion

    }
}