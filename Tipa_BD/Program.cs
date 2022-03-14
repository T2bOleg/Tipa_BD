using System.Text;


namespace MyBD
{
    class Program
    {
        static void Main(string[] args)
        {

            string data_file = @"D:\kinozaly.tipaBD";
            List<Zal> zal_list = new List<Zal>();
            Read_data();
            Console.Clear();
            while (true)
            {

                foreach (var _zal in zal_list)
                    Console.WriteLine(_zal.ToString());
                string comand = Console.ReadLine();
                comand = comand.Trim();
                switch (comand)
                {
                    case "help":
                        Console.Clear();
                        Console.WriteLine("new - новый зал\nadd - добавить сеанс\ndel - удалить сеанс\nupd - изменить количество билетов\nbuy - продать билеты\nref - вернуть билеты\nsave - сохранить данные и выйти\nquit - выйти без сохраненя");
                        break;
                    case "add":
                        Console.Clear();
                        Console.Write("Введите номер зала: ");
                        try
                        {
                            int z = int.Parse(Console.ReadLine());
                            Console.Write("Введите имя сеанса: ");
                            string n = Console.ReadLine();
                            Console.Write("Введите количество свободных мест: ");
                            int b = int.Parse(Console.ReadLine());
                            zal_list[z - 1].add_seans(n, b);
                            Console.Clear();
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Error");
                        }
                        break;
                    case "new":
                        zal_list.Add(new Zal(zal_list.Count + 1));
                        Console.Clear();
                        break;
                    case "del":
                        try
                        {
                            Console.Write("Введите номер зала: ");
                            int z = int.Parse(Console.ReadLine());
                            Console.Write("Введите имя сеанса: ");
                            string n = Console.ReadLine();
                            Console.Clear();
                            if (zal_list[z - 1].del_seans(n))
                                Console.WriteLine("Успешно");
                            else
                                Console.WriteLine("Сеанс не найден");
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Error");
                        }
                        break;
                    case "upd":
                        try
                        {
                            Console.Write("Введите номер зала: ");
                            int z = int.Parse(Console.ReadLine());
                            zal_list[z - 1].upd_seans();
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Error");
                        }
                        break;
                    case "buy":
                        try
                        {
                            Console.Write("Введите номер зала: ");
                            int z = int.Parse(Console.ReadLine());
                            zal_list[z - 1].upd_seans(false, false);
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Error");
                        }
                        break;
                    case "ref":
                        Console.Clear();
                        Console.Write("Введите номер зала: ");
                        try
                        {
                            int z = int.Parse(Console.ReadLine());
                            Console.Clear();
                            zal_list[z - 1].upd_seans(false);

                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine("Error");
                        }
                        break;
                    case "save":
                        Write_data();
                        Environment.Exit(0);
                        break;
                    case "quit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Ошибка команды");
                        break;

                }

            }



            bool Read_data()
            {

                FileStream fs = new FileStream(data_file, FileMode.OpenOrCreate);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                List<string> list_data = Encoding.Default.GetString(buffer).Split('#').ToList<string>();
                int num = 0;
                zal_list.Clear();
                foreach (string str in list_data)
                {

                    List<string> _list_data = str.Split(':').ToList<string>();
                    zal_list.Add(new Zal(num + 1));
                    for (int i = 0; i < _list_data.Count - 1; i += 2)
                    {
                        zal_list[num].add_seans(_list_data[i], int.Parse(_list_data[i + 1]));
                    }
                    num++;
                }
                fs.Close();
                return true;
            }
            void Write_data()
            {
                File.Delete(data_file);
                FileStream fs = new FileStream(data_file, FileMode.Create);
                string data = "";

                foreach (var _zal in zal_list)
                {
                    data += _zal.Pack() + "#";
                }
                data = data.Remove(data.Length - 1);
                fs.Write(Encoding.Default.GetBytes(data));
                fs.Close();


            }
        
    }
}
    class Seans
    {
        public Seans(string name, int bilets)
        {
            this.name = name;
            mesta = bilets;
        }
        public string name;
        public int mesta;
        public string Pack() => name + ":" + mesta.ToString();
        public void update(int value, bool upd = true)
        {
            if (upd)
                mesta = value;
            else mesta += value;
        }
        public override string ToString()
        {
            return name + ": " + mesta;
        }
    }
    class Zal
    {
        public Zal(int num)
        {
            seanses = new List<Seans>();
            this.num = num;
        }
        public List<Seans> seanses;
        int num;
        public void add_seans(string name, int bilets) => seanses.Add(new Seans(name, bilets));
        public string Pack()
        {
            string p = "";
            for (int i = 0; i < seanses.Count; i++) p += (i != 0 ? ":" : "") + seanses[i].Pack();
            return p;
        }
        public bool del_seans(string name)
        {
            for (int i = 0; i < seanses.Count; i++)
            {
                if (seanses[i].name == name)
                {
                    seanses.Remove(seanses[i]);
                    return true;
                }
            }
            return false;
        }
        public void upd_seans(bool update = true, bool plus = true)
        {
            Console.Write("Введите имя сеанса: ");
            string name = Console.ReadLine();
            Console.Write("Количество мест мест: ");
            int value = int.Parse(Console.ReadLine());
            if (!update && !plus) value *= -1;
            Console.Clear();
            bool b = true;
            for (int i = 0; i < seanses.Count; i++)
            {
                if (seanses[i].name == name)
                {
                    seanses[i].update(value, update);
                    Console.WriteLine("Успешно");
                    b = false;
                    break;
                }
            }
            if (b) Console.WriteLine("Сеанс не найден");
        }
        public override string ToString()
        {
            string s = num.ToString();
            for (int i = 0; i < seanses.Count; i++)
            {
                s += "\n\t" + seanses[i].ToString();
            }
            return s;
        }
    }
}