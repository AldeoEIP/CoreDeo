using System.Net.Http;
using Xunit;

namespace CoreDeo.Test {
    public class Connection {
        [Fact]
        public async void CanConnect() {
            //public void CanConnect() {
            const string login = "Kevin69";
            const string password = "DUCHAMP";
            var client = new Client ();

            var connected = await client.LoginAsync (login, password);
            //var connected = client.LoginAsync (login, password);

            Assert.True (connected);
        }

        [Fact]
        //public async void CantConnect() {
        public async void CantConnect() {
            const string login = "";
            const string password = "";
            var client = new Client ();

            //var connected = await client.LoginAsync (login, password);
            //var connected = ;

            await Assert.ThrowsAsync<HttpRequestException>(async () => await client.LoginAsync (login, password));
        }
    }
}