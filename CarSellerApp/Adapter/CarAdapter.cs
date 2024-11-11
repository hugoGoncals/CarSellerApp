using Android.Views;
using AndroidX.RecyclerView.Widget;
using CarSellerApp.ViewHolder;
using CarSellerCore.Model;

namespace CarSellerApp.Adapter;

public class CarAdapter : RecyclerView.Adapter
{
    private readonly ICarAdapterListenner _listenner;
    private List<Car> _carList = new();

    public CarAdapter(ICarAdapterListenner listenner)
    {
        _listenner = listenner;
    }

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
        return new CarViewHolder(itemView, _listenner);
    }

    public override int ItemCount => CarList.Count;
}