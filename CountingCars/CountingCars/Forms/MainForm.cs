using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CountingCars.Models;
using CountingCars.Controllers;

namespace CountingCars.Forms
{
	public partial class MainForm : Form
	{
		MainController control;
		List<RadioButton> typeButtons;
		List<RadioButton> descriptionButtons;
		List<RadioButton> weightButtons;
		public MainForm () {
			InitializeComponent();

			DataGridViewListener listen1 = new DataGridViewListener(backDataGridView);
			DataGridViewListener listen2 = new DataGridViewListener(frontDataGridView);
			SetupForm();
			control = new MainController(this);
			weightGroupBox.Visible = false;
			time1ComboBox.Text = control.LoadAppSettings();
		}

		// This function runs every time the window to the application is changed in size and automaticaly resizes certain components in the window.
		protected override void OnResize (EventArgs e) {
			base.OnResize(e);
			int tabHeight = this.Height;
			tabHeight -= 250;
			this.backDataGridView.Height = tabHeight;
			this.frontDataGridView.Height = tabHeight;

			this.backDataGridView.Refresh();
			this.frontDataGridView.Refresh();

			int boxYPos = this.Height - 96;
			Point boxPos = this.richTextBox1.Location;
			boxPos.Y = boxYPos;
			this.richTextBox1.Location = boxPos;
			this.richTextBox1.Refresh();

			boxPos = this.richTextBox2.Location;
			boxPos.Y = boxYPos;
			this.richTextBox2.Location = boxPos;
			this.richTextBox2.Refresh();

		}

		// This function runs every time the application closes and checks if there are any changes to the document that is open.
		protected override void OnFormClosing (FormClosingEventArgs e) {
			base.OnFormClosing(e);
			control.SaveAppSettings(time1ComboBox.Text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clearButton_Click (object sender, EventArgs e) {
			ClearForm();
			richTextBox1.Text = "";
			richTextBox2.Text = "";
			control.ClearAllData();
		}
		/// <summary>
		/// 
		/// </summary>
		private void ClearForm () {
			foreach (RadioButton item in typeButtons) {
				item.Checked = false;
			}
			foreach (RadioButton item in descriptionButtons) {
				item.Checked = false;
			}
			foreach (RadioButton item in weightButtons) {
				item.Checked = false;
			}
			otherTypeTextBox.Text = "";
			otherDescriptionTextBox.Text = "";
			addCheckBox.Checked = false;
			addTextBox.Text = "";
		}
		/// <summary>
		/// 
		/// </summary>
		private void SetupForm () {
			string addItem;
			for (int i = 0; i < 60; i++) {
				for (int j = 0; j < 60; j++) {
					addItem = i.ToString();
					if (i < 10) {
						addItem = "0" + i;
					}
					addItem += ": ";
					if (j < 10) {
						addItem += "0";
					}
					addItem += j;
					time1ComboBox.Items.Add(addItem);
				}
			}
			time1ComboBox.SelectedIndex = 0;
			backLabel.Text = "0";
			frontLabel.Text = "0";
			typeButtons = new List<RadioButton>();
			descriptionButtons = new List<RadioButton>();
			weightButtons = new List<RadioButton>();

			typeButtons.Add(bikeRadioButton);
			typeButtons.Add(carRadioButton);
			typeButtons.Add(suvRadioButton);
			typeButtons.Add(regTruckRadioButton);
			typeButtons.Add(crewTruckRadioButton);
			typeButtons.Add(extendedTruckRadioButton);
			typeButtons.Add(comVanRadioButton);
			typeButtons.Add(duallyTruckRadioButton);
			typeButtons.Add(twoTonTruckRadioButton);
			typeButtons.Add(cargoTruckRadioButton);
			typeButtons.Add(movingTruckRadioButton);
			typeButtons.Add(schoolBusRadioButton);
			typeButtons.Add(busRadioButton);
			typeButtons.Add(semiRadioButton);
			typeButtons.Add(otherTypeRadioButton);

			descriptionButtons.Add(noTrailerRadioButton);
			descriptionButtons.Add(trailerRadioButton);
			descriptionButtons.Add(camperRadioButton);
			descriptionButtons.Add(fifthWheelRadioButton);
			descriptionButtons.Add(fifthWheelFlatRadioButton);
			descriptionButtons.Add(longFlatBedRadioButton);
			descriptionButtons.Add(s1TrailerRadioButton);
			descriptionButtons.Add(s2TrailersRadioButton);
			descriptionButtons.Add(s3TrailersRadioButton);
			descriptionButtons.Add(shortBedRadioButton);
			descriptionButtons.Add(longBedRadioButton);
			descriptionButtons.Add(otherDescriptionRadioButton);

			weightButtons.Add(weightCarRadioButton);
			weightButtons.Add(weightLightGoodsRadioButton);
			weightButtons.Add(weightMedRadioButton);
			weightButtons.Add(weightHeavyRadioButton);
			weightButtons.Add(weightBusRadioButton);
			weightButtons.Add(weightBikeRadioButton);

			backRadioButton.Checked = true;
			ClearForm();

			time1ComboBox.KeyDown += new KeyEventHandler(time_KeyDown);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveButton_Click (object sender, EventArgs e) {
			try {
				SaveVehicle();
			}
			catch (InvalidOperationException exception) {
				MessageBox.Show(this, exception.Message, "More data needed", MessageBoxButtons.OKCancel);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void SaveVehicle () {
			VehicleClass vehicle = new VehicleClass();
			RadioButton typeSelect = new RadioButton();
			RadioButton descriptionSelect = new RadioButton(); ;
			RadioButton weightSelect = new RadioButton();
			bool found = false;
			foreach (RadioButton item in typeButtons) {
				if (item.Checked) {
					typeSelect = item;
					found = true;
					break;
				}
			}
			if (!found) {
				throw new InvalidOperationException("Type Needed");
			}
			found = false;
			CheckType();
			foreach (RadioButton item in descriptionButtons) {
				if (item.Checked) {
					descriptionSelect = item;
					found = true;
					break;
				}
			}
			if (!found) {
				descriptionSelect.Text = "";
			}
			found = false;
			foreach (RadioButton item in weightButtons) {
				if (item.Checked) {
					weightSelect = item;
					found = true;
					break;
				}
			}
			if (!found) {
				throw new InvalidOperationException("Weight Needed");
			}
			vehicle.vehicleType = typeSelect.Text;
			vehicle.description = descriptionSelect.Text;
			vehicle.time = time1ComboBox.Text;
			if (typeSelect == otherTypeRadioButton) {
				vehicle.vehicleType = otherTypeTextBox.Text;
			}
			if (descriptionSelect == otherDescriptionRadioButton) {
				vehicle.description = otherDescriptionTextBox.Text;
			}
			if (addCheckBox.Checked) {
				vehicle.description += " " + addTextBox.Text;
			}
			vehicle.weight = weightSelect.Text;
			if (backRadioButton.Checked) {
				vehicle.side = backRadioButton.Text;
			}
			else if (frontRadioButton.Checked) {
				vehicle.side = frontRadioButton.Text;
			}
			if (string.IsNullOrWhiteSpace(vehicle.side)) {
				throw new InvalidOperationException("Side Needed");
			}
			control.SaveVehicle(vehicle);

			ClearForm();
		}
		/// <summary>
		/// 
		/// </summary>
		private void CheckType () {
			RadioButton typeSelect = new RadioButton();
			bool found = false;
			foreach (RadioButton item in typeButtons) {
				if (item.Checked) {
					typeSelect = item;
					found = true;
					break;
				}
			}
			if (!found) {
				throw new InvalidOperationException("Type Needed");
			}
			found = false;
			if (typeSelect == carRadioButton || typeSelect == suvRadioButton || typeSelect == regTruckRadioButton) {
				weightCarRadioButton.Checked = true;
			}
			else if (typeSelect == extendedTruckRadioButton || typeSelect == crewTruckRadioButton || typeSelect == comVanRadioButton) {
				weightLightGoodsRadioButton.Checked = true;
			}
			else if (typeSelect == duallyTruckRadioButton || typeSelect == twoTonTruckRadioButton || typeSelect == cargoTruckRadioButton || typeSelect == movingTruckRadioButton) {
				weightMedRadioButton.Checked = true;
			}
			else if (typeSelect == schoolBusRadioButton || typeSelect == busRadioButton) {
				weightBusRadioButton.Checked = true;
			}
			if ((typeSelect == semiRadioButton) && noTrailerRadioButton.Checked) {
				weightMedRadioButton.Checked = true;
			}
			else if (typeSelect == semiRadioButton) {
				weightHeavyRadioButton.Checked = true;
				if (!longFlatBedRadioButton.Checked && !s1TrailerRadioButton.Checked && !s2TrailersRadioButton.Checked && !s3TrailersRadioButton.Checked) {
					s1TrailerRadioButton.Checked = true;
				}
			}
			if (typeSelect == bikeRadioButton) {
				weightBikeRadioButton.Checked = true;
			}
			if (typeSelect == regTruckRadioButton && longBedRadioButton.Checked) {
				weightLightGoodsRadioButton.Checked = true;
			}
			else if (typeSelect == regTruckRadioButton) {
				weightCarRadioButton.Checked = true;
			}
		}

		// TODO: Clear form and data.

		/// <summary>
		/// 
		/// </summary>
		/// <param name="back"></param>
		/// <param name="front"></param>
		public void UpdateCounts (int back, int front) {
			backLabel.Text = back.ToString();
			frontLabel.Text = front.ToString();
		}

		public DataGridView BackDataView{
			get {
				return backDataGridView;
			}
		}
		public DataGridView FrontDataView {
			get {
				return frontDataGridView;
			}
		}


		/// <summary>
		/// Listener: Used for the moving the time forward
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void time_KeyDown (object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.A) {
				int timeIndex = time1ComboBox.SelectedIndex;
				timeIndex--;
				time1ComboBox.SelectedIndex = timeIndex;
				time1ComboBox.Refresh();
				e.Handled = true;
			}
			if (e.KeyCode == Keys.Z) {
				int timeIndex = time1ComboBox.SelectedIndex;
				timeIndex++;
				time1ComboBox.SelectedIndex = timeIndex;
				e.Handled = true;
			}
			if (e.KeyCode == Keys.Enter) {
				saveButton_Click(sender, e);
			}
		}


		private void backRadioButton_CheckedChanged (object sender, EventArgs e) {
			groupBox6.BackColor = Color.MistyRose;
		}

		private void frontRadioButton_CheckedChanged (object sender, EventArgs e) {
			groupBox6.BackColor = Color.PaleGreen;
		}

		private void weightToggleButton_Click (object sender, EventArgs e) {
			Point toolLocation = weightPanel.Location;
			int toggleX = 1156;
			if (toolLocation.X == toggleX) {
				toolLocation = new Point(1000, toolLocation.Y);
				weightToggleButton.Text = ">";
				weightPanel.BorderStyle = BorderStyle.FixedSingle;
				weightGroupBox.Visible = true;
			}
			else {
				toolLocation = new Point(toggleX, toolLocation.Y);
				weightToggleButton.Text = "<";
				weightPanel.BorderStyle = BorderStyle.None;
				weightGroupBox.Visible = false;
			}
			weightPanel.Location = toolLocation;
			weightPanel.Invalidate();
		}
	}
}
