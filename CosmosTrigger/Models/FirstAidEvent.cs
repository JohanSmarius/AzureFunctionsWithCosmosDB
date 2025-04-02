using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CosmosTrigger.Models;

public class FirstAidEvent
{
    public Guid id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("customerName")]
    public string CustomerName { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("beginDate")]
    public DateTime BeginDate { get; set; }
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("numberOfVolunteersNeededPerShift")]
    public int? NumberOfVolunteersNeededPerShift { get; set; }

    [JsonPropertyName("shifts")]
    public List<Shift> Shifts { get; set; } = new();

    [JsonPropertyName("isDeleted")]
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
