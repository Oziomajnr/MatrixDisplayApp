
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
	[Activity (Label = "ChangePassword")]			
	public class ChangePassword : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Toast toast = new Toast(this);
			toast.SetGravity(GravityFlags.Center,0,0);
			SetContentView (Resource.Layout.ChangePassword);
			Button btnChangePassword = FindViewById<Button> (Resource.Id.btnChangePassword);
			EditText txtOldPassword  = FindViewById<EditText> (Resource.Id.txtOldPassword);
			EditText txtNewPassword  = FindViewById<EditText> (Resource.Id.txtNewPassword);
			EditText txtcNewPassword = FindViewById<EditText>(Resource.Id.txtcNewPassword);
			AlertDialog.Builder AlertDialog1 = new AlertDialog.Builder (this);
			AlertDialog1.SetTitle ("Error");

			btnChangePassword.Enabled = false;

			txtcNewPassword.TextChanged += delegate {

				if(txtcNewPassword.Text.Length == 4 && txtNewPassword.Text.Length  == 4 && txtOldPassword.Text.Length  == 4)
				{
					btnChangePassword.Enabled = true;

				}

				else
				{
					btnChangePassword.Enabled = false;
				}

			};

			txtNewPassword.TextChanged += delegate {

				if(txtcNewPassword.Text.Length == 4 && txtNewPassword.Text.Length == 4 && txtOldPassword.Text.Length  == 4)
				{
					btnChangePassword.Enabled = true;

				}

				else
				{
					btnChangePassword.Enabled = false;
				}

			};

			txtOldPassword.TextChanged += delegate {

				if(txtcNewPassword.Text.Length  == 4 && txtNewPassword.Text.Length  == 4 && txtOldPassword.Text.Length  == 4)
				{
					btnChangePassword.Enabled = true;

				}

				else
				{
					btnChangePassword.Enabled = false;
				}

			};
			btnChangePassword.Click += delegate {
				
				if(txtcNewPassword.Text == String.Empty || txtNewPassword.Text == String.Empty ||txtOldPassword.Text == String.Empty)
				{
					AlertDialog1.SetMessage("No field should be left empty");
					AlertDialog1.Show();
				}
				else	if (txtNewPassword.Text != txtcNewPassword.Text)
				{
					AlertDialog1.SetMessage("New Password does not match");
					AlertDialog1.Show();
				}

				else
				{
					if (MainActivity.socket != null)
					{
						byte[] passwordbyte = new byte[14];
						passwordbyte[0] = 72;
						passwordbyte[1] = 67;
						passwordbyte[2] = 1;
						passwordbyte[3] = 9;
						passwordbyte[4] = 8;

						byte[] suc = new byte[2];
						Char[] passwordChar = (txtOldPassword.Text + txtNewPassword.Text).ToCharArray();

						for(int x = 5; x <= 12; x++)
						{
							passwordbyte[x] = (byte)((int)((byte)passwordChar[x-5]) - 48);

						}
						byte checksum = 0;

						for(int x =0; x<passwordbyte.Length-1; x++)
						{
							checksum += passwordbyte[x];
						}
						passwordbyte[13] = checksum;

						MainActivity.socket.OutputStream.Write(passwordbyte,0,passwordbyte.Length);
						Thread.Sleep(500);
						MainActivity.socket.InputStream.Read(suc,0,2);
						Thread.Sleep(500);
						if(suc[0] == 79 && suc[1] == 75 )
						{
							toast	= Toast.MakeText(this,"Password Change Successful", ToastLength.Long);
							toast.Show();
						}

						else if(suc[0] == 69 && suc[1] == 82)
						{
							toast =	Toast.MakeText(this,"Old Password incorrect", ToastLength.Long);
							toast.Show();
						}

						else
						{
							
							toast = Toast.MakeText(this,"Password Change unsuccessful", ToastLength.Long);
							toast.Show();
						}
					}
					else
					{
						Toast.MakeText(this, "Not Connected To Device", ToastLength.Long).Show();

					 }
				}
			};

		}
	}
}

