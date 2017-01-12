using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var callButton = FindViewById<Button>(Resource.Id.CallButton);

            DisbableButton(callButton);

            var translatedNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
            {
                translatedNumber = TranslateUsersAlphanumericPhoneNumberToNumeric(phoneNumberText, callButton);
            };

            callButton.Click += (object sender, EventArgs e) =>
            {                
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });                
                callDialog.Show();
            };

        }

        private static void DisbableButton(Button button)
        {
            button.Enabled = false;
        }

        private static string TranslateUsersAlphanumericPhoneNumberToNumeric(EditText phoneNumberText, Button callButton)
        {
            string translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
            if (string.IsNullOrWhiteSpace(translatedNumber))
            {
                callButton.Text = "Call";
                callButton.Enabled = false;
            }
            else
            {
                callButton.Text = "Call " + translatedNumber;
                callButton.Enabled = true;
            }

            return translatedNumber;
        }
    }
}

