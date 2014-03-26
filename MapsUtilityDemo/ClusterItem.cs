using System;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace MapsUtilityDemo
{
	public class ClusterItem : Java.Lang.Object, IClusterItem
	{
		public LatLng Position { get; set;}

		public ClusterItem (double lat, double lng)
		{
			Position = new LatLng (lat, lng);
		}
	}
}

