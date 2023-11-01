using System.Net.Sockets;
using System.Text;
using NLog;

namespace UpdServer
{
    public class Server
    {
        private const int Port = 8001;
        private const string FilePath = "File.csv";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<School> SchoolList = new List<School>();
        static async Task Main()
        {
            using UdpClient udpServer = new UdpClient(Port);
            SchoolList = ReadAll();

            logger.Info("Сервер запущен и ждет подключения...");

            while (true)
            {
                var result = await udpServer.ReceiveAsync();
                string request = Encoding.UTF8.GetString(result.Buffer);

                logger.Info($"Запрос получен от {result.RemoteEndPoint}: {request}");

                string response = ProcessRequest(request);
                byte[] responseData = Encoding.UTF8.GetBytes(response);

                _ = udpServer.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                logger.Info($"Запрос отправлен: {result.RemoteEndPoint}: {response}");
            }
        }
        private static List<School> ReadAll()
        {
            if (File.Exists(FilePath))
            {
                using StreamReader reader = new StreamReader(FilePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');

                    if (data.Length == 4)
                    {
                        School school = new School()
                        {
                            Name = data[0].Trim(),
                            Surname = data[1].Trim(),
                            Age = int.Parse(data[2].Trim()),
                            IsBad = bool.Parse(data[3].Trim()),
                        };
                        SchoolList.Add(school);
                    }
                }
            }
            return SchoolList;
        }
        private static string ProcessRequest(string request)
        {
            string[] data = request.Split(",");
            string str = data[0];

            switch (str)
            {
                case "1":
                    return GetAllRecords();
                case "2":
                    try
                    {
                        return GetRecordByNumber(int.Parse(data[1]) - 1);
                    }
                    catch (Exception)
                    {
                        return $"{data[1]} не является номером";
                    }
                case "3":
                    bool isDeleted = DeleteRecordByNumber(int.Parse(data[1]) - 1);
                    if (isDeleted)
                    {
                        string response = "Запись успешно удалена.";
                        logger.Info("Запись успешно удалена");
                        return response;
                    }
                    else
                    {
                        string response = "Запись не найдена.";
                        logger.Info("Запись не найдена");
                        return response;
                    }
                case "4":
                    try
                    {
                        AddRecord(data[1], data[2], int.Parse(data[3]), bool.Parse(data[4]));
                        return "Запись добавлена";
                    }
                    catch (Exception)
                    {
                        return "Данные неверные";
                    }
                case "5":
                    return DeletAll();

                default:
                    return "Недопустимая команда";
            }
        }
        private static string GetAllRecords()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SchoolList.Count; i++)
            {
                string sch = $"\nЗапись {i + 1}: \nИмя: {SchoolList[i].Name}\nФамилия: {SchoolList[i].Surname}\nВозраст: {SchoolList[i].Age}\nЕсть ли двойки в четверти?: {SchoolList[i].IsBad}";
                sb.AppendLine(sch);
            }
            return sb.ToString();
        }

        private static string GetRecordByNumber(int number)
        {
            if (number >= 0 && number < SchoolList.Count)
            {
                return $"\nЗапись {number + 1}\nИмя: {SchoolList[number].Name}\nФамилия: {SchoolList[number].Surname}\nВозраст: {SchoolList[number].Age}\nЕсть ли двойки в четверти?: {SchoolList[number].IsBad}";
            }
            return "Запись не найдена.";
        }

        private static bool DeleteRecordByNumber(int number)
        {
            if (number >= 0 && number < SchoolList.Count)
            {
                SchoolList.RemoveAt(number);
                SaveData();
                return true;
            }
            return false;
        }

        private static string DeletAll()
        {
            File.WriteAllText(FilePath, string.Empty);
            SchoolList.Clear();
            return "Все записи удалены";
        }

        private static void AddRecord(string Name, string Surname, int Age, bool IsBad)
        {
            SchoolList.Add(new School { Name = Name, Surname = Surname, Age = Age, IsBad = IsBad });
            SaveData();
        }

        private static void SaveData()
        {
            using var writer = new StreamWriter(FilePath);
            foreach (School school in SchoolList)
            {
                string line = $"{school.Name} , {school.Surname} , {school.Age} , {school.IsBad}";
                writer.WriteLine(line);
            }
        }
    }
}



