using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GISdotNet.Map
{
    public class TargetInfo: INotifyPropertyChanged
    {
        private int TargetNumber;
        private int HC;
        private int R;
        private int A;
        private int H;
        private int V;
        private int Source;
        private int Corr;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Ntarget
        {
            get
            {
                return this.TargetNumber;
            }

            set
            {
                if (value != this.TargetNumber)
                {
                    this.TargetNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int RA
        {
            get
            {
                return this.R;
            }

            set
            {
                if (value != this.R)
                {
                    this.R = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
