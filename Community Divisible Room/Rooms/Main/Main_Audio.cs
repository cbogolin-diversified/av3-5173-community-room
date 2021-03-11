using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using DiversifiedLibrary.DeviceControl.DSPProtocols;

namespace Community_Divisible_Room.Rooms
{
	public class Main_Audio
	{
		public DSP DSP { get; private set; }

		//Mutes & Faders
		public Fader RoomA_MasterVol { get; private set; }
		public Mute RoomA_Privacy { get; private set; }
		public Fader RoomA_Mic { get; private set; }
		public Fader RoomA_ATCTransmit { get; private set; }
		public Fader RoomA_ATCReceive { get; private set; }

		public Fader RoomB_MasterVol { get; private set; }
		public Mute RoomB_Privacy { get; private set; }
		public Fader RoomB_Mic { get; private set; }
		public Fader RoomB_ATCTransmit { get; private set; }
		public Fader RoomB_ATCReceive { get; private set; }

		//Dialers
		public Dialer RoomA_Dialer { get; private set; }
		public Dialer RoomB_Dialer { get; private set; }

		public Main_Audio(Main_DivController system)
		{ 
			DSP = DSPs.Create(DSPDriver.Tesira, 1).Configure(system.Config.GetString("Audio", "DSP.IPAddress", "192.168.1.50"));

			//RoomA
			RoomA_MasterVol = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomA.MasterVol.Tag", "rooma_mastervol"),
				system.Config.GetFloat("Audio", "DSP.RoomA.MasterVol.Min", -40),
				system.Config.GetFloat("Audio", "DSP.RoomA.MasterVol.Max", 6));
			RoomA_Mic = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomA.Mic.Tag", "rooma_mic"),
				system.Config.GetFloat("Audio", "DSP.RoomA.Mic.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomA.Mic.Max", 3));
			RoomA_ATCTransmit = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomA.ATCTransmit.Tag", "rooma_atctransmit"),
				system.Config.GetFloat("Audio", "DSP.RoomA.ATCTransmit.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomA.ATCTransmit.Max", 3));
			RoomA_ATCReceive = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomA.ATCReceive.Tag", "rooma_atcreceive"),
				system.Config.GetFloat("Audio", "DSP.RoomA.ATCReceive.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomA.ATCReceive.Max", 3));
			RoomA_Privacy = DSP.CreateMute(
				system.Config.GetString("Audio", "DSP.RoomA.Privacy.Tag", "rooma_privacy"));
			RoomA_Dialer = DSP.CreateDialer(DialerType.VoIP, system.Config.GetString("Audio", "DSP.RoomA.Dialer.Tag", "rooma_dialer"));

			//RoomB
			RoomB_MasterVol = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomB.MasterVol.Tag", "roomb_mastervol"),
				system.Config.GetFloat("Audio", "DSP.RoomB.MasterVol.Min", -40),
				system.Config.GetFloat("Audio", "DSP.RoomB.MasterVol.Max", 6));
			RoomB_Mic = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomB.Mic.Tag", "roomb_mic"),
				system.Config.GetFloat("Audio", "DSP.RoomB.Mic.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomB.Mic.Max", 3));
			RoomB_ATCTransmit = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomB.ATCTransmit.Tag", "roomb_atctransmit"),
				system.Config.GetFloat("Audio", "DSP.RoomB.ATCTransmit.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomB.ATCTransmit.Max", 3));
			RoomB_ATCReceive = DSP.CreateFader(
				system.Config.GetString("Audio", "DSP.RoomB.ATCReceive.Tag", "roomb_atcreceive"),
				system.Config.GetFloat("Audio", "DSP.RoomB.ATCReceive.Min", -9),
				system.Config.GetFloat("Audio", "DSP.RoomB.ATCReceive.Max", 3));
			RoomB_Privacy = DSP.CreateMute(
				system.Config.GetString("Audio", "DSP.RoomB.Privacy.Tag", "roomb_privacy"));
			RoomB_Dialer = DSP.CreateDialer(DialerType.VoIP, system.Config.GetString("Audio", "DSP.RoomB.Dialer.Tag", "roomb_dialer"));
		}
	}
}