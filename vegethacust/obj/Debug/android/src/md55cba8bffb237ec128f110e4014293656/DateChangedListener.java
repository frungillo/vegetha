package md55cba8bffb237ec128f110e4014293656;


public class DateChangedListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.widget.DatePicker.OnDateChangedListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onDateChanged:(Landroid/widget/DatePicker;III)V:GetOnDateChanged_Landroid_widget_DatePicker_IIIHandler:Android.Widget.DatePicker/IOnDateChangedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("vegethacust.DateChangedListener, vegethacust, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DateChangedListener.class, __md_methods);
	}


	public DateChangedListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DateChangedListener.class)
			mono.android.TypeManager.Activate ("vegethacust.DateChangedListener, vegethacust, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDateChanged (android.widget.DatePicker p0, int p1, int p2, int p3)
	{
		n_onDateChanged (p0, p1, p2, p3);
	}

	private native void n_onDateChanged (android.widget.DatePicker p0, int p1, int p2, int p3);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
