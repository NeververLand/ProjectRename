using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public class TepTin
        {
            public string Filename { get; set; }
            public string New_Filename { get; set; }
            public string Pathfile { get; set; }
            public string extension { get; set; }
            public string newExtension { get; set; }
        }
        public class folder
        {
            public string folderName { get; set; }
            public string newFolderName { get;set; }
            public string folderPath { get; set; }
        }
        public static BindingList<TepTin> listdata = new BindingList<TepTin>();
        public static BindingList<folder> listfolder = new BindingList<folder>();
        private string TachLayTenChuoi(string path)
        {
            int length = path.Length;
            char[] arr = path.ToCharArray(0, length);
            int vitri = path.LastIndexOf("\\");
            int vitricuoi = path.LastIndexOf(".");
            string v = path.Substring(vitri + 1, length - vitri - 1 - (length - vitricuoi));
            return v;
        }
        private string TachLayExtension(string path)
        {
            int length = path.Length;
            char[] arr = path.ToCharArray(0, length);
            int vitri = path.LastIndexOf("\\");
            int vitricuoi = path.LastIndexOf(".");
            string v = path.Substring(vitricuoi + 1, length - vitricuoi - 1);
            return v;
        }
        //Thêm tiền tố
        private string AddPrefix(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("AddPrefix_");
            sb.Append(path);
            return sb.ToString();
        }
        //Thêm hậu tố
        private string AddSuffix(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append("_AddSuffix");
            return sb.ToString();
        }
        //Xóa tất cả các khoảng trắng
        private string NoSpace(string path)
        {
            path = path.Trim();
            RegexOptions options = RegexOptions.None;
            Regex reg = new Regex(@"\s\s+", options);
            path = reg.Replace(path, string.Empty);
            return path;
        }
        //Viết hoa chữ đầu tiên
        private string PascalCase(string path)
        {
            char[] charArray = path.ToCharArray();
            bool foundSpace = true;
            for (int i = 0; i < charArray.Length; i++)
            {
                if (Char.IsLetter(charArray[i]))
                {
                    if (foundSpace)
                    {
                        charArray[i] = Char.ToUpper(charArray[i]);
                        foundSpace = false;
                    }
                }
                else
                {
                    foundSpace = true;
                }
            }
            path = new string(charArray);
            return path;
        }
        //Chuyển toàn bộ kí tự thành viết thường
        private string AllLower(string path)
        {
            path = path.ToLower();
            return path;
        }
        //Xóa các khoảng trắng dư thừa
        private string OneSpace(string path)
        {
            path = path.Trim();
            RegexOptions options = RegexOptions.None;
            Regex reg = new Regex(@"\s\s+", options);
            path = reg.Replace(path, " ");
            return path;
        }
        //Xóa các kí tự đặc biệt
        private string RemoveSpecialCharacters(string path)
        {
            Regex reg = new Regex(@"[^a-zA-Z0-9.]");
            path = reg.Replace(path, string.Empty);
            return path;
        }
        static int count = 1;
        //Đánh số thứ tự 
        private string AddCount(string path)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append("("+count+")");
            count++;
            return sb.ToString();
        }
        //Gộp tất cả các quy tắc lại sau đó thêm nó vào newName
        private string AllRule(string path)
        {
            path = RemoveSpecialCharacters(path);
            path = OneSpace(path);
            path = AllLower(path);
            path = PascalCase(path);
            path = AddCount(path);
            path = AddPrefix(path);
            path = AddSuffix(path);
            return path;
        }

        private void OpenFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            //Lấy file txt hoặc tất cả các file
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //Vào file MyPictures trên máy
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames) //Cho phép mở nhiều file cùng lúc
                {
                    var data = new TepTin()
                    {
                        Filename = TachLayTenChuoi(filename),
                        New_Filename = AllRule(TachLayTenChuoi(filename)),
                        Pathfile = openFileDialog.FileName,
                        extension = TachLayExtension(filename),
                    };
                    listdata.Add(data);
                    ListViewFiles.Items.Add(data);
                }

            }
        }
        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderdialog = new System.Windows.Forms.FolderBrowserDialog();
            folderdialog.ShowNewFolderButton = false;
            folderdialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            System.Windows.Forms.DialogResult result = folderdialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderdialog.SelectedPath;
                DirectoryInfo folder = new DirectoryInfo(path);
                if (folder.Exists)
                {
                    var data = new folder()
                    {
                        folderName = folder.Name,
                        newFolderName = AllRule(folder.Name),
                        folderPath = folder.FullName,
                    };
                    listfolder.Add(data);
                    ListViewFolder.Items.Add(data);

                }
            }
        }
    }
}
