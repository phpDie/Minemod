using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// Change this to match your program's normal namespace
namespace MyProg
{
    class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public float ReadFloat(string Key, string Section = null, float defVal = 0)
        {

            string _valStr = Read(Key, Section);

            if (_valStr.Length <= 0)
            {
                return defVal;
            }

            
            
            return float.Parse(_valStr);
        }

        public int ReadInt(string Key, string Section = null, int defVal= 0)
        {

            string _valStr = Read(Key,Section);

            if (_valStr.Length <=0)
            {
                return defVal;
            }

            int _val = System.Convert.ToInt32(_valStr);

            return _val;
        }


        public bool ReadBool(string Key, string Section = null, bool defVal = false)
        {

            string _valStr = Read(Key, Section);

            if (_valStr.Length <= 0)
            {
                return defVal;
            }

            if (_valStr == "1") return true;
            if (_valStr == "0") return false;
            if (_valStr == "true") return true;
            if (_valStr == "false") return false;

            return defVal;
        }

        public string Read(string Key, string Section = null, string defVal = "")
        {
          
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);

            if (RetVal.ToString().Length <= 0) return defVal;

            return RetVal.ToString();
        }

        public void WriteBool(string Key, bool ValueIn, string Section = null)
        {
            string Value = "true";
            if(!ValueIn) Value = "false";
            Write(Key, Value, Section);
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}