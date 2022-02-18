using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using DAL3;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace WebAppMobile.ViewModel
{
    public class GamesViewModel : INotifyPropertyChanged
    {


        public ICommand SomeCommand => new Command(async value => { await LoadData(); });

        private ObservableCollection<Game> _games;

        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadData()
        {
            var client = new HttpClient(GetInsecureHandler());
            var response = await client.GetAsync("http://10.0.2.2:5000/test/getBooks").ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync();
            var games = JsonConvert.DeserializeObject<ObservableCollection<Game>>(json);
            Games = games;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

    }
}
