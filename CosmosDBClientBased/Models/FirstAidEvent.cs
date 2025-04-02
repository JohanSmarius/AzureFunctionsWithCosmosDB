using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBClientBased.Models;

public class FirstAidEvent
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? NumberOfVolunteersNeededPerShift { get; set; }
    public List<Shift> Shifts { get; set; } = new();
    public bool IsDeleted { get; set; } = false;

    public void AddShift(Shift shift)
    {
        if (shift.BeginTime >= shift.EndTime)
        {
            throw new ArgumentException("Begin time must be before end time.");
        }

        if (shift.BeginTime < BeginDate || shift.EndTime > EndDate)
        {
            throw new ArgumentException("Shift times must be within the event's boundaries.");
        }

        Shifts.Add(shift);
    }
}
