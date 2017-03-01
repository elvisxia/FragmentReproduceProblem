using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Util;
using System;
using Android.Content;
using Android;
using Android.Views;
namespace ReproduceProblem
{
    [Activity(Label = "Simplified version of problem", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ReminderDateDialog.OnNewDatePass,ReminderTimeDialog.OnNewTimePass//,taskedit_BaseFragment.OnChangeAddDataPass
    {
        DateTime _tempDateTime = DateTime.MinValue;

        ReminderTimeDialog timeDialog;
        ReminderDateDialog dateDialog;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);
            var btnChange =FindViewById<Button>(Resource.Id.ReallySurelyAButton);

            btnChange.Click += delegate
            {
                openDateDialog(3,bundle);
            };
        }

        public void closeTimeDialog(ReminderTimeDialog dialog)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
             ft.Remove(dialog);
             ft.AddToBackStack("close-time");
             //ft.Commit();//added by winffee
             dialog.Dismiss();
            dateDialog.Dismiss();
            //Also tried i.e. dialog.Dismiss(); here
        }



        public void closeDateDialog(ReminderDateDialog dialog)
        {
           FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Remove(dialog);
            
            ft.AddToBackStack("close-time");
            //ft.Commit();
            dateDialog = dialog;
            //Also tried i.e. dialog.Dismiss(); here
        }
        public void onNewDatePass(DateTime date, int id)
        {

            Console.WriteLine("New date passed");

            _tempDateTime = date; //gets saved into temp - ono
            
        }

        public void onNewTimePass(DateTime time, int id)
        {
            DateTime d = _tempDateTime;
            DateTime tTempDateTime = new DateTime(d.Year, d.Month, d.Day, time.Hour, time.Minute, 0);
            Toast.MakeText(this, "Date and time changed to :" + tTempDateTime.ToString(), ToastLength.Short).Show();
        }

        public void openTimeDialog(int id,Bundle bundle) {
             FragmentTransaction ft = FragmentManager.BeginTransaction();
              //Remove fragment else it will crash as it is already added to backstack
              Fragment prev = FragmentManager.FindFragmentByTag("dialog");
              if (prev != null)
              {
                  ft.Remove(prev);
              }
            
              ft.AddToBackStack("time-dialog");
            // Create and show the dialog.


            //Add fragment

              Bundle taskdata = new Bundle();
              taskdata.PutInt("id", 3);
              taskdata.PutInt("hour",  3);
              taskdata.PutInt("minute", 7);
            Console.WriteLine("Opening new time dialog!");
            ReminderTimeDialog timeDialog = ReminderTimeDialog.NewInstance(taskdata);
              timeDialog.Arguments = taskdata;
              timeDialog.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
              timeDialog.Show(ft, "dialog");
            //ft.Commit();//added by winffee
            
        }

    public void openDateDialog(int id, Bundle bundle)
    {
            Console.WriteLine("Trying to open new date dialog!");
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack("date-dialog");
            
            // Create and show the dialog.


            //Add fragment

            

            Bundle taskdata = new Bundle();
            taskdata.PutInt("id", 3);
            taskdata.PutInt("day", 9);
            taskdata.PutInt("month", 11);
            taskdata.PutInt("year", 2017);
            Console.WriteLine("Opening new date dialog!");
            ReminderDateDialog dateDialog = ReminderDateDialog.NewInstance(taskdata);
            dateDialog.Arguments = taskdata;
            dateDialog.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
            dateDialog.Show(ft, "dialog");

            //ft.Commit();//added by winffee
  
        }
    }
}

