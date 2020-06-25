using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Points;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Adapters.Points;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Points
{
    [Activity(Label = "Points", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ToasterPointsActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        public bool redeemedPointsShown;
        private ListView pointsList;
        private View headerView;
        private IMenuItem myMenu;
        private TextView userPoints;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private ToasterPointsAdapter ToasterPointsAdapter { get; set; }

        #endregion

        #region Methods

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.ToasterPoint);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                pointsList = FindViewById<ListView>(Resource.Id.toasterPointsList);
                await GetEarnedPoints();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);
            myMenu = menu.FindItem(Resource.Id.menuAction);
            myMenu.SetTitle(AppText.RedeemedPoints);

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuAction:
                    ResetTable();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void ResetTable()
        {
            try
            {
                if (!redeemedPointsShown || (myMenu != null && myMenu.TitleFormatted.ToString() == AppText.RedeemedPoints))
                {
                    await GetRedeemedPoints();
                }
                else
                {
                    await GetEarnedPoints();
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<Point> data)
        {
            if (headerView != null)
            {
                pointsList.RemoveHeaderView(headerView);
            }
            headerView = LayoutInflater.FromContext(this).Inflate(Resource.Layout.ToasterPointHeader, null);
            userPoints = headerView.FindViewById<TextView>(Resource.Id.myPointsLabel);
            pointsList.AddHeaderView(headerView);

            ToasterPointsAdapter = new ToasterPointsAdapter(this, data);
            pointsList.Adapter = ToasterPointsAdapter;
            pointsList.ItemClick += ToasterPointsAdapter.OnListItemClick;
            pointsList.DividerHeight = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task GetTotalEarnedPoints()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    var result = await App.ToasterPointsFactory.GetTotalEarnedPoints(this.CurrentUser.UserId);
                    userPoints.Text = "Total Earned Points: " + result.ToString();
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        public async Task GetTotalRedeemedPoints()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    var result = await App.ToasterPointsFactory.GetTotalRedeemedPoints(this.CurrentUser.UserId);
                    userPoints.Text = "Total Redeemed Points: " + result.ToString();
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task GetEarnedPoints()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    var result = await App.ToasterPointsFactory.GetEarnedPoints(this.CurrentUser.UserId);

                    if (result != null && result.Count > 0)
                    {
                        redeemedPointsShown = false;
                        myMenu.SetTitle(AppText.RedeemedPoints);
                        if (ToasterPointsAdapter == null)
                        {
                            InitTableView(result.ToList());
                        }
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                this.ToasterPointsAdapter.Points = result.ToList();
                                this.ToasterPointsAdapter.NotifyDataSetChanged();
                            });
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, ToastMessage.NoEarnedPoints, ToastLength.Short).Show();
                    }

                    await GetTotalEarnedPoints();

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetRedeemedPoints()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {

                    this.ShowProgressbar(true, "", ToastMessage.Loading);

                    var result = await App.ToasterPointsFactory.GetRedeemedPoints(this.CurrentUser.UserId);
                    if (result != null && result.Count > 0)
                    {
                        redeemedPointsShown = true;
                        myMenu.SetTitle(AppText.EarnedPoints);
                        if (ToasterPointsAdapter == null)
                        {
                            InitTableView(result.ToList());
                        }
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                this.ToasterPointsAdapter.Points = result.ToList();
                                this.ToasterPointsAdapter.NotifyDataSetChanged();
                            });
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, ToastMessage.NoEarnedPoints, ToastLength.Short).Show();
                    }

                    await GetTotalRedeemedPoints();

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
            }
        }

        #endregion

    }
}