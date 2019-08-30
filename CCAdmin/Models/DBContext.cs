using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCAdmin.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppCredentials> AppCredentials { get; set; }
        public virtual DbSet<ApplicantCoursesThisYearAndNext> ApplicantCoursesThisYearAndNext { get; set; }
        public virtual DbSet<CcdispatchQueue> CcdispatchQueue { get; set; }
        public virtual DbSet<CcsentMailControl> CcsentMailControl { get; set; }
        public virtual DbSet<StudentDetail> StudentDetail { get; set; }
        public virtual DbSet<ProSolutionPermissions> ProSolutionPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppCredentials>(entity =>
            {
                entity.HasKey(e => e.AppCredentialId);

                entity.Property(e => e.AppCredentialId).HasColumnName("AppCredentialID");

                entity.Property(e => e.ActivationWord)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Expiry)
                    .HasColumnName("expiry")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdentityKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProtectionString)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SystemRef)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CcdispatchQueue>(entity =>
            {
                entity.ToTable("CCDispatchQueue");

                entity.Property(e => e.CcdispatchQueueId).HasColumnName("CCDispatchQueueID");

                entity.Property(e => e.AddedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateStamp).HasColumnType("datetime");

                entity.Property(e => e.SystemRef)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CcsentMailControl>(entity =>
            {
                entity.HasKey(e => e.CcsentMailId);

                entity.ToTable("CCSentMailControl");

                entity.Property(e => e.CcsentMailId).HasColumnName("CCSentMailID");

                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.Receipiant)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.StudentDetailId).HasColumnName("StudentDetailID");
            });

            modelBuilder.Entity<ApplicantCoursesThisYearAndNext>(entity =>
            {
                entity.HasKey(e => new { e.StudentDetailId, e.OfferingId, e.Choice });

                entity.Property(e => e.StudentDetailId).HasColumnName("StudentDetailID");

                entity.Property(e => e.OfferingId).HasColumnName("OfferingID");

                entity.Property(e => e.AcademicYearId)
                    .IsRequired()
                    .HasColumnName("AcademicYearID")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CollegeDecisionId).HasColumnName("CollegeDecisionID");

                entity.Property(e => e.DecisionDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DecisionId).HasColumnName("DecisionID");

                entity.Property(e => e.FirstForename)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OfferDate).HasColumnType("smalldatetime");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.OfferingTypeId).HasColumnName("OfferingTypeID");

                entity.Property(e => e.RefNo)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Surname)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentDetail>(entity =>
            {
                entity.HasIndex(e => e.AcademicYearId)
                    .HasName("IX_StudentDetail_AcademicYear");

                entity.HasIndex(e => e.DateOfBirth)
                    .HasName("IX_Student_DateOfBirth");

                entity.HasIndex(e => e.FirstForename)
                    .HasName("IX_Enrolment_FirstForename");

                entity.HasIndex(e => e.IsPotentialStudent)
                    .HasName("IX_StudentDetail_IsPotential");

                entity.HasIndex(e => e.SchoolId)
                    .HasName("IX_StudentDetail_School");

                entity.HasIndex(e => e.Sex)
                    .HasName("IX_Student_Sex");

                entity.HasIndex(e => e.Surname);

                entity.HasIndex(e => new { e.RefNo, e.AcademicYearId })
                    .HasName("IX_StudentDetail_RefNo")
                    .IsUnique();

                entity.HasIndex(e => new { e.StudentId, e.AcademicYearId })
                    .HasName("IX_StudentDetail_Student")
                    .IsUnique();

                entity.HasIndex(e => new { e.Surname, e.FirstForename })
                    .HasName("IX_Student_Name");

                entity.HasIndex(e => new { e.AcademicYearId, e.Surname, e.FirstForename })
                    .HasName("IX_StudentDetail_AcYearName");

                entity.Property(e => e.StudentDetailId).HasColumnName("StudentDetailID");

                entity.Property(e => e.AbilityToShareId).HasColumnName("AbilityToShareID");

                entity.Property(e => e.AcademicYearId)
                    .IsRequired()
                    .HasColumnName("AcademicYearID")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AccommodationTypeId)
                    .HasColumnName("AccommodationTypeID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNoEncrypted)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AchievedEnglishGcsebyEndOfYear11).HasColumnName("AchievedEnglishGCSEByEndOfYear11");

                entity.Property(e => e.AchievedMathsGcsebyEndOfYear11).HasColumnName("AchievedMathsGCSEByEndOfYear11");

                entity.Property(e => e.AdditionalBankAccountNameEncrypted)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalBankAccountNoEncrypted)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalBankBranchId).HasColumnName("AdditionalBankBranchID");

                entity.Property(e => e.AdditionalBankId).HasColumnName("AdditionalBankID");

                entity.Property(e => e.AdditionalLearningSupportLevelId)
                    .HasColumnName("AdditionalLearningSupportLevelID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalSupportCost).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.AlevelScore).HasColumnName("ALevelScore");

                entity.Property(e => e.Algconfirmed).HasColumnName("ALGConfirmed");

                entity.Property(e => e.Alsdeclined).HasColumnName("ALSDeclined");

                entity.Property(e => e.Alsrequested).HasColumnName("ALSRequested");

                entity.Property(e => e.Alsrequired).HasColumnName("ALSRequired");

                entity.Property(e => e.AltTel2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AsylumSeekerRef)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BankBranchId).HasColumnName("BankBranchID");

                entity.Property(e => e.BankId).HasColumnName("BankID");

                entity.Property(e => e.BuildingSocietyRollNoEncrypted)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CampusIdentifier)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CanBeContactBySms).HasColumnName("CanBeContactBySMS");

                entity.Property(e => e.CarColour)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CarMake)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CarModel)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CarParkPassType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CarReg)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Categories)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Childcare)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ConsentGivenDate).HasColumnType("date");

                entity.Property(e => e.ContactedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId)
                    .HasColumnName("CountryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUsing)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CriminalConvictionId).HasColumnName("CriminalConvictionID");

                entity.Property(e => e.DateOfAddressCapture).HasColumnType("smalldatetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("smalldatetime");

                entity.Property(e => e.DisabilityNotes).IsUnicode(false);

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.EmaadHocNote)
                    .HasColumnName("EMAAdHocNote")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Emaconfirmed).HasColumnName("EMAConfirmed");

                entity.Property(e => e.EmacourseTypeId)
                    .HasColumnName("EMACourseTypeID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EmadataSentSuccessfully).HasColumnName("EMADataSentSuccessfully");

                entity.Property(e => e.EmaerrorText)
                    .HasColumnName("EMAErrorText")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EmafileSubmissionId).HasColumnName("EMAFileSubmissionID");

                entity.Property(e => e.EmaflexibleBonusFrequencyId)
                    .HasColumnName("EMAFlexibleBonusFrequencyID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EmagroupId).HasColumnName("EMAGroupID");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Emanote)
                    .HasColumnName("EMANote")
                    .IsUnicode(false);

                entity.Property(e => e.EmaprogrammeOfStudyId)
                    .HasColumnName("EMAProgrammeOfStudyID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EmasiteId).HasColumnName("EMASiteID");

                entity.Property(e => e.EmastudentHasSignedPart2).HasColumnName("EMAStudentHasSignedPart2");

                entity.Property(e => e.EmastudyPatternId).HasColumnName("EMAStudyPatternID");

                entity.Property(e => e.EmploymentBeneficiaries)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EsfendDate)
                    .HasColumnName("ESFEndDate")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Esfid).HasColumnName("ESFID");

                entity.Property(e => e.EsfstartDate)
                    .HasColumnName("ESFStartDate")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.EthnicGroupId)
                    .HasColumnName("EthnicGroupID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EuroResidentId).HasColumnName("EuroResidentID");

                entity.Property(e => e.ExcludeFromDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ExcludeToDate).HasColumnType("smalldatetime");

                entity.Property(e => e.FirstForename)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FreeMealsEligibilityId).HasColumnName("FreeMealsEligibilityID");

                entity.Property(e => e.FundingEntitlement2Id)
                    .HasColumnName("FundingEntitlement2ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FundingEntitlementId)
                    .HasColumnName("FundingEntitlementID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GcseenglishAchievementId)
                    .HasColumnName("GCSEEnglishAchievementID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.GcseenglishConditionOfFundingId).HasColumnName("GCSEEnglishConditionOfFundingID");

                entity.Property(e => e.GcseenglishQualificationGrade)
                    .HasColumnName("GCSEEnglishQualificationGrade")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.GcsemathsAchievementId)
                    .HasColumnName("GCSEMathsAchievementID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.GcsemathsConditionOfFundingId).HasColumnName("GCSEMathsConditionOfFundingID");

                entity.Property(e => e.GcsemathsQualificationGrade)
                    .HasColumnName("GCSEMathsQualificationGrade")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.GdprallowContactByEmail).HasColumnName("GDPRAllowContactByEmail");

                entity.Property(e => e.GdprallowContactByPhone).HasColumnName("GDPRAllowContactByPhone");

                entity.Property(e => e.GdprallowContactByPost).HasColumnName("GDPRAllowContactByPost");

                entity.Property(e => e.Gdprnotes)
                    .HasColumnName("GDPRNotes")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.HasPep).HasColumnName("HasPEP");

                entity.Property(e => e.HeardAboutCollegeId).HasColumnName("HeardAboutCollegeID");

                entity.Property(e => e.HefinTypeAccomDiscount).HasColumnName("HEFinTypeAccomDiscount");

                entity.Property(e => e.HefinTypeCash).HasColumnName("HEFinTypeCash");

                entity.Property(e => e.HefinTypeNearCash).HasColumnName("HEFinTypeNearCash");

                entity.Property(e => e.HefinTypeOther).HasColumnName("HEFinTypeOther");

                entity.Property(e => e.Hepreference).HasColumnName("HEPreference");

                entity.Property(e => e.HequalsOnEntryId)
                    .HasColumnName("HEQualsOnEntryID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.HespecialFeeIndicatorId).HasColumnName("HESpecialFeeIndicatorID");

                entity.Property(e => e.HighestQualId)
                    .HasColumnName("HighestQualID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.HouseholdSituation1Id).HasColumnName("HouseholdSituation1ID");

                entity.Property(e => e.HouseholdSituation2Id).HasColumnName("HouseholdSituation2ID");

                entity.Property(e => e.Identification)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ilaref)
                    .HasColumnName("ILARef")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IlarefConfirmedBy)
                    .HasColumnName("ILARefConfirmedBy")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IlarefConfirmedDate)
                    .HasColumnName("ILARefConfirmedDate")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.IndustrialSectorId)
                    .HasColumnName("IndustrialSectorID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.KnownAs)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.LastInstitutionId)
                    .HasColumnName("LastInstitutionID")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastOrganisationLeavingDate).HasColumnType("smalldatetime");

                entity.Property(e => e.LearnerStatusId).HasColumnName("LearnerStatusID");

                entity.Property(e => e.LearnerSupportReason1Id)
                    .HasColumnName("LearnerSupportReason1ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportReason2Id)
                    .HasColumnName("LearnerSupportReason2ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportReason3Id)
                    .HasColumnName("LearnerSupportReason3ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportReason4Id)
                    .HasColumnName("LearnerSupportReason4ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportType1Id)
                    .HasColumnName("LearnerSupportType1ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportType2Id)
                    .HasColumnName("LearnerSupportType2ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportType3Id)
                    .HasColumnName("LearnerSupportType3ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportType4Id)
                    .HasColumnName("LearnerSupportType4ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearnerSupportType5Id)
                    .HasColumnName("LearnerSupportType5ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LearningDiffOrDisId)
                    .HasColumnName("LearningDiffOrDisID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LocalDestinationId).HasColumnName("LocalDestinationID");

                entity.Property(e => e.LocalDestinationOrganisationId).HasColumnName("LocalDestinationOrganisationID");

                entity.Property(e => e.LocalLearnerMonitoring1)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.LocalLearnerMonitoring2)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.LrsstudentContactTypeId)
                    .HasColumnName("LRSStudentContactTypeID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Lscnumber)
                    .HasColumnName("LSCNumber")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MigrationId)
                    .HasColumnName("MigrationID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobileTel)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.MostRecentProgrammeStartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.NameOnAccountEncrypted)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NationalityId)
                    .HasColumnName("NationalityID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Ni)
                    .HasColumnName("NI")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.NoOfAlevelsId)
                    .HasColumnName("NoOfALevelsID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NoOfScehighersId)
                    .HasColumnName("NoOfSCEHighersID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.NoOfVocationalQualsId)
                    .HasColumnName("NoOfVocationalQualsID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.OtherForenames)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Otjhours).HasColumnName("OTJHours");

                entity.Property(e => e.PlaceOfBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlannedEephours).HasColumnName("PlannedEEPHours");

                entity.Property(e => e.PostcodeInEnrolment)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.PostcodeOutEnrolment)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PotentialStudentStatusId).HasColumnName("PotentialStudentStatusID");

                entity.Property(e => e.PreMergerLearningProviderId).HasColumnName("PreMergerLearningProviderID");

                entity.Property(e => e.PreviousRefNo)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousStudentRefNo)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousUklearningProvider).HasColumnName("PreviousUKLearningProvider");

                entity.Property(e => e.PriorAttainmentLevelId)
                    .HasColumnName("PriorAttainmentLevelID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.PriorAttainmentLevelInYearId)
                    .HasColumnName("PriorAttainmentLevelInYearID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderSpecified1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderSpecified2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PupilPremiumFundingEligibility2Id)
                    .HasColumnName("PupilPremiumFundingEligibility2ID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PupilPremiumFundingEligibilityId)
                    .HasColumnName("PupilPremiumFundingEligibilityID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonDidntEnrolId).HasColumnName("ReasonDidntEnrolID");

                entity.Property(e => e.ReasonForExclusion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RefNo)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.ReferencesConfirmedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferencesConfirmedDate).HasColumnType("datetime");

                entity.Property(e => e.ReferencesNotes).IsUnicode(false);

                entity.Property(e => e.ReferralDate).HasColumnType("datetime");

                entity.Property(e => e.ReferralSourceId).HasColumnName("ReferralSourceID");

                entity.Property(e => e.ScehighersScore).HasColumnName("SCEHighersScore");

                entity.Property(e => e.SchoolAttendedFrom).HasColumnType("datetime");

                entity.Property(e => e.SchoolAttendedTo).HasColumnType("datetime");

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SocioEconomicClassId)
                    .HasColumnName("SocioEconomicClassID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SococcupationId)
                    .HasColumnName("SOCOccupationID")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.StudentFirstLanguageId).HasColumnName("StudentFirstLanguageID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.StudentInitiative2Id)
                    .HasColumnName("StudentInitiative2ID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.StudentInitiativeId)
                    .HasColumnName("StudentInitiativeID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.StudentVisaRequirementId).HasColumnName("StudentVisaRequirementID");

                entity.Property(e => e.SupportNeed1Id).HasColumnName("SupportNeed1ID");

                entity.Property(e => e.SupportNeed2Id).HasColumnName("SupportNeed2ID");

                entity.Property(e => e.SupportNeed3Id).HasColumnName("SupportNeed3ID");

                entity.Property(e => e.SupportRef)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SurnameAtBirth)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Tel2)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TutorGroupId).HasColumnName("TutorGroupID");

                entity.Property(e => e.TutorId).HasColumnName("TutorID");

                entity.Property(e => e.UcasapplicationCode)
                    .HasColumnName("UCASApplicationCode")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UcaspersonalId)
                    .HasColumnName("UCASPersonalID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UcastariffPoints).HasColumnName("UCASTariffPoints");

                entity.Property(e => e.Upin)
                    .HasColumnName("UPIN")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined10)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined11)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined12)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined13)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined14)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined15)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined16)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined17)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined18)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined19)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined20)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined21)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined22)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined23)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined24)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined25)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined26)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined27)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined28)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined29)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined30)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined31)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined32)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined33)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined34)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined35)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined36)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined37)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined38)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined39)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined40)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined41)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined42)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined43)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined44)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined45)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined5)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined6)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined7)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined8)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefined9)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection10)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection11)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection12)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection13)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection14)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection15)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection5)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection6)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection7)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection8)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserDefinedDataProtection9)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VerificationOtherDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VerificationTypeId)
                    .HasColumnName("VerificationTypeID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.VlelastSent)
                    .HasColumnName("VLELastSent")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.WideningParticipationFactor).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.WideningParticipationId)
                    .HasColumnName("WideningParticipationID")
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProSolutionPermissions>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.PermissionObjectActionId });

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionObjectActionId).HasColumnName("PermissionObjectActionID");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataSourceId).HasColumnName("DataSourceID");

                entity.Property(e => e.ObjectCaption)
                    .HasMaxLength(437)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenId).HasColumnName("ScreenID");
            });
        }
    }
}
