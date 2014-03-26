using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace MapsUtilityDemo
{
	[Activity (Label = "MapsUtilityDemo", MainLauncher = true)]
	public class MainActivity : FragmentActivity, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
	{
		private GoogleMap _map;
		private SupportMapFragment _mapFragment;
		private ClusterManager _clusterManager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitMapFragment();
		}

		protected override void OnResume()
		{
			base.OnResume();
			SetupMapIfNeeded();
		}

		private void InitMapFragment()
		{
			_mapFragment = SupportFragmentManager.FindFragmentByTag("map") as SupportMapFragment;
			if (_mapFragment == null)
			{
				GoogleMapOptions mapOptions = new GoogleMapOptions()
					.InvokeMapType(GoogleMap.MapTypeNormal)
					.InvokeZoomControlsEnabled(false)
					.InvokeCompassEnabled(true);

				var fragTx = SupportFragmentManager.BeginTransaction();
				_mapFragment = SupportMapFragment.NewInstance(mapOptions);
				fragTx.Add(Resource.Id.map, _mapFragment, "map");
				fragTx.Commit();
			}
		}

		private void SetupMapIfNeeded()
		{
			if (_map == null)
			{
				_map = _mapFragment.Map;
				if (_map != null)
				{
					SetViewPoint (new LatLng(63.430515, 10.395053), false);

					_clusterManager = new ClusterManager (this, _map);
					_clusterManager.SetOnClusterClickListener (this);
					_clusterManager.SetOnClusterItemClickListener (this);
					_map.SetOnCameraChangeListener (_clusterManager);
					_map.SetOnMarkerClickListener (_clusterManager);

					AddClusterItems ();
				}
			}
		}

		private void AddClusterItems(){
			double lat = 63.430515;
			double lng = 10.395053;

			List<ClusterItem> items = new List<ClusterItem>();

			for(var i = 0; i< 10; i++){
				double offset = i / 60d;
				lat = lat + offset;
				lng = lng + offset;

				var item = new ClusterItem (lat, lng);
				items.Add (item);
			}

			_clusterManager.AddItems (items);
		}

		public void SetViewPoint(LatLng latlng, bool animated){
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(latlng);
			builder.Zoom(14.5F);
			CameraPosition cameraPosition = builder.Build();

			if (animated) {
				_map.AnimateCamera (CameraUpdateFactory.NewCameraPosition (cameraPosition));
			} else {
				_map.MoveCamera (CameraUpdateFactory.NewCameraPosition (cameraPosition));
			}
		}

		//Cluster override methods
		public bool OnClusterClick(ICluster cluster){
			Toast.MakeText (this, cluster.Items.Count + " items in cluster", ToastLength.Short).Show ();
			return false;
		}

		public bool OnClusterItemClick(Java.Lang.Object marker){
			Toast.MakeText (this, "Marker clicked", ToastLength.Short).Show ();
			return false;
		}
	}
}


