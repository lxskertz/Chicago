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
using Tabs.Mobile.ChicagoAndroid.Fragments.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Drinks
{
    [Activity(Label = "Add Drink", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class AddEditDrinkActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        #region constants, Enums, and Variables

        IMenu deleteMenu;
        FrameLayout pageLayout;
        TextInputLayout drinkNameLayout;
        TextInputEditText drinkName;
        TextInputLayout drinkPriceLayout;
        TextInputEditText drinkPrice;

        public enum ActionType
        {
            Add = 1,
            Edit = 2
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ActionType ScreenActionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessDrink Drink { get; set; }

        public int BusinessId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AddEditDrinks);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                pageLayout = FindViewById<FrameLayout>(Resource.Id.addEditDrinksLayout);
                drinkNameLayout = FindViewById<TextInputLayout>(Resource.Id.drinkName_layout);
                drinkName = FindViewById<TextInputEditText>(Resource.Id.drinkName);
                drinkPriceLayout = FindViewById<TextInputLayout>(Resource.Id.drinkPrice_layout);
                drinkPrice = FindViewById<TextInputEditText>(Resource.Id.drinkPrice);

                var save = FindViewById<Button>(Resource.Id.saveDrink);

                save.Click += delegate
                {
                    Save();
                };

                this.ScreenActionType = (ActionType)Intent.GetIntExtra("ScreenActionType", (int)ActionType.Add);

                if (ScreenActionType == ActionType.Edit)
                {
                    this.Drink = JsonConvert.DeserializeObject<BusinessDrink>(Intent.GetStringExtra("BusinessDrink"));
                    drinkName.Text = Drink != null && !string.IsNullOrEmpty(this.Drink.DrinkName) ? this.Drink.DrinkName : "";
                    drinkPrice.Text = Drink != null ? this.Drink.Price.ToString() : "";
                    SetTitle(AppText.EditDrink);
                    save.Text = AppText.Update;

                    if (deleteMenu != null)
                    {
                        deleteMenu.FindItem(Resource.Id.menuAction).SetTitle(AppText.Delete);
                    }
                }
                else
                {
                    this.BusinessId = Intent.GetIntExtra("BusinessId", 0);
                }
            }
            catch (Exception)
            {
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
                case Resource.Id.menuAction:
                    DeleteDrink();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);
            deleteMenu = menu;

            if (ScreenActionType == ActionType.Edit)
            {
                deleteMenu.FindItem(Resource.Id.menuAction).SetTitle(AppText.Delete);
            }
            

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async void DeleteDrink()
        {
            try
            {
                if (this.Drink == null)
                {
                    return;
                }

                if (CheckNetworkConnectivity() == null)
                {
                    ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }

                this.ShowProgressbar(true, "", ToastMessage.Deleting);
                await App.BusinessDrinkFactory.Delete(this.Drink.BusinessDrinkId);
                this.ShowProgressbar(false, "", ToastMessage.Saving);
                BusinessDrinksFragment.RequiresRefresh = true;
                this.Finish();
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Saving);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void Save()
        {
            try
            {
                // stop if no internet connection
                if (CheckNetworkConnectivity() == null)
                {
                    ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                if (!ValidateInput(drinkNameLayout, drinkName, ToastMessage.RequiredDrinkName))
                {
                    return;
                }
                if (!ValidateInput(drinkPriceLayout, drinkPrice, ToastMessage.RequiredDrinkPrice))
                {
                    return;
                }
                this.ShowProgressbar(true, "", ToastMessage.Saving);

                BusinessDrink drink = new BusinessDrink();
                drink.DrinkName = drinkName.Text;
                drink.Price = Convert.ToDouble(drinkPrice.Text);

                if (ScreenActionType == ActionType.Edit)
                {
                    drink.BusinessId = this.Drink != null ? this.Drink.BusinessId : 0;
                    drink.BusinessDrinkId = this.Drink != null ? this.Drink.BusinessDrinkId : 0;
                    await App.BusinessDrinkFactory.Update(drink.BusinessDrinkId, drink.DrinkName, drink.Price);
                }
                else
                {
                    drink.BusinessId = this.BusinessId;
                    await App.BusinessDrinkFactory.Add(drink);
                }

                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
                BusinessDrinksFragment.RequiresRefresh = true;
                this.Finish();
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.LoggingIn);
                Toast.MakeText(this, ToastMessage.ServerError, ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// handle when user tap done on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                //await Login();
            }
        }

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress
        /// Called when an action is being performed.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="actionId"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return true;
        }

        #endregion

    }
}