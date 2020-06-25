using System;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals
{
    public class BlockedToastersViewholder : Java.Lang.Object
    {

        #region Properties 

        private Action<int, BlockedToastersViewholder> ActionBtnListener { get; set; }

        public TextView Name { get; set; }

        public Button ActionButon { get; set; }

        private BlockedToastersAdapter BlockedToastersAdapter { get; set; }

        #endregion

        #region Constructors

        public BlockedToastersViewholder(BlockedToastersAdapter blockedToastersAdapter, View itemView,
            Action<int, BlockedToastersViewholder> btnClick)
        {
            this.BlockedToastersAdapter = blockedToastersAdapter;
            this.Name = itemView.FindViewById<TextView>(Resource.Id.title);
            this.ActionButon = itemView.FindViewById<Button>(Resource.Id.unblockBtn);

            ActionBtnListener = btnClick;
            this.ActionButon.Click += OnActionBtnListener;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnActionBtnListener(object sender, EventArgs e)
        {
            try
            {
                var position = Convert.ToInt32(this.ActionButon.GetTag(Resource.Id.unblockBtn));
                ActionBtnListener(position, this);
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}