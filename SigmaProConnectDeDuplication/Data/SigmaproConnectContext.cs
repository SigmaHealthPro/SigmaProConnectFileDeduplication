using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Runtime.Intrinsics.X86;


namespace SigmaProConnectDeDuplication.Data
{
    public partial class SigmaproConnectContext : DbContext
    {
        public SigmaproConnectContext()
        {
        }

        public SigmaproConnectContext(DbContextOptions<SigmaproConnectContext> options)
            : base(options)
        {
        }
        public virtual DbSet<patient_stage> patient_stage { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<County> Counties { get; set; }

        public virtual DbSet<Juridiction> Juridictions { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<State> States { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=sigmaprodb.postgres.database.azure.com,5432;Database=sigmapro_iis;Username=sigmaprodb_user;Password=Rules@23$$11;TrustServerCertificate=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<patient_stage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("patient_stage_pkey");
                entity.Property(e => e.Id)
             .HasDefaultValueSql("gen_random_uuid()")
             .HasColumnName("id");
                entity.Property(e => e.PatientName)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_name");
                entity.Property(e => e.PatientId)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_id");
                entity.Property(e => e.CreatedBy)
            .HasColumnType("character varying")
            .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            });
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Addresses_pkey");

                entity.ToTable("addresses");

                entity.HasIndex(e => e.CountyId, "fki_fk_countyids");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.AddressId)
                    .HasColumnType("character varying")
                    .HasColumnName("address_id");
                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.CountyId).HasColumnName("county_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Line1)
                    .HasColumnType("character varying")
                    .HasColumnName("line1");
                entity.Property(e => e.Line2)
                    .HasColumnType("character varying")
                    .HasColumnName("line2");
                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.Property(e => e.Suite)
                    .HasColumnType("character varying")
                    .HasColumnName("suite");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
                entity.Property(e => e.ZipCode)
                    .HasColumnType("character varying")
                    .HasColumnName("zip_code");

                entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("fk_cityids");

                entity.HasOne(d => d.Country).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("fk_countryids");

                entity.HasOne(d => d.County).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("fk_countyids");

                entity.HasOne(d => d.State).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("fk_stateids");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Cities_pkey");

                entity.ToTable("cities");

                entity.HasIndex(e => e.CountyId, "fki_fk_countyid");

                entity.HasIndex(e => e.StateId, "fki_fk_stateid");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.CityId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("city_id");
                entity.Property(e => e.CityName)
                    .HasColumnType("character varying")
                    .HasColumnName("city_name");
                entity.Property(e => e.CountyId).HasColumnName("county_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.County).WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("fk_countyid");

                entity.HasOne(d => d.State).WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("fk_stateid");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Countries_pkey");

                entity.ToTable("countries");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.Alpha2code)
                    .HasColumnType("character varying")
                    .HasColumnName("alpha2code");
                entity.Property(e => e.Alpha3code)
                    .HasColumnType("character varying")
                    .HasColumnName("alpha3code");
                entity.Property(e => e.CountryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("country_id");
                entity.Property(e => e.CountryName)
                    .HasColumnType("character varying")
                    .HasColumnName("country_name");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Counties_pkey");

                entity.ToTable("counties");

                entity.HasIndex(e => e.StateId, "fki_fk_stateids");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.CountyCode)
                    .HasColumnType("character varying")
                    .HasColumnName("county_code");
                entity.Property(e => e.CountyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("county_id");
                entity.Property(e => e.CountyName)
                    .HasColumnType("character varying")
                    .HasColumnName("county_name");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.State).WithMany(p => p.Counties)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("fk_stateids");
            });

            modelBuilder.Entity<Juridiction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Juridictions_pkey");

                entity.ToTable("juridictions");

                entity.HasIndex(e => e.AlternateId, "fki_fk_alternate_id");

                entity.HasIndex(e => e.StateId, "fki_fk_stateid_jurd");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.AlternateId).HasColumnName("alternate_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.JuridictionId)
                    .HasColumnType("character varying")
                    .HasColumnName("juridiction_id");
                entity.Property(e => e.JuridictionName)
                    .HasColumnType("character varying")
                    .HasColumnName("juridiction_name");
                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");


                entity.HasOne(d => d.State).WithMany(p => p.Juridictions)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("fk_stateid_jurd");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Organizations_pkey");

                entity.ToTable("organizations");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.JuridictionId).HasColumnName("juridiction_id");
                entity.Property(e => e.OrganizationName)
                    .HasColumnType("character varying")
                    .HasColumnName("organization_name");
                entity.Property(e => e.OrganizationsId)
                    .HasColumnType("character varying")
                    .HasColumnName("organizations_id");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Address).WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("fk_org_address_id");

                entity.HasOne(d => d.Juridiction).WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.JuridictionId)
                    .HasConstraintName("fk_juridiction_id");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Patient_pkey");

                entity.ToTable("patient");

                entity.HasIndex(e => e.PersonId, "fki_fk_patient_person_id");

                entity.HasIndex(e => e.CityId, "fki_patient_city_id");

                entity.HasIndex(e => e.CountryId, "fki_patient_country_id");

                entity.HasIndex(e => e.StateId, "fki_s");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.AliasFirstName)
                    .HasColumnType("character varying")
                    .HasColumnName("alias_first_name");
                entity.Property(e => e.AliasLastName)
                    .HasColumnType("character varying")
                    .HasColumnName("alias_last_name");
                entity.Property(e => e.AliasMidddleName)
                    .HasColumnType("character varying")
                    .HasColumnName("alias_midddle_name");
                entity.Property(e => e.CityId).HasColumnName("city_id");
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.DateOfHistoryVaccine).HasColumnName("date_of_history_vaccine");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.PatientId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("patient_id");
                entity.Property(e => e.PatientPrimaryLanguage)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_primary_language");
                entity.Property(e => e.PatientStatus)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_status");
                entity.Property(e => e.PatientStatusIndicatorProviderlevel)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_status_indicator_providerlevel");
                entity.Property(e => e.PatientStatusJuridictionLevel)
                    .HasColumnType("character varying")
                    .HasColumnName("patient_status_juridiction_level");
                entity.Property(e => e.PersonId).HasColumnName("person_id");
                entity.Property(e => e.ProtectionIndicator)
                    .HasColumnType("character varying")
                    .HasColumnName("protection_indicator");
                entity.Property(e => e.ProtectionIndicatorEffectiveDate).HasColumnName("protection_indicator_effective_date");
                entity.Property(e => e.ReminderRecallStatus)
                    .HasColumnType("character varying")
                    .HasColumnName("reminder_recall_status");
                entity.Property(e => e.ReminderRecallStatusEffectiveDate).HasColumnName("reminder_recall_status_effective_date");
                entity.Property(e => e.ResponsiblePersonFirstName)
                    .HasColumnType("character varying")
                    .HasColumnName("responsible_person_first_name");
                entity.Property(e => e.ResponsiblePersonLastName)
                    .HasColumnType("character varying")
                    .HasColumnName("responsible_person_last_name");
                entity.Property(e => e.ResponsiblePersonMidddleName)
                    .HasColumnType("character varying")
                    .HasColumnName("responsible_person_midddle_name");
                entity.Property(e => e.ResponsiblePersonRelationshipToPatient)
                    .HasColumnType("character varying")
                    .HasColumnName("responsible_person_relationship_to_patient");
                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.City).WithMany(p => p.Patients)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("patient_city_id");

                entity.HasOne(d => d.Country).WithMany(p => p.Patients)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("patient_country_id");

                entity.HasOne(d => d.Person).WithMany(p => p.Patients)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("fk_patient_person_id");

                entity.HasOne(d => d.State).WithMany(p => p.Patients)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("patient_state_id");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Person_pkey");

                entity.ToTable("person");

                entity.HasIndex(e => e.BirthStateId, "fki_patient_birth_state_id");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.BirthOrder)
                    .HasColumnType("character varying")
                    .HasColumnName("birth_order");
                entity.Property(e => e.BirthStateId).HasColumnName("birth_state_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("character varying")
                    .HasColumnName("date_of_birth");
                entity.Property(e => e.FirstName)
                    .HasColumnType("character varying")
                    .HasColumnName("first_name");
                entity.Property(e => e.Gender)
                    .HasColumnType("character varying")
                    .HasColumnName("gender");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.LastName)
                    .HasColumnType("character varying")
                    .HasColumnName("last_name");
                entity.Property(e => e.MiddleName)
                    .HasColumnType("character varying")
                    .HasColumnName("middle_name");
                entity.Property(e => e.MotherFirstName)
                    .HasColumnType("character varying")
                    .HasColumnName(" mother_first_name");
                entity.Property(e => e.MotherLastName)
                    .HasColumnType("character varying")
                    .HasColumnName(" mother_last_name");
                entity.Property(e => e.MotherMaidenLastName)
                    .HasColumnType("character varying")
                    .HasColumnName("mother_maiden_last_name");
                entity.Property(e => e.PersonId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("person_id");
                entity.Property(e => e.PersonType)
                    .HasColumnType("character varying")
                    .HasColumnName("person_type");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.BirthState).WithMany(p => p.People)
                    .HasForeignKey(d => d.BirthStateId)
                    .HasConstraintName("patient_birth_state_id");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("States_pkey");

                entity.ToTable("states");

                entity.HasIndex(e => e.CountryId, "fki_c");

                entity.HasIndex(e => e.CountryId, "fki_fk_countryids");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("id");
                entity.Property(e => e.CountryId).HasColumnName("country_id");
                entity.Property(e => e.CreatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("created_by");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.Isdelete).HasColumnName("isdelete");
                entity.Property(e => e.StateCode)
                    .HasColumnType("character varying")
                    .HasColumnName("state_code");
                entity.Property(e => e.StateId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("state_id");
                entity.Property(e => e.StateName)
                    .HasColumnType("character varying")
                    .HasColumnName("state_name");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("character varying")
                    .HasColumnName("updated_by");
                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Country).WithMany(p => p.States)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("fk_country_id");
            });

          
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
