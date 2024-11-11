using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace CarSellerApp.Decorators
{
    public class CommonItemSpaceDecoration : RecyclerView.ItemDecoration
    {
        private readonly Context _context;
        private readonly int _left;
        private readonly int _right;
        private readonly int _top;
        private readonly int _bottom;
        private readonly DisplayMetrics _displayMetrics;

        public CommonItemSpaceDecoration(Context context, int left, int right, int top, int bottom)
        {
            _context = context;
            _displayMetrics = _context.Resources.DisplayMetrics;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            var dpLeft = TypedValue.ApplyDimension(ComplexUnitType.Dip, _left, _displayMetrics);
            var dpRight = TypedValue.ApplyDimension(ComplexUnitType.Dip, _right, _displayMetrics);
            var dpTop = TypedValue.ApplyDimension(ComplexUnitType.Dip, _top, _displayMetrics);
            var dpBottom = TypedValue.ApplyDimension(ComplexUnitType.Dip, _bottom, _displayMetrics);

            if (parent.GetChildAdapterPosition(view) > 0)
            {
                outRect.Set((int)dpLeft, (int)dpTop, (int)dpRight, (int)dpBottom);
            }
        }
    }
}
