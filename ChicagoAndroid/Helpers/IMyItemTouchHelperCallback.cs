using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Graphics;
using Android.Graphics.Drawables;
using Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events;

namespace Tabs.Mobile.ChicagoAndroid.Helpers
{
    public class IMyItemTouchHelperCallback : ItemTouchHelper.Callback
    {
        #region Properties

        private ColorDrawable background = new ColorDrawable();
        public static float ALPHA_FULL = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        private EventsHomeAdapter EventsHomeAdapter { get; set; }

        #endregion

        #region Constructors

        public IMyItemTouchHelperCallback(EventsHomeAdapter eventsHomeAdapter)
        {
            this.EventsHomeAdapter = eventsHomeAdapter;
        }

        #endregion

        #region Methods

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            //if (recyclerView.getLayoutManager() instanceof GridLayoutManager) {
            //    final int dragFlags = ItemTouchHelper.UP | ItemTouchHelper.DOWN | ItemTouchHelper.LEFT | ItemTouchHelper.RIGHT;
            //    final int swipeFlags = 0;
            //    return makeMovementFlags(dragFlags, swipeFlags);
            //}else
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;

            return MakeMovementFlags(dragFlags, swipeFlags);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="viewHolder"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            try
            {
                if (viewHolder.ItemViewType != target.ItemViewType)
                {
                    return false;
                }

                this.EventsHomeAdapter.OnItemMove(viewHolder.AdapterPosition, target.AdapterPosition);
            }
            catch (Exception)
            {
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="direction"></param>
        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            try
            {
                this.EventsHomeAdapter.OnItemDismiss(viewHolder.AdapterPosition);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsLongPressDragEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsItemViewSwipeEnabled
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cValue"></param>
        /// <param name="recyclerView"></param>
        /// <param name="viewHolder"></param>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <param name="actionState"></param>
        /// <param name="isCurrentlyActive"></param>
        public override void OnChildDraw(Canvas cValue, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            try
            {
                var itemView = viewHolder.ItemView;
                var itemHeight = itemView.Bottom - itemView.Top;

                var deleteIcon = this.EventsHomeAdapter.MyContext.Resources.GetDrawable(Resource.Drawable.baseline_delete_black_24);
                var inWidth = deleteIcon.IntrinsicWidth;
                var inHeight = deleteIcon.IntrinsicHeight;

                // Draw the red delete background            
                background.Color = Color.ParseColor("#f44336");
                background.SetBounds(
                        itemView.Right + Convert.ToInt32(dX),
                        itemView.Top,
                        itemView.Right,
                        itemView.Bottom
                );
                background.Draw(cValue);

                // Calculate position of delete icon
                var iconTop = itemView.Top + (itemHeight - inHeight) / 2;
                var iconMargin = (itemHeight - inHeight) / 2;
                var iconLeft = itemView.Right - iconMargin - inWidth;
                var iconRight = itemView.Right - iconMargin;
                var iconBottom = iconTop + inHeight;

                // Draw the delete icon
                deleteIcon.SetBounds(iconLeft, iconTop, iconRight, iconBottom);
                deleteIcon.Draw(cValue);

                base.OnChildDraw(cValue, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);


                //if (actionState == ItemTouchHelper.ActionStateSwipe)
                //{
                //    // Fade out the view as it is swiped out of the parent's bounds
                //    float alpha = ALPHA_FULL - Math.Abs(dX) / (float)viewHolder.ItemView.Width;
                //    viewHolder.ItemView.Alpha = alpha;
                //    viewHolder.ItemView.TranslationX = dX;
                //}
                //else
                //{
                //    base.OnChildDraw(cValue, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                //}
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="actionState"></param>
        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            try
            {
                // We only want the active item to change
                if (actionState != ItemTouchHelper.ActionStateIdle)
                {
                    if (viewHolder is ViewHolders.Business.EventsHomeViewHolder)
                    {
                        // Let the view holder know that this item is being moved or dragged
                        ViewHolders.Business.EventsHomeViewHolder itemViewHolder = (ViewHolders.Business.EventsHomeViewHolder)viewHolder;
                        itemViewHolder.OnItemSelected();
                    }
                }
                base.OnSelectedChanged(viewHolder, actionState);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="viewHolder"></param>
        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            try
            {
                base.ClearView(recyclerView, viewHolder);

                viewHolder.ItemView.Alpha = ALPHA_FULL;

                if (viewHolder is ViewHolders.Business.EventsHomeViewHolder)
                {
                    // Tell the view holder it's time to restore the idle state
                    ViewHolders.Business.EventsHomeViewHolder itemViewHolder = (ViewHolders.Business.EventsHomeViewHolder)viewHolder;
                    itemViewHolder.OnItemCleared();
                }
            }
            catch (Exception)
            {
            }
        }

    #endregion

}

    #region InnerClass

    public interface ItemTouchHelperAdapter
    {
        void OnItemMove(int fromPosition, int toPosition);
        void OnItemDismiss(int position);
    }

    public interface OnStartDragListener
    {
        void onStartDrag(RecyclerView.ViewHolder viewHolder);
    }

    #endregion

}