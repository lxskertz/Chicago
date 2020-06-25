using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Stripe;
using Tabs.Mobile.ChicagoAndroid.Activities.Payments;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Payments
{

    public class PaymentMethodsAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Card> Cards { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private PaymentMethodsActivity PaymentMethodsActivity { get; set; }

        #endregion

        #region Constructors

        public PaymentMethodsAdapter(PaymentMethodsActivity paymentMethodsActivity,
            List<Card> cards)
        {
            this.PaymentMethodsActivity = paymentMethodsActivity;
            this.Cards = cards;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Get number of item to be displayed
        /// </summary>
        public override int Count
        {
            get
            {
                return this.Cards.Count;
            }
        }

        /// <summary>
        /// Gets item ID
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
		public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Get the type of View that will be created for the specified item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Get item at specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return "";
        }

        /// <summary>
        /// Gets view... list cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup container)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(this.PaymentMethodsActivity).Inflate(Resource.Layout.PaymentMethodsListItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            var deleteBtn = convertView.FindViewById<Button>(Resource.Id.deleteCard);

            var item = this.Cards.ElementAt(position);

            if (item != null)
            {
                string stars = "****";
                title.Text = string.IsNullOrEmpty(item.Last4) ? "" : stars + item.Last4 + "    " + item.ExpMonth + "/" + item.ExpYear;
                //cell.Accessory = indexPath.Row == 0 ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

                deleteBtn.Click += delegate
                {
                    this.PaymentMethodsActivity.DeleteCard(position);
                };

            }

            return convertView;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Cards.ElementAt(e.Position);
            if (this.PaymentMethodsActivity.FromQuantityActivity)
            {
                Activities.Drinks.DrinkQuantityActivity.RequiresRefresh = true;
                Activities.Drinks.DrinkQuantityActivity.defaultPayment = item;
                //Activities.Drinks.DrinkQuantityActivity.StripeCustomerInfo = this.PaymentMethodsActivity.StripeCustomerInfo;
                this.PaymentMethodsActivity.Finish();
            }
            else
            {
                //var item = this.Cards.ElementAt(e.Position);
                //Intent activity = new Intent(this.BusinessDrinksFragment.HomeContext, typeof(Activities.Drinks.AddEditDrinkActivity));
                //activity.PutExtra("ScreenActionType", (int)Activities.Drinks.AddEditDrinkActivity.ActionType.Edit);
                //activity.PutExtra("BusinessDrink", JsonConvert.SerializeObject(item));
                //this.BusinessDrinksFragment.HomeContext.StartActivity(activity);
            }
        }

        #endregion

    }
}