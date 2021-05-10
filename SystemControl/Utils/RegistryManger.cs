using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.Utils
{

	public enum SectionEnum
	{
		Root,
		User,
		Machine,
		Users,
		Config
	}

	class RegistryManger
	{
		private RegistryKey registryKeySection(SectionEnum section)
		{
			RegistryKey result = null;
			switch (section)
			{
				case SectionEnum.Root:
					result = Registry.ClassesRoot;
					break;
				case SectionEnum.User:
					result = Registry.CurrentUser;
					break;
				case SectionEnum.Machine:
					result = Registry.LocalMachine;
					break;
				case SectionEnum.Users:
					result = Registry.Users;
					break;
				case SectionEnum.Config:
					result = Registry.CurrentConfig;
					break;
			}
			return result;
		}

		private void grantAcces(SectionEnum section, string path) {
			RegistryKey key = this.buildRegKey(section, path);
			RegistrySecurity rs = new RegistrySecurity();
			rs = key.GetAccessControl();
			string currentUserStr = Environment.UserDomainName + "\\" + Environment.UserName;
			rs.AddAccessRule(new RegistryAccessRule(currentUserStr, RegistryRights.WriteKey | RegistryRights.ReadKey | RegistryRights.Delete | RegistryRights.FullControl, AccessControlType.Allow));
			rs.SetOwner(new NTAccount(currentUserStr));
			key.SetAccessControl(rs);
		}

		private RegistryKey buildRegKey(SectionEnum section, string path)
		{
			RegistryKey registryKey = this.registryKeySection(section);
			return registryKey.OpenSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
		}


		public RegistryKey registryOpenSubkey(SectionEnum section, string path) {
			return this.buildRegKey(section, path);
		}

		public object getRegKeyValue(SectionEnum section, string path,string valueName) {
			if (this.checkIfRegKeyExist(section, path, valueName)) {
				 RegistryKey registryKey = this.registryOpenSubkey(section, path);
				return registryKey.GetValue(valueName);
			}
			return null;
		}

		public IEnumerable<string> getAllSubKeys(SectionEnum section, string path) {
			List<string> subKeysList = new List<string>();
			RegistryKey registryKey = this.registryOpenSubkey(section, path);
			String[] names = registryKey.GetSubKeyNames();
			foreach (var subPath in names) {
				subKeysList.Add(registryKey.Name + "\\" + names);
			}
			return subKeysList;
		}


		public Dictionary<string, object> getKeyValuePairs(SectionEnum section, string path) {
			Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
			IEnumerable<string> allValNames = getAllValuNames(section, path);
			IEnumerable<object> allValues = getAllValues(section, path);
			for(int i = 0; i < allValNames.Count();i++) {
				keyValuePairs.Add(allValNames.ElementAt(i), allValues.ElementAt(i));
			}
			return keyValuePairs;
		}


		public IEnumerable<object> getAllValues(SectionEnum section, string path)
		{
			List<object> values = new List<object>();
			RegistryKey registryKey = this.registryOpenSubkey(section, path);
			IEnumerable<string> allValnames = this.getAllValuNames(section, path);
			foreach (var val in allValnames) {
				values.Add(registryKey.GetValue(val));
			}
			return values;
		}
		public IEnumerable<string> getAllValuNames(SectionEnum section, string path)
		{
			List<string> valueNames = new List<string>();
			RegistryKey registryKey = this.registryOpenSubkey(section, path);
			String[] names = registryKey.GetValueNames();
			valueNames.AddRange(names);
			return valueNames;
		}
		
		public void setRegKeyValue(SectionEnum section, string path, string valName, object newValue) {
			RegistryKey registryKey = this.registryOpenSubkey(section, path);
			if (registryKey != null) {
				try
				{
					registryKey.SetValue(valName, newValue);
				}
				catch
				{
					this.grantAcces(section, path);
					registryKey.SetValue(valName, newValue);
				}
			}			
		}

		public bool checkIfRegKeyExist(SectionEnum section, string path, string valName) {
			RegistryKey registryKey = this.registryOpenSubkey(section, path);
			if (registryKey == null || registryKey.GetValue(valName) == null)
			{
				return false;
			}
			bool result;
			try
			{
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
        
	}
}
