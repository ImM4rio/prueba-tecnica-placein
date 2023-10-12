using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaCriticApp.Shared
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Critic { get; set; }
    }
}
