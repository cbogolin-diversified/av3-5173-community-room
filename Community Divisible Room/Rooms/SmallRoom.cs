using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.Utilities.Rooms;
using DiversifiedLibrary.Utilities.Rooms.Divisible;
using DiversifiedLibrary.Utilities.IO;
using DiversifiedLibrary.UserInterface;
using DiversifiedLibrary.Utilities.Common;
using Crestron.SimplSharpPro.UI;
using Community_Divisible_Room.Rooms.Main;

namespace Community_Divisible_Room.Rooms
{
	public class SmallRoom : DivisibleRoom
	{
		//Div variables
		private Main_DivController _system;
		private Configuration _config;

		//UI variables
		public TouchPanel TouchPanel { get; private set; }
		private TouchPanelStandardInterface _interface;
		public Interlock PageFlipInterlock { get { return _interface.PageFlipInterlock; } }
		public Interlock LeftMenuInterlock { get { return _interface.LeftMenuInterlock; } }
		public Interlock AdminTabsInterlock { get { return _interface.AdminTabsInterlock; } }
		public ShutdownDialog ShutdownDialog { get { return _interface.ShutdownDialog; } }
		public TouchPanelStandardInterfaceJoins PreProgrammedJoins { get { return _interface.PreProgrammedJoins; } }

		//Misc
		private BasicScheduler _scheduler;
		private SimpleTimeout _timeout;

		//CTor
		public SmallRoom(Main_DivController system, DivisibleRoomId id)
			: base(system, "Small Room B", "roomb")
		{
			_system = system;
			Identifier = id;
			TouchPanel = new TouchPanel();
			_interface = new TouchPanelStandardInterface(TouchPanel, TouchPanelStandardInterfaceJoinsVersion.Version2);
		}

		//Overrides
		protected override void OnInitialize()
		{
			base.OnInitialize();

			_config = ControlSystem.SysConfig;

			//Init Methods
			InitTouchPanel();
		}

		protected override void OnRoomStarting()
		{
			base.OnRoomStarting();
		}

		protected override void OnRoomStopping()
		{
			base.OnRoomStopping();
		}

		protected override void OnCombinationChanged()
		{
			throw new NotImplementedException();
		}

		//Init Setup Methods
		private void InitTouchPanel()
		{
			_interface.Initialize(TouchPanel, this);
			//_interface.AdminTabsInterlock.SelectedJoinChanged += OnAdminTabsInterlockEvent;
			//_interface.LeftMenuInterlock.SelectedJoinChanged += OnLeftMenuInterlockEvent;
			//_interface.PageFlipInterlock.SelectedJoinChanged += OnPageFlipInterlockEvent;
			TouchPanel.AddPanel(new Tsw770(0x04, System));
			TouchPanel.LoadSmartObjects(FileEx.GetSGDFile());
			TouchPanel.Set(Joins.RoomName, _config.GetString("UI", "RoomB.RoomName", "Room B"));
			TouchPanel.AddPressToggleAction(Joins.Help);
		}
	}
}