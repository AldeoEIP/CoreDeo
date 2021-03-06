﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreDeo {
    public class Client : IDisposable {
        private readonly HttpHelper _httpHelper = new HttpHelper(BaseUrl);
        private const string BaseUrl = "https://aldeo.io/api/";
        private string Token { get; set; }

        // open to refactoring
        public async Task<bool> LoginAsync(string login, string password) {
        //public bool LoginAsync(string login, string password) {
            //try {
            var encodedContent = new FormUrlEncodedContent (
                new Dictionary<string, string> { { "login", login }, { "password", password } });

            var httpResponse = await _httpHelper.PostAsync ("users/login", encodedContent).ConfigureAwait (false);

            httpResponse.EnsureSuccessStatusCode ();
            var content = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);


            Token = content.Replace ("\"", string.Empty);
            return true;
            //}
            //catch (Exception ex) {
            //    Debug.WriteLine (ex.Message);
            //    return false;
            //}
        }

        // dead == 403 error
        public async Task<bool> IsStillAliveAsync() {
            var isAlive = await _httpHelper.GetAsync (Token, "users/stillalive").ConfigureAwait (false);
            throw new NotImplementedException ();
        }

        #region user
        public Task<IEnumerable<User>> GetUsersAsync() {
            const string requestUri = "users";
            return GetTableAsync<User> (Token, requestUri);
        }

        public async Task<User> GetUserAsync(int id) {
            const string requestUri = "users";
            var json = await _httpHelper.GetAsync (Token, $"{requestUri}/{id}").ConfigureAwait (false);
            var table = JsonConvert.DeserializeObject<User> (json);
            return table;
        }

        public async Task<string> CreateUserAsync(Noob noob) {
            const string requestUri = "users";
            var response = await CreateFieldAsync (noob, Token, requestUri).ConfigureAwait (false);
            // check for fail
            return response;
        }
        #endregion
        #region wiki
        public Task<Knowledge> GetDictionaryDefintionAsync(string word) {
            const string requestUri = "wiktionaries/getDefinition";
            const string key = "word";
            return GetKnowledgeAsync (requestUri, key, word.ToLower ());
        }

        public Task<Knowledge> GetEncyclopediaDefintionAsync(string word) {
            const string requestUri = "wikipedias/getExtract";
            const string key = "text";
            return GetKnowledgeAsync (requestUri, key, word.ToLower ());
        }

        public async Task<Knowledge> GetKnowledgeAsync(string requestUri, string key, string notion) {
            var json = await _httpHelper.PostAsync (Token, requestUri, key, notion).ConfigureAwait (false);
            var knowledge = JsonConvert.DeserializeObject<Knowledge> (json);
            return knowledge;
        }
        #endregion
        #region translation
        public async Task<Translation> TranslateToAsync(string requestUri, string to, string text) {
            return await TranslateAsync (Token, "translators/translate", new Dictionary<string, string> {
                { "langTo", to },
                { "text", text }
            }).ConfigureAwait (false);
        }

        public async Task<Translation> TranslateFromAsync(string requestUri, string from, string to, string text) {
            return await TranslateAsync (Token, "translators/translateFrom", new Dictionary<string, string> {
                { "langFrom", from },
                { "langTo", to },
                { "text", text }
            }).ConfigureAwait (false);
        }

        public async Task<Translation> TranslateAsync(string token, string requestUri, IDictionary<string, string> content) {
            var json = await _httpHelper.PostCollectionAsync (token, "translators/translateFrom", content).ConfigureAwait (false);
            var translation = JsonConvert.DeserializeObject<Translation> (json);
            return translation;
        }
        #endregion
        #region aiml
        public async Task<string> GetAnswerAsync(string question) {
            const string requestUri = "aimls/answer";
            const string key = "text";
            var json = await _httpHelper.PostAsync (Token, requestUri, key, question);
            var answer = JsonConvert.DeserializeObject<Answer> (json);
            return answer.BotSay;
        }
        #endregion
        #region math
        // avoir le résultat d’un calcul
        public Task<string> ResolveAsync(string input) {
            const string requestUri = "wolframalphas/getMath";
            const string key = "input";
            return _httpHelper.PostAsync (Token, requestUri, key, input.ToLower ());
        }
        #endregion
        #region field
        // http://stackoverflow.com/questions/4737970/what-does-where-t-class-new-mean
        // voir tous les champs existants
        public Task<IEnumerable<T>> GetTableAsync<T>(string token, string requestUri) where T : class {
            return GetFieldAsync<IEnumerable<T>> (token, requestUri);
        }

        // voir un champs
        // ne supporte pas /light
        public async Task<T> GetFieldAsync<T>(string token, string requestUri) where T : class {
            var json = await _httpHelper.GetAsync (token, requestUri).ConfigureAwait (false);
            var t = JsonConvert.DeserializeObject<T> (json);
            return t;
        }

        //créer un champs
        public Task<string> CreateFieldAsync<T>(T field, string token, string requestUri) {

            Func<string, string> e = s => $"\"s\"";

            using (var multipartFormDataContent = new MultipartFormDataContent ()) {
                foreach (var keyValuePair in new[]
                {
                    new KeyValuePair<string, string>("login", "TheLordOfTheRings"),
                    new KeyValuePair<string, string>("email", "Soso91@hotmail.fr"),
                    new KeyValuePair<string, string>("password", "1ringToRule"),
                    new KeyValuePair<string, string>("firstname", "Sauron"),
                    new KeyValuePair<string, string>("lastname", "Duhamel"),
                    new KeyValuePair<string, string>("companionName", "Saroumane")
                })
                    multipartFormDataContent.Add (new StringContent (keyValuePair.Value), e (keyValuePair.Key));

                //var encodedContent = new FormUrlEncodedContent(field.ToDictionaryOfString<T>());
                var httpContent = multipartFormDataContent;
                return _httpHelper.SendAsync (token, HttpMethod.Post, requestUri + "/create", httpContent);
            }
        }

        // editer un champs
        //public static Task EditFieldAsync<T>(string token, string requestUri) {
        //    var httpContent = new StringContent (string.Empty);  // should add user in content as json?
        //    return SendAsync (token, HttpMethod.Put, requestUri, httpContent);
        //}

        // supprimer un champs
        //public static Task DeleteFieldAsync<T>(string token, string requestUri) {
        //    var httpContent = new StringContent (string.Empty);  // should add user in content as json?
        //    return SendAsync (token, HttpMethod.Delete, requestUri, httpContent);
        //}
        #endregion

        public void Dispose() {
            _httpHelper.Dispose ();
        }
    }
}
