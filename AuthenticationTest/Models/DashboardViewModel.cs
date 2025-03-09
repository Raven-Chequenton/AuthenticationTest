public class DashboardViewModel
{
    public Dictionary<string, int> TicketCounts { get; set; }
    public int TicketsClosedThisWeek { get; set; }
    public List<AgentTicketCount> AgentTicketCounts { get; set; }
}

public class AgentTicketCount
{
    public string Assignee { get; set; }
    public int TicketCount { get; set; }
}
