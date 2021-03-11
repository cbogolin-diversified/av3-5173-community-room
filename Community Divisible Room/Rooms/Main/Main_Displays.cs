using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.DeviceControl.DisplayProtocols;
using DiversifiedLibrary.DeviceControl.Standalone;
using Community_Divisible_Room.Modules;
using Crestron.SimplSharpPro;
using DiversifiedLibrary.DeviceControl;

namespace Community_Divisible_Room.Rooms
{
	public class Main_Displays
	{
		public Display RoomARearDisplay { get; private set; }
		public Display RoomAProjector { get; private set; }
		public Display RoomBProjector { get; private set; }

		public Main_Displays(Main_DivController system)
		{
			RoomAProjector = Displays.Create(system.Config.GetEnum<DisplayDriver>("Displays", "RoomA.Projector.Driver", DisplayDriver.SonyADCPProjector).Value, 1);
			RoomAProjector.Configure(system.Config.GetString("Displays", "RoomA.Projector.IPAddress", "192.168.1.100"));
			RoomAProjector.ScreenControl.SetRelays(Devices.ControlSystem, 
				system.Config.GetUInt32("Displays", "RoomA.Projector.Screen.Up.RelayPort", 1),
				system.Config.GetUInt32("Displays", "RoomA.Projector.Screen.Down.RelayPort", 2));
			RoomAProjector.ScreenControl.CommandInterval = system.Config.GetInt32("Displays", "RoomA.Projector.Screen.RelayPulseLength.Seconds", 2) * 1000;

			RoomARearDisplay = Displays.Create(system.Config.GetEnum<DisplayDriver>("Displays", "RoomA.RearDisplay.Driver", DisplayDriver.LGX).Value, 3);
			RoomARearDisplay.Configure(system.Config.GetString("Displays", "RoomA.RearDisplay.IPAddress", "192.168.1.102"));

			RoomBProjector = Displays.Create(system.Config.GetEnum<DisplayDriver>("Displays", "RoomB.Projector.Driver", DisplayDriver.SonyADCPProjector).Value, 2);
			RoomBProjector.Configure(system.Config.GetString("Displays", "RoomB.Projector.IPAddress", "192.168.1.101"));
			RoomBProjector.ScreenControl.SetRelays(Devices.ControlSystem,
				system.Config.GetUInt32("Displays", "RoomB.Projector.Screen.Up.RelayPort", 3),
				system.Config.GetUInt32("Displays", "RoomB.Projector.Screen.Down.RelayPort", 4));
			RoomBProjector.ScreenControl.CommandInterval = system.Config.GetInt32("Displays", "RoomB.Projector.Screen.RelayPulseLength.Seconds", 2) * 1000;
		}
	}
}