using UnityEditor;
using System.Threading.Tasks;

namespace IdxZero.Editor
{
    public class AddNameSpace : AssetModificationProcessor
    {
        public async static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0) return;
            string file = path.Substring(index);
            if (file != ".cs" && file != ".js" && file != ".boo") return;
            index = UnityEngine.Application.dataPath.LastIndexOf("Assets");
            path = UnityEngine.Application.dataPath.Substring(0, index) + path;
            await Task.Delay(100);

            file = System.IO.File.ReadAllText(path);

            string lastPart = path.Substring(path.IndexOf("Assets"));
            string _namespace = lastPart.Substring(0, lastPart.LastIndexOf('/'));
            _namespace = _namespace.Replace('/', '.');
            if (_namespace.Contains("Assets.Scripts"))
            {
                _namespace = _namespace.Replace("Assets.Scripts", "IdxZero");
            }
            else if (_namespace.Contains("Assets."))
            {
                _namespace = _namespace.Replace("Assets", "IdxZero");
            }
            file = file.Replace("#NAMESPACE#", _namespace);

            System.IO.File.WriteAllText(path, file);
            AssetDatabase.Refresh();
        }
    }
}
