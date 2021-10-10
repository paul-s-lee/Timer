using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Timer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Cpu.Start();
            Mem.Start();
        }

        public void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Start stopwatch
            if (e.KeyCode == Keys.D1)
            {
                Timer.Start();
                HourLabel.ForeColor = Color.Cyan;
                MinuteLabel.ForeColor = Color.Cyan;
                SecondLabel.ForeColor = Color.Cyan;
            }
            else if (e.KeyCode == Keys.D2)
            {
                Timer.Stop();
                HourLabel.ForeColor = Color.Magenta;
                MinuteLabel.ForeColor = Color.Magenta;
                SecondLabel.ForeColor = Color.Magenta;
                
            }
            else if (e.KeyCode == Keys.D3)
            {
                Timer.Stop();
                Clock.second = 0;
                Clock.minute = 0;
                Clock.hour = 0;

                SecondLabel.Text = "00";
                MinuteLabel.Text = "00";
                HourLabel.Text = "00";

                HourLabel.ForeColor = Color.White;
                MinuteLabel.ForeColor = Color.White;
                SecondLabel.ForeColor = Color.White;
            }
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            ++Clock.second;
            if (Clock.second < 10)
            {
                SecondLabel.Text = "0" + Clock.second.ToString();
            }
            else
            {
                SecondLabel.Text = Clock.second.ToString();
            }
            if (Clock.second == 60)
            {
                Clock.second = 0;
                SecondLabel.Text = "00";
                ++Clock.minute;
                if (Clock.minute < 10)
                {
                    MinuteLabel.Text = "0" + Clock.minute.ToString();
                }
                else
                {
                    MinuteLabel.Text = Clock.minute.ToString();
                }
                if (Clock.minute == 60)
                {
                    Clock.minute = 0;
                    MinuteLabel.Text = "00";
                    ++Clock.hour;
                    if (Clock.hour < 10)
                    {
                        HourLabel.Text = "0" + Clock.hour.ToString();
                    }
                    else
                    {
                        HourLabel.Text = Clock.hour.ToString();
                    }
                }
            }
        }

        public void Cpu_Tick(object sender, EventArgs e)
        {
            int current = Convert.ToInt32(CentralProcessingUnit.counter.NextValue());
            CpuValue.Text = current.ToString() + " %";
        }

        public void Mem_Tick(object sender, EventArgs e)
        {
            // Get Total Physical Memory
            double totalB = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            
            // Convert Total Physical Memory from Bytes to MegaBytes
            int totalMB = Convert.ToInt32(totalB / 1048576); // 16,XXX

            // Get Current Memory Usage of System
            double current = Convert.ToInt32(MemoryProcessingUnit.counter.NextValue());
            
            // Subtract Total from Current
            double result = totalMB - current;

            // Calculate Percentage of Memory Used in System
            double divided = result / totalMB;
            int final = Convert.ToInt32(divided * 100);
            MemValue.Text = final.ToString() + " %";
        }
    }

    public static class Clock
    {
        static public uint second = 0;
        static public uint minute = 0;
        static public uint hour = 0;
    }
    
    public static class CentralProcessingUnit
    {
        static public PerformanceCounter counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    }

    public static class MemoryProcessingUnit
    {
        static public PerformanceCounter counter = new PerformanceCounter("Memory", "Available MBytes");
    }
}
