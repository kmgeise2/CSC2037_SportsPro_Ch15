﻿namespace CSC2037_SportsPro_Ch15.Models
{
    public class TechIncidentViewModel
    {
        public Technician Technician { get; set; } = null!;
        public Incident Incident { get; set; } = null!;
        public IEnumerable<Incident> Incidents { get; set; } = null!;
    }
}
