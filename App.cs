//
// (C) Copyright 1974-$year$ by Computational Engineering and Information Technology Lab., NTUST.
// 
// Author : Yisheng Kang (M9805508@mail.ntust.edu.tw)
// $year$-01-01
//
#region Namespaces
using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
#endregion

namespace $safeprojectname$
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    class MainApp : IExternalApplication
    {
        public static RibbonPanel WorkingPanel = null;
        // uiApplication
        public static UIApplication UiApp = null;
        // ExternalCommands assembly path
        public static string AddInPath = typeof(MainApp).Assembly.Location;
		
        public Result OnStartup( UIControlledApplication a )
        {
            //Create Buttons
            string tabName = "CEITL Tools";
            string panelName = "Hello";	
					
			try
            {
                // create customer Ribbon Items
                this.CreateRibbonSamplePanel(a, tabName, panelName);

                MainApp.WorkingPanel = (from panel in a.GetRibbonPanels(tabName)
                                        where panel.Name.Equals(panelName)
                                        select panel).FirstOrDefault();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("CITEL App", ex.ToString());

                return Autodesk.Revit.UI.Result.Failed;
            }
											  
											  
            //Bind Events
            a.ControlledApplication.DocumentCreated += new EventHandler<Autodesk.Revit.DB.Events.DocumentCreatedEventArgs>(this.DocumentCreated);
            a.ControlledApplication.DocumentOpened += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(this.DocumentOpened);
			a.ControlledApplication.DocumentClosed += new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(this.DocumentClosed);
			
            return Result.Succeeded;
        }

        public Result OnShutdown( UIControlledApplication a )
        {
            //Unbind Events
            a.ControlledApplication.DocumentCreated -= new EventHandler<Autodesk.Revit.DB.Events.DocumentCreatedEventArgs>(this.DocumentCreated);
            a.ControlledApplication.DocumentOpened -= new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(this.DocumentOpened);
			a.ControlledApplication.DocumentClosed -= new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(this.DocumentClosed);
			
            //Dereference List
			MainApp.UiApp = null;
			MainApp.WorkingPanel = null;
			
            return Result.Succeeded;
        }
		
		private void CreateRibbonSamplePanel(UIControlledApplication a, string tabName, string panelName)
        {
            try
            {
                a.CreateRibbonTab(tabName);
            }
            catch (Exception ex)
            {
            }

            MainApp.WorkingPanel = a.CreateRibbonPanel(tabName, panelName);

            //Load local command
            PushButtonData btnData = new PushButtonData("HelloBtn", "Hello", MainApp.AddInPath, "$safeprojectname$.Command");

            //Add buttons to Ribbon panel
            MainApp.WorkingPanel.AddItem(btnData);
        }

        public void DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            this.Initialize(e);
        }

        public void DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            this.Initialize(e);
        }

		public void DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
        }
		
        private void Initialize(dynamic e)
        {
            MainApp.UiApp = new UIApplication(e.Document.Application);
        }
  }
}
