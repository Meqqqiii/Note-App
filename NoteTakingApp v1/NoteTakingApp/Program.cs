namespace NoteTakingApp;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;


class Program
{

    static void Main(string[] args)
    {
        Run();
        Console.ReadLine();
    }
    private static string NoteDirectory = "/Users/mekigelashvili/Projects/NoteTakingApp/Notes/";

    private static void Run()
    {
        
        Console.Write(Directory.GetDirectoryRoot(NoteDirectory));
        int v;
        do
        {
            v = GetMenuPoint();
            switch (v)
            {
                case 1:
                    NewNote();
                    Main(null);
                    break;
                case 2:
                    EditNote();
                    Main(null);
                    break;
                case 3:
                    DeleteNote();
                    Main(null);  
                    break;
                case 4:
                    ShowNotes();
                    Main(null);
                    break;
                case 5:
                    ReadNote();
                    Main(null);
                    break;              
                default:
                    Console.WriteLine("\nBye!");
                    break;

            }
        } while (v != 0);
    }
    private static int GetMenuPoint()
    {
        int v;
        Console.WriteLine("\n********************************");
        Console.WriteLine("0. Exit");
        Console.WriteLine("1. Create a new note");
        Console.WriteLine("2. Edit a note");
        Console.WriteLine("3. Delet a note");
        Console.WriteLine("4. Show existing notes");
        Console.WriteLine("5. Read note");
        Console.WriteLine("****************************************");

        v = int.Parse(Console.ReadLine());

        return v;
    }
    public static void NewNote()
    {
        Console.WriteLine("Enter a note");
        string inp = Console.ReadLine();
        Console.WriteLine("Enter note name");
        string name = Console.ReadLine();

        XmlWriterSettings NoteSettings = new XmlWriterSettings();

        NoteSettings.CheckCharacters = false;
        NoteSettings.ConformanceLevel = ConformanceLevel.Auto;
        NoteSettings.Indent = true;

        string FileName = name + ".xml";

        using (XmlWriter newNote = XmlWriter.Create(NoteDirectory + FileName, NoteSettings))
        {
            newNote.WriteStartDocument();
            newNote.WriteStartElement("Note");
            newNote.WriteElementString("body", inp + "\n" + DateTime.Now.ToString("dd-MM-yy"));
            newNote.WriteEndElement();

            newNote.Flush();
            newNote.Close();
        }
    }
    public static void EditNote()
    {
        Console.WriteLine("Enter file name");
        string FileName = Console.ReadLine().ToLower() + ".xml";
        if (File.Exists(NoteDirectory + FileName))
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(NoteDirectory + FileName);
                Console.WriteLine(doc.SelectSingleNode("//body").InnerText);
                Console.WriteLine("Enter new input");
                string ReadInput = Console.ReadLine();

                if (ReadInput.ToLower() == "cancel")
                {
                    Main(null);
                }
                else
                {
                    string neText = doc.SelectSingleNode("//body").InnerText = ReadInput + "\n" + DateTime.Now.ToString("dd-MM-yy");
                    doc.Save(NoteDirectory + FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not edit : " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("File not found");
        }
    }
    public static void DeleteNote()
    { 
        Console.WriteLine("Enter note name");
        string Nname = Console.ReadLine() + ".xml";

        if (File.Exists(NoteDirectory + Nname))
        {
            Console.WriteLine(Environment.NewLine + "Are you sure you want to delete this file ? Y/N \n");

            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y")
            {
                try
                {
                    File.Delete(NoteDirectory + Nname);
                    Console.WriteLine("File has been deleted");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File has not been found: " + ex.Message);
                }
            }
            else if (confirmation == "n")
            {
                Main(null);
            }
            else
            {
                Console.WriteLine("Invalid input");
                DeleteNote();
            }
        }
        else
        {
            Console.WriteLine("File does not exist");
            DeleteNote();
        }
    }
    public static void ReadNote()
    { 
        Console.WriteLine("Enter note name: ");
        string filename = Console.ReadLine() + ".xml";

        if (File.Exists(NoteDirectory + filename))
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(NoteDirectory + filename);
                Console.WriteLine(doc.SelectSingleNode("//body").InnerText);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read the note: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("File not found");
            ReadNote();
        }
    }
    public static void ShowNotes()
    {
        string Notelocation = NoteDirectory;
        DirectoryInfo Dir = new DirectoryInfo(Notelocation);

        if (Directory.Exists(Notelocation))
        {
            FileInfo[] NoteFiles = Dir.GetFiles("*.xml");

            if (NoteFiles.Count() != 0)
            {
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                Console.WriteLine("+------------+");
                foreach (var item in NoteFiles)
                {
                    Console.WriteLine(" " + item.Name);
                }
                Console.WriteLine(Environment.NewLine);
            }
            else
            {
                Console.WriteLine("Notes not found");
            }
        }
        else
        {
            Console.WriteLine("Directory does not exist.");
            Console.WriteLine("Would you like to create it ? N/Y");
            string answer = Console.ReadLine();

            if (answer.ToLower() == "y")
            {
                Directory.CreateDirectory(Notelocation);

            }
            else
            {
                Main(null);
            }
        }
    }
}