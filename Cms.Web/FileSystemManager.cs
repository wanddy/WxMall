using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;


namespace Tryine.FileManager
{
    /// <summary>
    /// FileSystemManager
    /// </summary>
    public class FileSystemManager
    {
        private static string strRootFolder;

        static FileSystemManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath;
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        /// д��Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <returns></returns>
        public static List<FileSystemItem> GetItems()
        {
            return GetItems(strRootFolder);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileSystemItem> GetItems(string path)
        {
            if (path.IndexOf("Upload") < 0)
            {
                path += "//Upload//image";
            }
            string[] folders = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            List<FileSystemItem> list = new List<FileSystemItem>();
            foreach (string s in folders)
            {
                FileSystemItem item = new FileSystemItem();
                DirectoryInfo di = new DirectoryInfo(s);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.FullNameOne = "//Upload//image//" + di.Parent.ToString() + di.Name;
                item.CreationDate = di.CreationTime;
                item.LastWriteDate = di.LastWriteTime;
                item.IsFolder = true;
                list.Add(item);
            }
            foreach (string s in files)
            {
                FileSystemItem item = new FileSystemItem();
                FileInfo fi = new FileInfo(s);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.FullNameOne = "/Upload/image/" + fi.CreationTimeUtc.Year + getMonth(fi.CreationTimeUtc.Month) + getMonth(fi.CreationTimeUtc.Day) + "/" + fi.Name;
                item.CreationDate = fi.CreationTime;
                item.LastWriteDate = fi.LastWriteTime;
                
                item.IsFolder = true;
                item.Size = fi.Length;
                list.Add(item);
            }

            if (path.ToLower() != strRootFolder.ToLower())
            {
                //FileSystemItem topitem = new FileSystemItem();
                //DirectoryInfo topdi = new DirectoryInfo(path).Parent;
                //topitem.Name = "[<b><font color=red>Superior</font></b>]";
                //topitem.FullName = topdi.FullName;
                //list.Insert(0, topitem);

                FileSystemItem rootitem = new FileSystemItem();
                DirectoryInfo rootdi = new DirectoryInfo(strRootFolder);
                rootitem.Name = "[<b><font color=red>Root</font></b>]";
                rootitem.FullName = rootdi.FullName;
                list.Insert(0, rootitem);

            }
            return list;
        }

        public static string getMonth(int month)
        {
            string result = "";
            if (month < 10)
            {
                result = "0" + month;
            }
            else
            {
                result = month.ToString();
            }
            return result;
        }

        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        public static void CreateFolder(string name, string parentName)
        {
            DirectoryInfo di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        /// ɾ���ļ���
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFolder(string path)
        {
            try
            {
                Directory.Delete(path);
            }
            catch
            {

                HttpContext.Current.Response.Write("<script>alert('����ɾ���ļ��������ļ���')</script>");
            }
        }

        /// <summary>
        /// �ƶ��ļ���
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void MoveFolder(string oldPath, string newPath)
        {
            Directory.Move(oldPath, newPath);
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public static void CreateFile(string filename, string path)
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Close();
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void CreateFile(string filename, string path, byte[] contents)
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Write(contents, 0, contents.Length);
            fs.Close();
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// �ƶ��ļ�
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void MoveFile(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        /// <summary>
        /// ��ȡ�ļ���Ϣ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileSystemItem GetItemInfo(string path)
        {
            FileSystemItem item = new FileSystemItem();
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = true;
                item.LastAccessDate = di.LastAccessTime;
                item.LastWriteDate = di.LastWriteTime;
                item.FileCount = di.GetFiles().Length;
                item.SubFolderCount = di.GetDirectories().Length;
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.LastAccessDate = fi.LastAccessTime;
                item.LastWriteDate = fi.LastWriteTime;
                item.IsFolder = false;
                item.Size = fi.Length;
            }
            return item;
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyFolder(string source, string destination)
        {
            String[] files;
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            files = Directory.GetFileSystemEntries(source);
            foreach (string element in files)
            {
                if (Directory.Exists(element))
                    CopyFolder(element, destination + Path.GetFileName(element));
                else
                    File.Copy(element, destination + Path.GetFileName(element), true);
            }
        }
    }
}