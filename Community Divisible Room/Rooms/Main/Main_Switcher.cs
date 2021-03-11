using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.DeviceControl.SwitcherProtocols.Crestron;
using DiversifiedLibrary.DeviceControl;
using DiversifiedLibrary.Utilities.Rooms.Divisible;

namespace Community_Divisible_Room.Rooms
{
	public enum DmpsInputs : uint
	{
		Blank = 0,
		RoomA_AirMedia = 1,
		RoomB_AirMedia = 2,
		RoomA_RoomPC = 3,
		RoomA_Laptop = 7,
		RoomB_Laptop = 8
	}

	public enum DmpsOutputs : uint
	{
		RoomA_Monitor = 1,
		RoomA_Projector = 3,
		RoomB_Projector = 4,
		RoomA_Audio = 6,
		RoomB_Audio = 7
	}

	public class Main_Switcher
	{
		public DmpsSwitcher DmpsSwitcher { get; private set; }
		private Main_DivController _system;

		public Main_Switcher(Main_DivController system)
		{
			_system = system;
			DmpsSwitcher = new DmpsSwitcher(system.System, 1);
		}

		public void MakeRoute(DivisibleRoomId roomId, DmpsInputs input)
		{
			if (roomId == DivisibleRoomId.A)
			{
				DmpsSwitcher.SetInput((uint)DmpsOutputs.RoomA_Projector, (uint)DmpsOutputs.RoomA_Audio, (uint)input);
				DmpsSwitcher.SetInput((uint)DmpsOutputs.RoomA_Monitor, (uint)input);
				if (_system.RoomA.IsCombinedWith(_system.RoomB))
				{
					MakeRoute(DivisibleRoomId.B, input);
				}
			} 
			else if (roomId == DivisibleRoomId.B)
			{
				DmpsSwitcher.SetInput((uint)DmpsOutputs.RoomB_Projector, (uint)DmpsOutputs.RoomB_Audio, (uint)input);
			}
		}
	}
}