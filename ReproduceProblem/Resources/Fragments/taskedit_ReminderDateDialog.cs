using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace ReproduceProblem
{
    public class ReminderDateDialog : DialogFragment
    {
        public DateTime _workingdate = DateTime.MinValue;
        ViewGroup _container;
        bool dismissed = false;
        public interface OnNewDatePass
        {
            void onNewDatePass(DateTime date, int id);
            void openTimeDialog(int id, Bundle bundle);
            void closeDateDialog(ReminderDateDialog dialog);
        }

        public OnNewDatePass dataPasser
        {
            get; set;
        }

        public interface OnDialogFragHide
        {
            void onFragmentDismissed();
        }

        OnDialogFragHide mListener;

        public void setOnFragDismissedListener(OnDialogFragHide listener)
        {
            mListener = listener;
        }

        public static ReminderDateDialog NewInstance(Bundle bundle)
        {
            ReminderDateDialog fragment = new ReminderDateDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);
            Console.WriteLine("Hello world! I am DateDialog And I have just been attached");
            dataPasser = (OnNewDatePass)a;
            Toast.MakeText(dataPasser as Context, "DateDialog On Attach", ToastLength.Short).Show();
            Console.WriteLine("activity casted to data passer");
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_date_dialog, container, false);//end potentially true

            Button buttonNext = view.FindViewById<Button>(Resource.Id.DialogButtonNext);
            int day = Arguments.GetInt("day");
            int month = Arguments.GetInt("month");
            int year = Arguments.GetInt("year");

            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityEditDateField);
            buttonNext.Click += delegate{
                

                goToNext(view,savedInstanceState);
              


            };
            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Toast.MakeText(dataPasser as Context, "DateDialog Created by Winffee", ToastLength.Short).Show();
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Console.WriteLine("Fragment dismissed.");
            /*Dismiss();*/
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            if (mListener != null && dismissed)
            {
                dismissed = true;
                mListener.onFragmentDismissed();
            }
            else
            {
                Log.Info("sometag", "DialogFragmentDismissed not set");
            }
            base.OnCancel(dialog);
        }
        public void goToNext(View view, Bundle savedInstanceState)
        {

            var DateField = view.FindViewById<CalendarView>(Resource.Id.ActivityEditDateField);
            DateField.DateChange += (s, e) =>
            {
                int day = e.DayOfMonth;
                int month = e.Month;
                int year = e.Year;
                _workingdate = new System.DateTime(year, month, day);
                Console.WriteLine("Date:" + day + " / " + month + " / " + year);
            };
            Console.WriteLine("Got data");
            int id = Arguments.GetInt("id");
            dataPasser.onNewDatePass(_workingdate,id);
            dataPasser.openTimeDialog(id,savedInstanceState);
            dataPasser.closeDateDialog(this);
            Console.WriteLine("gotonext -> making dialog fragment disappear.");
        }
    }
}