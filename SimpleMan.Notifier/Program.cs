using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SimpleMan.Notifier
{
    class Program
    {
        private const string _applicationId = "SimpleMan.Notifier.Toaster";

        static void Main(string[] args)
        {
            Dictionary<string, string> options = new Dictionary<string, string>()
            {
                { "ApplicationId", _applicationId },
                { "Text", "No text!" },
                { "Header", "Notification!" }
            };

            // set notification text
            string text = GetOptionValue(args, "-t");
            if (!String.IsNullOrEmpty(text))
                options["Text"] = text;

            // set notification header
            string header = GetOptionValue(args, "-h");
            if (!String.IsNullOrEmpty(header))
                options["Header"] = header;

            SimpleShortcut simepleShortcut = new SimpleShortcut();

            simepleShortcut.TryCreateShortcut(_applicationId);

            Notify notify = new Notify();
            notify.NotifyText(options);

            Thread.Sleep(10000);

        }

        static string GetOptionValue(string[] args, string option)
        {
            if (args.Count() == 0 || !args.Contains(option))
            {
                return null;
            }

            int indexOfTextValue = Array.IndexOf(args, option) + 1;

            if (indexOfTextValue >= args.Count())
                return null;
            else
                return args[indexOfTextValue];
        }
    }


    class Notify
    {
        public void NotifyText(Dictionary<string, string> options)
        {
            // Get a toast XML template
            // Can be any template type. REF: https://docs.microsoft.com/en-us/uwp/api/windows.ui.notifications.toasttemplatetype
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            // Header
            stringElements[0].AppendChild(toastXml.CreateTextNode(options["Header"]));

            // Notification text
            stringElements[1].AppendChild(toastXml.CreateTextNode(options["Text"]));

            // Specify the absolute path to an image; 
            // FOR IF YOU ARE USING A IMAGE TOAST TEMPLATE
            //String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            //XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            //imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            ToastNotification toast = new ToastNotification(toastXml);
            toast.Activated += toast_Activated;

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(options["ApplicationId"]).Show(toast);

        }

        void toast_Activated(ToastNotification sender, object args)
        {
            
        }

    }

    class SimpleShortcut
    {
        public bool TryCreateShortcut(string appId)
        {
            String shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\SimpleMan Toast Notifier.lnk";
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(shortcutPath, appId);
                return true;
            }
            return false;
        }

        private void InstallShortcut(String shortcutPath, string applicationId)
        {
            // Find the path to the current executable 
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe 
            ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
            ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property 
            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(applicationId))
            {
                ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
                ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk 
            IPersistFile newShortcutSave = (IPersistFile)newShortcut;

            ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }
    }
}
