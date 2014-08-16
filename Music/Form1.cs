using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Globalization; //CultureInfo

namespace Music
{
    public partial class Form1 : Form
    {
        Song song;
        uint[] TrackIndex;
        uint BPM;

        public Form1()
        {
            /* Computer  constructor */
            InitializeComponent();
            /* Other constructor code */
            this.BPM = Convert.ToUInt32(SongTempo.Text.ToString());
            this.TrackIndex = new uint[1];
            this.song = new Song(this.BPM, this.tabPage1, this.TrackIndex);
            AttackTimeLabel.Text = AttackTime.Value.ToString();
            DecayTimeLabel.Text = DecayTime.Value.ToString();
            SustainTimeLabel.Text = SustainTime.Value.ToString();
            ReleaseTimeLabel.Text = ReleaseTime.Value.ToString();
            AttackAmplitudeLabel.Text = AttackAmplitude.Value.ToString();
            DecayAmplitudeLabel.Text = DecayAmplitude.Value.ToString();
            SustainAmplitudeLabel.Text = SustainAmplitude.Value.ToString();
            ReleaseAmplitudeLabel.Text = ReleaseAmplitude.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        public readonly float[] Notes = 
        {   261.63F,
            277.18F,
            293.66F,
            311.13F,
            329.63F,
            349.23F,
            369.99F,
            392.00F,
            415.30F,
            440.00F,
            466.16F,
            493.88F
        };

        const int VolMax = 32760;

        private void OctaveSlider1_ValueChanged(object sender, EventArgs e)
        {
            OctaveBox1.Text = OctaveSlider1.Value.ToString();
        }

        private void FrequencySlider1_ValueChanged(object sender, EventArgs e)
        {
            switch(FrequencySlider1.Value)
            {
                case 0:
                    FrequencyBox1.Text = "C";
                    break;
                case 1:
                    FrequencyBox1.Text = "C#/Db";
                    break;
                case 2:
                    FrequencyBox1.Text = "D";
                    break;
                case 3:
                    FrequencyBox1.Text = "D#/Fb";
                    break;
                case 4:
                    FrequencyBox1.Text = "E";
                    break;
                case 5:
                    FrequencyBox1.Text = "F";
                    break;
                case 6:
                    FrequencyBox1.Text = "F#/Gb";
                    break;
                case 7:
                    FrequencyBox1.Text = "G";
                    break;
                case 8:
                    FrequencyBox1.Text = "G#/Ab";
                    break;
                case 9:
                    FrequencyBox1.Text = "A";
                    break;
                case 10:
                    FrequencyBox1.Text = "A#/Bb";
                    break;
                case 11:
                    FrequencyBox1.Text = "B";
                    break;
            }
        }

        private void VolumeSlider1_ValueChanged(object sender, EventArgs e)
        {
            if (VolumeSlider1.Value == 0)
            {
                VolumeBox1.Text = "0%";
            }
            else
            {
                VolumeBox1.Text = VolumeSlider1.Value.ToString() + "0%";
            }
        }

        private void OctaveSlider2_ValueChanged(object sender, EventArgs e)
        {
            OctaveBox2.Text = OctaveSlider2.Value.ToString();
        }

        private void FrequencySlider2_ValueChanged(object sender, EventArgs e)
        {
            switch (FrequencySlider2.Value)
            {
                case 0:
                    FrequencyBox2.Text = "C";
                    break;
                case 1:
                    FrequencyBox2.Text = "C#/Db";
                    break;
                case 2:
                    FrequencyBox2.Text = "D";
                    break;
                case 3:
                    FrequencyBox2.Text = "D#/Fb";
                    break;
                case 4:
                    FrequencyBox2.Text = "E";
                    break;
                case 5:
                    FrequencyBox2.Text = "F";
                    break;
                case 6:
                    FrequencyBox2.Text = "F#/Gb";
                    break;
                case 7:
                    FrequencyBox2.Text = "G";
                    break;
                case 8:
                    FrequencyBox2.Text = "G#/Ab";
                    break;
                case 9:
                    FrequencyBox2.Text = "A";
                    break;
                case 10:
                    FrequencyBox2.Text = "A#/Bb";
                    break;
                case 11:
                    FrequencyBox2.Text = "B";
                    break;
            }
        }

        private void VolumeSlider2_ValueChanged(object sender, EventArgs e)
        {
            if (VolumeSlider2.Value == 0)
            {
                VolumeBox2.Text = "0%";
            }
            else
            {
                VolumeBox2.Text = VolumeSlider2.Value.ToString() + "0%";
            }
        }

        private void OctaveSlider3_ValueChanged(object sender, EventArgs e)
        {
            OctaveBox3.Text = OctaveSlider3.Value.ToString();
        }

        private void FrequencySlider3_ValueChanged(object sender, EventArgs e)
        {
            switch (FrequencySlider3.Value)
            {
                case 0:
                    FrequencyBox3.Text = "C";
                    break;
                case 1:
                    FrequencyBox3.Text = "C#/Db";
                    break;
                case 2:
                    FrequencyBox3.Text = "D";
                    break;
                case 3:
                    FrequencyBox3.Text = "D#/Fb";
                    break;
                case 4:
                    FrequencyBox3.Text = "E";
                    break;
                case 5:
                    FrequencyBox3.Text = "F";
                    break;
                case 6:
                    FrequencyBox3.Text = "F#/Gb";
                    break;
                case 7:
                    FrequencyBox3.Text = "G";
                    break;
                case 8:
                    FrequencyBox3.Text = "G#/Ab";
                    break;
                case 9:
                    FrequencyBox3.Text = "A";
                    break;
                case 10:
                    FrequencyBox3.Text = "A#/Bb";
                    break;
                case 11:
                    FrequencyBox3.Text = "B";
                    break;
            }
        }

        private void VolumeSlider3_ValueChanged(object sender, EventArgs e)
        {
            if (VolumeSlider3.Value == 0)
            {
                VolumeBox3.Text = "0%";
            }
            else
            {
                VolumeBox3.Text = VolumeSlider3.Value.ToString() + "0%";
            }
        }

        private void ClearADSRChart()
        {
            this.ADSRChart.ChartAreas.Clear();
            this.ADSRChart.Series.Clear();
        }

        private void UpdateADSRChart()
        {
            float duration = 0;
            foreach (System.Windows.Forms.Control control in this.DurationGroupBox.Controls)
            {
                System.Windows.Forms.RadioButton button = (System.Windows.Forms.RadioButton)control;
                if (button.Checked)
                {
                    duration = float.Parse(button.Tag.ToString(), CultureInfo.InvariantCulture.NumberFormat)/(this.BPM/60);
                }
            }

            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.Maximum = duration;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.Title = "Amplitude";
            chartArea1.Name = "ChartArea1";
            this.ADSRChart.ChartAreas.Add(chartArea1);

            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)AttackTime.Value / 1000), 0F);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)AttackTime.Value / 1000), (float)AttackAmplitude.Value);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)DecayTime.Value / 1000), (float)DecayAmplitude.Value);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)SustainTime.Value / 1000), (float)SustainAmplitude.Value);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)ReleaseTime.Value / 1000), (float)ReleaseAmplitude.Value);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(duration * ((float)ReleaseTime.Value / 1000), 0F);
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.Color = System.Drawing.Color.Red;
            this.ADSRChart.Series.Add(series1);
        }

        private void AttackTime_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            AttackTimeLabel.Text = AttackTime.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void DecayTime_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            DecayTimeLabel.Text = DecayTime.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void SustainTime_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            SustainTimeLabel.Text = SustainTime.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void ReleaseTime_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            ReleaseTimeLabel.Text = ReleaseTime.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void AttackAmplitude_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            AttackAmplitudeLabel.Text = AttackAmplitude.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void DecayAmplitude_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            DecayAmplitudeLabel.Text = DecayAmplitude.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void SustainAmplitude_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            SustainAmplitudeLabel.Text = SustainAmplitude.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void ReleaseAmplitude_ValueChanged_1(object sender, EventArgs e)
        {
            /* Change the value box */
            ReleaseAmplitudeLabel.Text = ReleaseAmplitude.Value.ToString();
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void ClearSoundWaveChart()
        {
            this.SoundWaveChart.ChartAreas.Clear();
            this.SoundWaveChart.Series.Clear();
        }

        private void UpdateSoundWaveChart(uint numSamples, short[] data)
        {
            /* Clear the previous chart */
            ClearSoundWaveChart();

            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.Maximum = numSamples;
            chartArea1.AxisX.Minimum = 0;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            //chartArea1.AxisY.Maximum = 1;
            //chartArea1.AxisY.Minimum = 0;
            chartArea1.AxisY.Title = "Amplitude";
            chartArea1.Name = "ChartArea1";
            this.SoundWaveChart.ChartAreas.Add(chartArea1);

            /* Add the points to the chart */
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            for (int i = 0; i < numSamples; i++)
            {
                System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint = new System.Windows.Forms.DataVisualization.Charting.DataPoint(i, data[i]);
                series1.Points.Add(dataPoint);
            }
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.Color = System.Drawing.Color.Red;
            this.SoundWaveChart.Series.Add(series1);
        }

        private void button1_MouseClick_1(object sender, MouseEventArgs e)
        {
            WaveExampleType[] selection = new WaveExampleType[3];
            for (int i = 0; i < 3; i++)
            {
                selection[i] = WaveExampleType.SineWave;
            }
            RadioButton[][] buttons = new RadioButton[3][];
            for (int i = 0; i < 3; i++)
            {
                buttons[i] = new RadioButton[6];
            }
            buttons[0][0] = SineButton1; buttons[0][1] = SquareButton1; buttons[0][2] = SawtoothButton1; buttons[0][3] = TriangleButton1; buttons[0][4] = WhiteButton1; buttons[0][5] = NothingButton1;
            buttons[1][0] = SineButton2; buttons[1][1] = SquareButton2; buttons[1][2] = SawtoothButton2; buttons[1][3] = TriangleButton2; buttons[1][4] = WhiteButton2; buttons[1][5] = NothingButton2;
            buttons[2][0] = SineButton3; buttons[2][1] = SquareButton3; buttons[2][2] = SawtoothButton3; buttons[2][3] = TriangleButton3; buttons[2][4] = WhiteButton3; buttons[2][5] = NothingButton3;
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i][0].Checked)
                {
                    selection[i] = WaveExampleType.SineWave;
                }
                else if (buttons[i][1].Checked)
                {
                    selection[i] = WaveExampleType.SquareWave;
                }
                else if (buttons[i][2].Checked)
                {
                    selection[i] = WaveExampleType.SawtoothWave;
                }
                else if (buttons[i][3].Checked)
                {
                    selection[i] = WaveExampleType.TriangleWave;
                }
                else if (buttons[i][4].Checked)
                {
                    selection[i] = WaveExampleType.WhiteNoise;
                }
                else if (buttons[i][5].Checked)
                {
                    selection[i] = WaveExampleType.Nothing;
                }
                else
                {
                    selection[i] = WaveExampleType.Nothing;
                }
            }

            /* Find the frequency of each note */
            double[] frequency = new double[3];
            TrackBar[] octaveSliders = new TrackBar[3];
            TrackBar[] frequencySliders = new TrackBar[3];
            octaveSliders[0] = OctaveSlider1; octaveSliders[1] = OctaveSlider2; octaveSliders[2] = OctaveSlider3;
            frequencySliders[0] = FrequencySlider1; frequencySliders[1] = FrequencySlider2; frequencySliders[2] = FrequencySlider3;
            for (int i = 0; i < 3; i++)
            {
                double power = octaveSliders[i].Value - 3;
                double multiplier = Math.Pow(2, power);
                frequency[i] = Notes[frequencySliders[i].Value] * multiplier;
            }

            /* Find the amplitude of each note */
            int[] volume = new int[3];
            TrackBar[] volumeSliders = new TrackBar[3];
            volumeSliders[0] = VolumeSlider1; volumeSliders[1] = VolumeSlider2; volumeSliders[2] = VolumeSlider3;
            for (int i = 0; i < 3; i++)
            {
                volume[i] = volumeSliders[i].Value * (VolMax / 10);
            }

            /* Find the ADSR times */
            double[] ADSRtime = new double[6];
            float clipDuration = 0;
            foreach (System.Windows.Forms.Control control in this.DurationGroupBox.Controls)
            {
                System.Windows.Forms.RadioButton button = (System.Windows.Forms.RadioButton)control;
                if (button.Checked)
                {
                    clipDuration = float.Parse(button.Tag.ToString(), CultureInfo.InvariantCulture.NumberFormat) / (this.BPM / 60);
                }
            }
            ADSRtime[0] = 0;
            ADSRtime[1] = AttackTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[2] = DecayTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[3] = SustainTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[4] = ReleaseTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[5] = clipDuration * 44100;

            /* Find the ADSR amplitudes */
            double[] ADSRvolume = new double[4];
            ADSRvolume[0] = (float)AttackAmplitude.Value / 100;
            ADSRvolume[1] = (float)DecayAmplitude.Value / 100;
            ADSRvolume[2] = (float)SustainAmplitude.Value / 100;
            ADSRvolume[3] = (float)ReleaseAmplitude.Value / 100;

            string filePath = Directory.GetCurrentDirectory() + "\\sound.wav";
            WaveGenerator wave = new WaveGenerator(false);
            wave.makeNote(selection,
                frequency,
                volume,
                clipDuration,
                ADSRtime,
                ADSRvolume
                );
            wave.Save(filePath);

            /* Show what the final plot looks like */
            UpdateSoundWaveChart(wave.getNumSamples(), wave.getData());

            SoundPlayer player = new SoundPlayer(filePath);
            player.Play();
        }

        private void DurationBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            /* Remove any existing charts */
            ClearADSRChart();
            /* Update the chart */
            UpdateADSRChart();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AddTrackButton_Click(object sender, EventArgs e)
        {
            this.song.AddTrack();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.song.DeleteTrack();
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            WaveExampleType[] selection = new WaveExampleType[3];
            for (int i = 0; i < 3; i++)
            {
                selection[i] = WaveExampleType.SineWave;
            }
            RadioButton[][] buttons = new RadioButton[3][];
            for (int i = 0; i < 3; i++)
            {
                buttons[i] = new RadioButton[6];
            }
            buttons[0][0] = SineButton1; buttons[0][1] = SquareButton1; buttons[0][2] = SawtoothButton1; buttons[0][3] = TriangleButton1; buttons[0][4] = WhiteButton1; buttons[0][5] = NothingButton1;
            buttons[1][0] = SineButton2; buttons[1][1] = SquareButton2; buttons[1][2] = SawtoothButton2; buttons[1][3] = TriangleButton2; buttons[1][4] = WhiteButton2; buttons[1][5] = NothingButton2;
            buttons[2][0] = SineButton3; buttons[2][1] = SquareButton3; buttons[2][2] = SawtoothButton3; buttons[2][3] = TriangleButton3; buttons[2][4] = WhiteButton3; buttons[2][5] = NothingButton3;
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i][0].Checked)
                {
                    selection[i] = WaveExampleType.SineWave;
                }
                else if (buttons[i][1].Checked)
                {
                    selection[i] = WaveExampleType.SquareWave;
                }
                else if (buttons[i][2].Checked)
                {
                    selection[i] = WaveExampleType.SawtoothWave;
                }
                else if (buttons[i][3].Checked)
                {
                    selection[i] = WaveExampleType.TriangleWave;
                }
                else if (buttons[i][4].Checked)
                {
                    selection[i] = WaveExampleType.WhiteNoise;
                }
                else if (buttons[i][5].Checked)
                {
                    selection[i] = WaveExampleType.Nothing;
                }
                else
                {
                    selection[i] = WaveExampleType.Nothing;
                }
            }

            /* Find the frequency of each note */
            double[] frequency = new double[3];
            TrackBar[] octaveSliders = new TrackBar[3];
            TrackBar[] frequencySliders = new TrackBar[3];
            octaveSliders[0] = OctaveSlider1; octaveSliders[1] = OctaveSlider2; octaveSliders[2] = OctaveSlider3;
            frequencySliders[0] = FrequencySlider1; frequencySliders[1] = FrequencySlider2; frequencySliders[2] = FrequencySlider3;
            for (int i = 0; i < 3; i++)
            {
                double power = octaveSliders[i].Value - 3;
                double multiplier = Math.Pow(2, power);
                frequency[i] = Notes[frequencySliders[i].Value] * multiplier;
            }

            /* Find the amplitude of each note */
            int[] volume = new int[3];
            TrackBar[] volumeSliders = new TrackBar[3];
            volumeSliders[0] = VolumeSlider1; volumeSliders[1] = VolumeSlider2; volumeSliders[2] = VolumeSlider3;
            for (int i = 0; i < 3; i++)
            {
                volume[i] = volumeSliders[i].Value * (VolMax / 10);
            }

            /* Find the ADSR times */
            double[] ADSRtime = new double[6];
            float clipDuration = 0;
            foreach (System.Windows.Forms.Control control in this.DurationGroupBox.Controls)
            {
                System.Windows.Forms.RadioButton button = (System.Windows.Forms.RadioButton)control;
                if (button.Checked)
                {
                    clipDuration = float.Parse(button.Tag.ToString(), CultureInfo.InvariantCulture.NumberFormat) / (this.BPM / 60);
                }
            }
            ADSRtime[0] = 0;
            ADSRtime[1] = AttackTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[2] = DecayTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[3] = SustainTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[4] = ReleaseTime.Value * (clipDuration / 1000) * 44100;
            ADSRtime[5] = clipDuration * 44100;

            /* Find the ADSR amplitudes */
            double[] ADSRvolume = new double[4];
            ADSRvolume[0] = (float)AttackAmplitude.Value / 100;
            ADSRvolume[1] = (float)DecayAmplitude.Value / 100;
            ADSRvolume[2] = (float)SustainAmplitude.Value / 100;
            ADSRvolume[3] = (float)ReleaseAmplitude.Value / 100;

            string filePath = Directory.GetCurrentDirectory() + "\\sound.wav";
            WaveGenerator wave = new WaveGenerator(false);
            wave.makeNote(selection,
                frequency,
                volume,
                clipDuration,
                ADSRtime,
                ADSRvolume
                );

            /* Show what the final plot looks like */
            UpdateSoundWaveChart(wave.getNumSamples(), wave.getData());

            /* Add the note to the correct track */
            song.AddNote(wave.getData(), TrackIndex[0]);
        }

        private void MasterPlayButton_Click(object sender, EventArgs e)
        {
            /* Prepare tracks for left and right */
            int leftCount = 0, rightCount = 0;
            short[] rawLeft = null;
            short[] rawRight = null;
            foreach (Track track in song.Tracks)
            {
                if (track.speakerSide())
                {
                    leftCount++;
                    if (rawLeft != null)
                    {
                        if (rawLeft.Length < track.getLength())
                        {
                            short[] tempLeft = new short[track.getLength()];
                            track.getData().CopyTo(tempLeft, 0);
                            for (int i = 0; i < rawLeft.Length; i++)
                            {
                                tempLeft[i] += rawLeft[i];
                            }
                            rawLeft = tempLeft;
                        }
                        else
                        {
                            for (int i = 0; i < track.getLength(); i++)
                            {
                                rawLeft[i] += track.getData()[i];
                            }
                        }
                    }
                    else
                    {
                        rawLeft = new short[track.getLength()];
                        track.getData().CopyTo(rawLeft, 0);
                    }
                }
                else
                {
                    rightCount++;
                    if (rawRight != null)
                    {
                        if (rawRight.Length < track.getLength())
                        {
                            short[] tempRight = new short[track.getLength()];
                            track.getData().CopyTo(tempRight, 0);
                            for (int i = 0; i < rawRight.Length; i++)
                            {
                                tempRight[i] += rawRight[i];
                            }
                            rawRight = tempRight;
                        }
                        else
                        {
                            for (int i = 0; i < track.getLength(); i++)
                            {
                                rawRight[i] += track.getData()[i];
                            }
                        }
                    }
                    else
                    {
                        rawRight = new short[track.getLength()];
                        track.getData().CopyTo(rawRight, 0);
                    }
                }
            }
            /* Tell the .wav which tracks we are using */
            int trackNum = 0;
            if (leftCount == 0 && rightCount == 0)
            {
                return;//neither track
            }
            else if (leftCount > 0 && rightCount > 0)
            {
                trackNum = 2;//use both
                /* Make sure the two sides are of equal length */
                if (rawLeft.Length != rawRight.Length)
                {
                    //return;
                }
            }
            else if (leftCount > 0)
            {
                trackNum = 0;//left only
            }
            else
            {
                trackNum = 1;//right only
            }
            /* Make the .wav */
            string filePath = Directory.GetCurrentDirectory() + "\\song.wav";
            WaveGenerator wave;
            /* Construct the .wav as mono or stereo as appropriate */
            if (trackNum < 2)
            {
                wave = new WaveGenerator(false);
            }
            else
            {
                wave = new WaveGenerator(true);
            }
            wave.makeSong(rawLeft, rawRight, trackNum);
            wave.Save(filePath);
            SoundPlayer player = new SoundPlayer(filePath);
            player.Play();
        }

    }
}
