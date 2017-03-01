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
    public class ReminderTimeDialog : DialogFragment
    {
        public DateTime _workingtime = DateTime.MinValue;
        public int _id;
        ViewGroup _container;
        public interface OnNewTimePass
        {
            void onNewTimePass(DateTime date, int id);
            void closeTimeDialog(ReminderTimeDialog dialog);
        }

        public OnNewTimePass dataPasser
        {
            get; set;
        }

        public static ReminderTimeDialog NewInstance(Bundle bundle)
        {
            ReminderTimeDialog fragment = new ReminderTimeDialog();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);
            Console.WriteLine("Hello world! I am TimeDialog And I have just been attached");
            dataPasser = (OnNewTimePass)a;
            Console.WriteLine("activity casted to data passer");
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_time_dialog, container, false);//end potentially true
            this.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
            Button buttonNext = view.FindViewById<Button>(Resource.Id.DialogButtonEnd);

            var TimeField = view.FindViewById<TimePicker>(Resource.Id.ActivityEditTimeField);
           /* int hour = Arguments.GetInt("hour");
            int minute = Arguments.GetInt("minute");
            TimeField.Hour = hour;
            TimeField.Minute = minute;*/
            buttonNext.Click += delegate {
                goToNext(view,savedInstanceState);

                Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
                

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
        }

        public void goToNext(View view, Bundle savedInstanceState)
        {
            //get data 

            var TimeField = view.FindViewById<TimePicker>(Resource.Id.ActivityEditTimeField);
            TimeField.TimeChanged += (s, e) =>
            {
                int hour = e.HourOfDay;
                int min = e.Minute;
                _workingtime = new System.DateTime(0, 0, 0, hour, min, 0);
                Console.WriteLine("Time:" + hour + " / " + min);
            };
            Console.WriteLine("Got data");

            int id = Arguments.GetInt("id");
            _id = Arguments.GetInt("id");
            dataPasser.onNewTimePass(_workingtime, _id);
            dataPasser.closeTimeDialog(this);
            Console.WriteLine("gotonext -> making dialog fragment disappear.");
        }
    }
}