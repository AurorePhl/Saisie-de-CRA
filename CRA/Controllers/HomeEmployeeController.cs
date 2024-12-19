using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Mono.TextTemplating;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace CRA.Controllers
{
    public class HomeEmployeeController : Controller
    {

        private readonly IEmployeeRepository _repository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IPeriodRepository _periodRepository;
        public HomeEmployeeController(IEmployeeRepository repository, IScheduleRepository scheduleRepository, IAssignmentRepository assignmentRepository, ITimeSlotRepository timeSlotRepository, IPeriodRepository periodRepository)
        {
            _repository = repository;
            _scheduleRepository = scheduleRepository;
            _assignmentRepository = assignmentRepository;
            _timeSlotRepository = timeSlotRepository;
            _periodRepository = periodRepository;
        }

        public IActionResult Index(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        public IActionResult Edit(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateEmployee(employee);
                ViewData["EmployeeId"] = employee.Id;
                return RedirectToRoute(new
                {
                    controller = "HomeEmployee",
                    action = "Details",
                    id = employee.Id
                });
            }
            return View(employee);
        }

        public IActionResult Details(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        public IActionResult Schedules(Guid id, DateTime? selectedDate)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            // Si aucune date n'est sélectionnée, utilisez la date du jour
            DateTime date = selectedDate ?? DateTime.Now;

            // Calculer le premier jour de la semaine (lundi)
            var firstDayOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);

            // Calculer le dernier jour de la semaine (dimanche)
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);

            var schedules = _scheduleRepository.GetByEmployeeId(id);

            // Récupérer les assignments associés aux schedules
            var assignments = schedules.SelectMany(schedule =>
                _assignmentRepository.GetByScheduleId(schedule.Id)).ToList();

            // Récupérer les TimeSlots correspondant à la période et inclure les libellés des assignments
            var timeSlotsWithAssignments = assignments
                .SelectMany(a => _timeSlotRepository.GetByAssignmentCode(a.Code)
                    .Where(ts => ts.Start.Date >= firstDayOfWeek.Date && ts.Start.Date <= lastDayOfWeek.Date)
                    .Select(ts => new TimeSlotViewModel
                    {
                        Start = ts.Start,
                        End = ts.End,
                        State = ts.State,
                        AssignmentLibelle = a.Libelle
                    }))
                .ToList();

            ViewData["EmployeeId"] = id;
            ViewData["SelectedDate"] = date;
            return View(timeSlotsWithAssignments);
        }

        public IActionResult ActivityEntry(Guid id, DateTime? selectedDate)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            // Si aucune date n'est sélectionnée, utilisez la date du jour
            DateTime date = selectedDate ?? DateTime.Now;

            // Calculer le premier jour de la semaine (lundi)
            var firstDayOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);

            // Calculer le dernier jour de la semaine (dimanche)
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);

            var schedules = _scheduleRepository.GetByEmployeeId(id);

            // Récupérer les assignments associés aux schedules
            var assignments = schedules.SelectMany(schedule =>
                _assignmentRepository.GetByScheduleId(schedule.Id)).ToList();

            // Récupérer les TimeSlots correspondant à la période et inclure les libellés des assignments
            var timeSlotsWithAssignments = assignments
                .SelectMany(a => _timeSlotRepository.GetByAssignmentCode(a.Code)
                    .Where(ts => ts.Start.Date >= firstDayOfWeek.Date && ts.Start.Date <= lastDayOfWeek.Date)
                    .Select(ts => new TimeSlotViewModel
                    {
                        Id = ts.Id,
                        Start = ts.Start,
                        End = ts.End,
                        AssignmentLibelle = a.Libelle,
                        AssignmentCode = a.Code,
                    }))
                .ToList();

            IEnumerable<Assignment> allAssignments;
            List<Assignment> assignmentsList = new List<Assignment>();
            IEnumerable<Period> periods;
            List<Period> periodsList = new List<Period>();
            foreach (var assignment in assignments)
            {
                periodsList.Add(_periodRepository.GetPeriodById(assignment.PeriodId));

            }
            periods = periodsList;
            
            foreach(var period in periods)
            {
                if(period.Start <= firstDayOfWeek &&  period.End >= lastDayOfWeek)
                {
                    assignmentsList.Add(_assignmentRepository.GetByPeriodId(period.Id));
                }
            }
            allAssignments = assignmentsList;

            IEnumerable<TimeSlot> timeSlots;
            List<TimeSlot> timeSlotsList = new List<TimeSlot>();
            foreach (var assignment in allAssignments)
            {

                var timeSlot = _timeSlotRepository.GetByAssignmentCode(assignment.Code);
                foreach(var t in timeSlot)
                {
                    if(t.Start >= firstDayOfWeek && t.End <= lastDayOfWeek)
                    {
                        timeSlotsList.AddRange(timeSlot);
                    }
                }
            }
            timeSlots = timeSlotsList;

            ViewBag.TimeSlots = timeSlots;
            ViewData["EmployeeId"] = id;
            ViewData["SelectedDate"] = date;
            return View(allAssignments);
        }

        public IActionResult Assignments(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            IEnumerable<Assignment> assignments;
            List<Assignment> assignmentsList = new List<Assignment>();
            foreach (var schedule in schedules)
            {
                assignments = _assignmentRepository.GetByScheduleId(schedule.Id);
                assignmentsList.AddRange(assignments);
            }
            assignments = assignmentsList;
            ViewData["EmployeeId"] = id;
            return View(assignments);
        }

        public IActionResult AssignmentDetails(Guid employeeId, Guid code)
        {
            var employee = _repository.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }
            // Récupérer l'Assignment avec le Code de la mission
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound("Mission introuvable."); // Si la mission n'existe pas
            }

            // Récupérer les TimeSlots associés à cette mission
            var timeSlots = _timeSlotRepository.GetByAssignmentCode(code);

            var periodId = assignment.PeriodId;
            var period = _periodRepository.GetPeriodById(periodId);

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            ViewData["AssignmentLibelle"] = assignment.Libelle;
            ViewData["AssignmentDescription"] = assignment.Description;
            ViewData["PeriodStart"] = period.Start;
            ViewData["PeriodEnd"] = period.End;
            return View(timeSlots);
        }

        public IActionResult AddTimeSlot(Guid employeeId, Guid code)
        {
            var employee = _repository.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound("Mission introuvable.");
            }

            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            if (period == null)
            {
                return NotFound("Période introuvable.");
            }

            var timeSlot = new TimeSlot
            {
                State = "ADDED",
                AssignmentCode = code
            };

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            return View(timeSlot);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTimeSlot(Guid employeeId, Guid code, TimeSlot timeSlot)
        {
            var employee = _repository.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound("Mission introuvable.");
            }

            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            if (period == null)
            {
                return NotFound("Période introuvable.");
            }

            // Validation des dates du TimeSlot
            if (timeSlot.Start < period.Start || timeSlot.End > period.End || timeSlot.Start >= timeSlot.End)
            {
                ModelState.AddModelError("", "Le créneau horaire doit être compris dans la période de la mission et avoir des horaires de début et de fin valides.");
            }

            // Vérifier les conflits avec d'autres TimeSlots
            var existingTimeSlots = _timeSlotRepository.GetByAssignmentCode(code);
            if (existingTimeSlots.Any(ts => ts.Start < timeSlot.End && ts.End > timeSlot.Start))
            {
                ModelState.AddModelError("", "Le créneau horaire chevauche un créneau existant.");
            }

            // Vérifier que les jours des dates de début et de fin de créneau sont les mêmes
            if (timeSlot.Start.Day != timeSlot.End.Day)
            {
                ModelState.AddModelError("", "Les jours de début et de fin doivent être le même.");
            }

            // Vérifier que le nombre total d'heures effectuées (de créneaux) par jour ne dépassent pas 10 heures
            // On suppose qu'une heure vaut 0.1 et que la somme total d'heures par jour ne dépasse pas 1 
            var totalHoursForDay = existingTimeSlots
                .Where(ts => ts.Start.Date == timeSlot.Start.Date) 
                .Sum(ts => (ts.End - ts.Start).TotalHours); 

            var newTimeSlotDuration = (timeSlot.End - timeSlot.Start).TotalHours;

            if (totalHoursForDay + newTimeSlotDuration > 10)
            {
                ModelState.AddModelError("", "Le nombre total d'heures pour ce jour dépasse les 10 heures autorisées.");
            }

            else if (timeSlot.Start >= period.Start && timeSlot.End <= period.End && timeSlot.Start < timeSlot.End)
            {

                // Vérification que l'heure de début est après ou égale à 7h00 et l'heure de fin est avant ou égale à 23h00
                if (timeSlot.Start.TimeOfDay >= TimeSpan.FromHours(7) && timeSlot.End.TimeOfDay <= TimeSpan.FromHours(23))
                {
                    if (ModelState.IsValid)
                    {
                        _timeSlotRepository.AddTimeSlot(timeSlot);
                        ViewData["EmployeeId"] = employeeId;
                        ViewData["AssignmentCode"] = code;
                        ViewData["AssignmentLibelle"] = assignment.Libelle;
                        ViewData["AssignmentDescription"] = assignment.Description;
                        ViewData["PeriodStart"] = period.Start;
                        ViewData["PeriodEnd"] = period.End;
                        return RedirectToAction(nameof(AssignmentDetails), new { employeeId, code });
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Le créneau horaire doit être compris dans la période de la mission et avoir des horaires de début et de fin valides.");
            }

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            return View(timeSlot);
        }

        

        public IActionResult DeleteTimeSlot(Guid employeeId, Guid code, Guid timeSlotId)
        {
            var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            return View(timeSlot);
        }

        [HttpPost, ActionName("DeleteTimeSlot")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTimeSlotConfirmed(Guid employeeId, Guid code, Guid timeSlotId)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            _timeSlotRepository.DeleteTimeSlot(timeSlotId);
            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            ViewData["AssignmentLibelle"] = assignment.Libelle;
            ViewData["AssignmentDescription"] = assignment.Description;
            ViewData["PeriodStart"] = period.Start;
            ViewData["PeriodEnd"] = period.End;
            return RedirectToAction(nameof(AssignmentDetails), new { employeeId, code });
        }

        public IActionResult DeleteTimeSlot2(Guid employeeId, Guid code, Guid timeSlotId)
        {
            var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            ViewData["SelectedDate"] = timeSlot.Start;
            return View(timeSlot);
        }

        [HttpPost, ActionName("DeleteTimeSlot2")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTimeSlotConfirmed2(Guid employeeId, Guid code, Guid timeSlotId)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            _timeSlotRepository.DeleteTimeSlot(timeSlotId);
            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            ViewData["AssignmentLibelle"] = assignment.Libelle;
            ViewData["AssignmentDescription"] = assignment.Description;
            ViewData["PeriodStart"] = period.Start;
            ViewData["PeriodEnd"] = period.End;
            ViewData["SelectedDate"] = timeSlot.Start;
            return RedirectToAction(nameof(ActivityEntry), new { id = employeeId, code});
        }





        [HttpPost]
        public IActionResult SaveEntry(Guid employeeId, List<Guid> timeSlotIds)
        {
            foreach (var timeSlotId in timeSlotIds)
            {
                var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
                if (timeSlot == null)
                {
                    return NotFound("Créneau horaire introuvable.");
                }
                timeSlot.State = "SAVED";
                _timeSlotRepository.UpdateTimeSlot(timeSlot);
            }
            
            ViewData["EmployeeId"] = employeeId;
            return RedirectToRoute(new
            {
                controller = "HomeEmployee",
                action = "ActivityEntry",
                id = employeeId
            });
        }


        [HttpPost]
        public IActionResult SendEntry(Guid employeeId, List<Guid> timeSlotIds)
        {
            foreach (var timeSlotId in timeSlotIds)
            {
                var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
                if (timeSlot == null)
                {
                    return NotFound("Créneau horaire introuvable.");
                }
                timeSlot.State = "SENT";
                _timeSlotRepository.UpdateTimeSlot(timeSlot);
            }

            ViewData["EmployeeId"] = employeeId;
            return RedirectToRoute(new
            {
                controller = "HomeEmployee",
                action = "ActivityEntry",
                id = employeeId
            });
        }

        public IActionResult CopyPreviousWeek(Guid employeeId, DateTime currentDate)
        {
            // Calculer la date de début de la semaine précédente
            var previousWeekStart = currentDate.AddDays(-7);
            var previousWeekEnd = previousWeekStart.AddDays(6);



            ViewData["EmployeeId"] = employeeId;
            return RedirectToAction(nameof(AssignmentDetails), new { employeeId });  // Charger la vue de saisie avec les données copiées
        }


        public IActionResult EditTimeSlot(Guid employeeId, Guid code, Guid timeSlotId)
        {
            var timeSlot = _timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            ViewData["TimeSlotId"] = timeSlotId;
            return View(timeSlot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTimeSlot(Guid employeeId, Guid code, TimeSlot timeSlot)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            var ts = _timeSlotRepository.GetTimeSlotById(timeSlot.Id);
            if (ts == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            if (timeSlot.Start == default || timeSlot.End == default)
            {
                ModelState.AddModelError("", "Les dates de début et de fin doivent être valides.");
            }

            if (timeSlot.Start >= period.Start && timeSlot.End <= period.End && timeSlot.Start < timeSlot.End && timeSlot.End > timeSlot.Start)
            {
                if (ModelState.IsValid)
                {
                    _timeSlotRepository.UpdateTimeSlot(timeSlot);
                    ViewData["EmployeeId"] = employeeId;
                    ViewData["AssignmentCode"] = code;
                    ViewData["AssignmentLibelle"] = assignment.Libelle;
                    ViewData["AssignmentDescription"] = assignment.Description;
                    ViewData["PeriodStart"] = period.Start;
                    ViewData["PeriodEnd"] = period.End;
                    return RedirectToAction(nameof(AssignmentDetails), new { employeeId, code, timeSlot.Id });
                }
            }

            else
            {
                ModelState.AddModelError("", "Le créneau horaire doit être compris dans la période de la mission et avoir des horaires de début et de fin valides.");
            }

            ViewData["EmployeeId"] = employeeId;
            ViewData["AssignmentCode"] = code;
            return View(timeSlot);
        }

        public IActionResult Report(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            // Récupérer les assignments
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            List<Assignment> assignmentsList = new List<Assignment>();
            foreach (var schedule in schedules)
            {
                var assignments = _assignmentRepository.GetByScheduleId(schedule.Id);
                assignmentsList.AddRange(assignments);
            }

            // Préparez un ViewModel simple pour afficher la page Report
            var reportViewModel = new ReportViewModel
            {
                EmployeeName = $"{employee.Surname.ToUpper()} {employee.Name}",
                Month = "",
                Year = "",
                Assignments = assignmentsList.Select(a => new AssignmentReportItem
                {
                    Code = a.Code,
                    Libelle = a.Libelle,
                    Description = a.Description,
                    TotalWorkedDays = 0 // Les heures ne sont pas calculées ici
                }).ToList()
            };

            ViewData["EmployeeId"] = id;
            return View(reportViewModel);
        }

        public IActionResult Generate(Guid id, int month, int year)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employé introuvable.");
            }

            // Récupérer les assignments et les créneaux horaires pour le mois et l'année spécifiés
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            List<Assignment> assignmentsList = new List<Assignment>();
            foreach (var schedule in schedules)
            {
                var assignments = _assignmentRepository.GetByScheduleId(schedule.Id);
                assignmentsList.AddRange(assignments);
            }

            // Filtrer les créneaux horaires
            List<TimeSlot> timeSlotList = new List<TimeSlot>();
            foreach (var assignment in assignmentsList)
            {
                var timeSlots = _timeSlotRepository.GetByAssignmentCode(assignment.Code)
                    .Where(ts => ts.Start.Month == month && ts.Start.Year == year);
                timeSlotList.AddRange(timeSlots);
            }

            // Préparer le modèle de vue
            var reportViewModel = new ReportViewModel
            {
                EmployeeName = $"{employee.Surname.ToUpper()} {employee.Name}",
                Month = new DateTime(year, month, 1).ToString("MMMM").First().ToString().ToUpper() + new DateTime(year, month, 1).ToString("MMMM").Substring(1),
                Year = year.ToString(),
                Assignments = assignmentsList.Select(a => new AssignmentReportItem
                {
                    Code = a.Code,
                    Libelle = a.Libelle,
                    Description = a.Description,
                    TotalWorkedDays = timeSlotList
    .Where(ts => ts.AssignmentCode == a.Code)
    .Select(ts => ts.Start.Date)  
    .Distinct() 
    .Count()
                }).ToList()
            };

            ViewData["EmployeeId"] = id;
            return View(reportViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout(Guid id)
        {
            ViewData["EmployeeId"] = id;
            return RedirectToRoute(new
            {
                controller = "HomeEmployee",
                action = "Index",
                id = id
            });
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
