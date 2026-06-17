using System;
using System.Linq;
using System.Windows.Forms;
using App.core.Contracts;
using App.core.Utilities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;

namespace App.WindowsApp
{
    public partial class ChartView : UserControl
    {
        private IAppointmentService _appointmentService;
        private IPatientService _patientService;
        private PieChart _pieChart;
        private CartesianChart _barChart;
        private System.Windows.Forms.Timer _loadTimer;

        public ChartView(IAppointmentService appointmentService,
                         IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            InitializeComponent();
            BuildCharts();
            StartLoadTimer();
        }

        private void BuildCharts()
        {
            _pieChart = new PieChart
            {
                Dock = DockStyle.Fill,
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom
            };
            pnlChart1.Controls.Add(_pieChart);

            _barChart = new CartesianChart
            {
                Dock = DockStyle.Fill
            };
            pnlChart2.Controls.Add(_barChart);
        }

        private void StartLoadTimer()
        {
            _loadTimer = new System.Windows.Forms.Timer();
            _loadTimer.Interval = 300;
            _loadTimer.Tick += (s, e) =>
            {
                _loadTimer.Stop();
                _loadTimer.Dispose();
                LoadData();
            };
            _loadTimer.Start();
        }

        public void LoadData()
        {
            RenderPieChart();
            RenderBarChart();
        }

        private void RenderPieChart()
        {
            try
            {
                var list = _appointmentService.GetAll();

                int scheduled = list.Count(a => a.Status == AppointmentStatus.Scheduled);
                int confirmed = list.Count(a => a.Status == AppointmentStatus.Confirmed);
                int pending = list.Count(a => a.Status == AppointmentStatus.Pending);
                int cancelled = list.Count(a => a.Status == AppointmentStatus.Cancelled);

                _pieChart.Series = new ISeries[]
                {
                    new PieSeries<double>
                    {
                        Name      = $"Scheduled ({scheduled})",
                        Values    = new double[] { scheduled > 0 ? scheduled : 0.01 },
                        Fill      = new SolidColorPaint(new SKColor(55, 138, 221)),
                        Stroke    = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        Pushout   = 0,
                        InnerRadius = 0
                    },
                    new PieSeries<double>
                    {
                        Name      = $"Confirmed ({confirmed})",
                        Values    = new double[] { confirmed > 0 ? confirmed : 0.01 },
                        Fill      = new SolidColorPaint(new SKColor(99, 153, 34)),
                        Stroke    = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        Pushout   = 0,
                        InnerRadius = 0
                    },
                    new PieSeries<double>
                    {
                        Name      = $"Pending ({pending})",
                        Values    = new double[] { pending > 0 ? pending : 0.01 },
                        Fill      = new SolidColorPaint(new SKColor(239, 159, 39)),
                        Stroke    = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        Pushout   = 0,
                        InnerRadius = 0
                    },
                    new PieSeries<double>
                    {
                        Name      = $"Cancelled ({cancelled})",
                        Values    = new double[] { cancelled > 0 ? cancelled : 0.01 },
                        Fill      = new SolidColorPaint(new SKColor(226, 75, 74)),
                        Stroke    = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        Pushout   = 0,
                        InnerRadius = 0
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pie chart error: " + ex.Message);
            }
        }

        private void RenderBarChart()
        {
            try
            {
                var list = _patientService.GetAll();

                int male = list.Count(p => p.Gender == Gender.Male);
                int female = list.Count(p => p.Gender == Gender.Female);

                _barChart.Series = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Name   = $"Male ({male})",
                        Values = new double[] { male },
                        Fill   = new SolidColorPaint(new SKColor(55, 138, 221)),
                        Stroke = null,
                        MaxBarWidth = 60
                    },
                    new ColumnSeries<double>
                    {
                        Name   = $"Female ({female})",
                        Values = new double[] { female },
                        Fill   = new SolidColorPaint(new SKColor(226, 75, 74)),
                        Stroke = null,
                        MaxBarWidth = 60
                    }
                };

                _barChart.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels    = new string[] { "Male", "Female" },
                        TextSize  = 14,
                        LabelsPaint = new SolidColorPaint(new SKColor(30, 30, 40))
                    }
                };

                _barChart.YAxes = new Axis[]
                {
                    new Axis
                    {
                        TextSize  = 14,
                        MinLimit  = 0,
                        LabelsPaint = new SolidColorPaint(new SKColor(30, 30, 40))
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bar chart error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}