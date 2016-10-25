using System;
using System.Linq;


using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.Util;
using Java.Lang;


namespace MatrixDisplay
{
	[Activity (Label = "PasswordVerification", MainLauncher =false)]			
	public class PasswordVerification : Activity
	{
		public static	BluetoothAdapter  BluetoothAdapter1 = BluetoothAdapter.DefaultAdapter;
		public static	BluetoothSocket socket = null;
		public static	BluetoothDevice device = null;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here
			SetContentView (Resource.Layout.PasswordVerification);
			AlertDialog.Builder AlertDialog1 = new AlertDialog.Builder (this);
			Button btnPassword = FindViewById<Button> (Resource.Id.btnPassword);
			EditText txtPassword = FindViewById<EditText> (Resource.Id.txtPassword);
			AlertDialog1.SetTitle ("Error");

			btnPassword.Click +=  delegate {


				if (BluetoothAdapter1 == null) {
					AlertDialog1.SetMessage ("Device Does not support bluetooth");

					AlertDialog1.Show ();
				}


				if (!BluetoothAdapter1.IsEnabled) {
					AlertDialog1.SetMessage ("Bluetooth is not enabled");
					AlertDialog1.Show ();
				} else {

					device = (from bd in BluetoothAdapter1.BondedDevices
						where bd.Name == "HC-06" || bd.Name == "HC-05"
						select bd).FirstOrDefault ();

					if (device == null) {
						AlertDialog1.SetMessage ("Device Not Found \n Make sure you are paired to the bluetooth device");
						AlertDialog1.Show ();

					} else {
						//ParcelUuid[] list = device.GetUuids();
						//	string MyUUID = list[0].ToString();

						byte[] mess = new byte[6];
						mess [0] = 48;
						mess [1] = 66;
						mess [2] = 76;
						mess [3] = 64;
						mess [4] = 84;
						mess [5] = 30;


						try {
							if (socket == null) {
								socket = device.CreateRfcommSocketToServiceRecord (UUID.FromString ("00001101-0000-1000-8000-00805f9b34fb"));
							}



							if (!socket.IsConnected) {

								//socket.ConnectAsync ();

								//btnConnect.Text = "Bluetooth Connected";
								if(txtPassword.Text == "1234" )
								{
									//socket.OutputStream.Write(mess,0,5);
									AlertDialog1.SetMessage("PassWord Correct");

									StartActivity(typeof(MainActivity));
								}
								else
								{
									//AlertDialog1.SetMessage("Incorrect Password Try Again");
									//AlertDialog1.Show();
									Toast.MakeText(this,"Incorrect PassWord, Try again", ToastLength.Long).Show();
								}

								//socket.OutputStream.Write(mess,0,5);
							} 
							//else {
							//	BluetoothAdapter1.CancelDiscovery();
							//	socket.Close ();
							//	socket.Dispose ();
							//	socket = null;
							//	btnConnect.Text = "Bluetooth Disconnected";
							//}



						} catch (System.Exception e) {
							AlertDialog1.SetMessage (e.Message.ToString ());
							AlertDialog1.Show ();
						}

						//btnConnect.Text = socket.IsConnected.ToString();
					}
				}

			};

		}
	}
}

