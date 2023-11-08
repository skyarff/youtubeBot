using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace YoutubeDownloadBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string token = ConfigurationManager.AppSettings["token"];
        public static string DIR = ConfigurationManager.AppSettings["DIR"];
        public static TelegramBotClient client;
        public static string filePath = "";
        public static string requireQuality = "720";
        public static bool isDownloading = false;
        public static Dictionary<string, string> patterns = new Dictionary<string, string>()
        {
            ["2160"] = "contentLength\":\"(\\d)+\",\"quality\":\"hd2160\",",
            ["1440"] = "contentLength\":\"(\\d)+\",\"quality\":\"hd1440\",",
            ["1080"] = "contentLength\":\"(\\d)+\",\"quality\":\"hd1080\",",
            ["720"] = "contentLength\":\"(\\d)+\",\"quality\":\"hd720\",",
            ["480"] = "contentLength\":\"(\\d)+\",\"quality\":\"large\",",
            ["360"] = "contentLength\":\"(\\d)+\",\"quality\":\"medium\",",
            ["240"] = "contentLength\":\"(\\d)+\",\"quality\":\"small\",",
            ["144"] = "contentLength\":\"(\\d)+\",\"quality\":\"tiny\",",
        };
        public static Queue<string> queue = new Queue<string>();

        async private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = new Icon(Directory.GetCurrentDirectory() + "\\1.ico");
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.High;


            textBox1.Text = (ConfigurationManager.AppSettings["token"]).ToString();
            textBox2.Text = (ConfigurationManager.AppSettings["DIR"]).ToString();
            if (!Directory.Exists(ConfigurationManager.AppSettings["DIR"])) Directory.CreateDirectory(ConfigurationManager.AppSettings["DIR"]);


            client = new TelegramBotClient(token);
            client.StartReceiving(Update, Error);


        }


        async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {

            var message = update.Message;

            if (message.Text != null)
            {
                string[] mes = message.Text.Split('-');

                if (mes[0].ToLower() == $"show")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $" - поиск качества: до {requireQuality}\n - скачивание: {isDownloading}");
                }
                else if (mes[0].ToLower() == $"set")
                {
                    bool flag1 = false;

                    foreach (var item in patterns)
                    {
                        if (item.Key == mes[1])
                        {
                            flag1 = true;
                        }
                    }

                    if (flag1)
                    {
                        requireQuality = mes[1];
                        await client.SendTextMessageAsync(message.Chat.Id, $" - установлен поиск качества до {mes[1]}!");
                    }
                    else
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, $" - неверное значение!");
                    }

                }
                else if (mes[0].ToLower() == $"catalog")
                {

                    string[] videos = Directory.GetFiles(DIR);

                    string output = "";
                    for (int i = 0; i < videos.Length; i++)
                    {

                        output += $"{i + 1}: " + videos[i].Substring(10) + "\n";
                    }

                    await client.SendTextMessageAsync(message.Chat.Id, output);

                }
                else if (mes[0].ToLower() == $"q")
                {

                    string s1 = $"—качиваетс€: {queue.Count} видео!";

                    await client.SendTextMessageAsync(message.Chat.Id, s1);

                }
                else if (mes[0].ToLower() == $"get")
                {
                    try
                    {
                        string[] videos = Directory.GetFiles(DIR);

                        if (mes.Length > 1)
                        {
                            int k = Convert.ToInt32(mes[1]) - 1;

                            using (FileStream fs = new FileStream(videos[k], FileMode.Open))
                            {
                                InputFileStream file = new InputFileStream(fs);
                                await client.SendVideoAsync(message.Chat.Id, file);
                            }
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                            {
                                InputFileStream file = new InputFileStream(fs);
                                await client.SendVideoAsync(message.Chat.Id, file);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, " - ошибка получени€!");
                        return;
                    }

                }
                else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text && (Uri.TryCreate(message.Text, UriKind.Absolute, out Uri uri)))
                {
                    queue.Enqueue(message.Text);

                    await client.SendTextMessageAsync(message.Chat.Id, "ƒобавлено в очередь..");

                    if (!isDownloading)
                    {

                        Thread newThread = new Thread(() =>
                        {
                            ParseHTML(client, message, requireQuality, patterns);
                        });

                        newThread.Priority = ThreadPriority.Highest;
                        newThread.Start();
                    }

                }
                else if (mes[0].ToLower() == $"help")
                {
                    string guide = " оротка€ справка команд:" +
                                "\n\nshow - получить отклик от приложени€" +
                                "\n\nq - количество скачиваемых видео" +
                                "\n\nset-nnn - установить искомое качество" +
                                "\n\ncatalog - получить список названий из дериктории" +
                                "\n\nget-n получить n в каталоге" +
                                "\n\nскачивание видео по ссылке";

                    await client.SendTextMessageAsync(message.Chat.Id, guide);
                }
            }


            return;
        }
        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public static void OverlayAudio(string videoPath, string audioPath, string outputVideoPath)
        {
            var process = new Process();

            process.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\ffmpeg.exe";
            process.StartInfo.Arguments = $"-i {videoPath} -i {audioPath} -codec copy -shortest {outputVideoPath}";

            process.StartInfo.UseShellExecute = true;

            process.Start();
            process.WaitForExit();
        }
        async public static void ParseHTML(ITelegramBotClient client, Telegram.Bot.Types.Message message, string requireQuality, Dictionary<string, string> patterns)
        {
            while (queue.Count > 0)
            {
                try
                {
                    isDownloading = true;
                    var videoUrl = queue.Peek();

                    var httpClient = new HttpClient();
                    var response = httpClient.GetAsync(videoUrl).Result;
                    var html = response.Content.ReadAsStringAsync().Result;


                    var title1 = Regex.Match(html, @"<title>.*</title>", RegexOptions.IgnoreCase).ToString();
                    title1 = title1.Substring(7, title1.Length - 25);
                    title1 = title1.Replace("&quot;", "").Replace("\"", "").Replace("\\", "")
                        .Replace("/", "").Replace(":", "").Replace("?", "").Replace("<", "")
                        .Replace(">", "").Replace(" ", "_");


                    var m31 = Regex.Match(html, @$"https://rr(\d)(\S*),", RegexOptions.IgnoreCase).ToString();
                    var m32 = m31.Substring(0, m31.Length - 2);
                    var m33 = m32!.Replace("\\u0026", "&");


                    bool flag = false;
                    List<System.Text.RegularExpressions.Match> m11 = null;
                    foreach (var pat in patterns)
                    {
                        if (pat.Key == requireQuality) flag = true;

                        if (flag)
                        {
                            m11 = Regex.Matches(html, pat.Value, RegexOptions.IgnoreCase).ToList();

                            if (m11.Count > 0) break;
                        }
                    }

                    string m23 = "";
                    try
                    {
                        var m12 = m11.Select(x => x.Value.Substring(16, 7)).ToList();

                        var m21 = Regex.Match(html, @$"https://rr(\d)(\S*){m12[0]}(\S*),", RegexOptions.IgnoreCase).ToString();
                        var m22 = m21.Substring(0, m21.Length - 2);
                        m23 = m22!.Replace("\\u0026", "&");
                    }
                    catch { }


                    string f_v = DIR + "\\_V.mp4";
                    string f_a = DIR + "\\_A.mp4";
                    filePath = DIR + $"\\{title1}.mp4";



                    using (var webClient = new WebClient())
                    {
                        try
                        {
                            if (m23 != "")
                            {
                                webClient.DownloadFile(m23, f_v);
                                webClient.DownloadFile(m33, f_a);

                                OverlayAudio(f_v, f_a, filePath);

                                System.IO.File.Delete(f_v);
                                System.IO.File.Delete(f_a);
                            }
                            else
                            {
                                webClient.DownloadFile(m33, filePath); //m33
                            }
                        }
                        catch
                        {
                            //«амена в url символов '%2F', '%3F', '%3D', '%26', '%2523' на '/', '?', '=', '&', '#'

                            webClient.DownloadFile(m33, filePath); //m33
                        }
                    }
                    isDownloading = false;
                    queue.Dequeue();
                    await client.SendTextMessageAsync(message.Chat.Id, $" {title1}\n - скачал!");
                }
                catch (Exception)
                {
                    isDownloading = false;
                    queue.Dequeue();
                    await client.SendTextMessageAsync(message.Chat.Id, $" - ошибка скачивани€!");
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["token"].Value = textBox1.Text;

            config.Save(ConfigurationSaveMode.Modified);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["DIR"].Value = textBox2.Text;

            config.Save(ConfigurationSaveMode.Modified);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            NotifyIcon notifyIcon = new NotifyIcon();

            this.Hide();
            notifyIcon.Visible = true;
            notifyIcon.Icon = new Icon("1.ico");
            notifyIcon.Text = "YoutubeDownloadBot";



            void notifyIcon_DoubleClick(object sender, EventArgs e)
            {
                this.Show();
                notifyIcon.Visible = false;
            }
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_DoubleClick);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
