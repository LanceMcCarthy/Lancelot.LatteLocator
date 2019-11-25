using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;

namespace LatteLocator.Helpers
{
    public static class PasswordUtilities
    {
        public static async Task<bool> SavePasswordAsync(string password)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings != null)
                {
                    // Encrypt password before storing
                    var encryptedPwd = await DataProtectionUtilities.ProtectAsync(password);
                    ApplicationData.Current.LocalSettings.Values["Password"] = encryptedPwd.ToArray();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SavePassword(): Exception: {ex.Message}");
                return false;
            }
        }

        public static async Task<string> GetPasswordAsync()
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Password", out var obj))
                {
                    byte[] protectedPasswordBytes = (byte[]) obj;
                    var plainTextPwd = await DataProtectionUtilities.UnprotectData(protectedPasswordBytes.AsBuffer());
                    return plainTextPwd;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetPassword(): Exception: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> CheckPasswordAsync(string password)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Password", out var obj))
                {
                    byte[] protectedPasswordBytes = (byte[])obj;
                    var plainTextPwd = await DataProtectionUtilities.UnprotectData(protectedPasswordBytes.AsBuffer());
                    return plainTextPwd == password;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CheckPassword(): Exception: {ex.Message}");
                return false;
            }
        }

        public static bool HasPassword()
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.TryGetValue("Password", out var obj))
                {
                    return obj != null;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HasPassword(): Exception: {ex.Message}");
                return false;
            }
        }

        public static bool ResetPassword()
        {
            if (ApplicationData.Current.LocalSettings == null)
                return false;

            ApplicationData.Current.LocalSettings.Values["Password"] = null;
            return true;
        }
    }
}
