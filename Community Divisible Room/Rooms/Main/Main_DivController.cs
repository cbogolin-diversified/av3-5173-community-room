using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.Utilities.Rooms.Divisible;
using Crestron.SimplSharpPro.GeneralIO;
using DiversifiedLibrary.Utilities.IO;
using Community_Divisible_Room.Rooms.Main;

namespace Community_Divisible_Room.Rooms
{
	public class Main_DivController : DivisibleRoomController
	{
		//Divisible Stuff
		public GlsPartCn PhysicalSensor { get; private set; }
		public DivisibleRoomPartition Partition { get; private set; }

		//Shared Classes
		public Main_Displays Displays { get; private set; }
		public Main_Audio Audio { get; private set; }
		public Main_Switcher Switcher { get; private set; }

		//Rooms
		public LargeRoom RoomA { get; private set; }
		public SmallRoom RoomB { get; private set; }

		public Configuration Config { get; private set; }

		public Main_DivController()
			: base("div", "Debug Commands for Divisible Room Controller")
		{

			Config = ControlSystem.SysConfig;

			//Sensor Setup
			InitPartitionSensor();

			//Device Control Setup
			Audio = new Main_Audio(this);
			Switcher = new Main_Switcher(this);
			Displays = new Main_Displays(this);

			//Room Setup
			RoomA = new LargeRoom(this, DivisibleRoomId.A);
			RoomB = new SmallRoom(this, DivisibleRoomId.B);
			RoomA.Initialize();
			RoomB.Initialize();

			//Div Setup
			AddCombination(RoomA, new DivisibleRoom[] { RoomA, RoomB }, Partition);
			SetChangeRequirement(DivisibleRoomControllerChangeRequirement.AllStopped);
		}

		private void InitPartitionSensor()
		{
			PhysicalSensor = new GlsPartCn(Convert.ToUInt32(Config.GetString("Sensors", "PartitionSensor.CresnetID", "97"), 16), System);
			Partition = new DivisibleRoomPartition(PhysicalSensor);
			PhysicalSensor.Register();
			PhysicalSensor.Enable.BoolValue = true;

            Commands.AddCommand("wallsensor", "Debug Command to See if Sensor is Online", _ => {
                CrestronConsole.PrintLine("Partition Sensor Online Status: {0}", PhysicalSensor.IsOnline ? "Online" : "Offline");
            });

			Commands.AddCommand("partition", "Debug Commands for Partition", param =>
			{
				if (param == "open")
				{
					Partition.SetOverride(DivisibleRoomPartitionState.Open);
					CrestronConsole.PrintLine("Partition set to {0}.", Partition.State.ToString());
				}
				else if (param == "close")
				{
					Partition.SetOverride(DivisibleRoomPartitionState.Closed);
					CrestronConsole.PrintLine("Partition set to {0}.", Partition.State.ToString());
				}
				else if (param == "clear")
				{
					Partition.ClearOverride();
					CrestronConsole.PrintLine("Partition Override Cleared.");
				}
				else if (param == "state")
				{
					CrestronConsole.PrintLine("Current state of Partition: {0}", Partition.State.ToString());
				}
				else
				{
					CrestronConsole.PrintLine("Not a valid parameter. Valid parameters: open, close, clear, state");
				}
			});
		}

	}
}