using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SigmaProConnectDeDuplication.Data;
using SigmaProConnectDeDuplication;
using FuzzySharp;
using Google.Protobuf.Compiler;

public class PatientData
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? DOB { get; set; }
    public string? MothersName { get; set; }
}
public class PersonData
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? DOB { get; set; }
    public string? MothersName { get; set; }
}
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
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        try
        {
            using (var context = CreateDbContext())
            {
                // Retrieve patient_stage data asynchronously
                var patient_StageList = await context.patient_stage.ToListAsync();

                List<PatientData> patientData = patient_StageList.Select(p => new PatientData
                {
                    Id = p.Id,
                    Name = p.PatientName,
                    DOB = p.CreatedDate.ToString()
                    //Email = p.Email,
                    //MothersName = p.MothersName
                }).ToList();

                var personList = await context.Persons.ToListAsync();

                List<PersonData> personData = personList.Select(p => new PersonData
                {
                    Id = p.Id,
                    Name = p.FirstName,
                    DOB = p.DateOfBirth,
                    //Email = p.Email,
                    MothersName = p.MotherFirstName + p.MotherLastName
                }).ToList();

                foreach (var patient in patientData)
                {
                    var patientComposite = CreateCompositeString(patient);
                    var bestMatch = Process.ExtractOne(patientComposite, personData.Select(p => CreateCompositeString(p)));

                    // Console.WriteLine($"Best match for '{patientComposite}' is '{bestMatch.Index}' with a score of {bestMatch.Score}");

                    if (bestMatch.Score > 80)
                    {
                        var matchedPerson = personData[bestMatch.Index];
                        //Add Patient to Duplicate

                        var duplicateRecord = new PatientDuplicateRecord()
                        {
                            DuplicatePersonId = matchedPerson.Id,
                            FirstName = patient.Name
                        };
                        var result = context.Add(duplicateRecord);
                        context.SaveChanges();
                    }
                    else
                    {
                        var newPatientData = new PatientNewRecord()
                        {

                            FirstName = patient.Name,
                            MotherFirstName = patient.MothersName
                        };
                        var result = context.Add(newPatientData);
                        context.SaveChanges();
                    }

                }


            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during deduplication process: {ex.Message}");
        }
    }

    private string CreateCompositeString(PatientData patient)
    {
        return $"{patient.Name} {patient.DOB}";
    }

    private string CreateCompositeString(PersonData p)
    {
        return $"{p.Name} {p.DOB}";
    }

    private SigmaproConnectContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SigmaproConnectContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return new SigmaproConnectContext(optionsBuilder.Options);
    }

    




}
