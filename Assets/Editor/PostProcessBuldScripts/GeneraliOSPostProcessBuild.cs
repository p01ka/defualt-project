#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Linq;
using IndexZero.UnityEditor.iOS.Xcode;
using UnityEngine;
#endif

namespace IdxZero.Editor
{
    public class GeneraliOSPostProcessBuild
    {
        private const bool ENABLED = true;
        private const bool USE_ATTRIBUTION_ENDPOINT = true;
        private const string AppLovinAdvertisingAttributionEndpoint = "https://appsflyer-skadnetwork.com/";
        private const string PHOTO_LIBRARY_USAGE_DESCRIPTION = "App needs permission to access your Photo Library to load photos, so you can play with them.";
        private const string CAMERA_USAGE_DESCRIPTION = "App needs permission to access your Camera to make funny slimes, so you can wow others.";

        private static string[] _supportedSKAdNetworkIds = new string[] { "SU67R6K2V3.skadnetwork", "4PFYVQ9L8R.skadnetwork", "cstr6suwn9.skadnetwork", "4DZT52R2T5.skadnetwork" };
        private static string[] _supportedLanguages = new string[] { "en" };

#if UNITY_IOS
#pragma warning disable 0162
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string buildPath)
    {
        if (!ENABLED)
            return;

        if (target == BuildTarget.iOS)
        {
            string plistPath = Path.Combine(buildPath, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            SetAttributionReportEndpointIfNeeded(plist);
            AddSupportedLanguages(plist, _supportedLanguages);

            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NSPhotoLibraryUsageDescription", PHOTO_LIBRARY_USAGE_DESCRIPTION);
            rootDict.SetString("NSPhotoLibraryAddUsageDescription", PHOTO_LIBRARY_USAGE_DESCRIPTION);
            rootDict.SetString("NSCameraUsageDescription", CAMERA_USAGE_DESCRIPTION);

            PlistElementArray array = rootDict.CreateArray("LSApplicationQueriesSchemes");
            array.AddString("instagram");
            array.AddString("fb");

            TryToAddSKAdNetworkIds(rootDict);

            rootDict.SetBoolean("FirebaseMessagingAutoInitEnabled", false);

            File.WriteAllText(plistPath, plist.WriteToString());
            TryToAddCapabillities(buildPath);
            TryToEditProj(buildPath);
            LocalizePermissions(buildPath);
        }
    }

    private static void TryToAddSKAdNetworkIds(PlistElementDict rootDict)
    {
        int supportedSkaNetworksCount = _supportedSKAdNetworkIds.Length;
        if (supportedSkaNetworksCount > 0)
        {
            PlistElementArray skadNetworkItemsArray = rootDict.CreateArray("SKAdNetworkItems");

            for (int i = 0; i < supportedSkaNetworksCount; i++)
            {
                PlistElementDict ironsourceElementDict = skadNetworkItemsArray.AddDict();
                ironsourceElementDict.SetString("SKAdNetworkIdentifier", _supportedSKAdNetworkIds[i]);
            }
        }
    }

    static void TryToEditProj(string pathToBuiltProject)
    {
        string pbxPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var proj = new UnityEditor.iOS.Xcode.PBXProject();

        proj.ReadFromFile(pbxPath);

        //"Unity-iPhone"
        var guid = proj.GetUnityMainTargetGuid();
        proj.SetBuildProperty(guid, "ENABLE_BITCODE", "NO");

        //"UnityFramework"
        guid = proj.GetUnityFrameworkTargetGuid();
        proj.SetBuildProperty(guid, "ENABLE_BITCODE", "NO");

        proj.WriteToFile(pbxPath);
    }

    private static void SetAttributionReportEndpointIfNeeded(PlistDocument plist)
    {
        if (USE_ATTRIBUTION_ENDPOINT)
        {
            plist.root.SetString("NSAdvertisingAttributionReportEndpoint", AppLovinAdvertisingAttributionEndpoint);
        }
        else
        {
            PlistElement attributionReportEndPoint;
            plist.root.values.TryGetValue("NSAdvertisingAttributionReportEndpoint", out attributionReportEndPoint);

            // Check if we had previously set the attribution endpoint and un-set it.
            if (attributionReportEndPoint != null && AppLovinAdvertisingAttributionEndpoint.Equals(attributionReportEndPoint.AsString()))
            {
                plist.root.values.Remove("NSAdvertisingAttributionReportEndpoint");
            }
        }
    }

    private static void AddSupportedLanguages(PlistDocument plist, string[] languages)
    {
        var localizationKey = "CFBundleLocalizations";

        var localizations = plist.root.values
        .Where(kv => kv.Key == localizationKey)
        .Select(kv => kv.Value)
        .Cast<PlistElementArray>()
        .FirstOrDefault();

        if (localizations == null)
            localizations = plist.root.CreateArray(localizationKey);

        foreach (var language in languages)
        {
            if (localizations.values.Select(el => el.AsString()).Contains(language) == false)
                localizations.AddString(language);
        }
    }

    private static void LocalizePermissions(string pathToBuiltProject)
    {
        string LocalizationRoot = pathToBuiltProject + "/AppLocalization";
        if (!Directory.Exists(LocalizationRoot))
            Directory.CreateDirectory(LocalizationRoot);

        var project = new PBXProject();
        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

        project.ReadFromFile(projPath);

        // Set the Language Overrides
        foreach (var code in _supportedLanguages)
        {
            Debug.Log("CODE " + code);
            if (code == null || code.Length < 2)
                continue;
            Debug.Log("CODE  CONTINUE");
            var LanguageDirRoot = LocalizationRoot + "/" + code + ".lproj";
            if (!Directory.Exists(LanguageDirRoot))
                Directory.CreateDirectory(LanguageDirRoot);

            Debug.Log("LANG DIR ROOT " + LanguageDirRoot);
            var infoPlistPath = LanguageDirRoot + "/InfoPlist.strings";

            var libraryUsageDescription = string.Format("NSPhotoLibraryUsageDescription = \"{0}\";", GetLibraryUsageDescription(code));
            Debug.Log(libraryUsageDescription);
            var libraryAddUsageDescription = string.Format("NSPhotoLibraryAddUsageDescription = \"{0}\";", GetLibraryUsageDescription(code));
            Debug.Log(libraryAddUsageDescription);
            var cameraUsageDescription = string.Format("NSCameraUsageDescription = \"{0}\";", GetCameraUsageDescription(code));
            Debug.Log(cameraUsageDescription);

            Debug.Log("INFO PLIST PATH " + infoPlistPath);

            File.WriteAllLines(infoPlistPath, new string[] { libraryUsageDescription, libraryAddUsageDescription, cameraUsageDescription });

            var stringPaths = LanguageDirRoot + "/Localizable.strings";
            Debug.Log("STRING PATHS " + stringPaths);
            File.WriteAllText(stringPaths, string.Empty);

            project.AddLocalization("AppLocalization", code, infoPlistPath);
            project.AddLocalization("AppLocalization", code, stringPaths);
        }

        project.WriteToFile(projPath);
    }

    private static void TryToAddCapabillities(string pathToBuiltProject)
    {
        string pbxPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var proj = new UnityEditor.iOS.Xcode.PBXProject();
        proj.ReadFromFile(pbxPath);
        var guid = proj.GetUnityMainTargetGuid();

        // get entitlements path
        string[] idArray = UnityEngine.Application.identifier.Split('.');
        var entitlementsPath = $"Unity-iPhone/{idArray[idArray.Length - 1]}.entitlements";

        // create capabilities manager
        var capManager = new UnityEditor.iOS.Xcode.ProjectCapabilityManager(pbxPath, entitlementsPath, null, guid);

        // Add necessary capabilities
        capManager.AddPushNotifications(false);

        // Write to file
        capManager.WriteToFile();
    }

    private static string GetCameraUsageDescription(string code)
    {
        switch (code)
        {
            case "en":
                return CAMERA_USAGE_DESCRIPTION;
                break;

            case "ru":
                return "App нужно разрешение на доступ к вашей камере, чтобы делать смешные слаймы.";
                break;

            case "it":
                return "App ha bisogno dell'autorizzazione per accedere alla tua Camera per creare slimes divertenti per stupire gli altri.";
                break;

            case "es":
                return "App requiere el permiso de acceso a tu cámara, para crear Slimes chistosos que podrán sorprender.";
                break;

            case "fr":
                return "App a besoin d'une autorisation pour accéder à votre appareil photo et créer des slimes amusants, afin que vous puissiez impressionner les autres.";
                break;
        }
        return string.Empty;
    }

    private static string GetLibraryUsageDescription(string code)
    {
        switch (code)
        {
            case "en":
                return PHOTO_LIBRARY_USAGE_DESCRIPTION;
                break;

            case "ru":
                return "App требуется разрешение на доступ к вашей библиотеке для загрузки фотографий, чтобы вы могли играть с ними.";
                break;

            case "it":
                return "App ha bisogno dell'autorizzazione per accedere alla tua Libreria Foto per caricare foto in modo da poter giocare con loro.";
                break;

            case "es":
                return "App requiere el permiso de acceso a tu biblioteca para guardar fotos, para que puedas jugar con ellas.";
                break;

            case "fr":
                return "App a besoin d'une autorisation pour accéder à votre galerie et charger les photos avec lesquelles vous jouerez.";
                break;
        }
        return string.Empty;
    }
#pragma warning restore 0162
#endif
    }
}