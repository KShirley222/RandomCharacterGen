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
        public static async Task<SpellHelperModel> test()
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress+"api/spells/"))
                {
                    if (response.IsSuccessStatusCode)
                        {
                            SpellHelperModel spellarray = await response.Content.ReadAsAsync<SpellHelperModel>();
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response);
                            Console.WriteLine(spellarray.Results);
                            Console.WriteLine("*****STAR*****");
                            foreach (object Spell in spellarray.Results)
                            {;
                                Console.WriteLine(Spell);
                                SpellArrayHelper myJsonObject = (SpellArrayHelper)Spell;
                                Console.WriteLine(myJsonObject.url);
                            }
                            return spellarray;
                        }
                    else
                        {
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine("*****STAR*****");
                            Console.WriteLine(response.ReasonPhrase);
                            Console.WriteLine("*****STAR*****");
                            throw new Exception(response.ReasonPhrase);
                        }
                }
        }
    }
}