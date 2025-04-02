using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeBased.Models;

public class Shift
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime BeginTime { get; set; }
    public DateTime EndTime { get; set; }

    public int NumberOfVolunteersNeeded { get; set; }

}
