using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Management.Deployment;
using System.Security.Principal;
using Windows.ApplicationModel;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace SystemControl.SystemSettings.Windows_settings
{
    class AppX
    {
        [DllImport("shlwapi.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, ThrowOnUnmappableChar = true)]
        private static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

        internal static string ExtractStringFromPriFile(string pathToPri, string resourceKey)
        {
            string pszSource = string.Concat(new string[]
            {
                    "@{",
                    pathToPri,
                    "? ",
                    resourceKey,
                    "}"
            });
            StringBuilder stringBuilder = new StringBuilder(1024);
            SHLoadIndirectString(pszSource, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
            return stringBuilder.ToString();
        }
        public IEnumerable<AppXItem> GetAppXItems()
        {
            IEnumerable<Package> enumerable = null;
            PackageManager packageManager = null;
            try
            {
                packageManager = new PackageManager();
            }
            catch (Exception e)
            {
                return null;
            }

            SecurityIdentifier user = WindowsIdentity.GetCurrent().User;
            string text = ((user != null) ? user.Value : null);

            if (text != null)
                enumerable = packageManager.FindPackagesForUserWithPackageTypes(text, PackageTypes.Main);

            if (enumerable == null)
                return null;

            List<AppXItem> appXItems = new List<AppXItem>();

            Parallel.ForEach(enumerable, (package) =>
            {
                string name = null;
                try
                {
                    name = package.Id.Name;
                }
                catch (Exception)
                {
                    return;
                }
                if (package.IsFramework)
                    return;
                string path = null;
                try
                {
                    path = package.InstalledLocation.Path;
                    if (!path.Contains("\\WindowsApps\\"))
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    return;
                }

                string xmlPath = Path.Combine(path, "AppxManifest.xml");
                if (!File.Exists(xmlPath))
                {
                    return;
                }
                string xmlString = null;
                try
                {
                    xmlString = File.ReadAllText(xmlPath);
                }
                catch (Exception)
                {
                    return;
                }

                XElement xelement;
                try
                {
                    int packagePropertiesStart = xmlString.IndexOf("<Properties>", StringComparison.Ordinal);
                    int packagePropertiesEnd = xmlString.IndexOf("</Properties>", StringComparison.Ordinal);
                    xelement = XElement.Parse(xmlString.Substring(packagePropertiesStart, packagePropertiesEnd - packagePropertiesStart + 13).Replace("uap:", string.Empty));
                }
                catch (Exception)
                {
                    return;
                }
                string logoPath = null;
                string displayName = null;
                try
                {
                    XElement xelement2 = xelement.Element("Logo");
                    logoPath = ((xelement2 != null) ? xelement2.Value : null);
                    XElement xelement3 = xelement.Element("DisplayName");
                    displayName = ((xelement3 != null) ? xelement3.Value : null);
                    displayName = ExtractDisplayName(path, package.Id.Name, displayName);
                }
                catch (Exception e)
                {

                }

                AppXItem item = new AppXItem
                {
                    Name = package.Id.Name,
                    FullName = package.Id.FullName,
                    DisplayName = (string.IsNullOrWhiteSpace(displayName) ? package.Id.Name : displayName),
                    Logo = this.ExtractLogo(path, logoPath)
                };
                appXItems.Add(item);
            });
            appXItems = appXItems.OrderBy(o => o.DisplayName).ToList();
            return appXItems;
        }
        private string ExtractLogo(string appPath, string logoPath)
        {
            string text = Path.Combine(appPath, logoPath);
            if (File.Exists(text))
            {
                return text;
            }
            text = Path.Combine(appPath, Path.ChangeExtension(text, "scale-100.png"));
            if (File.Exists(text))
            {
                return text;
            }
            string text2 = Path.Combine(Path.Combine(appPath, "en-us"), logoPath);
            text2 = Path.Combine(appPath, Path.ChangeExtension(text2, "scale-100.png"));
            if (!File.Exists(text2))
            {
                return null;
            }
            return text2;
        }

        private string ExtractDisplayName(string appPath, string packageName, string displayName)
        {
            Uri uri;
            if (!Uri.TryCreate(displayName, UriKind.Absolute, out uri))
            {
                return displayName;
            }
            string pathToPri = Path.Combine(appPath, "resources.pri");
            string resourceKey = "ms-resource://" + packageName + "/resources/" + uri.Segments.Last<string>();
            string text = ExtractStringFromPriFile(pathToPri, resourceKey);
            if (!string.IsNullOrEmpty(text.Trim()))
            {
                return text;
            }
            string str = string.Concat(uri.Segments.Skip(1));
            resourceKey = "ms-resource://" + packageName + "/" + str;
            return ExtractStringFromPriFile(pathToPri, resourceKey);
        }
        public Task<bool> RemovePackage(string packageFamilyName)
        {
            return Task.Factory.StartNew<bool>(delegate ()
            {
                bool result;
                try
                {
                    new PackageManager().RemovePackageAsync(packageFamilyName);
                    result = true;
                }
                catch
                {
                    result = false;
                }
                return result;
            });
        }

    }
}
