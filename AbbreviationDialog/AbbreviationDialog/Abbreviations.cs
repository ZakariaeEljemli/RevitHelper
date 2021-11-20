using System.Collections.Generic;
using System.Linq;
using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace AbbreviationDialog
{
    [Transaction(TransactionMode.Manual)]
    internal class Abbreviations : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the current Document
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //Custom Collection Class of Abbreviation Items
            List<Abbreviations> Abbreviations = new List<Abbreviations>();
            //Collect the Abbreviation Schedule. May be more useful here for a drop down / selection feature to prevent the wrong one being used
        ViewSchedule vs = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Schedules)
        .Cast<ViewSchedule>().First(x => x.Name.Contains("ABBREVIATION"));
            //Get the Elements from the Schedule
            IList<Element> items = new FilteredElementCollector(doc, vs.Id).ToElements();
            //Create a string list to check again and remove from the items in the schedule (Notes)
            List<string> check = new List<string> { "1", "2", "3", "4", "5", "6", "7" };
            //Iterate each Element from the schedule, remove it if it matches the check list, and then get Parameter Values
            foreach (Element item in items)
            {
                if (!check.Contains(item.LookupParameter("RDG Abbreviations").AsString()))
                {
                    Abbreviations.Add(new Abbreviations()
                    {
                        Abbreviation = item.LookupParameter("RDG Abbreviations").AsString(),
                        Description = item.LookupParameter("RDG Complete Spelling").AsString()
                    });
                }
            }
            //Initialize the WPF Form and pass the Abbreviations List
            AbbreviationsWPF form = new AbbreviationsWPF(Abbreviations);
            //Magic Code from Jeremy Tammik to make the Modeless form know its Owner
            //JtWindowHandle rvtwin = new JtWindowHandle(commandData.Application.MainWindowHandle);
            UIApplication uiapp = commandData.Application;
            IntPtr rvtwin;
            rvtwin = uiapp.MainWindowHandle;
            System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(form)
            {
                Owner = rvtwin
            };
            //Display the form Modeless
            form.Show();
            //Use Cancel here so no transaction is submitted
            return Result.Cancelled;
        }
    }
    
}
