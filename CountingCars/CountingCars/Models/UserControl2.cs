using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace DataUtils {
  public partial class UserControl2 : UserControl {
    private class DataStruct {
      public float x,y;
      public PointF activeTrace;
      public int dir;
      public List<PointF> trace;
      public Color visColor;
    }

    private System.Windows.Forms.Timer timer1;
    private  System.ComponentModel.Container components;
    private  bool success;
    private  bool failure;
    private  bool suspended;
    private Random random;
    private Form dlg;
    private float scalingFactor;
    private  long processTimestamp;
    private List<DataStruct> data;
    private const float epsilon=0.001F;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    public UserControl2() {
      components = new System.ComponentModel.Container();
      timer1 = new System.Windows.Forms.Timer(this.components);
      SuspendLayout();
      timer1.Interval = 15;
      timer1.Tick += new System.EventHandler(this.timer1_Tick);
      MaximumSize = new System.Drawing.Size(300, 200);
      MinimumSize = new System.Drawing.Size(300, 200);
      Size = new System.Drawing.Size(300, 200);
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
      dlg.ClientSize = new Size(300, 200);
      dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
      dlg.Show();
    }

    private void terminate() {
      timer1.Enabled = false;
      dlg.Hide();
    }

    private void adjustScale(float amt) {
      scalingFactor += amt;
      if (scalingFactor < 10) {
        scalingFactor = 10;
      }
    }

    const int WM_KEYDOWN = 0x100;
    const int WM_SYSKEYDOWN = 0x104;


    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
      if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN) {
        switch (keyData) {
          case Keys.P:
            suspended = !suspended;
            break;
          case Keys.X:
            terminate(); break;
          case Keys.Q:
            terminate(); break;
          case Keys.N:
            processInitialize();
            break;
          case Keys.Add:
            adjustScale(10); break;
          case Keys.Oemplus:
            adjustScale(10); break;
          case Keys.PageUp:
            adjustScale(10); break;
          //case Keys.Up:
          //  adjustScale(10); break;
          case Keys.Subtract:
            adjustScale(-10); break;
          case Keys.OemMinus:
            adjustScale(-10); break;
          case Keys.PageDown:
            adjustScale(-10); break;
          //case Keys.Down:
          //  adjustScale(-10); break;
          case Keys.Right:
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = (data[0].dir + 1) % 4;
            break;
          case Keys.Left:
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = (data[0].dir + 3) % 4;
            break;
          /*
                     case Keys.Up:
          if (data[0].dir % 2 == 1) {
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = 0;
          }
          break;
        case Keys.Down:
          if (data[0].dir % 2 == 1) {
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = 2;
          }
          break;
        case Keys.Right:
          if (data[0].dir % 2 == 0) {
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = 1;
          }
          break;
        case Keys.Left:
          if (data[0].dir % 2 == 0) {
            data[0].trace.Add(new PointF(data[0].x, data[0].y));
            data[0].dir = 3;
          }
          break;
* 
           */

        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void OnVisibleChanged(EventArgs e) {
      base.OnVisibleChanged(e);
      timer1.Enabled = Visible;
    }

    private void Clear(Graphics graph) { graph.FillRectangle(new SolidBrush(Color.Black), this.DisplayRectangle); }

    private void Visualize(Graphics graph) {
      foreach (DataStruct datum in data) {
        Pen pen = new Pen(datum.visColor);
        PointF lastTrace = datum.trace[0];
        for (int i= 1; i < datum.trace.Count; i++) {
          graph.DrawLine(pen, lastTrace, datum.trace[i]);
          lastTrace = datum.trace[i];
        }
        graph.DrawLine(pen, lastTrace, new PointF(datum.x, datum.y));
      }
    }

    protected override void OnPaint(PaintEventArgs e) {
      Graphics graph = e.Graphics;
      //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      Clear(graph);
      Visualize(graph);
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
      random = new Random();

      data = new List<DataStruct>();
      for (int i = 0; i < 2; i++) {
        DataStruct datum = new DataStruct();
        datum.x = 100 * (i + 1);
        datum.y = 100;
        datum.dir = i * 2 + 1;
        datum.activeTrace = new PointF();
        datum.trace = new List<PointF>();
        datum.trace.Add(new PointF(datum.x, datum.y));
        datum.visColor = Color.FromArgb(i*127+ 128, 128, (1-i)*127+128);
        data.Add(datum);
      }
      scalingFactor = 40;
      suspended = false;
      processTimestamp = DateTime.Now.Ticks;
      success = false;
      failure = false;
    }

    private void Process() {
      if ((success == false) && (failure == false)) {
        if (suspended == false) {
          checkBoundaryConditions();
          checkData();
          refreshData();
          if (data.Count < 2) {
            if (!failure) {
              success = true;
            }
            suspended = true;
          }
        }
      }
    }

    private void generate(DataStruct datum) {
      datum.trace.Add(new PointF(datum.x, datum.y));
      int hold = random.Next(2) * 2 + 1;
      datum.dir += hold;
      datum.dir %= 4;
    }

    private void refreshData() {
      for (int i = 1; i < data.Count; i++) {
        DataStruct datum = data[i];
        PointF forecast = new PointF(datum.x, datum.y);
        switch (datum.dir) {
          case 0: forecast.Y -= 3;
            if (forecast.Y < 0) {
              generate(datum);
              continue;
            }
            break;
          case 1: forecast.X += 3;
            if (forecast.X > 300) {
              generate(datum);
              continue;
            }
            break;
          case 2: forecast.Y += 3;
            if (forecast.Y > 200) {
              generate(datum);
              continue;
            }
            break;
          case 3: forecast.X -= 3;
            if (forecast.X < 0) {
              generate(datum);
              continue;
            }
            break;
        }
        for (int j = 0; j < data.Count; j++) {
          DataStruct d2 = data[j];
          //trace check
          PointF lastTrace = d2.trace[0];
          for (int k = 1; k < d2.trace.Count; k++) {
            PointF newTrace = d2.trace[k];
            if (intersects(newTrace, lastTrace, forecast, new PointF(datum.x, datum.y))) {
              generate(datum);
              j = data.Count; //trace check is complete.
              break;
            }
            lastTrace = newTrace;
          }
          if (j != data.Count) {
            if (i != j) {
              if (intersects(d2.activeTrace, lastTrace, forecast, new PointF(datum.x, datum.y))) {
                generate(datum);
                break;
              }
            }
          }
        }
      }
    }

    private void checkBoundaryConditions() {
      for (int i=0; i < data.Count; i++) {
        DataStruct datum = data[i];
        if ((datum.x < 1) || (datum.x > 299) || (datum.y < 1) || (datum.y > 199)) {
          datum.dir = 100;
        }
      }
    }

    private bool vertical(PointF alpha, PointF beta) {
      return alpha.X == beta.X;
    }

    private bool horizontal(PointF alpha, PointF beta) {
      return alpha.Y == beta.Y;
    }

    private bool intersects(PointF alpha, PointF beta, PointF gamma, PointF delta) {
      bool v1 = vertical(alpha, beta);
      bool v2 = vertical(gamma, delta);
      float a=0,b=0,c=0,d=0;
      if (v1 && v2) {
        if (!vertical(alpha, gamma)) return false;
        a = alpha.Y;
        b = beta.Y;
        c = gamma.Y;
        d = delta.Y;
      } else if (!v1 && !v2) {
        if (!horizontal(alpha, gamma)) return false;
        a = alpha.X;
        b = beta.X;
        c = gamma.X;
        d = delta.X;
      } else {
        if (v1) {
          c = gamma.X - alpha.X;
          d = delta.X - alpha.X;
          a = alpha.Y - gamma.Y;
          b = beta.Y - gamma.Y;
        } else {
          c = gamma.Y - alpha.Y;
          d = delta.Y - alpha.Y;
          a = alpha.X - gamma.X;
          b = beta.X - gamma.X;
        }
        if (c * d <= 0) {
          if (a * b < 0) return true;
        }
        return false;
      }
      b = b - a;
      c = c - a;
      d = d - a;
      if ((b > 0) && ((c < 0) && (d < 0))) return false;
      if ((b < 0) && ((c > 0) && (d > 0))) return false;
      if ((b > 0) && ((c > b) && (d > b))) return false;
      if ((b < 0) && ((c < b) && (d < b))) return false;
      return true;
    }

    private void checkData() {
      long newStamp = DateTime.Now.Ticks;
      long timeStep = newStamp - processTimestamp; // ticks            
      processTimestamp = newStamp;
      float delta = scalingFactor * timeStep / 10000000;
      for (int i = 0; i < data.Count; i++) {
        DataStruct datum = data[i];
        datum.activeTrace.X = datum.x;
        datum.activeTrace.Y = datum.y;
        switch (datum.dir) {
          case 0: datum.y -= delta; break;
          case 1: datum.x += delta; break;
          case 2: datum.y += delta; break;
          case 3: datum.x -= delta; break;
        }
      }
      for (int i = 0; i < data.Count; i++) {
        DataStruct datum = data[i];
        for (int j = 0; j < data.Count; j++) {
          DataStruct d2 = data[j];
          //trace check
          PointF lastTrace = d2.trace[0];
          for (int k = 1; k < d2.trace.Count; k++) {
            PointF newTrace = d2.trace[k];
            if (intersects(newTrace, lastTrace, datum.activeTrace, new PointF(datum.x, datum.y))) {
              datum.dir = 100;
              j = data.Count; //trace check is complete.
              break;
            }
            lastTrace = newTrace;
          }
          if (j!=data.Count) {
            if (i != j) {
              if (intersects(d2.activeTrace, lastTrace, datum.activeTrace, new PointF(datum.x, datum.y))) {
                datum.dir = 100;
                break;
              }
            }
          }
        }
        if (datum.dir > 3) {
          if (i == 0) {
            failure = true;
            return;
          }
          i--;
          data.Remove(datum);
        }
      }
    }

    private void timer1_Tick(object sender, EventArgs e) { Process(); Refresh(); }
  }
}
