using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Core.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration;
        public string Description { get; set; }

        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; } //int? id //instanser av kopplingstabellen - detta är ett navigation property. Instanser där GymClass Id:t är med. Från kopplingstabllen har vi GymCLass och ApplicationsUser. Om vi tar ThenInclude så kan vi ta med alla members som finns i kopplingstabllen. (Seb:) Vi skickar m.a.o Idt för GymClass ???
    }
}
