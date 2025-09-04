using Newtonsoft.Json;

namespace SilksongDecryptor
{
    internal class Program
    {
        public class TextAsset
        {
            public required string m_Name;
            public required string m_Script;
        }

        static void Main(string[] args)
        {
            if(args.Length > 2)
            {
                Console.WriteLine("Usage: SilksongDecryptor -decrypt|-encrypt <folder_path>");
                return;
            }
            if (args[0] == "-decrypt")
            {
                string outpath = Path.Combine(Path.GetDirectoryName(args[1]), Path.GetFileName(args[1]) + "_Decrypted");
                Directory.CreateDirectory(outpath);
                foreach (string path in Directory.GetFiles(args[1], "*.json", SearchOption.AllDirectories))
                {
                    string json = File.ReadAllText(path);
                    TextAsset asset = JsonConvert.DeserializeObject<TextAsset>(json) ?? throw new Exception("Json deresialize failed!");
                    string decrypted = Encryption.Decrypt(asset.m_Script);
                    Console.WriteLine($"Decrypted {asset.m_Name}");
                    decrypted = asset.m_Name + "\n" + decrypted;
                    File.WriteAllText(Path.Combine(outpath, Path.ChangeExtension(Path.GetFileName(path), ".txt")), decrypted);
                }
            }else if(args[0] == "-encrypt")
            {
                string outpath = Path.Combine(Path.GetDirectoryName(args[1]), Path.GetFileName(args[1]) + "_Encrypted");
                Directory.CreateDirectory(outpath);
                foreach (string path in Directory.GetFiles(args[1], "*.txt", SearchOption.AllDirectories))
                {
                    string[] lines = File.ReadAllLines(path);
                    if (lines.Length < 2) continue;
                    string name = lines[0];
                    string script = string.Join("\n", lines.Skip(1));
                    string encrypted = Encryption.Encrypt(script);
                    TextAsset asset = new()
                    {
                        m_Name = name,
                        m_Script = encrypted
                    };
                    string json = JsonConvert.SerializeObject(asset, Formatting.Indented);
                    Console.WriteLine($"Encrypted {asset.m_Name}");
                    File.WriteAllText(Path.Combine(outpath, Path.ChangeExtension(Path.GetFileName(path), ".json")), json);
                }
            }
            else
            {
                Console.WriteLine("Usage: SilksongDecryptor -decrypt|-encrypt <folder_path>");
            }
        }
    }
}
