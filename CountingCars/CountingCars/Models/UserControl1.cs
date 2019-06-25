using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace DataUtils {
  public partial class UserControl1 : UserControl {
    private struct DataStruct {
      public int a,b,c,d,e,f,g;
    }

    private int processControlIndex;
    private double processControlSize;
    private int processRunCount;
    private List<DataStruct> data;
    private double offset1, offset2;
    private System.Windows.Forms.Timer timer1;
    private  System.ComponentModel.Container components;
    private  bool success;
    private  bool failure;
    private  bool processingActive;
    private  bool suspended;
    private  double epsilon;
    private  int processSequence;
    private  long processTimestamp;
    private  double scalingFactor;
    private Random random;
    private Form dlg;


    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    public UserControl1() {
      components = new System.ComponentModel.Container();
      timer1 = new System.Windows.Forms.Timer(this.components);
      SuspendLayout();
      timer1.Interval = 15;
      timer1.Tick += new System.EventHandler(this.timer1_Tick);
      MaximumSize = new System.Drawing.Size(300, 300);
      MinimumSize = new System.Drawing.Size(300, 300);
      Size = new System.Drawing.Size(300, 300);
      ResumeLayout(false);
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      SetStyle(ControlStyles.ResizeRedraw, true);
      Focus();
      processInitialize();
      dlg = new Form();
      dlg.Controls.Add(this);
      Location = new Point(0, 0);
      dlg.ClientSize = new Size(300, 300);
      dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
      dlg.Show();
    }

    protected override void OnMouseMove(MouseEventArgs e) {
      if (!success) {
        processControlIndex = e.X;
        if (e.X < processControlSize / 2) {
          processControlIndex = (int) processControlSize / 2;
        }
        if (e.X > 300 - (processControlSize / 2)) {
          processControlIndex = (int) (300 - (processControlSize / 2));
        }
      }
    }

    protected override void OnMouseClick(MouseEventArgs e) {
      suspended = false;
      if (e.Button == MouseButtons.Right) {
        suspended = true;
      }
      if (!processingActive) {
        scalingFactor = 150;
        epsilon = Math.PI / 2 * random.NextDouble() + (Math.PI / 4);
        processingActive = true;
      }
    }

    protected override void OnKeyDown(KeyEventArgs e) {
      switch (e.KeyCode) {
        case Keys.P:
          suspended = !suspended;
          break;
        case Keys.X:
        case Keys.Q:
          timer1.Enabled = false;
          dlg.Hide();
          break;
        case Keys.N:
          processInitialize();
          break;
        case Keys.Add:
        case Keys.Up:
        case Keys.Oemplus:
        case Keys.PageUp:
          scalingFactor += 10;
          break;
        case Keys.Subtract:
        case Keys.Down:
        case Keys.OemMinus:
        case Keys.PageDown:
          scalingFactor = scalingFactor < 10 ? 1 : scalingFactor - 10;
          break;
        case Keys.Decimal:
        case Keys.Right:
        case Keys.OemPeriod:
          processControlSize += 5;
          break;
        case Keys.Oemcomma:
        case Keys.Left:
          processControlSize -= 5;
          break;
      }
    }

    protected override void OnVisibleChanged(EventArgs e) {
      base.OnVisibleChanged(e);
      timer1.Enabled = Visible;
    }
    private void Clear(Graphics graph) { graph.FillRectangle(new SolidBrush(Color.Black), this.DisplayRectangle); }
    private void DrawP(Graphics graph) {
      graph.FillRectangle(new SolidBrush(Color.White), new Rectangle(processControlIndex - (int) (processControlSize / 2), 280, (int) processControlSize, 5));
    }
    private void DrawB1(Graphics graph, double a, double b) {
      graph.FillEllipse(new SolidBrush(Color.White), new Rectangle((int) a-3, (int) b-3, 6, 6));
      for (int i = 0; i < processRunCount; i++) {
        graph.FillEllipse(new SolidBrush(Color.White), new Rectangle(i * 7 + 1, 293, 6, 6));
      }
    }
    private void DrawB2(Graphics graph) { for (int i = 0; i < data.Count; i++) { DataStruct dat = data[i]; graph.FillRectangle(new SolidBrush(Color.FromArgb(200, dat.e, dat.f, dat.g)), new Rectangle(dat.a, dat.b, dat.c, dat.d)); } }

    protected override void OnPaint(PaintEventArgs e) {
      Graphics graph = e.Graphics;
      graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      Clear(graph);
      DrawP(graph);
      DrawB1(graph, offset1, offset2);
      DrawB2(graph);
      StringFormat centered = new StringFormat();
      centered.Alignment = StringAlignment.Center;
      if (success == true) {
        graph.FillRectangle(new SolidBrush(Color.FromArgb(0x7f, 0, 0, 0)), new Rectangle(1, 1, 298, 298));
        graph.DrawString("\x0059\x006f\x0075\x0020\x0057\x0069\x006e\x0021", this.Font, new SolidBrush(Color.FromArgb(0xdd, 255, 255, 255)), new RectangleF(0, 120, 300, 20), centered);
        graph.DrawString("\x0050\x0072\x0065\x0073\x0073\x0020\x0027\x004e\x0027\x0020\x0074\x006f\x0020\x0073\x0074\x0061\x0072\x0074\x0020\x0061\x0020\x006e\x0065\x0077\x0020\x0067\x0061\x006d\x0065", this.Font, new SolidBrush(Color.FromArgb(0xdd, 255, 255, 255)), new RectangleF(0, 160, 300, 20), centered);
        return;
      }
      if (failure == true) {
        graph.FillRectangle(new SolidBrush(Color.FromArgb(0x7f, 0, 0, 0)), new Rectangle(1, 1, 298, 298));
        graph.DrawString("\x0059\x006f\x0075\x0020\x004c\x006f\x0073\x0065\x0021", this.Font, new SolidBrush(Color.FromArgb(0xdd, 255, 255, 255)), new RectangleF(0, 120, 300, 20), centered);
        graph.DrawString("\x0050\x0072\x0065\x0073\x0073\x0020\x0027\x004e\x0027\x0020\x0074\x006f\x0020\x0073\x0074\x0061\x0072\x0074\x0020\x0061\x0020\x006e\x0065\x0077\x0020\x0067\x0061\x006d\x0065", this.Font, new SolidBrush(Color.FromArgb(0xdd, 255, 255, 255)), new RectangleF(0, 160, 300, 20), centered);
        return;
      }
      if (suspended == true) {
        graph.FillRectangle(new SolidBrush(Color.FromArgb(0x7f, 0, 0, 0)), new Rectangle(1, 1, 298, 298));
        graph.DrawString("\x0050\x0061\x0075\x0073\x0065\x0064", this.Font, new SolidBrush(Color.FromArgb(0xdd, 255, 255, 255)), new RectangleF(0, 140, 300, 20), centered);
      }
    }

    private void processInitialize() {
      data = new List<DataStruct>();
      random = new Random();

      for (var i = 0; i < 10; i++) {
        for (var j = 0; j < 5; j++) {
          DataStruct datum;
          datum.a = i * 30 + 1;
          datum.b = j * 15 + 1;
          datum.c = 28;
          datum.d = 13;
          datum.e = random.Next(128) + 128;
          datum.f = random.Next(128) + 128;
          datum.g = random.Next(128) + 128;
          data.Add(datum);
        }
      }
      processRunCount = 3;
      processingActive = false;
      offset1 = 150;
      offset2 = 277;
      scalingFactor = 0;
      epsilon = 0;
      processControlIndex = 150;
      processControlSize = 60;
      suspended = false;
      processTimestamp = DateTime.Now.Ticks;
      processSequence = 0;
      success = false;
      failure = false;
    }

    private void Process() {
      if ((success == false) && (failure == false)) {
        if (processingActive == true) {
          if (suspended == false) {
            long timeStep = DateTime.Now.Ticks - processTimestamp; // ticks            
            var delta = scalingFactor * timeStep / 10000000;
            double deltaX = delta * Math.Cos(epsilon);
            double deltaY = delta * Math.Sin(epsilon);
            if (deltaX > 6) deltaX = 6;
            if (deltaX < -6) deltaX = -6;
            if (deltaY > 6) deltaY = 6;
            if (deltaY < -6) deltaY = -6;
            offset1 += deltaX;
            offset2 -= deltaY;
            checkBoundaryConditions();
            checkControlSurface();
            checkData();
            if (data.Count < 1) {
              success = true;
              suspended = true;
              processRunCount = 0;
            }
          }
        } else {
          offset1 = processControlIndex;
          offset2 = 277;
        }
      }
      processTimestamp = DateTime.Now.Ticks;

    }

    private void invertOffset1(double bound, bool adjust) {
      double rand = 0;
      if (adjust == true) {
        rand = (random.NextDouble() - 0.5) / 4;
      }
      epsilon = Math.PI - epsilon + rand;
      offset1 = bound;
      scalingFactor += 10 * (random.NextDouble() - 0.4);
    }
    private void invertOffset2(double bound, bool adjust) {
      double rand = 0;
      if (adjust == true) {
        rand = (random.NextDouble() - 0.5) / 4;
      }
      epsilon = 2 * Math.PI - epsilon + rand;
      offset2 = bound;
      scalingFactor += 10 * (random.NextDouble() - 0.4);
    }
    private void checkBoundaryConditions() {
      if (offset1 < 3) {
        invertOffset1(3, false);
      }
      if (offset1 > 297) {
        invertOffset1(297, false);
      }
      if (offset2 < 3) {
        invertOffset2(3, false);
      }
      if (offset2 > 297) {
        processingActive = false;
        processRunCount--;
        if (processRunCount < 0) {
          failure = true;
        }
        processSequence = 0;
        processControlSize = 60;
      }
    }
    private void checkControlSurface() {
      var alpha = processControlIndex - (processControlSize / 2);
      var beta = processControlIndex + (processControlSize / 2);
      if (offset2 > 277) {
        if ((offset1 >= alpha) && (offset1 <= beta)) {
          adjustProcessControl();
          invertOffset2(277, true);
        }
      }
    }
    private void checkData() {
      for (var i = 0; i < data.Count; i++) {
        var datum = data[i];
        var alpha = datum.a;
        var beta = datum.b;
        var gamma = alpha + datum.c;
        var delta = beta + datum.d;
        if ((offset1 > alpha) && (offset1 < gamma)) {
          if ((offset2 > beta - 3) && (offset2 < delta)) {
            invertOffset2(beta - 3, true);
            data.RemoveAt(i);
            continue;
          } else if ((offset2 < delta + 3) && (offset2 > beta)) {
            invertOffset2(delta + 3, true);
            data.RemoveAt(i);
            continue;
          }
        } else if ((offset2 > beta) && (offset2 < delta)) {
          if ((offset1 > alpha - 3) && (offset1 < gamma)) {
            invertOffset1(alpha - 3, true);
            data.RemoveAt(i);
            continue;
          } else if ((offset1 < gamma + 3) && (offset1 > alpha)) {
            invertOffset1(gamma + 3, true);
            data.RemoveAt(i);
            continue;
          }
        }
        var ydir = -Math.Sin(epsilon);
        var xdir = Math.Cos(epsilon);
        if ((offset1 < gamma + 3) && (offset1 > alpha)) {
          if ((offset2 < delta + 3) && (offset2 > beta)) {
            if (xdir < 0) {
              invertOffset1(gamma + 3, false);
            }
            if (ydir < 0) {
              invertOffset2(delta + 3, false);
            }
            data.RemoveAt(i);
            continue;
          } else if ((offset2 < delta) && (offset2 > beta - 3)) {
            if (xdir < 0) {
              invertOffset1(gamma + 3, false);
            }
            if (ydir > 0) {
              invertOffset2(beta - 3, false);
            }
            data.RemoveAt(i);
            continue;
          }
        } else if ((offset1 > alpha - 3) && (offset1 < gamma)) {
          if ((offset2 < delta + 3) && (offset2 > beta)) {
            if (xdir > 0) {
              invertOffset1(alpha - 3, false);
            }
            if (ydir < 0) {
              invertOffset2(delta + 3, false);
            }
            data.RemoveAt(i);
            continue;
          } else if ((offset2 < delta) && (offset2 > beta - 3)) {
            if (xdir > 0) {
              invertOffset1(alpha - 3, false);
            }
            if (ydir > 0) {
              invertOffset2(beta - 3, false);
            }
            data.RemoveAt(i);
            continue;
          }
        }
      }
    }

    private void adjustProcessControl() {
      if (success == false) {
        if (suspended == false) {
          if (processingActive == true) {
            if (processControlSize > 5) {
              processSequence++;
              if (processSequence > 3) {
                processSequence = 0;
                processControlSize -= 0.5;
              }
            }
          }
        }
      }
    }

    private void timer1_Tick(object sender, EventArgs e) { Process(); Refresh(); }
  }
}
