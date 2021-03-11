using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.DeviceControl.DisplayProtocols;
using DiversifiedLibrary.Utilities.IO;

namespace Community_Divisible_Room.Modules
{

    public static class Helpers
	{
		public static Display CreateDisplay(string driverName, DisplayDriver defaultDriver, IComPortDevice comport, int tag)
		{
			Display display = null;
			comport.AutoRegisterComPort();

			try
			{
				defaultDriver = (DisplayDriver)Enum.Parse(typeof(DisplayDriver), driverName, true);
			}
			catch (ArgumentException e)
			{
				ErrorLog.Error(e.ToString());
				CrestronConsole.PrintLine("Error Occured During Creation of Display {0}. Check Driver Name Used.", tag);
				CrestronConsole.PrintLine("Current Driver Value read from Config File: {0}", driverName);
				CrestronConsole.PrintLine("Error Reported: {0}\n", e.ToString());
				CrestronConsole.PrintLine("Display being defaulted to {0}", defaultDriver.ToString());
			}
			finally
			{
				display = Displays.Create(defaultDriver, tag);
				CrestronConsole.PrintLine("Display {0} created as {1}.", display.Tag, display.DeviceName);
			}

			display.Configure(comport);

			return display;
		}

		public static Display CreateDisplay(string driverName, DisplayDriver defaultDriver, string ipAddress, int tag)
		{
			Display display = null;

			try
			{
				defaultDriver = (DisplayDriver)Enum.Parse(typeof(DisplayDriver), driverName, true);
			}
			catch (ArgumentException e)
			{
				ErrorLog.Error(e.ToString());
				CrestronConsole.PrintLine("Error Occured During Creation of Display {0}. Check Driver Name Used.", tag);
				CrestronConsole.PrintLine("Current Driver Value read from Config File: {0}", driverName);
				CrestronConsole.PrintLine("Error Reported: {0}\n", e.ToString());
				CrestronConsole.PrintLine("Display being defaulted to {0}", defaultDriver.ToString());
			}
			finally
			{
				display = Displays.Create(defaultDriver, tag);
				CrestronConsole.PrintLine("Display {0} created as {1}.", display.Tag, display.DeviceName);
			}

			display.Configure(ipAddress);

			return display;
		}

		public static uint ConfigurableIPID(Configuration config, string key, int ipid)
		{
			return Convert.ToUInt16(config[key, "" + (ipid).ToString("X2")], 16);
		}

		public static uint ConfigurableIPID(ConfigurationSection config, string key, int ipid)
		{
			return Convert.ToUInt16(config[key, "" + (ipid).ToString("X2")], 16);
		}

	}
}