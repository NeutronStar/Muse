using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Music
{
    public enum WaveExampleType
    {
        SineWave = 0,
        SquareWave = 1,
        SawtoothWave = 2,
        TriangleWave = 3,
        Nothing = 4,
        WhiteNoise = 5
    }

    public class WaveGenerator
    {
        // Header, Format, Data chunks
        WaveHeader header;
        WaveFormatChunk format;
        WaveDataChunk data;
        uint numSamples;
        double[] envelope;

        public WaveGenerator(bool channels)
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk(channels);
            data = new WaveDataChunk();
        }

        public short[] makeNote(WaveExampleType[] type, double[] frequency, int[] volume, float duration, double[] ADSRtime, double[] ADSRamp)
        {
            // Number of samples = sample rate * channels * bytes per sample * seconds
            numSamples = (uint)(format.dwSamplesPerSec * format.wChannels * duration);

            // Initialize the 16-bit array
            data.shortArray = new short[numSamples];
            short[][] music = new short[3][];
            for (int i = 0; i < 3; i++) music[i] = new short[numSamples];

            int numWaves = 3;
            // Fill the data array with sample data
            for (int index = 0; index < 3; index++)
            {
                int amplitude = volume[index];  // Max amplitude for 16-bit audio
                double freq = frequency[index];   // Concert A: 440Hz

                switch (type[index])
                {
                    case WaveExampleType.SineWave:
                        {
                            double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);
                            // The "angle" used in the function, adjusted for the number of channels and sample rate.
                            // This value is like the period of the wave.
                            for (int i = 0; i < numSamples; i += format.wChannels)
                            {
                                short tempSample = Convert.ToInt16(amplitude * Math.Sin(t * i));
                                music[index][i] = tempSample;
                                //for (int channel = 0; channel < format.wChannels; channel++)
                                //{
                                //    music[index][i + channel] = tempSample;
                                //}
                            }
                            break;
                        }
                    case WaveExampleType.SquareWave:
                        {
                            // The "angle" used in the function, adjusted for the number of channels and sample rate.
                            // This value is like the period of the wave.
                            double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);
                            for (int i = 0; i < numSamples - 1; i++)
                            {
                                music[index][i] = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(t * i)));
                            }
                            break;
                        }
                    case WaveExampleType.SawtoothWave:
                        {
                            // Determine the number of samples per wavelength
                            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (freq / format.wChannels / 2));

                            // Determine the amplitude step for consecutive samples
                            short ampStep = Convert.ToInt16((amplitude * 2) / samplesPerWavelength);

                            // Temporary sample value, added to as we go through the loop
                            short tempSample = (short)-amplitude;
                            // Total number of samples written so we know when to stop
                            int totalSamplesWritten = 0;

                            while (totalSamplesWritten < numSamples)
                            {
                                tempSample = (short)-amplitude;

                                for (uint i = 0; i < samplesPerWavelength && totalSamplesWritten < numSamples; i++)
                                {
                                    tempSample += ampStep;
                                    music[index][totalSamplesWritten] = tempSample;
                                    totalSamplesWritten++;
                                }
                            }
                            break;
                        }
                    case WaveExampleType.TriangleWave:
                        {
                            // Determine the number of samples per wavelength
                            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (freq / format.wChannels));

                            // Determine the amplitude step for consecutive samples
                            short ampStep = Convert.ToInt16((amplitude * 2) / samplesPerWavelength);

                            // Temporary sample value, added to as we go through the loop
                            short tempSample = (short)-amplitude;

                            for (int i = 0; i < numSamples - 1; i++)
                            {
                                for (int channel = 0; channel < format.wChannels; channel++)
                                {
                                    // Negate ampstep whenever it hits the amplitude boundary
                                    if (Math.Abs(tempSample) > amplitude)
                                    {
                                        tempSample = (short)(Math.Sign(tempSample) * amplitude);
                                        ampStep = (short)-ampStep;
                                    }
                                    tempSample += ampStep;
                                    music[index][i + channel] = tempSample;
                                }
                            }
                            break;
                        }
                    case WaveExampleType.WhiteNoise:
                        {
                            Random rnd = new Random();
                            short randomValue = 0;

                            for (int i = 0; i < numSamples; i += format.wChannels)
                            {
                                for (int channel = 0; channel < format.wChannels; channel++)
                                {
                                    randomValue = Convert.ToInt16(rnd.Next(-amplitude, amplitude));
                                    music[index][i + channel] = randomValue;
                                }
                            }
                            break;
                        }
                    case WaveExampleType.Nothing:
                        for (int i = 0; i < numSamples; i += format.wChannels)
                        {
                            music[index][i] = 0;
                        }
                        numWaves--;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < numSamples; i++)
            {
                if (numWaves > 0)
                {
                    data.shortArray[i] = Convert.ToInt16((music[0][i] + music[1][i] + music[2][i]) / numWaves);
                }
            }

            ADSR(ADSRtime, ADSRamp);

            // Calculate data chunk size in bytes
            data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
            return data.shortArray;
        }

        private void ADSR(double[] ADSRtime, double[] ADSRamp)
        {
            double[] envelope = new double[numSamples];
            double[] amplitude = new double[5];
            double[] slope = new double[5];
            for (int i = 0; i < 3; i++ )
            {
                slope[i+1] = (ADSRamp[i + 1] - ADSRamp[i]) / (ADSRtime[i + 2] - ADSRtime[i + 1]);
                amplitude[i + 1] = ADSRamp[i];
            }
            
            /* No sound for values out side of the envelope */
            amplitude[0] = 0; amplitude[4] = 0;
            slope[0] = 0; slope[4] = 0;

            for (int i = 0; i < 5; i++ )
            {
                for (int j = (int)ADSRtime[i]; j < (int)ADSRtime[i + 1]; j++)
                {
                    envelope[j] = (slope[i] * (j - ADSRtime[i])) + amplitude[i];
                }
            }

            this.envelope = envelope;

            for (int i = 0; i < numSamples; i++)
            {
                data.shortArray[i] = (short)(data.shortArray[i] * envelope[i]);
            }
        }

        public void Save(string filePath)
        {
            // Create a file (it always overwrites)
            FileStream fileStream = new FileStream(filePath, FileMode.Create);   
     
            // Use BinaryWriter to write the bytes to the file
            BinaryWriter writer = new BinaryWriter(fileStream);
     
            // Write the header
            writer.Write(header.sGroupID.ToCharArray());
            writer.Write(header.dwFileLength);
            writer.Write(header.sRiffType.ToCharArray());
    
            // Write the format chunk
            writer.Write(format.sChunkID.ToCharArray());
            writer.Write(format.dwChunkSize);
            writer.Write(format.wFormatTag);
            writer.Write(format.wChannels);
            writer.Write(format.dwSamplesPerSec);
            writer.Write(format.dwAvgBytesPerSec);
            writer.Write(format.wBlockAlign);
            writer.Write(format.wBitsPerSample);
    
            // Write the data chunk
            writer.Write(data.sChunkID.ToCharArray());
            writer.Write(data.dwChunkSize);
            foreach (short dataPoint in data.shortArray)
            {
                writer.Write(dataPoint);
            }
    
            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);
       
            // Clean up
            writer.Close();
            fileStream.Close();            
        }

        public void makeTrack(short[] raw)
        {
            if (raw != null)
            {
                data.shortArray = new short[raw.Length];
                data.shortArray = raw;
                data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
            }
        }

        public void makeSong(short[] rawLeft, short[] rawRight, int tracks)
        {
            if (tracks == 0) //left only
            {
                data.shortArray = new short[rawLeft.Length];
                data.shortArray = rawLeft;
                data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
            }
            else if (tracks == 1) //right only
            {
                data.shortArray = new short[rawRight.Length];
                data.shortArray = rawRight;
                data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
            }
            else //both sides
            {
                data.shortArray = new short[2*rawLeft.Length];
                for (int i = 0; i < rawLeft.Length; i++)
                {
                    data.shortArray[2*i] = rawLeft[i];
                    data.shortArray[2*i + 1] = rawRight[i];
                }
                data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
            }
        }

        public uint getNumSamples()
        {
            return numSamples;
        }

        public short[] getData()
        {
            return data.shortArray;
        }
    }

}
