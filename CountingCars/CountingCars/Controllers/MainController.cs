using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountingCars.Forms;
using CountingCars.Models;
using System.Windows.Forms;
using System.IO;

namespace CountingCars.Controllers
{
	class MainController
	{
		private MainForm mainForm;
		private DataGridView backView;
		private DataGridView frontView;
		private int backIndex;
		private int frontIndex;

		string path = Directory.GetCurrentDirectory();
		string filename = "settings.txt";


		public MainController (MainForm form) {
			this.mainForm = form;
			this.backView = mainForm.BackDataView;
			this.frontView = mainForm.FrontDataView;
			this.backIndex = 0;
			this.frontIndex = 0;
			path = Path.Combine(path, filename);
		}

		public string LoadAppSettings () {
			if (File.Exists(path)) {
				return FileIn(path)[0];
			}
			else {
				return "00: 00";
			}
		}

		public void SaveAppSettings (string time) {
			List<string> timeOut = new List<string>();
			timeOut.Add(time);
			FileOut(path, timeOut);
		}
		/// <summary>
		/// Reads in a file from the specified path
		/// </summary>
		/// <param name="filePath">Path to the file</param>
		/// <returns>The data with each line as a seperate item in the ArrayList</returns>
		public static List<string> FileIn (string filePath) {
			List<string> fileInput = new List<string>();
			string[] byLine;
			byLine = File.ReadAllLines(filePath);
			fileInput.AddRange(byLine);
			return fileInput;
		}

		/// <summary>
		/// Opens a file and overwrites the data with the data from the ArrayList
		/// Each item in the ArrayList will be on a seperate line
		/// </summary>
		/// <param name="filePath">Path to the file</param>
		/// <param name="data">Data to be written to the file in string format</param>
		/// <returns>True if the write was successfull</returns>
		public static bool FileOut (string filepath, List<string> data) {
			bool fileWriteSuccess = false;
			string[] output = new string[data.Count];
			int location = 0;
			foreach (string item in data) {
				output[location] = item;
				location++;
			}
			try {
				if (File.Exists(filepath)) {
					File.Delete(filepath);
				}
				File.WriteAllLines(filepath, output);
				fileWriteSuccess = true;
			}
			catch {
				string exception = "File cannot be written: " + filepath;
				throw new System.InvalidOperationException(exception);
			}
			return fileWriteSuccess;
		}

		public void SaveVehicle (VehicleClass vehicle) {
			if (vehicle.side.Equals("Back")) {
				InsertVehicle(backView, vehicle, backIndex + 1);
				backIndex++;
				if ((backView.Rows.Count - 1) != backIndex) {
					backIndex = backView.Rows.Count - 1;
				}
			}
			if (vehicle.side.Equals("Front")) {
				InsertVehicle(frontView, vehicle, frontIndex + 1);
				frontIndex++;
				if ((frontView.Rows.Count - 1) != frontIndex) {
					frontIndex = frontView.Rows.Count - 1;
				}
			}
			mainForm.UpdateCounts(backIndex, frontIndex);
		}

		private void InsertVehicle (DataGridView view, VehicleClass vehicle, int index) {
			DataGridViewRow gridRow;
			gridRow = (DataGridViewRow)view.Rows[0].Clone();
			gridRow.Height = 15;
			gridRow.Cells[0].Value = index;
			view.Rows.Add(gridRow);

			gridRow.Cells[1].Value = vehicle.vehicleType + " " + vehicle.description;
			gridRow.Cells[2].Value = vehicle.time;
			if (vehicle.weight.Equals("Bike")) {
				gridRow.Cells[3].Value = 1;
			}
			if (vehicle.weight.Equals("Car")) {
				gridRow.Cells[4].Value = 1;
			}
			if (vehicle.weight.Equals("Lt. Goods")) {
				gridRow.Cells[5].Value = 1;
			}
			if (vehicle.weight.Equals("Bus")) {
				gridRow.Cells[6].Value = 1;
			}
			if (vehicle.weight.Equals("Med")) {
				gridRow.Cells[7].Value = 1;
			}
			if (vehicle.weight.Equals("Heavy")) {
				gridRow.Cells[8].Value = 1;
			}
		}

		public void ClearAllData () {
			backView.Rows.Clear();
			frontView.Rows.Clear();
			backIndex = 0;
			frontIndex = 0;
			mainForm.UpdateCounts(backIndex, frontIndex);
		}

	}
}
