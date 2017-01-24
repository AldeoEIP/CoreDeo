using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CoreDeo.Test {
    public class Aiml {
        [Fact]
        public async void CanAnswer() {
            const string question = "Quel est mon nom ?";
            const string login = "Kevin69";
            const string password = "DUCHAMP";
            var client = new Client ();

            var connected = await client.LoginAsync (login, password);
            Assert.True (connected);

            var answer = await client.GetAnswerAsync (question);
            Assert.Equal ("ton nom est Toto.", answer);
        }

        [Fact]
        public async void CanCalcul() {
            const string question = "Combien font 2 + 2 ?";
            const string login = "Kevin69";
            const string password = "DUCHAMP";
            var client = new Client ();

            var connected = await client.LoginAsync (login, password);
            Assert.True (connected);

            var answer = await client.GetAnswerAsync (question);
            Assert.Equal ("4", answer);
        }

        [Fact]
        public async void CanAvoidEmpty() {
            const string question = "";
            const string login = "Kevin69";
            const string password = "DUCHAMP";
            var client = new Client ();

            var connected = await client.LoginAsync (login, password);
            Assert.True (connected);

            await Assert.ThrowsAsync<HttpRequestException> (async () => await client.GetAnswerAsync (question));
        }
    }
}