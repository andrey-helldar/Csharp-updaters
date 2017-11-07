using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _Hell_Updater
{
    public class rootUpdater
    {
        public List<Updater> updater { get; set; }
    }

    public class Updater
    {
        public string art { get; set; }
        public string version { get; set; }
        public string changes { get; set; }
        public string file { get; set; }
        public string date { get; set; }
    }
}
