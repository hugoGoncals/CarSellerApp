using System.Collections.Generic;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using CarSellerApp.ViewHolder;
using CarSellerCore.Model;

namespace CarSellerApp.Adapter;

public class CarAdapter : RecyclerView.Adapter
{
    private List<Car> _carList;

    public List<Car> CarList
    {
        get => _carList;
        set
        {
            _carList = value;
            NotifyDataSetChanged();
        }
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var viewHolder = holder as CarViewHolder;
        var car = _carList[position];
        viewHolder.Bind(car);
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_car, parent, false);
        return new CarViewHolder(itemView);
    }

    public override int ItemCount => CarList.Count;
}