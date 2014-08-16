using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace Music
{
    class Track
    {
        System.Windows.Forms.TabPage TabPage;
        System.Windows.Forms.GroupBox SelectBox;
        System.Windows.Forms.GroupBox PositionBox;
        System.Windows.Forms.RadioButton left;
        System.Windows.Forms.RadioButton right;
        System.Windows.Forms.Button play;
        bool selected;
        bool position;
        short[] data;

        public Track(int x, int y, System.Windows.Forms.TabPage TabPage)
        {
            this.selected = false;
            this.TabPage = TabPage;
            this.SelectBox = new System.Windows.Forms.GroupBox();
            this.PositionBox = new System.Windows.Forms.GroupBox();
            this.play = new System.Windows.Forms.Button();
            left = new System.Windows.Forms.RadioButton();
            right = new System.Windows.Forms.RadioButton();
            this.PositionBox.Controls.Add(left);
            this.PositionBox.Controls.Add(right);
            this.PositionBox.Controls.Add(play);
            this.TabPage.Controls.Add(PositionBox);
            this.TabPage.Controls.Add(SelectBox);
            /* Set positions of things */
            this.PositionBox.Location = new System.Drawing.Point(x, y);
            this.SelectBox.SetBounds(x - 30, y, 25, this.PositionBox.Size.Height);
            this.left.SetBounds(15, 25, 15, 15);
            this.left.Select();
            this.right.SetBounds(40, 25, 15, 15);
            this.play.SetBounds(15, 50, 50, 25);
            this.play.Text = "Play";
            this.play.Click += new System.EventHandler(this.PlayButton_Click);
        }

        public void Select(bool select)
        {
            if (select)
            {
                this.selected = true;
                this.SelectBox.BackColor = System.Drawing.Color.LawnGreen;
                this.PositionBox.BackColor = System.Drawing.Color.LawnGreen;
            }
            else
            {
                this.selected = false;
                this.SelectBox.BackColor = System.Drawing.Color.Transparent;
                this.PositionBox.BackColor = System.Drawing.Color.Transparent;
            }
        }

        public void AddNote(short[] rawData)
        {
            if (data != null)
            { 
                short[] tempData = new short[data.Length + rawData.Length];
                data.CopyTo(tempData, 0);
                rawData.CopyTo(tempData, data.Length);
                data = tempData;
            }
            else
            {
                data = rawData;
            }
        }

        public void PlayButton_Click(object sender, EventArgs e)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\track.wav";
            WaveGenerator wave = new WaveGenerator(false);
            wave.makeTrack(data);
            wave.Save(filePath);
            SoundPlayer player = new SoundPlayer(filePath);
            player.Play();
        }

        public bool speakerSide()
        {
            if (left.Checked) return true;
            else return false;
        }

        public short[] getData()
        {
            return data;
        }

        public int getLength()
        {
            if (data != null)
            {
                return data.Length;
            }
            else
            {
                return 0;
            }
        }
    }
}
