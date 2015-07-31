using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WatsonTests
{
	[Activity (Label = "WatsonTests", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += async delegate {
				var data = await PostAsync (@"https://gateway.watsonplatform.net/visual-recognition-beta/api/v1/tag/recognize");
			};
		}

		public async Task<string> PostAsync (string url)
		{
			try {

				var bitmap = ResourceLoader.GetEmbeddedResourceBytes ("Steve.jpg");

				var multiPartContent = new MultipartFormDataContent ();
				var byteArrayContent = new ByteArrayContent (bitmap);
				byteArrayContent.Headers.Add ("Content-Type", "image/jpeg");
				multiPartContent.Add (byteArrayContent, "img_File", "Steve.jpg");
				using (var client = new HttpClient ()) {
					client.DefaultRequestHeaders.Accept.Clear ();
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Basic", GetBase64CredentialString ());
					var response = await client.PostAsync (url, multiPartContent);
					return await HandleResponseAsync (response);
				}
			} catch (Exception ex) {
				return ex.Message;
			}

		}


		async Task<string> HandleResponseAsync (HttpResponseMessage response)
		{
			string bb = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
			return bb;
		}

		private string GetCredentialString ()
		{
			return string.Format ("{0} {1}", "Basic", GetBase64CredentialString ());
		}

		private string GetBase64CredentialString ()
		{

			var auth = string.Format ("{0}:{1}", "26ff15e6-360d-425d-8b2e-4132625d7e3d", "vI3Bh80PSYew");
			return Convert.ToBase64String (Encoding.UTF8.GetBytes (auth));
		}
	}
}


