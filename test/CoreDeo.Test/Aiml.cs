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
        public async void CanUseWiki() {
            const string question = "Quel est la définition de poney?";
            const string login = "Kevin69";
            const string password = "DUCHAMP";
            var client = new Client ();

            var connected = await client.LoginAsync (login, password);
            Assert.True (connected);

            var answer = await client.GetAnswerAsync (question);
            Assert.Equal ("\n== Français ==\n\n\n=== Étymologie ===\nDe l’anglais pony lui-même issu de l’ancien français poulenet, diminutif médiéval de poulain.\n\n\n=== Nom commun ===\nponey \\pɔ.nɛ\\ masculin (équivalent féminin : ponette)\n\nCheval de petite taille (adulte moins de 1,49 m de hauteur au garrot), au trot rapide et sec, servant maintenant surtout pour l'agrément, souvent cheval de loisir, cheval de selle ou cheval polyvalent.\n[…] la comtesse Vanska, avec ses poneys pie ; madame Daste, et ses fameux stappers noirs ; madame de Guende et madame Tessière, en coupé. — (Émile Zola, La Curée, 1871)\nLe poney est le seul moyen de transport dans ce pays dépourvu de voies de communication; il est à l’Islandais ce que le chameau est au Bédouin : il porte le voyageur et tous ses impedimenta. — (Jules Leclercq, La Terre de glace, Féroë, Islande, les geysers, le mont Hékla, Paris : E. Plon & Cie, 1883, p.9)\nJ'avais laissé Reykjavik, un grand village ; […] ; des poneys et une ou deux mauvaises carrioles assuraient le transport. — (Jean-Baptiste Charcot, Dans la mer du Groenland, 1928)\n\n\n==== Dérivés ====\n\n\n==== Traductions ====\n\n\n==== Hyperonymes ====\ncheval (Equus caballus)\n\n\n==== Hyponymes ====\n(exemples de races)\n\n\n=== Prononciation ===\nFrance  : écouter « un poney [œ̃ pɔ.nɛ] »\n\n\n=== Voir aussi ===\nponey sur Wikipédia \n\n\n=== Références ===\nTout ou partie de cet article est extrait du Dictionnaire de l’Académie française, huitième édition, 1932-1935 (poney), mais l’article a pu être modifié depuis.\nChristian Meyer, éditeur scientifique, Dictionnaire des sciences animales, Cirad, Montpellier, France, 2017", answer);
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