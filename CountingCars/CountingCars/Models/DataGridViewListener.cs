using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CountingCars.Models
{
	class DataGridViewListener {
		private DataGridView dataGridView;

		public DataGridViewListener (DataGridView gridView) {
			this.dataGridView = gridView;
			dataGridView.KeyDown += new KeyEventHandler(importDataGridView_KeyDown);
		}
		/// <summary>
		/// Listener: Used for the Copy/Paste functionality in the data grid view
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importDataGridView_KeyDown (object sender, KeyEventArgs e) {
			if (e.Control && e.KeyCode == Keys.C) {
				DataObject d = dataGridView.GetClipboardContent();
				Clipboard.SetDataObject(d);
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.V) {
				string s = Clipboard.GetText();
				string[] lines = s.Split('\n');
				int row = dataGridView.CurrentCell.RowIndex;
				int col = dataGridView.CurrentCell.ColumnIndex;
				// Alternate fill for rows
				//for (int i = 1; i < lines.Count(); i++) {
				//    dataGridView.Rows.Add();
				//}
				DataGridViewRow gridRow;
				for (int i = 1; i < lines.Count(); i++) {
					gridRow = (DataGridViewRow)dataGridView.Rows[0].Clone();
					gridRow.Cells[0].Value = i;
					dataGridView.Rows.Add(gridRow);
				}

				foreach (string line in lines) {
					if (row < dataGridView.RowCount && line.Length > 0) {
						string[] cells = line.Split('\t');
						for (int i = 0; i < cells.GetLength(0); ++i) {
							if (col + i < this.dataGridView.ColumnCount) {
								dataGridView[col + i, row].Value = Convert.ChangeType(cells[i], dataGridView[col + i, row].ValueType);
							}
							else {
								break;
							}
						}
						row++;
					}
					else {
						break;
					}
				}
			}
		}
	}
}
