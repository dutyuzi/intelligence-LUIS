namespace LuisBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using System.Net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Web.Script.Serialization;
 

    //https://webchat.botframework.com/embed/Dut-Bot?s=0bm-Av1zeuk.cwA.8pU.tbFKX4x6PrRV6QEba5GsJ0nMkj1D_mq5Xk1e_WSDf4c
    [LuisModel("2958fbdd-d9a4-4166-88c5-86110e4788b9", "12d6ef7baf01488991b414e029b9e7db")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        private const string EntityGeographyCity = "builtin.geography.city";

        private const string EntityCollegeName = "院系";
        private const string EntityCityName = "省市地区";
        private const string EntityNoun = "名词";
        private const string EntityNumber = "数量";
        private const string EntityPeople = "人物";
        private const string Entityverb = "动词";
        private const string EntityAdress = "地址";


        private const string EntityAirportCode = "AirportCode";

        private IList<string> titleOptions = new List<string> { "“Very stylish, great stay, great staff”", "“good hotel awful meals”", "“Need more attention to little things”", "“Lovely small hotel ideally situated to explore the area.”", "“Positive surprise”", "“Beautiful suite and resort”" };

        public object MessageBox { get; private set; }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            IList<string> message = new List<string> { "你在说啥！本Bot还在学习中<(￣ˇ￣)/", "对不起，本Bot大脑短路了", "未知的世界还需要我去探索", "沉迷学习无法自拔<(￣ˇ￣)/", "我也不知道==" };
            Random rnd = new Random();
            int number = rnd.Next(0, 5);
            await context.PostAsync(message[number]);
            context.Wait(this.MessageReceived);

        }
        
      
        [LuisIntent("打招呼")]
        public async Task SayHello(IDialogContext context, LuisResult result)
        {
            IList<string> message = new List<string> { "你好！我是聪明的大工Bot(*^__^*)", "hi(*^__^*)", "你好呀(*^__^*)", "我是大工Bot(*^__^*)，有什么可以帮您？", "hi，亲爱的小伙伴(*^__^*)" };
            Random rnd = new Random();
            int number = rnd.Next(0, 5);
            await context.PostAsync(message[number]);
            context.Wait(this.MessageReceived);
        }


        [LuisIntent("查询录取分数线")]
        public async Task Search(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation YX;
            EntityRecommendation CITY;
            if (result.TryFindEntity(EntityCollegeName, out YX))
            {
                if(result.TryFindEntity(EntityCityName, out CITY))
                await context.PostAsync($"'{CITY.Entity}'的 '{YX.Entity}'的录取分数线是555");
            }

        

        }

        [LuisIntent("查询数量")]
        public async Task SearchNumber(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation Noun;
         
            if (result.TryFindEntity(EntityNoun, out Noun))
            {
                await context.PostAsync(QNaMaker("有多少" + Noun.Entity));
            }
            else
            {
              
            }

        }

        [LuisIntent("查询人物")]
        public async Task SearchPerson(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation People;

            if (result.TryFindEntity(EntityPeople, out People))
            {
                await context.PostAsync(QNaMaker(People.Entity+"是谁"));
                //await context.PostAsync((People.Entity));
            }
            else
            {
                await context.PostAsync("不知道要查谁");
            }

        }


        [LuisIntent("一般查询")]
        public async Task SearchOther(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation Noun;

            if (result.TryFindEntity(EntityNoun, out Noun))
            {
                await context.PostAsync(QNaMaker(Noun.Entity + "是什么"));
                //await context.PostAsync((People.Entity));
            }
            else
            {
                await context.PostAsync("大脑短路请稍等");
            }

        }
        [LuisIntent("查询地址")]
        public async Task SearchAdress(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation Noun;
            EntityRecommendation adress;
            if (result.TryFindEntity(EntityAdress, out adress))
            {
                if(result.TryFindEntity(EntityNoun, out Noun))
                {
        
                    await context.PostAsync(QNaMaker(Noun.Entity + "在哪里"));
                }
                else
                {
                
                    await context.PostAsync(QNaMaker("在哪里"));
                }
              
                //await context.PostAsync((People.Entity));
            }
            else
            {
                await context.PostAsync("大脑短路请稍等");
            }

        }
        


        [LuisIntent("一般疑问")]
        public async Task YesorNo(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            EntityRecommendation People;
            EntityRecommendation Noun;
            if (result.TryFindEntity(EntityPeople, out People))
            {
                string QNaAnswer = QNaMaker(People.Entity + "是谁");
                if (result.Query.Contains(QNaAnswer))
                {
                    await context.PostAsync("是呀是呀O(∩_∩)O嗯!");
                }
                else
                {
                    await context.PostAsync("不是这样的！~~~^_^~~~");
                }
            }
            else if (result.TryFindEntity(EntityNoun, out Noun))
            {
                string QNaAnswer = QNaMaker(Noun.Entity + "是什么");
                if (result.Query.Contains(QNaAnswer))
                {
                    await context.PostAsync("是呀是呀O(∩_∩)O嗯!");
                }
                else
                {
                    await context.PostAsync("不是这样的！~~~^_^~~~");
                }
            }
            else
            {

            }

        }


        [LuisIntent("查询时间")]
        public async Task QuestionTime(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            EntityRecommendation verb;
            EntityRecommendation Noun;
            string subject;


            if (result.TryFindEntity(Entityverb, out verb))
            {   
                if(result.TryFindEntity(EntityCollegeName, out Noun))
                {
                    subject = Noun.Entity;

                }
                else if (result.TryFindEntity(EntityNoun, out Noun))
                {
                    subject = Noun.Entity;
                }
                else
                {
                    subject = "";
                }

                string QNaAnswer = QNaMaker(subject+"什么时候" +verb.Entity );
                await context.PostAsync(QNaAnswer);
            }
            else
            {

            }

        }
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'search hotels in Seattle', 'search hotels near LAX airport' or 'show me the reviews of The Bot Resort'");

            context.Wait(this.MessageReceived);
        }

        private string  QNaMaker(string Query)
        {
            var query = Query; //User Query
            var knowledgebaseId = "d9c89203-5df6-42ed-b37e-4ed0b153e573"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "c29fc7e890934924865f4b5f45338282"; //Use subscription key assigned to you.
            string responseString = string.Empty;
            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;
                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
                //await context.PostAsync(responseString);
                int index = responseString.IndexOf("\"answer\"");
                string str = responseString.Substring(index + 10);
                int index1 = str.IndexOf("\"");
                string str1 = str.Substring(0, index1);

                return str1;
           
                /* 
                 * 反序列化总是为空 暂时没找到原因
                //De-serialize the response
                QnAMakerResult response;
                try
                {
                    response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
                    //throw new Exception(response.Answer);
                }
                catch
                {
                    throw new Exception("Unable to deserialize QnA Maker response string.");
                }

                await context.PostAsync(response.Answer);
                */

            }

        }

       
    }
}
