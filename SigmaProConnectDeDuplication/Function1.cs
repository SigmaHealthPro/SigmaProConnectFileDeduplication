using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SigmaProConnectDeDuplication.Data;
using SigmaProConnectDeDuplication;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly string _connectionString;

    public Function1(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Function1>();
        _connectionString = "Host=sigmaprodb.postgres.database.azure.com;Port=5432;Database=sigmapro_iis;Username=sigmaprodb_user;Password=Rules@23$$11;SSL Mode=Require;TrustServerCertificate=True";
    }

    [Function("Function1")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        try
        {
            using (var context = CreateDbContext())
            {
                IEnumerable<patient_stage> patients = await context.patient_stage.ToListAsync();
                PersonDeduplicationRule rule = new PersonDeduplicationRule();
                foreach (var patient in patients)
                {
                    if (IsDuplicatePatient(patient, context, rule))
                    {
                        var existingPatient = await context.Patients.FindAsync(patient.PatientId);
                        if (existingPatient != null)
                        {
                            existingPatient.PersonId = patient.Id;
                        }
                    }
                   
                }

                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during deduplication process: {ex.Message}");
        }
    }

    private SigmaproConnectContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SigmaproConnectContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return new SigmaproConnectContext(optionsBuilder.Options);
    }

    private bool IsDuplicatePatient(patient_stage patient, SigmaproConnectContext context, PersonDeduplicationRule rule)
    {
    

        var existingPatients = context.Persons
            .Where(person =>
                (string.IsNullOrEmpty(rule.FirstName) || person.FirstName == rule.FirstName) &&
                (string.IsNullOrEmpty(rule.LastName) || person.LastName == rule.LastName) &&
                (string.IsNullOrEmpty(rule.Gender) || person.Gender == rule.Gender) &&
                (rule.DateOfBirth == default || person.DateOfBirth == rule.DateOfBirth))
            .Select(person => new Person
            {
                
                FirstName = string.IsNullOrEmpty(rule.FirstName) ? person.FirstName : rule.FirstName,
                LastName = string.IsNullOrEmpty(rule.LastName) ? person.LastName : rule.LastName,
                Gender = string.IsNullOrEmpty(rule.Gender) ? person.Gender : rule.Gender,
                DateOfBirth = rule.DateOfBirth == default ? person.DateOfBirth : rule.DateOfBirth,
           
            })
            .ToList();

        foreach (var existingPatient in existingPatients)
        {
            if (ArePatientsSimilar(existingPatient, patient, rule))
            {
                return true;
            }
        }

        return false;
    }

    private bool ArePatientsSimilar(Person existingPatient, patient_stage newPatient, PersonDeduplicationRule rule)
    {
      
        bool isFirstNameSimilar = string.IsNullOrEmpty(rule.FirstName) || existingPatient.FirstName == newPatient.PatientName;
        bool isLastNameSimilar = string.IsNullOrEmpty(rule.LastName) || existingPatient.LastName == newPatient.PatientName;
        bool isGenderSimilar = string.IsNullOrEmpty(rule.Gender) || existingPatient.Gender == newPatient.PatientName;
        bool isDateOfBirthSimilar = rule.DateOfBirth == default || existingPatient.DateOfBirth == newPatient.PatientId;

        return  isFirstNameSimilar && isLastNameSimilar && isGenderSimilar && isDateOfBirthSimilar;
    }

}
