using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
using Autodesk.Revit.Attributes;

namespace ButtonForRevit
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {

            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("CustomButton", "CustomButton", thisAssemblyPath, "ButtonForRevit.Command"))
                is PushButton button)
            {
                button.ToolTip = "My First Plugin";

                Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "button.png"));
                BitmapImage bitmapImage = new BitmapImage(uri);
                button.LargeImage = bitmapImage;

            }



            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }

        public RibbonPanel RibbonPanel(UIControlledApplication app)
        {
            string tab = "AlexDevTab";
            RibbonPanel ribbonPanel = null;
            try
            {
                app.CreateRibbonTab(tab);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            try
            {
                app.CreateRibbonPanel(tab, "AlexDev");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            List<RibbonPanel> panels = app.GetRibbonPanels(tab);

            foreach (var panel in panels.Where(p => p.Name == "AlexDev"))
            {
                ribbonPanel = panel;
            }

            return ribbonPanel;
        }
    }
}