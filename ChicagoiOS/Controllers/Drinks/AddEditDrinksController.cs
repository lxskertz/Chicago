using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Drinks;
using Tabs.Mobile.Shared.Models.Drinks;
using BigTed;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class AddEditDrinksController : BaseViewController
    {

        #region constants, Enums, and Variables

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

        #region Constructors

        public AddEditDrinksController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                DrinkName.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    DrinkPrice.BecomeFirstResponder();

                    return true;
                };

                DrinkPrice.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    Save();

                    return true;
                };

                if (ScreenActionType == ActionType.Edit)
                {
                    DrinkName.Text = Drink != null && !string.IsNullOrEmpty(this.Drink.DrinkName) ? this.Drink.DrinkName : "";
                    DrinkPrice.Text = Drink != null ? this.Drink.Price.ToString() : "";
                    SetTitle(AppText.EditDrink);
                    SaveBtn.SetTitle(AppText.Update, UIControlState.Normal);

                    this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Trash, (sender, args) =>
                    {
                        DeleteDrink();
                    }), true);
                }
            }
            catch (Exception)
            {
            }
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
                if(this.Drink == null)
                {
                    return;
                }

                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                    return;
                }

                BTProgressHUD.Show(ToastMessage.Deleting, -1f, ProgressHUD.MaskType.Black);

                await AppDelegate.BusinessDrinkFactory.Delete(this.Drink.BusinessDrinkId);

                BTProgressHUD.Dismiss();
                BusinessDrinksController.RequiresRefresh = true;
                this.NavigationController.PopViewController(true);
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// Set title
        /// </summary>
        private void SetTitle(string title)
        {
            this.NavigationItem.Title = title;
        }

        /// <summary>
        /// Dismiss the keyboard when one or more fingers touches the screen.
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            DrinkName.ResignFirstResponder();
            DrinkPrice.ResignFirstResponder();
        }


        partial void SaveBtn_TouchUpInside(UIButton sender)
        {
            Save();
        }

        private async void Save()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                    return;
                }

                if (string.IsNullOrEmpty(DrinkName.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredDrinkName, Helpers.ToastTime.ErrorTime);

                    return;
                }

                if (string.IsNullOrEmpty(DrinkPrice.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredDrinkPrice, Helpers.ToastTime.ErrorTime);

                    return;
                }

                BTProgressHUD.Show(ToastMessage.Saving, -1f, ProgressHUD.MaskType.Black);

                BusinessDrink drink = new BusinessDrink();
                drink.DrinkName = DrinkName.Text;
                drink.Price = Convert.ToDouble(DrinkPrice.Text);

                if (ScreenActionType == ActionType.Edit)
                {
                    drink.BusinessId = this.Drink != null ? this.Drink.BusinessId : 0;
                    drink.BusinessDrinkId = this.Drink != null ? this.Drink.BusinessDrinkId : 0;
                    await AppDelegate.BusinessDrinkFactory.Update(drink.BusinessDrinkId, drink.DrinkName, drink.Price);
                }
                else
                {
                    drink.BusinessId = this.BusinessId;
                    await AppDelegate.BusinessDrinkFactory.Add(drink);
                }

                BTProgressHUD.Dismiss();
                BusinessDrinksController.RequiresRefresh = true;
                this.NavigationController.PopViewController(true);
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                //BTProgressHUD.Dismiss();
            }
        }


        #endregion
    }
}