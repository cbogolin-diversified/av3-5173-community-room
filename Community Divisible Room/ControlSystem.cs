using Crestron.SimplSharpPro;
using DiversifiedLibrary.Utilities.Sys;
using DiversifiedLibrary.Utilities.IO;
using Community_Divisible_Room.Rooms;

namespace Community_Divisible_Room
{
	public class ControlSystem : CrestronControlSystem
	{
		public static ControlSystem Controller { get; private set; }

		public static Configuration SysConfig { get; private set; }

		public ControlSystem()
			: base()
		{
			/*
			 * This method calls Devices.SetControlSystem and then runs the provided delegate.
			 * If there is an error, rather than attempting to restart the program,
			 * this method will print the error to console and to the error log,
			 * and then manually stop the program.
			 */
			SystemUtilities.Start(this, () =>
			{
				//Do your programming here...
				SysConfig = Configuration.Default;
				var divController = new Main_DivController();
			});

			//Don't manually set Thread.MaxNumberOfUserThreads unless told otherwise.
			//Don't override InitializeSystem -- DiversifiedLibrary handles the relevant code now.
		}
	}
}