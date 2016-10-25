
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MatrixDisplay
{
	[Activity (Label = "Message", MainLauncher = false)]			
	public class Message : Activity
	{
		String[] Scrolling = {"RtL", "LtR", "TtB", "BtT"};


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			AlertDialog.Builder AlertDialog1 = new AlertDialog.Builder (this);


			ArrayAdapter<String> adbScrolling = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1,Scrolling);
			adbScrolling.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			// Create your application here
			SetContentView(Resource.Layout.Message);


		//	Button btnSettings = FindViewById<Button> (Resource.Id.btnSettings);
				Spinner Spinner1 = FindViewById<Spinner> (Resource.Id.spinner1);
				Spinner Spinner2 = FindViewById<Spinner> (Resource.Id.spinner2);
				Spinner Spinner3 = FindViewById<Spinner> (Resource.Id.spinner3);
				Spinner Spinner4 = FindViewById<Spinner> (Resource.Id.spinner4);
				Spinner Spinner5 = FindViewById<Spinner> (Resource.Id.spinner5);
				Spinner Spinner6 = FindViewById<Spinner> (Resource.Id.spinner6);
				Spinner Spinner7 = FindViewById<Spinner> (Resource.Id.spinner7);
				Spinner Spinner8 = FindViewById<Spinner> (Resource.Id.spinner8);
				Spinner Spinner9 = FindViewById<Spinner> (Resource.Id.spinner9);
				Spinner Spinner10 = FindViewById<Spinner> (Resource.Id.spinner10);
				Spinner Spinner11 = FindViewById<Spinner> (Resource.Id.spinner11);
				Spinner Spinner12 = FindViewById<Spinner> (Resource.Id.spinner12);
				Spinner Spinner13 = FindViewById<Spinner> (Resource.Id.spinner13);
				Spinner Spinner14 = FindViewById<Spinner> (Resource.Id.spinner14);
				Spinner Spinner15 = FindViewById<Spinner> (Resource.Id.spinner15);
				Spinner Spinner16 = FindViewById<Spinner> (Resource.Id.spinner16);
				Spinner Spinner17 = FindViewById<Spinner> (Resource.Id.spinner17);
				Spinner Spinner18 = FindViewById<Spinner> (Resource.Id.spinner18);
				Spinner Spinner19 = FindViewById<Spinner> (Resource.Id.spinner19);
				Spinner Spinner20 = FindViewById<Spinner> (Resource.Id.spinner20);

		     	Button Button1 =  FindViewById<Button> (Resource.Id.button1);
			    Button Button2 =  FindViewById<Button> (Resource.Id.button2);
			EditText  EditText1 = FindViewById<EditText> (Resource.Id.editText1);

			EditText  EditText2 = FindViewById<EditText> (Resource.Id.editText2);
			Spinner1.Adapter = adbScrolling;
			Spinner2.Adapter = adbScrolling;
			Spinner3.Adapter = adbScrolling;
			Spinner4.Adapter = adbScrolling;
			Spinner5.Adapter = adbScrolling;
			Spinner6.Adapter = adbScrolling;
			Spinner7.Adapter = adbScrolling;
			Spinner8.Adapter = adbScrolling;
			Spinner9.Adapter = adbScrolling;
			Spinner10.Adapter = adbScrolling;
			Spinner11.Adapter = adbScrolling;
			Spinner12.Adapter = adbScrolling;
			Spinner13.Adapter = adbScrolling;
			Spinner14.Adapter = adbScrolling;
			Spinner15.Adapter = adbScrolling;
			Spinner16.Adapter = adbScrolling;
			Spinner17.Adapter = adbScrolling;
			Spinner18.Adapter = adbScrolling;
			Spinner19.Adapter = adbScrolling;
			Spinner20.Adapter = adbScrolling;



			//btnSettings.Click += delegate {
		//		StartActivity(typeof(MainActivity));
		//	};

			Button1.Click += delegate {

				try{
				char[] mess;

				byte checksum=0;
				mess =  EditText1.Text.ToUpper().ToCharArray();

				byte [] messbyte = new byte[mess.Length+6];
				for(int x = 5; x< mess.Length+5; x++)
				{
					
					messbyte[x] = Convert.ToByte(mess[x-5]);

				}
				//Array.Copy(mess,messbyte);

				messbyte[0]=72;
				messbyte[1]=67;
				messbyte[2]=02;
				messbyte[3]=3;
				messbyte[4]=Convert.ToByte( mess.Length);

				foreach(var div in messbyte)
				{
					checksum += div;
				}
				messbyte[messbyte.Length-1] = checksum;

				if (MainActivity.socket != null)
				{
				MainActivity.socket.OutputStream.Write(messbyte,0,messbyte.Length);
				}
				else
				{

					AlertDialog1.SetTitle("ERROR");
					AlertDialog1.SetMessage("Connect First Before sending message");
					AlertDialog1.Show();	
					}

				}
				catch
				{
					Toast.MakeText(this, "Error occurred", ToastLength.Short);

				}
			};

			Button2.Click += delegate {

				try{
				char[] mess;

				byte checksum=0;
				mess  =  EditText2.Text.ToUpper().ToCharArray();

				byte [] messbyte = new byte[mess.Length+6];
				for(int x = 5; x< mess.Length+5; x++)
				{

					messbyte[x] = Convert.ToByte(mess[x-5]);

				}
				//Array.Copy(mess,messbyte);

				messbyte[0]=72;
				messbyte[1]=67;
				messbyte[2]=02;
				messbyte[3]=8;
				messbyte[4]=Convert.ToByte( mess.Length);

				foreach(var div in messbyte)
				{
					checksum += div;
				}
				messbyte[messbyte.Length-1] = checksum;

				if (MainActivity.socket != null)
				{
					MainActivity.socket.OutputStream.Write(messbyte,0,messbyte.Length);

						byte[] consucess = new byte[2];
						Thread.Sleep(500);
					//	MainActivity.socket.InputStream.Read(consucess,0,2);
					//	Thread.Sleep(500);
					//	if(consucess[0] == 79 && consucess[1]== 75 )
					//	{
					//		Toast.MakeText(this, "Message Recieved Successfully", ToastLength.Long);

					//	}

					//	else
					//	{
					//		Toast.MakeText(this, "Error Occured while sending message", ToastLength.Long);
					//	}
							
						
				}
				else
				{

					AlertDialog1.SetTitle("ERROR");
					AlertDialog1.SetMessage("Connect First Before sending message");
					AlertDialog1.Show();
				}
				}
				catch
				{
					Toast.MakeText(this, "Error occurred", ToastLength.Short);

				}
			};

		}


	}
}

