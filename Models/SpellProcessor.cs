using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

// *
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CharacterGenerator.Models
{
    public class SpellProcessor
    {
        public static async Task<SpellArrayHelperModel> test(string addonstring)
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress+addonstring))
                {
                    if (response.IsSuccessStatusCode)
                        {
                            SpellArrayHelperModel spell = await response.Content.ReadAsAsync<SpellArrayHelperModel>();
                            // Console.WriteLine("*****STAR*****");
                            // Console.WriteLine(response);
                            // Console.WriteLine(spellarray);
                            // Console.WriteLine("*****STAR*****");
                            // foreach (object Spell in spellarray.Results)
                            // {;
                            //     Console.WriteLine(Spell);
                            //     // SpellArrayHelper myJsonObject = (SpellArrayHelper)Spell;
                            //     // Console.WriteLine(myJsonObject.url);
                            // }
                            Console.WriteLine(ApiHelper.ApiClient.BaseAddress+addonstring);
                            return spell;
                        }
                    else
                        {
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(ApiHelper.ApiClient.BaseAddress+addonstring);
                            throw new Exception(response.ReasonPhrase);
                        }
                }
        }
    }
}