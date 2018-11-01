using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IRRemote
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TcpClient TcpClient { get; } = new TcpClient();
        private object Locker { get; } = new object();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar =
                FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FindViewById<Button>(Resource.Id.tv_open).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(49); //0
                    Thread.Sleep(100);
                    SendTCP(51); //3
                });
            };
            FindViewById<Button>(Resource.Id.tv_change_input).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(50); //2
                });
            };
            FindViewById<Button>(Resource.Id.tv_enter).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(57); //9
                });
            };
            FindViewById<Button>(Resource.Id.volume_up).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(52); //4
                });
            };
            FindViewById<Button>(Resource.Id.volume_down).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(53); //5
                });
            };
            FindViewById<Button>(Resource.Id.switch_1).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(54); //6
                });
            };
            FindViewById<Button>(Resource.Id.switch_2).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(55); //7
                });
            };
            FindViewById<Button>(Resource.Id.switch_3).Click += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    SendTCP(56); //8
                });
            };
        }

        /*public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }*/

        private void SendTCP(byte str)
        {
            TcpClient.SendTimeout = 1000;
            TcpClient.ReceiveTimeout = 1000;
            lock (Locker)
            {
                try
                {
                    if (!TcpClient.Connected)
                        if (!TcpClient.ConnectAsync("192.168.1.101", 9000).Wait(1000))
                            throw new TimeoutException();
                    var stream = TcpClient.GetStream();
                    stream.WriteByte(str);
                    stream.WriteByte(10); // '\n'
                }
                catch
                {
                    if (TcpClient.Connected)
                        TcpClient.Close();
                    RunOnUiThread(() => Toast.MakeText(this, "Failed to send control message", ToastLength.Short).Show());
                }
            }
        }
    }
}

