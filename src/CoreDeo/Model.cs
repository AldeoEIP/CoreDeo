using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreDeo {

    public class Answer {
        public string BotSay { get; set; }
    }

    public class Knowledge {
        public string Extract { get; set; }
        public string Title { get; set; }
        public string DetailUrl { get; set; }
    }

    public class Noob : TheoricUser {
        public string CompanionName { get; set; }
    }

    public class ServerDateTime {
        public string Date { get; set; }
        [JsonProperty ("timezone_type")]
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }

    public class TheoricUser {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Translation {
        public string TranslatedText { get; set; }
        public string LangTo { get; set; }
        public string LangFrom { get; set; }
    }

    public class User : TheoricUser {
        public int Id { get; set; }
        public string Salt { get; set; }
        public object GradeOther { get; set; }
        public ServerDateTime Created { get; set; }
        public ServerDateTime Modified { get; set; }
    }
}
