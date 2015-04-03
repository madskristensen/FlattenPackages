using System;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MadsKristensen.FlattenPackages
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Version, IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [Guid(GuidList.guidFlattenPackagesPkgString)]
    public sealed class FlattenPackagesPackage : Package
    {
        public const string Version = "1.0";
        public const string Name = "Flatten Packages";
        private DTE2 _dte;
        private string _directory;

        protected override void Initialize()
        {
            base.Initialize();
            _dte = GetService(typeof(DTE)) as DTE2;

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID menuCommandID = new CommandID(GuidList.guidFlattenPackagesCmdSet, (int)PkgCmdIDList.cmdidFlattenPackages);
                OleMenuCommand menuItem = new OleMenuCommand(ButtonClicked, menuCommandID);
                menuItem.BeforeQueryStatus += BeforeButtonClicked;
                mcs.AddCommand(menuItem);
            }
        }

        private void BeforeButtonClicked(object sender, EventArgs e)
        {
            OleMenuCommand button = (OleMenuCommand)sender;

            string item = GetSelectedItemPath();

            if (string.IsNullOrEmpty(item))
                return;

            string directory = item;

            if (File.Exists(item))
            {
                if (Path.GetFileName(item) == "package.json")
                    directory = Path.GetDirectoryName(item);
                else
                    return;
            }

            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir.Name.Equals("node_modules", StringComparison.OrdinalIgnoreCase))
                directory = dir.Parent.FullName;

            string nodeModules = Path.Combine(directory, "node_modules");

            if (!Directory.Exists(nodeModules))
                return;

            _directory = directory;

            if (Directory.Exists(_directory))
                button.Enabled = true;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            bool moduleInstalled = ProcessHelper.IsNodeModuleInstalled();
            if (!moduleInstalled)
            {
                string firstTime = "The first time you run this command I will have to install the 'flatten-packages' npm package globally. \r\r I'll run 'npm install flatten-packages -g' \r\rDo you wish to install it (internet connection required)?";
                var result = MessageBox.Show(firstTime, Name, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result != DialogResult.OK)
                    return;

                moduleInstalled = ProcessHelper.RunProcessSync("/c npm install flatten-packages -g", _directory);
            }

            if (!moduleInstalled)
                return;

            string dir = Path.Combine(_directory, "node_modules");

            string question = "This will flatten the packages in " + dir + " \r\r Do you want to continue?";
            var answer = MessageBox.Show(question, Name, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (answer == DialogResult.OK)
                ProcessHelper.RunProcessSync("/k flatten-packages", _directory);
        }

        private string GetSelectedItemPath()
        {
            try
            {
                var items = (Array)_dte.ToolWindows.SolutionExplorer.SelectedItems;
                foreach (UIHierarchyItem selItem in items)
                {
                    var item = selItem.Object as ProjectItem;

                    if (item != null && item.Properties != null)
                        return item.Properties.Item("FullPath").Value.ToString();
                }
            }
            catch { /* Something weird is happening. Ignore this and return null */}

            return null;
        }
    }
}
