using Ardalis.Result;
using Connectied.Application.Contracts;
using Connectied.Application.Repositories;
using Connectied.Domain.Guests;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Connectied.Application.Dashboard;
public record GuestStats
{
    public int Event1Quota { get; set; }
    public int Event2Quota { get; set; }
    public int Quota => Event1Quota + Event2Quota;
    public int Event1Attendance { get; set; }
    public int Event2Attendance { get; set; }
    public int Attendance => Event1Attendance + Event2Attendance;

    public int Event1Angpao { get; set; }
    public int Event2Angpao { get; set; }
    public int Angpao => Event1Angpao + Event2Angpao;
    public int Event1Gift { get; set; }
    public int Event2Gift { get; set; }
    public int Gift => Event1Gift + Event2Gift;
    public int Event1Souvenir { get; set; }
    public int Event2Souvenir { get; set; }
    public int Souvenir => Event1Souvenir + Event2Souvenir;
}
