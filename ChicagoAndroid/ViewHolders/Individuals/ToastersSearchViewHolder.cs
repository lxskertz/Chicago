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
using Android.Support.V7.Widget;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Resources;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals
{
    public class ToastersSearchViewHolder : RecyclerView.ViewHolder
    {

        #region Properties

        /// <summary>
        /// Gets or sets request btn
        /// </summary>
        public Button ToasterRequest { get; set; }

        /// <summary>
        /// Gets or sets the profile pic
        /// </summary>
        public ImageView ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public TextView Name { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public TextView Username { get; set; }

        /// <summary>
        /// Gets or sets the card view
        /// </summary>
        public CardView ToasterSearchCard { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        public ToastersSearchAdapter ToastersSearchAdapter { get; set; }

        /// <summary>
        /// Gets or sets adapter
        /// </summary>
        public ToastersAdapter ToastersAdapter { get; set; }

        /// <summary>
        /// Gets or sets item
        /// </summary>
        public ToastersSearchItem RowItem { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchViewHolder(ToastersSearchAdapter adapter, View itemView) : base(itemView)
        {
            this.ToastersSearchAdapter = adapter;
            this.ToasterSearchCard = itemView.FindViewById<CardView>(Resource.Id.toastersSearchCard);
            this.ProfilePic = itemView.FindViewById<ImageView>(Resource.Id.userAvatar);
            this.Username = itemView.FindViewById<TextView>(Resource.Id.username);
            this.Name = itemView.FindViewById<TextView>(Resource.Id.name);
            this.ToasterRequest = itemView.FindViewById<Button>(Resource.Id.toasterRequest);
            //this.ToasterRequest.Click += (sender, e) => toasterRequestClick(this.Name);

            this.ToasterSearchCard.Click += OpenToasterProfile;
        }

        public ToastersSearchViewHolder(ToastersAdapter adapter, View itemView) : base(itemView)
        {
            this.ToastersAdapter = adapter;
            this.ToasterSearchCard = itemView.FindViewById<CardView>(Resource.Id.toastersSearchCard);
            this.ProfilePic = itemView.FindViewById<ImageView>(Resource.Id.userAvatar);
            this.Username = itemView.FindViewById<TextView>(Resource.Id.username);
            this.Name = itemView.FindViewById<TextView>(Resource.Id.name);
            this.ToasterRequest = itemView.FindViewById<Button>(Resource.Id.toasterRequest);
            //this.ToasterRequest.Click += (sender, e) => toasterRequestClick(this.Name);

            this.ToasterSearchCard.Click += OpenToasterProfile;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToasterProfile(object sender, EventArgs e)
        {
            int position = base.AdapterPosition;
            if (position != RecyclerView.NoPosition)
            {
                Shared.Models.Users.Users searchedUser = new Shared.Models.Users.Users();
                if (this.ToastersSearchAdapter != null)
                {
                    ToastersSearchItem row = new ToastersSearchItem();
                    row = this.ToastersSearchAdapter.Rows.ElementAt(position);
                    searchedUser = new Shared.Models.Users.Users()
                    {
                        Email = row.Email,
                        UserId = row.UserId,
                        FirstName = row.Name,
                        LastName = row.Name,
                        Username = row.Username
                    };

                    Intent activity = new Intent(this.ToastersSearchAdapter.Owner.Activity, typeof(Activities.Individuals.SearchToasterProfileActivity));
                    activity.PutExtra("FromSearchedUser", true);
                    activity.PutExtra("SearchedUser", JsonConvert.SerializeObject(searchedUser));
                    this.ToastersSearchAdapter.Owner.StartActivity(activity);
                }
                else if(this.ToastersAdapter != null)
                {
                    Toasters toaster = new Toasters();
                    toaster = this.ToastersAdapter.Rows.ElementAt(position);
                    searchedUser = new Shared.Models.Users.Users()
                    {
                        Email = toaster.Email,
                        UserId = toaster.UserId,
                        FirstName = toaster.FirstName,
                        LastName = toaster.LastName,
                        Username = toaster.Username
                    };

                    Intent activity = new Intent(this.ToastersAdapter.Owner, typeof(Activities.Individuals.SearchToasterProfileActivity));
                    activity.PutExtra("FromSearchedUser", true);
                    activity.PutExtra("FromRequestPending", this.ToastersAdapter.Owner.pendingRequestShown);
                    activity.PutExtra("SearchedUser", JsonConvert.SerializeObject(searchedUser));
                    activity.PutExtra("FromToasters", true);
                    activity.PutExtra("Toaster", JsonConvert.SerializeObject(toaster));
                    this.ToastersAdapter.Owner.StartActivity(activity);
                }
            }
        }

        #endregion

    }
}