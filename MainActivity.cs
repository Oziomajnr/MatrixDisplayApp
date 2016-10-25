//Declare system namespace
using System;
using System.Linq;

//Declare android namespace
using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;

//declare java namespace
using Java.Util;
using Java.Lang;


namespace MatrixDisplay
{
	[Activity (Label = "MatrixDisplay", MainLauncher = true ,Icon = "@drawable/icon")]

	public class MainActivity : Activity
	{
		//Declare bluetooth Variables
		public static	BluetoothAdapter  BluetoothAdapter1 = BluetoothAdapter.DefaultAdapter;
		public static	BluetoothSocket socket = null;
		public static	BluetoothDevice device = null;

		string[] displayResolution = { "5x7","8x8"};

	//	ArrayAdapter<String> adbScrolling = new ArrayAdapter<String> (this, Android.Resource.Layout.,Scrolling);

		protected override void OnStop()
		{
			
			socket = null;
			device = null;

		}

	//Override the oncreate method
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ArrayAdapter<string> adbResolution = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1,displayResolution);
			adbResolution.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);



			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


	

			// declare ui variables
			AlertDialog.Builder AlertDialog1 = new AlertDialog.Builder (this);
			AlertDialog1.SetTitle ("Error");

			Button btnMessage = FindViewById<Button> (Resource.Id.btnMessage);
			Button btnConnect = FindViewById<Button> (Resource.Id.btnConnect);
			Button btnSendTime = FindViewById<Button> (Resource.Id.btnSendTime);
			Button btnSetResolution = FindViewById<Button> (Resource.Id.btnSetResolution);
			Switch btnSwitch = FindViewById<Switch> (Resource.Id.switch1);

			Spinner spnResolution = FindViewById<Spinner> (Resource.Id.spnResolution);


		 //   spnResolution.Adapter = displayResolution;

			ImageButton btnMenu = FindViewById<ImageButton> (Resource.Id.btnMenu);

			SeekBar seekbarSpeed = FindViewById<SeekBar> (Resource.Id.seekbarSpeed);
			SeekBar seekbarBrightness = FindViewById<SeekBar> (Resource.Id.seekbarBrightness);

			TimePicker timePicker1 = FindViewById<TimePicker> (Resource.Id.timePicker1);

			EditText txtEnterPassword = FindViewById<EditText> (Resource.Id.txtEnterPassword);
			TextView txtConStatus = FindViewById<TextView> (Resource.Id.txtConStatus);


			timePicker1.SetIs24HourView (Java.Lang.Boolean.True);
			btnMessage.Click += BtnMessage_Click;

			btnConnect.Enabled = false;
			spnResolution.Adapter = adbResolution;

			//	seekbarSpeed.CanScrollHorizontally = true;

			btnSetResolution.Click += delegate {

				if(socket != null)
				{
					byte[] data = new byte[6];
					data[0] = 72;
					data[1]= 67;
					data[2]= 1;
					data[4] = 0;


						Toast.MakeText(this, "on ", ToastLength.Short).Show();


						socket.OutputStream.Write(data,0,6);



				}

				else{

					AlertDialog1.SetMessage("Not Connected to device");
					AlertDialog1.Show();
				}

			};

			btnSwitch.Click += delegate {
				if(socket != null)
				{
					byte[] data = new byte[6];
					data[0] = 72;
					data[1]= 67;
					data[2]= 1;
					data[4] = 0;

					if(btnSwitch.Checked)
					{
						data[3] = 10;
						data[5] = 150;
						socket.OutputStream.Write(data,0,6);
						Toast.MakeText(this, "on ", ToastLength.Short).Show();

					}
					else if(!btnSwitch.Checked)
					{
						data[3] = 11;
						data[5] = 151;
						socket.OutputStream.Write(data,0,6);
						Toast.MakeText(this, "off", ToastLength.Short).Show();
					}
				}

				else{

					AlertDialog1.SetMessage("Not Connected to device");
					AlertDialog1.Show();
				}
			};


			txtEnterPassword.TextChanged += delegate {
				// Disable button while password text length is less than 4

				if(txtEnterPassword.Text.Length < 4)
				{
					txtEnterPassword.Error ="Password must 4 characters long";



					btnConnect.Enabled = false;
					
				}
				else
				{
					btnConnect.Enabled = true;

				}
			};


			btnMenu.Click += delegate {
				//Popup menu for options
				PopupMenu menu = new PopupMenu (this, btnMenu);
				menu.MenuInflater.Inflate (Resource.Menu.popup_menu, menu.Menu);
				menu.Show();

				menu.MenuItemClick += (s1, arg1) => {
					if(arg1.Item.TitleFormatted.ToString() == "Send Text To Display" )
					{
						StartActivity(typeof(Message));
					}
					else if(arg1.Item.TitleFormatted.ToString() == "Change Password")
					{
						StartActivity(typeof(ChangePassword));
					}
					else if(arg1.Item.TitleFormatted.ToString() == "Unpair all Hc devices")
					{
						
						unpairdevice();
					 }
				};
			};

			btnSendTime.Click += delegate {
				try{
					
				char[] mess;
				byte checksum = 0;
				string time;
				if ((int)timePicker1.CurrentHour > 9) {
					time = timePicker1.CurrentHour.ToString () + ":";
				} else {
					time = "0";
					time += timePicker1.CurrentHour.ToString () + ":";
				}
				if ((int)timePicker1.CurrentMinute < 9) {
					time += "0";
				}
				time += timePicker1.CurrentMinute.ToString ();
				mess = time.ToCharArray ();
				byte[] messbyte = new byte[mess.Length + 6];
				for (int x = 5; x < mess.Length + 5; x++) {

					messbyte [x] = Convert.ToByte (mess [x - 5]);

				}
				//Array.Copy(mess,messbyte);

				messbyte [0] = 72;
				messbyte [1] = 67;
				messbyte [2] = 02;
				messbyte [3] = 04;
				messbyte [4] = Convert.ToByte (mess.Length);
				foreach (var div in messbyte) {
					checksum += div;
				}

				messbyte [messbyte.Length - 1] = checksum;

				if (socket != null) {
					MainActivity.socket.OutputStream.Write (messbyte, 0, messbyte.Length);
				} else {

					AlertDialog1.SetTitle ("ERROR");
					AlertDialog1.SetMessage ("Connect First Before sending message");
					AlertDialog1.Show ();

				}
				}
				catch
				{

					Toast.MakeText(this, "Error", ToastLength.Short );
				}
			};

			seekbarSpeed.ProgressChanged += delegate {
				try{
				byte checksum = 0;
				
				byte[] messbyte = new byte[7];
				messbyte [0] = 72;
				messbyte [1] = 67;
				messbyte [2] = 01;
				messbyte [3] = 05;
				messbyte [4] = 01;
				messbyte [5] = Convert.ToByte (seekbarSpeed.Progress);
				foreach (var div in messbyte) {
					checksum += div;
				}
				messbyte [6] = checksum;

				if (socket != null) {
					socket.OutputStream.Write (messbyte, 0, messbyte.Length);
				} else {

					//AlertDialog1.SetTitle ("ERROR");
					//AlertDialog1.SetMessage ("Connect First Before sending message");
					//AlertDialog1.Show ();

				}
				}
				catch
				{

					Toast.MakeText(this, "Error", ToastLength.Short );
				}

			};

			seekbarBrightness.ProgressChanged += delegate {
				try{
				byte checksum = 0;

				byte[] messbyte = new byte[7];
				messbyte [0] = 72;
				messbyte [1] = 67;
				messbyte [2] = 01;
				messbyte [3] = 06;
				messbyte [4] = 01;
				messbyte [5] = Convert.ToByte (seekbarBrightness.Progress);
				foreach (var div in messbyte) {
					checksum += div;
				}
				messbyte [6] = checksum;

				if (socket != null) {
					socket.OutputStream.Write (messbyte, 0, messbyte.Length);
				} else {

					//AlertDialog1.SetTitle ("ERROR");
					//AlertDialog1.SetMessage ("Connect First Before sending message");
					//AlertDialog1.Show ();

				}
				}
				catch
				{

					Toast.MakeText(this, "Error", ToastLength.Short );
				}
			};
			btnConnect.Click += async delegate {
				try {
				if(txtEnterPassword.Text == System.String.Empty)
				{
					AlertDialog1.SetMessage("Please Type in Device password");
					AlertDialog1.Show();
				}
				else{
					string password = txtEnterPassword.Text;
					char[] passwordchar = password.ToCharArray();
					byte[] connectionpacket = new byte[10];
					connectionpacket[0] = 72;

					connectionpacket[1] = 67;
					connectionpacket[2] = 1;
					connectionpacket[3] = 7;
					connectionpacket[4] = 4;
						connectionpacket[5] = (byte)((int)((byte)passwordchar[0]) - 48) ;
						connectionpacket[6] = (byte)((int)((byte)passwordchar[1]) - 48);
						connectionpacket[7] = (byte)((int)((byte)passwordchar[2]) - 48);
						connectionpacket[8] = (byte)((int)((byte)passwordchar[3]) - 48);
					byte checksum = 0;
					foreach(var x in connectionpacket)
					{
						checksum+=x;
					}
					connectionpacket[9] = checksum;
				if (BluetoothAdapter1 == null) {
					AlertDialog1.SetMessage ("Device Does not support bluetooth");

					AlertDialog1.Show ();
				}


				if (!BluetoothAdapter1.IsEnabled) {
					AlertDialog1.SetMessage ("Bluetooth is not enabled");
					AlertDialog1.Show ();
				} else {
							txtConStatus.Text = "Connecting";
							txtConStatus.SetTextColor(Android.Graphics.Color.Blue);

							//Use Linq to query paired bluetooth device for HC-05 Or HC-06
					device = (from bd in BluetoothAdapter1.BondedDevices
					          where bd.Name == "HC-06" || bd.Name == "HC-05"
					          select bd).FirstOrDefault ();

					if (device == null) {
						AlertDialog1.SetMessage ("Device Not Found \n Make sure you are paired to the bluetooth device");
						AlertDialog1.Show ();

					}

							else {
						//ParcelUuid[] list = device.GetUuids();
						//	string MyUUID = list[0].ToString();

						byte[] mess = new byte[6];
						mess [0] = 48;
						mess [1] = 66;
						mess [2] = 76;
						mess [3] = 64;
						mess [4] = 84;
						mess [5] = 30;


						
							if (socket == null) {
								socket = device.CreateRfcommSocketToServiceRecord (UUID.FromString ("00001101-0000-1000-8000-00805f9b34fb"));
							}



							if (!socket.IsConnected) {
								
								await socket.ConnectAsync ();
									socket.OutputStream.Write(connectionpacket,0,connectionpacket.Length);
									byte[] consucess = new byte[2];
									Thread.Sleep(500);
									socket.InputStream.Read(consucess,0,2);
									Thread.Sleep(500);
									if(consucess[0] == 79 && consucess[1]== 75 )
									{
										Toast.MakeText(this, "Connection Successful", ToastLength.Long).Show();
										btnConnect.Text = "Disconnect From Device";
										txtConStatus.Text = "Connected";
										txtConStatus.SetTextColor(Android.Graphics.Color.Green);
									}

									else if(consucess[0] == 69 && consucess[1]== 82 )
									{
										//Toast.MakeText(this, "Incorrect Password", ToastLength.Long );
										AlertDialog1.SetMessage("Incorrect Password");
										btnConnect.Text = "Connect To Device";
										txtConStatus.Text = "Disconnected";
										txtConStatus.SetTextColor(Android.Graphics.Color.Red);
										AlertDialog1.Show();
										socket.Close ();
										socket.Dispose ();
										socket = null;
									}
									else
									{
										//Toast.MakeText(this, "HandShake Failed", ToastLength.Long );
										AlertDialog1.SetMessage("Error occured");
										btnConnect.Text = "Connect To Device";
										txtConStatus.Text = "Disconnected";
										txtConStatus.SetTextColor(Android.Graphics.Color.Red);
										AlertDialog1.Show();

										socket.Close ();
										socket.Dispose ();
										socket = null;
									}



								
							} else {
								//	BluetoothAdapter1.CancelDiscovery();
								socket.Close ();
								socket.Dispose ();
								socket = null;
								btnConnect.Text = "Connect To Device";
									txtConStatus.Text = "Disconnected";
									txtConStatus.SetTextColor(Android.Graphics.Color.Red);
							}
						
						

						} 

						//btnConnect.Text = socket.IsConnected.ToString();
					}
				}
				}

				catch (Java.Lang.Exception e) {
					AlertDialog1.SetMessage (e.Message.ToString ());
					AlertDialog1.Show ();
				}
			};

		//	ToggleButton toggleButton1 = FindViewById<ToggleButton> (Resource.Id.toggleButton1);
			EditText txtOldPassword = FindViewById<EditText> (Resource.Id.txtOldPassword);



		}

		//	toggleButton1.Click += async delegate {
				
			//}; 


		void BtnMessage_Click (object sender, EventArgs e)
		{
				StartActivity(typeof(Message));
		}

		public void AssignDevice()
		{
			device = (from bd in BluetoothAdapter1.BondedDevices where bd.Name == "HC-05"  select bd).FirstOrDefault();

		}
		// unpair all hc-05 & hc-06 devices
		public void unpairdevice()
		{
			
			try{
			foreach (var bd in BluetoothAdapter1.BondedDevices)
			{
				if(bd.Name == "HC-06" || bd.Name == "HC-05")
				{
			Java.Lang.Reflect.Method m1 = bd.Class.GetMethod("removebond", null);
			m1.Invoke (bd,null);
				}
			}
			}

			catch {
				Toast.MakeText (this, "error", ToastLength.Long);

			}

		}


}
}


