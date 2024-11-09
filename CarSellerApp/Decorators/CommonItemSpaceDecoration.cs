using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace CAPMobile.Droid.ItemDecoration
{
    public class CommonItemSpaceDecoration : RecyclerView.ItemDecoration
    {
        private readonly Context _context;
        private readonly int _left;
        private readonly int _right;
        private readonly int _top;
        private readonly int _bottom;
        private readonly bool _applyToFirstAndLast;
        private readonly DisplayMetrics _displayMetrics;

        public CommonItemSpaceDecoration(Context context, int left, int right, int top, int bottom, bool applyToFirstAndLast = false)
        {
            _context = context;
            _displayMetrics = _context.Resources.DisplayMetrics;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
            _applyToFirstAndLast = applyToFirstAndLast;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            var dpLeft = TypedValue.ApplyDimension(ComplexUnitType.Dip, _left, _displayMetrics);
            var dpRight = TypedValue.ApplyDimension(ComplexUnitType.Dip, _right, _displayMetrics);
            var dpTop = TypedValue.ApplyDimension(ComplexUnitType.Dip, _top, _displayMetrics);
            var dpBottom = TypedValue.ApplyDimension(ComplexUnitType.Dip, _bottom, _displayMetrics);

            if (_applyToFirstAndLast)
            {
                if (parent.GetChildAdapterPosition(view) == 0)
                {
                    outRect.Set((int)dpLeft * 2, (int)dpTop, (int)dpRight, (int)dpBottom);
                }
                else if (parent.GetChildAdapterPosition(view) == parent.GetAdapter().ItemCount - 1)
                {
                    outRect.Set((int)dpLeft, (int)dpTop, (int)dpRight * 2, (int)dpBottom);
                }
                else
                {

                    outRect.Set((int)dpLeft, (int)dpTop, (int)dpRight, (int)dpBottom);
                }
                return;
            }

            if (parent.GetChildAdapterPosition(view) > 0)
            {
                outRect.Set((int)dpLeft, (int)dpTop, (int)dpRight, (int)dpBottom);
            }
        }
    }
}
