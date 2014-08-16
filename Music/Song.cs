using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class Song
    {
        List<System.Windows.Forms.RadioButton> TrackSelect;
        public List<Track> Tracks;
        uint BPM;
        System.Windows.Forms.TabPage TabPage;
        uint[] TrackIndex;

        public Song(uint BPM, System.Windows.Forms.TabPage TabPage, uint[] TrackIndex)
        {
            this.BPM = BPM;
            this.TabPage = TabPage;
            TrackSelect = new List<System.Windows.Forms.RadioButton>();
            Tracks = new List<Track>();
            this.TrackIndex = TrackIndex;
        }

        public void AddTrack()
        {
            //TabPage.SetBounds(0, 0, TabPage.Size.Width, TabPage.Size.Height + 100);
            /* Add track select button to TabPage */
            TrackSelect.Add(new System.Windows.Forms.RadioButton());
            this.TabPage.Controls.Add(TrackSelect[TrackSelect.Count - 1]);
            TrackSelect[TrackSelect.Count - 1].SetBounds(15, 100 * TrackSelect.Count + 10, 15, 15);
            TrackSelect[TrackSelect.Count - 1].Click += new System.EventHandler(this.TrackSelect_Click);
            TrackSelect[TrackSelect.Count - 1].Tag = (TrackSelect.Count - 1).ToString();
            /* Create new track */
            this.Tracks.Add(new Track(40, 100 * TrackSelect.Count, this.TabPage));
            TrackSelect[TrackSelect.Count - 1].PerformClick();
        }

        public void DeleteTrack() //fuck this doesn't work for like... a lot of reasons
        {
            foreach (System.Windows.Forms.RadioButton button in TrackSelect)
            {
                if (button.Checked)
                {
                    button.Dispose();
                    break;
                }
            }
        }

        public void UpdateTempo(uint BPM)
        {
            this.BPM = BPM;
        }

        public void TrackSelect_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton button = (System.Windows.Forms.RadioButton)sender;
            this.TrackIndex[0] = Convert.ToUInt32(button.Tag);
            foreach (Track track in Tracks)
            {
                track.Select(false);
            }
            this.Tracks[(int)this.TrackIndex[0]].Select(true);
        }

        public void AddNote(short[] rawData, uint track)
        {
            Tracks[(int)track].AddNote(rawData);
        }
    }
}
