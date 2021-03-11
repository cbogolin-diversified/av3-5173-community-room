using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.Utilities.Rooms;
using DiversifiedLibrary.Utilities.Rooms.Divisible;
using DiversifiedLibrary.UserInterface;
using DiversifiedLibrary.Utilities.IO;
using Crestron.SimplSharpPro.UI;
using Community_Divisible_Room.Rooms.Main;
using DiversifiedLibrary.Utilities.Common;
using DiversifiedLibrary.DeviceControl.DisplayProtocols;
using DiversifiedLibrary.DeviceControl.SwitcherProtocols.Crestron;

namespace Community_Divisible_Room.Rooms
{
	public class LargeRoom : DivisibleRoom
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

		//Devices
		private Display _roomAProjector, _roomARearDisplay, _roomBProjector;
		private DmpsSwitcher _switcher;

		//Misc
		private BasicScheduler _scheduler;
		private SimpleTimeout _timeout;

		//CTor
		public LargeRoom(Main_DivController system, DivisibleRoomId id)
			: base(system, "Large Room A", "rooma")
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
			InitDisplays();
		}

		protected override void OnRoomStarting()
		{
			base.OnRoomStarting();

			_system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomA_RoomPC);
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
			TouchPanel.AddPanel(new Tsw770(0x03, System));
			TouchPanel.LoadSmartObjects(FileEx.GetSGDFile());
			TouchPanel.Set(Joins.RoomName, _config.GetString("UI", "RoomA.RoomName", "Room A"));
			TouchPanel.AddPressToggleAction(Joins.Help);
		}

		private void InitDisplays()
		{
			_roomAProjector = _system.Displays.RoomAProjector;
			_roomAProjector.CreateUI(TouchPanel)
				.SetButtons(Joins.RoomA_DisplayOn, Joins.RoomA_DisplayOff);
			_roomAProjector.ScreenControl.CreateUI(TouchPanel)
				.SetButtons(Joins.RoomA_ScreenUp, Joins.RoomA_ScreenDown, 0);

			_roomARearDisplay = _system.Displays.RoomARearDisplay;
			_roomARearDisplay.CreateUI(TouchPanel)
				.SetButtons(Joins.RoomA_RearDisplayOn, Joins.RoomA_RearDisplayOff);

			_roomBProjector = _system.Displays.RoomBProjector;
			_roomBProjector.CreateUI(TouchPanel)
				.SetButtons(Joins.RoomB_DisplayOn, Joins.RoomB_DisplayOff);
			_roomBProjector.ScreenControl.CreateUI(TouchPanel)
				.SetButtons(Joins.RoomB_ScreenUp, Joins.RoomB_ScreenDown, 0);
		}

		private void InitDSP()
		{ }

		private void InitSwitcher()
		{
			_switcher = _system.Switcher.DmpsSwitcher;

			//Routing Buttons
			TouchPanel
				.AddDigitalJoinAction(Joins.Blank, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.Blank))
				.AddDigitalJoinAction(Joins.RoomPC, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomA_RoomPC))
				.AddDigitalJoinAction(Joins.RoomA_Laptop, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomA_Laptop))
				.AddDigitalJoinAction(Joins.RoomA_AirMedia, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomA_AirMedia))
				.AddDigitalJoinAction(Joins.RoomB_Laptop, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomB_Laptop))
				.AddDigitalJoinAction(Joins.RoomB_AirMedia, press => _system.Switcher.MakeRoute(Identifier, DmpsInputs.RoomB_AirMedia));

			_switcher.BaseEvent += SwitcherBaseEvent;
		}

		void SwitcherBaseEvent(DiversifiedLibrary.DeviceControl.SwitcherProtocols.Switcher sender, DiversifiedLibrary.DeviceControl.SwitcherProtocols.SwitcherBaseEventArgs args)
		{
			if (args.EventId == DiversifiedLibrary.DeviceControl.SwitcherProtocols.SwitcherEventId.OutputVideoChanged && args.Output == (uint)DmpsOutputs.RoomA_Projector)
			{
				TouchPanel.Set(Joins.Blank, args.Input == (uint)DmpsInputs.Blank);
				TouchPanel.Set(Joins.RoomPC, args.Input == (uint)DmpsInputs.RoomA_RoomPC);
				TouchPanel.Set(Joins.RoomA_Laptop, args.Input == (uint)DmpsInputs.RoomA_Laptop);
				TouchPanel.Set(Joins.RoomA_AirMedia, args.Input == (uint)DmpsInputs.RoomA_AirMedia);
				TouchPanel.Set(Joins.RoomB_Laptop, args.Input == (uint)DmpsInputs.RoomB_Laptop);
				TouchPanel.Set(Joins.RoomB_AirMedia, args.Input == (uint)DmpsInputs.RoomB_AirMedia);

				/*
				 Source Routed Icon Feedback 
				 * 0 == blank, 
				 * 1 == room pc, 
				 * 2 == room a laptop, 
				 * 3 == room a wireless, 
				 * 4 == room b laptop, 
				 * 5 == room b wireless
				 */

				TouchPanel.Set(Joins.RoomA_SourceRouted, 
					(ushort)(args.Input == (uint)DmpsInputs.Blank ? 0 : 
					args.Input == (uint)DmpsInputs.RoomA_RoomPC ? 1 : 
					args.Input == (uint)DmpsInputs.RoomA_Laptop ? 2 : 
					args.Input == (uint)DmpsInputs.RoomA_AirMedia ? 3 : 
					args.Input == (uint)DmpsInputs.RoomB_Laptop ? 4 : 
					args.Input == (uint)DmpsInputs.RoomB_AirMedia ? 5 : 0));
			}
		}
	}
}