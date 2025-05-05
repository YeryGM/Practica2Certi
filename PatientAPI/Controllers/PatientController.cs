using BusinessLogic.Managers;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Serilog;

namespace PatientAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientManager _manager;

        public PatientsController(ILogger<PatientsController> logger)
        {
            _manager = new PatientManager("patients.txt");
        }


        // GET: api/patients
        [HttpGet]
        public ActionResult<List<Patient>> GetAll()
        {
            return Ok(_manager.GetAllPatients());
        }

        // GET: api/patients/{ci}
        [HttpGet]
        [Route("{ci}")]
        public ActionResult<Patient> GetByCI(string ci)
        {
            Log.Information("Buscando paciente con CI: {Ci}", ci);

            var patient = _manager.GetPatientByCI(ci);

            if (patient == null)
            {
                Log.Error("Paciente no encontrado con CI: {Ci}", ci);
                return NotFound();
            }

            Log.Information("Paciente encontrado: {Name}", patient.Name);
            return Ok(patient);
        }


        // POST: api/patients
        [HttpPost]
        public ActionResult Add([FromBody] Patient patient)
        {
            try
            {
                _manager.AddPatient(patient);
                return CreatedAtAction(nameof(GetByCI), new { ci = patient.CI }, patient);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/patients/{ci}
        [HttpPut("{ci}")]
        public ActionResult Update(string ci, [FromBody] Patient updatedData)
        {
            var success = _manager.UpdatePatient(ci, updatedData.Name, updatedData.LastName);
            if (!success)
                return NotFound("Patient not found.");

            return NoContent();
        }

        // DELETE: api/patients/{ci}
        [HttpDelete]
        [Route("{ci}")]
        public IActionResult Delete(string ci)
        {
            Log.Information("Solicitud para eliminar paciente con CI: {Ci}", ci);

            var deleted = _manager.DeletePatient(ci);
            if (!deleted)
            {
                Log.Error("No se pudo eliminar el paciente con CI: {Ci}", ci);
                return NotFound();
            }

            Log.Information("Paciente con CI {Ci} eliminado correctamente", ci);
            return NoContent();
        }




        // Assignar un regalo a un paciente basado en el nombre "Yery" >:)

        [HttpPost]
        [Route("assign-gift")]
        public ActionResult<Electronic> GetGift([FromBody] Patient patient)
        {
            Log.Information("Solicitud de asignación de regalo para {Name}", patient.Name);

            var gift = _manager.AssignAGiftForStudent(patient);

            if (gift == null || string.IsNullOrEmpty(gift.Name))
            {
                Log.Error("No se pudo asignar un regalo a {Name}", patient.Name);
                return NotFound("No gift could be assigned.");
            }

            Log.Information("Regalo {Gift} asignado a {Name}", gift.Name, patient.Name);
            return Ok(gift);
        }


    }
}
